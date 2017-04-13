USE kefico

DROP PROCEDURE IF EXISTS showSTSV;
DROP PROCEDURE IF EXISTS showPHV;
DROP PROCEDURE IF EXISTS showSOV;
DROP PROCEDURE IF EXISTS showBundle;
DROP PROCEDURE IF EXISTS showTCT;
DROP PROCEDURE IF EXISTS showRT;

DROP PROCEDURE IF EXISTS showTotalSummaryView;
DROP PROCEDURE IF EXISTS _createTemporaryMeasureSummaryTable;
DROP PROCEDURE IF EXISTS showTestSummary;


DROP PROCEDURE IF EXISTS showTestAndCycleTimes;
DROP PROCEDURE IF EXISTS showRetest;
DROP PROCEDURE IF EXISTS showMeasureInfo;

DROP PROCEDURE IF EXISTS _extractInfoFromTSVRow;
DROP PROCEDURE IF EXISTS _routeTSVRowIntoProgram;

DELIMITER $$


CREATE PROCEDURE showTotalSummaryView(
	in_startDay DATE
	, in_endDay DATE
	, in_quickViewMode BOOLEAN
)
	COMMENT 'Shows total summary on fixed period.'
BEGIN
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE('SQL error happend in showTotalSummaryView.');
		RESIGNAL;
	END;

	IF in_quickViewMode THEN
		SELECT t1.*
			FROM topSummary_vw t1
			LEFT OUTER JOIN topSummary_vw t2
			ON t1._ccsId = t2._ccsId
				AND t1._pdvId = t2._pdvId
				AND t1.startTime < t2.startTime
			WHERE t2.id IS NULL
				AND t1.day BETWEEN in_startDay AND in_endDay
			ORDER BY t1.day, t1.id
		;
	ELSE
		SELECT *
		FROM topSummary_vw
		WHERE day BETWEEN in_startDay AND in_endDay
		ORDER BY day, id
		;
	END IF;

END$$


-- CALL showStepSummaryView( '2016-08-07', '2016-08-07', 1, 3, NULL);	-- ccsId, pdvId, batch=NULL




-- CALL _createTemporaryMeasureSummaryTable( '2016-08-07', '2016-08-07', 1, 3, NULL);	-- ccsId, pdvId, batch=NULL

/*
 * TSV 에서 특정 row {= ccsid, pdvid, 날짜}  를 지정하였을 때, 
 * 해당 hw-sec 에서 진행한 모든 검사 결과를 summary 해서 보여줌.
 */
CREATE PROCEDURE _createTemporaryMeasureSummaryTable(
	in_startDay DATE
	, in_endDay DATE
	, in_ccsId INT		-- Nullable
	, in_pdvId INT		-- Nullable
	, in_batchName VARCHAR(16)	-- Nullable
)
	COMMENT 'Shows section test summary on fixed {hw-sec, date, batch}.'
BEGIN
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE('SQL error happend in _createTemporaryMeasureSummaryTable.');
		RESIGNAL;
	END;


	-- 입력 조건에 부합하는 measure 를 찾아서 temporary table 작성
	DROP TEMPORARY TABLE IF EXISTS tt_measureSummary;

	CREATE TEMPORARY TABLE tt_measureSummary
		ENGINE=MEMORY
	AS
		SELECT *
		FROM measure
		WHERE (in_ccsId IS NULL OR ccsId = in_ccsId)
			AND (in_pdvId IS NULL OR pdvId = in_pdvId)
			AND (in_batchName IS NULL OR batchName=in_batchName)
			AND (day BETWEEN in_startDay AND in_endDay)
			-- AND type = 'NM'		-- NM(=normal measure) only ????
		;
END$$



CREATE PROCEDURE showTestSummary(
	in_startDay DATE
	, in_endDay DATE
	, in_ccsId INT
	, in_pdvId INT
	, in_batchName VARCHAR(16)	-- if NULL, no batch filter
)
	COMMENT 'Shows test summary on fixed {hw-sec, date, batch}.'
BEGIN
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE('SQL error happend in showTestSummary.');
		RESIGNAL;
	END;

	CALL _createTemporaryMeasureSummaryTable(in_startDay, in_endDay, in_ccsId, in_pdvId, in_batchName);
	SELECT *
	FROM tt_measureSummary
	ORDER BY ccsId, pdvId
	;
END$$



/*
 * TSV 의 선택된 하나의 row(=>in_tsvId) 에 대한 부가 정보를 추출
 */
CREATE PROCEDURE _extractInfoFromTSVRow(
	in_tsvId INT
	, in_isFromDynamic BOOLEAN
	, OUT out_day DATE
	, OUT out_ccsId INT
	, OUT out_pdvId INT
	, OUT out_batchName VARCHAR(16)
)
	COMMENT 'Extracts ccs, pdv id from selectecd TSV row'
BEGIN
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE('SQL error happend in _extractInfoFromTSVRow.');
		RESIGNAL;
	END;

	SELECT day, _ccsId, _pdvId, _batchName
    INTO out_day, out_ccsId, out_pdvId, out_batchName
    FROM topSummary_vw
    WHERE id=in_tsvId AND _isFromDynamic=in_isFromDynamic
    ;

	IF out_day IS NULL OR out_ccsId IS NULL OR out_pdvId IS NULL THEN
		CALL addLogE(CONCAT('FAILED: _extractInfoFromTSVRow(', in_tsvId, ', ', in_isFromDynamic, ')'));
	END IF;

END$$



CREATE PROCEDURE _routeTSVRowIntoProgram(
	in_proc VARCHAR(255)
	, in_tsvId INT
	, in_isFromDynamic BOOLEAN
)
	COMMENT	'route TSV row into specific procedure'
BEGIN
	DECLARE l_day DATE;
	DECLARE l_ccsId INT;
	DECLARE l_pdvId INT;
	DECLARE l_batchName VARCHAR(16);
	DECLARE l_command VARCHAR(1024);

	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE(CONCAT('SQL error happend in _routeTSVRowIntoProgram(', in_proc, ', ', in_tsvId, ', ', in_isFromDynamic, ').'));
		RESIGNAL;
	END;

	-- TSV 의 선택된 하나의 row(=>in_tsvId) 에 대한 부가 정보를 추출
	CALL _extractInfoFromTSVRow(in_tsvId, in_isFromDynamic, l_day, l_ccsId, l_pdvId, l_batchName);

	SET l_command =
		CONCAT(
			'CALL ', in_proc, '(', QUOTE(l_day), ', ', QUOTE(l_day), ', ', l_ccsId, ', ', l_pdvId, ', '
			, IF(l_batchName IS NULL, 'NULL', QUOTE(l_batchName)), ');'
		)
	;

	CALL addLogD9( CONCAT(' => ', l_command));
	CALL executeStatement(l_command);
END$$	






CREATE PROCEDURE showSTSV(in_tsvId INT, in_isFromDynamic BOOLEAN)
	COMMENT 'Shows STSV helper.'
BEGIN
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE('SQL error happend in showSTSV.');
		RESIGNAL;
	END;

	CALL _routeTSVRowIntoProgram('_createTemporaryMeasureSummaryTable', in_tsvId, in_isFromDynamic);
	SELECT * FROM tt_measureSummary;
END$$


CREATE PROCEDURE showTCT(in_tsvId INT, in_isFromDynamic BOOLEAN)
	COMMENT	'Show test and cycle times.'
BEGIN
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE('SQL error happend in showTCT.');
		RESIGNAL;
	END;

	CALL _routeTSVRowIntoProgram('showTestAndCycleTimes', in_tsvId, in_isFromDynamic);
END$$




CREATE PROCEDURE showPHV(
	in_tsvId INT
	, in_isFromDynamic BOOLEAN
	, in_stepId INT
)
	COMMENT 'Shows PHV'
BEGIN
	DECLARE l_day DATE;
	DECLARE l_ccsId INT;
	DECLARE l_pdvId INT;
	DECLARE l_batchName VARCHAR(16);

	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE('SQL error happend in showPHV.');
		RESIGNAL;
	END;


	-- TSV 의 선택된 하나의 row(=>in_tsvId) 에 대한 부가 정보를 추출
	CALL _extractInfoFromTSVRow(in_tsvId, in_isFromDynamic, l_day, l_ccsId, l_pdvId, l_batchName);
	
	IF @verbose THEN
		SELECT CONCAT('day=', l_day, ', ccsId=', l_ccsId, ', pdvId=', l_pdvId,
			', batch=', IFNULL(l_batchName, 'NULL'), ', dynamic=', in_isFromDynamic);
		
		-- TSV 의 row 에 대한 measure 정보 display
		SELECT 'VERBOSE: Measurement summary..';

		SELECT *
		FROM measure
		WHERE day = l_day
			AND ccsId = l_ccsId
			AND pdvId = l_pdvId
			AND (l_batchName IS NULL OR batchName=l_batchName)
			AND type = 'NM'			-- (=normal measure)only ????
		;
	END IF;

	CALL showStepHistoryView(l_day, l_day, l_ccsId, l_pdvId, in_stepId, l_batchName);
END$$

/*
 * SOV : {startDay, endDay} 가 주어졌을 때 해당하는 sum Good/NG Overview 를 추출
  */
CREATE DEFINER=`stored_program`@`%` PROCEDURE `showSOV`(
	in_startDay DATE
	, in_endDay DATE
)
    COMMENT 'Shows total sum Good/NG overview'
BEGIN
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE('SQL error happend in showTotalSummaryView.');
		RESIGNAL;
	END;
    

SELECT 
    *
FROM
    sumOverview_vw
WHERE
    day BETWEEN in_startDay AND in_endDay;

END$$


CREATE  PROCEDURE `showBundle`(in_measureId INT)
    COMMENT 'Show bundles.'
BEGIN
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE('SQL error happend in showBundle.');
		RESIGNAL;
	END;

	SELECT
		b.measureId AS _measureId
        , s.id AS _positionId
		, s.position AS posNr
		, s.step AS Teststep
		, s.modName AS Modname
		, s.min AS Min
		, b.value AS Value
		, s.max AS Max
		, d.name AS Dim
		, b.message
		, getDeviationInfo(min, b.value, max) AS info
		, s.parameter
	FROM bundle b
		JOIN step s ON (b.stepId = s.id)
		JOIN dimension d ON(s.dim = d.id)
	WHERE measureId = in_measureId
	ORDER BY stepId
	;
END$$


/*
 * TSV 에서 특정 {hw-sec, 날짜, batch}  를 지정하였을 때,
 * 해당 hw-sec 에서 진행한 모든 검사 결과를 TTime, Cycletime 으로 보여줌
 *
 *  1. measure 에서 조건으로 filtering 
 *  2. Cycle time 을 구하기 위해서는 measure table 내에서 인접 row 끼리의 비교가 필요함
 *		- 인접 row 를 gurantee 하기 위해서 강제로 순차적 row id 생성해서 temporary table 에 저장
 *  3. Temporary table 이용해서 self join
 *		- 하나의 query 문에서는 동일한 temporary table 을 두번 사용할 수 없으므로 temporary table 을 복제
 *		- SELF JOIN 을 이용해서 cycle time 구하기
 */
CREATE PROCEDURE showTestAndCycleTimes(
	in_startDay DATE
	, in_endDay DATE
	, in_ccsId INT
	, in_pdvId INT
	, in_batchName VARCHAR(16)  -- if NULL, no batch filter
)
	COMMENT	'Show test and cycle times.'
BEGIN
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE('SQL error happend in showTestAndCycleTimes.');
		RESIGNAL;
	END;

	SET @row = 0;
	DROP TEMPORARY TABLE IF EXISTS tt_measureTable1;
	DROP TEMPORARY TABLE IF EXISTS tt_measureTable2;

	CREATE TEMPORARY TABLE tt_measureTable1 ENGINE=MEMORY
	AS
		SELECT *, @row := @row + 1 AS row
		FROM measure
		WHERE ccsId = in_ccsId
			AND pdvId = in_pdvId
			AND (in_batchName IS NULL OR batchName = in_batchName)
			AND day BETWEEN in_startDay AND in_endDay
			AND type = 'NM'     -- NM(=normal measure) only ????
	;

	-- temporary table 은 하나의 query 에서 두번이상 조회 안됨.  ERROR 1137: Can't reopen table
	CREATE TEMPORARY TABLE tt_measureTable2 ENGINE=MEMORY
	AS
		SELECT * FROM tt_measureTable1
	;

	SELECT
		m1.id, m1.day, m1.time
		, ADDTIME(m1.time, SEC_TO_TIME(m1.duration)) AS 'Completion of Test'
		, m1.ccsId, m1.pdvId
		, m1.duration AS TTime
		, IF(m1.row = 1, NULL, 
			getSecondsFromTime(
				SUBTIME(
					ADDTIME(m1.time, SEC_TO_TIME(m1.duration))
					, ADDTIME(m2.time, SEC_TO_TIME(m2.duration)))) ) AS Cycletime
		-- , m1.row m1row, m2.row m2row
		-- , m2.time AS m2start
		-- , m2.duration as m2duration
	FROM
		tt_measureTable1 m1
		, tt_measureTable2 m2
	WHERE m1.row = m2.row + 1 OR (m1.row = 1 AND m2.row = 1)
	;


END$$



/*
 * TSV 에서 특정 {hw-sec, 날짜, batch}  를 지정하였을 때,
 * Re-test 결과를 보여줌.
 *
 *  1. measure 에서 조건으로 filtering 
 *	2. measure 에서 중복된 ecuid 를 갖는 항목을 검사한다.
 */
CREATE PROCEDURE showRT(in_tsvId INT, in_isFromDynamic BOOLEAN)
	COMMENT	'Show re-test.'
BEGIN
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE('SQL error happend in showRT.');
		RESIGNAL;
	END;

	CALL _routeTSVRowIntoProgram('showRetest', in_tsvId, in_isFromDynamic);
END$$








/*
 * TSV 에서 특정 row {= ccsid, pdvid, 날짜}  를 지정하였을 때, 
 * Re-test 결과를 보여줌.
 */
CREATE PROCEDURE showRetest(
	in_startDay DATE
	, in_endDay DATE
	, in_ccsId INT
	, in_pdvId INT
	, in_batchName VARCHAR(16)	-- if NULL, no batch filter
)
	COMMENT 'Shows section test summary on fixed {hw-sec, date, batch}.'
BEGIN
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE('SQL error happend in showRetest.');
		RESIGNAL;
	END;


	SELECT
		-- in_startDay	AS startDay
		-- , in_endDay	AS endDay
		in_ccsId		AS _ccsId
		, ecuid
		, count(ecuid)	AS Total
		, DATE(splitString(GROUP_CONCAT(day SEPARATOR '|'), '|', 1)) AS day
	FROM measure m
	WHERE ccsId = in_ccsId
		AND pdvId = in_pdvId
		AND (in_batchName IS NULL OR batchName = in_batchName)
		AND day BETWEEN in_startDay AND in_endDay
		AND type = 'NM'     -- NM(=normal measure) only ????
	GROUP BY ecuid
	HAVING count(ecuid) > 1
	;

END$$

/*
 * 특정 tsv ID기준으로  차트에 표현하기 위한 position 정보를 추출 한다
 */
CREATE PROCEDURE `showMeasureInfo`(in_tsvId INT, in_isFromDynamic BOOL, in_stepId INT)
    COMMENT 'Show measure infomation.'
BEGIN
	DECLARE l_day DATE;
	DECLARE l_ccsId INT;
	DECLARE l_pdvId INT;
	DECLARE l_batchName VARCHAR(16);
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE('SQL error happend in showMeasureInfo.');
		RESIGNAL;
	END;


	CALL _extractInfoFromTSVRow(in_tsvId, in_isFromDynamic, l_day, l_ccsId, l_pdvId, l_batchName);
        
	SELECT  
		CAST(ADDTIME(day, time) AS DATETIME) AS startDateTime
		-- day
		-- , time
		, host
		, partNumber
		, position
		, step
		, modName
		, min
		, max
		, d.name			AS dim
	FROM
		measure m
		JOIN step s			ON (s.id = in_stepId)
		JOIN dimension d	ON (s.dim = d.id)
		JOIN pdv p			ON (p.id = l_pdvId)
		JOIN ccs c			ON (c.id = l_ccsId)
	LIMIT 1
	;
END$$




DELIMITER ;

