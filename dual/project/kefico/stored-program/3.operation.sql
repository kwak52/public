USE kefico

DROP FUNCTION IF EXISTS shift50LeftOneBit;
DROP PROCEDURE IF EXISTS verifyPdvExist;
DROP PROCEDURE IF EXISTS extractInfoFromPdv;
DROP PROCEDURE IF EXISTS insertMeasure;
DROP PROCEDURE IF EXISTS bitTest;
DROP PROCEDURE IF EXISTS deleteStep;

DELIMITER $$

CREATE FUNCTION shift50LeftOneBit( in_value BIT(50) )
	RETURNS BIT(50)
	COMMENT 'Returns one bit left shifted value.'
BEGIN
	DECLARE l_value	BIT(51) DEFAULT in_value;		-- Buffer overflow error 방지를 위해서 1-bit 더 큰 크기로 준비
	-- HEX 값 '3 FFFF FFFF FFFF' 는 모든 bit 가 1 인 50 bit 의 표현
	RETURN ((l_value << 1) & CONV('3FFFFFFFFFFFF', 16, 10));
END$$


/*
 * PDV 에서 해당 정보가 존재하는지 확인
 */
CREATE PROCEDURE verifyPdvExist(in_pdvId INT)
	COMMENT 'Check whether given id exists in pdv table.'
BEGIN
	DECLARE l_message VARCHAR(255) DEFAULT CONCAT('PDV entry ', in_pdvId, ' not found');

	IF NOT EXISTS( SELECT NULL FROM pdv WHERE id = in_pdvId ) THEN
		SIGNAL SQLSTATE '99999'
			SET MESSAGE_TEXT = l_message;
	END IF;
END$$


/*
 * PDV id 값을 이용해서 product 및 partNr 정보 추출
 */
CREATE PROCEDURE extractInfoFromPdv(
	in_pdvId INT
	, OUT out_product VARCHAR(32)
	, OUT out_partNr VARCHAR(32)
)
	COMMENT 'Extracts product and part information from pdv table.'
BEGIN
	DECLARE l_id INT DEFAULT NULL;

	SET out_product = NULL;
	SET out_partNr = NULL;

	SELECT id, product, pamType
	INTO l_id, out_product, out_partNr
	FROM pdv
	WHERE id = in_pdvId
	;
	
	IF l_id IS NULL THEN
		SIGNAL SQLSTATE '99999'
			SET MESSAGE_TEXT = 'No PDV entry found';
	END IF;
END$$



/*
 * 시험기 1회 테스트 완료시, bundle 에 대한 summary 정보를 insert 한다.
 *  - header 와 bundle 모두에 대해 transaction 으로 구성
 * 사전 조건 : tt_bundle 에 실제 measure data 를 채워 놓아야 한다.
 */
CREATE PROCEDURE insertMeasure(
	in_day DATE
	, in_time TIME(2)
	, in_duration DECIMAL(8, 2)
	, in_ccsId INT
	, in_pdvId INT
	, in_ecuId CHAR(10)
	, in_eprom CHAR(20)
	, in_fixture CHAR(6)
	, in_batchName VARCHAR(16)
	, in_ok BOOLEAN		-- NG 여부.  1이면 NG, 0 이면 OK
)
	COMMENT 'Insert measure summary.  CCS should call this procedure when one bundle test finished.'
BEGIN
	DECLARE l_measureId			INT;
	DECLARE l_endTime			TIME(2)		DEFAULT ADDTIME(in_time, SEC_TO_TIME(in_duration));
	DECLARE l_durationSum		DECIMAL(18, 2)	DEFAULT 0;
	DECLARE l_countTotal		SMALLINT	DEFAULT 0;
	DECLARE l_lastEcu100First	BIT(51)		DEFAULT 0;
	DECLARE l_lastEcu100Last	BIT(51)		DEFAULT 0;
	
	DECLARE l_id				INT	DEFAULT -100;
	DECLARE l_shiftBit			BOOLEAN;
	
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE('SQL error happend in insertMeasure.');
		RESIGNAL;
	END;


	-- message 와 value 둘다 NULL 인 경우는 measure data 로 의미가 없다.
	SELECT EXISTS(SELECT * FROM tt_bundle WHERE value IS NULL and message IS NULL) into @invalidEntry;
	IF @invalidEntry THEN
        SIGNAL SQLSTATE '99999'
            SET MESSAGE_TEXT = 'There is invalid entry with both value and message are null.';
	END IF;

	-- test mode 에서 duration 을 random 으로 설정한다.
	SELECT val INTO @enableTestDataInsert FROM preference WHERE name = 'enableTestDataInsert';
	IF @enableTestDataInsert = 'Y' THEN
		SELECT val INTO @nextMinimalStartTime FROM preference WHERE name = 'nextMinimalStartTime';
		SET sql_mode = '';	-- STR_TO_DATE() 함수를 위해서 필요.
				-- http://dev.mysql.com/doc/refman/5.7/en/date-and-time-functions.html#function_str-to-date
		SET in_duration = getRandomN(1, 1000) / POW(10, getRandomN(0, 4));

		IF @nextMinimalStartTime IS NOT NULL THEN
			SET in_time = ADDTIME(STR_TO_DATE(@nextMinimalStartTime, '%H:%i:%s'), SEC_TO_TIME(getRandomN(0, 100) / 10));
		END IF;

		SET @nextMinimalStartTime = ADDTIME(in_time, SEC_TO_TIME(in_duration));
		INSERT INTO preference (name, val)
		VALUES('nextMinimalStartTime', @nextMinimalStartTime)
		ON DUPLICATE KEY
			UPDATE val = @nextMinimalStartTime
		;
	END IF;
	
	CALL addLogD(CONCAT('Starts insertMeasure: CCS=', in_ccsId, ' PDV=', in_pdvId));
	CALL addLogD(CONCAT('  ecu=', in_ecuId, ', fixture=', in_fixture));

	-- PDV 에서 해당 정보가 존재하는지 확인
	CALL verifyPdvExist(in_pdvId);
	
	SELECT id, durationSum, total, lastEcu100First, lastEcu100Last
	INTO l_id, l_durationSum, l_countTotal, l_lastEcu100First, l_lastEcu100Last
	FROM dynamicTopSummary
	WHERE day = in_day
	  AND ccsId = in_ccsId
	  AND (pdvId IS NULL OR pdvId = in_pdvId)
	  AND (in_batchName IS NULL OR _batchName = in_batchName)
	FOR UPDATE
	;
	
	
	IF l_id < 0 THEN
		CALL addLogE(CONCAT('  No proper row on dynamicTopSummary.  ccs=', in_ccsId,
				', pdv=', in_pdvId, ', batch=', IFNULL(in_batchName, 'NULL')));
		
		-- 현재 dynamicTopSummary 에 현재 section 에 대한 정보가 없는 경우 
		SIGNAL SQLSTATE '99999'
			SET MESSAGE_TEXT = 'No dynamicTopSummary row entry.  Did you send notifyBatchChange()?';
	ELSE
		IF l_countTotal > 0 THEN
			IF l_countTotal < 50 THEN
				SET l_lastEcu100First = 0;
			ELSE
				-- HEX 값 '2 0000 0000 0000' 는 최상위 bit 만 1 의 값을 갖는 50 bit 표현
				SET l_shiftBit = IF(l_lastEcu100Last & CONV('2000000000000', 16, 10), 1, 0);
				SET l_lastEcu100First = shift50LeftOneBit(l_lastEcu100First) + l_shiftBit;
			END IF;
		
		END IF;
		
		SET l_lastEcu100Last = shift50LeftOneBit(l_lastEcu100Last) + IF(in_ok, 1, 0);
		
		START TRANSACTION;

			UPDATE dynamicTopSummary
			SET
			  total = total + 1
			  , durationSum = durationSum + in_duration
			  , ngCount = ngCount + IF(in_ok, 0, 1)
			  , lastEcu100First = l_lastEcu100First
			  , lastEcu100Last = l_lastEcu100Last
			  , endTime = l_endTime
			  , pdvId = IFNULL(pdvId, in_pdvId)
			WHERE id = l_id
			;

			
			-- measure table 에 정보 추가
			INSERT INTO measure (day, time, duration, ccsId, pdvId, ecuid, eprom, fixture, batchName, ok, type)
			VALUES
			  (in_day, in_time, in_duration, in_ccsId, in_pdvId, in_ecuId, in_eprom, in_fixture, in_batchName, in_ok, 'NM')
			;

			SET l_measureId = LAST_INSERT_ID();
			
			INSERT INTO bundle(day, measureId, stepId, value, message, ok)
			SELECT
				in_day, l_measureId
				, stepId, value, message, ok
			FROM tt_bundle
			;

			TRUNCATE TABLE tt_bundle;

		COMMIT;

	END IF;

END$$


DELIMITER ;

