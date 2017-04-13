USE kefico;

DROP PROCEDURE IF EXISTS notifyPowerOnOffStatusChange;
DROP PROCEDURE IF EXISTS notifyBatchChange;
DROP PROCEDURE IF EXISTS _closePendingBatch;
DROP FUNCTION IF EXISTS getCcsIdFromHost;


DELIMITER $$


/*
 * PowerON, batch change notify 시에 호출.
 */
CREATE PROCEDURE _closePendingBatch(
    in_ccsId INT
)
	COMMENT	'Close batch and move from dynamic to static'
BEGIN
	DECLARE l_id INT;
	DECLARE l_total INT;
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE(CONCAT('SQL error happend in _closePendingBatch on ccsId=', in_ccsId));
		RESIGNAL;
	END;

CALL addLogD(CONCAT('Closing current ccs=', in_ccsId));

	SELECT id, total
	INTO l_id, l_total
	FROM dynamicTopSummary
	WHERE ccsId = in_ccsId
	FOR UPDATE
	;

	IF l_id IS NOT NULL
	THEN
		-- total(=l_total) 이 zero 이면 켜진 후에 한번도 검사한 항목이 없는 경우.
		-- 집계에서 제외한다.
		-- divide by zero 
		IF l_total > 0 THEN
			INSERT INTO staticTopSummary(
				day, ccsId, pdvId, startTime, endTime
				, durationAvg
				, total
				, ngCount
				, percentageGood
				, percentageGood100
				, lastECUs
				, _durationSum
				, _lastEcu100First
				, _lastEcu100Last
				, _batchName
			)
			SELECT
				day, ccsId, pdvId, startTime, endTime
				, durationSum/total AS durationAvg
				, total
				, ngCount
				, percentGood_gc		AS percentageGood
				, percentGood100_gc		AS percentageGood100
				, lastECUs_gc			AS LastECUs
				, durationSum
				, lastEcu100First
				, lastEcu100Last
				, _batchName
			FROM dynamicTopSummary
			WHERE id = l_id
			;
		END IF;
		

		-- 원본에서 제거
		DELETE
		FROM dynamicTopSummary
		WHERE id = l_id
		;

	END IF;

END$$

CREATE FUNCTION getCcsIdFromHost(
	in_host INT
	, in_sec VARCHAR(4)
)
	RETURNS INT
BEGIN
	DECLARE l_ccsId INT DEFAULT NULL;

	SELECT id
	INTO l_ccsId
	FROM ccs
	WHERE host = in_host
		AND sec = in_sec
	;

	IF l_ccsId IS NULL THEN
		SET @msg = CONCAT('No CCS id found: Host=', in_host, ', SEC=', in_sec);
        SIGNAL SQLSTATE '99999'
            SET MESSAGE_TEXT = @msg;
	END IF;

	RETURN l_ccsId;
END$$


CREATE PROCEDURE notifyPowerOnOffStatusChange(
	in_eventDt DATETIME
	, in_host INT
	, in_sec VARCHAR(4)
	, in_powerOn BOOLEAN
	, in_fixture CHAR(6)
	, in_batchName VARCHAR(16)
)
	COMMENT 'CCS should call this procedure when power status changed.'
BEGIN
	DECLARE l_eventDay		DATE DEFAULT DATE(in_eventDt);
	DECLARE l_eventTime		TIME DEFAULT TIME(in_eventDt);
	DECLARE l_ccsId			INT DEFAULT getCcsIdFromHost(in_host, in_sec);

	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE('SQL error happend in notifyPowerOnOffStatusChange.');
		RESIGNAL;
	END;



	CALL addLogD(CONCAT('On ', in_eventDt, ', Power ', IF(in_powerOn, 'ON', 'OFF'), ', ccsId=', l_ccsId,
		', batch=', IFNULL(in_batchName, 'NULL'), ', fixture=', IFNULL(in_fixture, 'NULL')));

	CALL _closePendingBatch(l_ccsId);

	IF in_powerOn THEN

		INSERT INTO dynamicTopSummary
		  (day, ccsId, startTime, endTime, _batchName)
		VALUES
		  (l_eventDay, l_ccsId, l_eventTime, l_eventTime, in_batchName)
		;
	END IF;
	
	-- measure table 에 정보 추가
	INSERT INTO measure (day, time, ccsId, fixture, batchName, type)
	VALUES
		(l_eventDay, l_eventTime, l_ccsId, in_fixture, in_batchName, IF(in_powerOn, 'PN', 'PF'))
	;
END$$


CREATE PROCEDURE notifyBatchChange(
	in_eventDt DATETIME
	, in_ccsId INT
	, in_pdvId INT
	, in_ecuId CHAR(10)
	, in_eprom CHAR(20)
	, in_fixture CHAR(6)
	, in_newBatchName VARCHAR(16)
)
	MODIFIES SQL DATA
	COMMENT 'CCS should call this procedure when batch changed.'
BEGIN
	DECLARE l_eventDay		DATE DEFAULT DATE(in_eventDt);
	DECLARE l_eventTime		TIME DEFAULT TIME(in_eventDt);
	DECLARE l_batchChange	TINYINT DEFAULT NULL;

	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE('SQL error happend in notifyBatchChange.');
		RESIGNAL;
	END;
	
	IF in_newBatchName IS NULL THEN
        SIGNAL SQLSTATE '99999'
            SET MESSAGE_TEXT = 'in notifyBatchChange(), got NULL batch name.';
	END IF;

CALL addLogD(CONCAT('BatchChange : ccsId=', in_ccsId, ', batch=', in_newBatchName));

	CALL verifyPdvExist(in_pdvId);
	
	-- OLD batch 에 해당하는 dynamicTopSummary row 를 마감한다.
	CALL _closePendingBatch(in_ccsId);
	
	INSERT INTO dynamicTopSummary
		(day, ccsId, pdvId, startTime, endTime, _batchName)
	VALUES
		(l_eventDay, in_ccsId, in_pdvId, l_eventTime, l_eventTime, in_newBatchName)
	;

	-- measure table 에 정보 추가
	INSERT INTO measure (day, time, ccsId, pdvId, ecuid, eprom, fixture, batchName, type)
	VALUES
		(l_eventDay, l_eventTime, in_ccsId, in_pdvId, in_ecuId, in_eprom, in_fixture, in_newBatchName, 'BC')
	;

END$$

DELIMITER ;

