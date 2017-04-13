USE kefico;

DROP PROCEDURE IF EXISTS showSPSV;
DROP PROCEDURE IF EXISTS showSPEV;
DROP PROCEDURE IF EXISTS showSPEDV;

DROP PROCEDURE IF EXISTS _createTemporaryStepSummaryTable;
DROP PROCEDURE IF EXISTS _showStepSummaryViewHelper;
DROP PROCEDURE IF EXISTS showStepSummaryView;
DROP PROCEDURE IF EXISTS showStepErrorView;
DROP PROCEDURE IF EXISTS showStepErrorDetailView;
DROP PROCEDURE IF EXISTS showStepHistoryView;
DROP PROCEDURE IF EXISTS showStepChangeHistory;
DROP PROCEDURE IF EXISTS showDeletedSteps;




DELIMITER $$


CREATE PROCEDURE showSPSV(in_tsvId INT, in_isFromDynamic BOOLEAN)
	COMMENT 'Shows SSV helper.'
BEGIN
	CALL _routeTSVRowIntoProgram('showStepSummaryView', in_tsvId, in_isFromDynamic);
END$$


CREATE PROCEDURE showSPEV(in_tsvId INT, in_isFromDynamic BOOLEAN)
	COMMENT 'Shows Sectional position error view.  Bosch\'s Measurement list(Grouped)'
BEGIN
	CALL _routeTSVRowIntoProgram('showStepErrorView', in_tsvId, in_isFromDynamic);
END$$

CREATE PROCEDURE showSPEDV(in_tsvId INT, in_isFromDynamic BOOLEAN)
	COMMENT 'Shows Sectional position error view.  Bosch\'s Measurement list(Grouped)'
BEGIN
	CALL _routeTSVRowIntoProgram('showStepErrorDetailView', in_tsvId, in_isFromDynamic);
END$$


CREATE PROCEDURE _createTemporaryStepSummaryTable(
	in_startDay DATE
	, in_endDay DATE
	, in_ccsId INT		-- Nullable
	, in_pdvId INT		-- NOT NULL
)
	COMMENT 'Create temporary table tt_stepSummary for step summary'
BEGIN
	DECLARE l_today DATE DEFAULT CURDATE();
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE('SQL error happend in _createTemporaryStepSummaryTable.');
		RESIGNAL;
	END;

	-- CREATE TEMPORARY TABLE tt_stepSummary LIKE staticDailyStepSummary;
	DROP TEMPORARY TABLE IF EXISTS tt_stepSummary;

	CREATE TEMPORARY TABLE tt_stepSummary ENGINE=MEMORY
	AS SELECT * FROM staticDailyStepSummary LIMIT 0;

	ALTER TABLE tt_stepSummary
		DROP column id
    ;

	INSERT INTO tt_stepSummary(day, ccsId, pdvId, stepId, total, ngCount, valSum, valSqSum)
	SELECT day, ccsId, pdvId, stepId, total, ngCount, valSum, valSqSum
	FROM staticDailyStepSummary
	WHERE day <> l_today
		AND day BETWEEN in_startDay AND in_endDay
		AND pdvId = in_pdvId
		AND (ccsId = in_ccsId OR in_ccsId IS NULL)
	;

	--
	-- 오늘 data 는 StepSummary 에 포함되어 있지 않으므로, raw data 로 부터 계산한다.
	--
	IF l_today BETWEEN in_startDay AND in_endDay THEN
		INSERT INTO tt_stepSummary(day, ccsId, pdvId, stepId, total, ngCount, valSum, valSqSum)
		SELECT
			l_today				AS day
			, in_ccsId			AS ccsId
			, in_pdvId			AS pdvId
			, b.stepId			AS stepId
			, b.Total
			, b.ngCount
			, b.valSum
			, b.valSqSum
		FROM (
			SELECT
				stepId
				, count(ok)				AS Total
				, SUM(NOT ok)			AS ngCount
				, SUM(value)			AS valSum
				, SUM(POW(value, 2))	AS valSqSum
			FROM bundle
			WHERE
				/* STR 등과 같은 message 항목도 포함하려면 아래의 value not null 조건을 comment 처리 한다. */
				/* Step Summary 에서는 value 값을 가지지 않는 항목은 display 하지 않음. */
				value IS NOT NULL
				AND 
					measureId IN (
					SELECT id FROM measure
					WHERE day = l_today
						AND pdvId = in_pdvId
						AND (ccsId = in_ccsId OR in_ccsId IS NULL)
						AND type = 'NM'     -- NM(=normal measure) only ????
				)
			GROUP BY stepId
		) b
		;
	END IF;
END$$



/*
 * TSV 에서 특정 row {= ccsid, pdvid, 날짜}  를 지정하였을 때, 
 * 해당 hw-sec 에서 진행한 모든 검사 결과를 position 별로 summary 해서 보여줌.
 *  1. measure 에서 조건으로 filtering 하여 measure id 확보
 *  2. bundle 에서 해당 measure id 에 해당하는 bundle filtering
 *	3. step 정보와 join 해서 min/max 참조
 * 	4. NG 정보 생성(measure 당 주어진 step 의 NG)
 * 	5. summary 생성해서 결과 출력
 */
CREATE PROCEDURE _showStepSummaryViewHelper(
	in_startDay DATE
	, in_endDay DATE
	, in_ccsId INT
	, in_pdvId INT
	, in_onlyNG BOOLEAN
)
	COMMENT 'Shows section position summary on fixed {hw-sec, date, batch}.'
BEGIN
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE('SQL error happend in _showStepSummaryViewHelper.');
		RESIGNAL;
	END;

	CALL addLogD(CONCAT('_createTemporaryStepSummaryTable ', in_startDay, ', ', in_endDay, ', ', in_ccsId, ', ', in_pdvId));
	CALL _createTemporaryStepSummaryTable(in_startDay, in_endDay, in_ccsId, in_pdvId);
	
	SELECT
		st.id					AS _positionId
		, st.position
		, st.step
		, st.min						AS Min
		, TRUNCATE(ss.Average, 2)		AS Average
		, st.max						AS Max
		, ss.Total
		, ss.NG
		, TRUNCATE(getSigmaRange (st.min, st.max,  ss.Average, ss.ValSqSum,  ss.Total ), 2) AS CPK
		, d.name						AS Dim
	FROM (
		SELECT 
			stepId						AS stepId
			, SUM(valSum)/SUM(total)	AS Average
			, SUM(total)				AS Total
			, SUM(ngCount)				AS NG
			, SUM(valSqSum)				AS ValSqSum
		FROM tt_stepSummary
		GROUP BY stepId
	) ss
		JOIN step st ON ss.stepId = st.id
		JOIN dimension d ON st.dim = d.id
	WHERE NOT in_onlyNG OR ss.NG > 0
	;
END$$

CREATE PROCEDURE showStepSummaryView(
	in_startDay DATE
	, in_endDay DATE
	, in_ccsId INT
	, in_pdvId INT
	, in_batchName VARCHAR(16)	-- NOT USED!!! if NULL, no batch filter
)
	COMMENT 'Shows section position summary on fixed {hw-sec, date, batch}.'
BEGIN
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE('SQL error happend in showStepSummaryView.');
		RESIGNAL;
	END;

	CALL _showStepSummaryViewHelper(in_startDay, in_endDay, in_ccsId, in_pdvId, False);
END$$


CREATE PROCEDURE showStepErrorView(
	in_startDay DATE
	, in_endDay DATE
	, in_ccsId INT
	, in_pdvId INT
	, in_batchName VARCHAR(16)	-- NOT USED!! if NULL, no batch filter
)
	COMMENT 'Shows section position summary on fixed {hw-sec, date, batch}.'
BEGIN
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE('SQL error happend in showStepErrorView.');
		RESIGNAL;
	END;
	CALL _showStepSummaryViewHelper(in_startDay, in_endDay, in_ccsId, in_pdvId, True);
END$$



/*
 * TSV 에서 특정 row {= ccsid, pdvid, 날짜}  를 지정하였을 때, 
 * 해당 hw-sec 에서 진행한 모든 검사 중, error 를 포함하는 position 을 보여줌.
 * 
 *  1. measure 에서 조건으로 filtering 하여 measure id 확보
 *  2. bundle 에서 해당 measure id, NG 에 해당하는 bundle filtering
 *		-> 성능상의 문제로 bundle 에서 NG, OK 결과를 가지도록 함.
 *	3. step 정보와 join 해서 min/max 참조
 * 	4. NG 정보 생성(measure 당 주어진 step 의 NG)
 * 	5. summary 생성해서 결과 출력
 */
CREATE PROCEDURE showStepErrorDetailView(
	in_startDay DATE
	, in_endDay DATE
	, in_ccsId INT		-- Nullable
	, in_pdvId INT		-- NOT NULL
	, in_batchName VARCHAR(16)	-- if NULL, no batch filter
)
	COMMENT 'Shows sectional position error detail on fixed {hw-sec, date, batch}.'
BEGIN
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE('SQL error happend in showStepErrorDetailView.');
		RESIGNAL;
	END;

	-- bundle 에서 measure id 로 filter
	-- SELECT day, measureId, stepId, value, message
	SELECT
		stepId AS _stepId
		, m.day			AS DAY
		, m.fixture
		, m.time		AS TIME
		, m.ecuid
		, position
		, step
		, modName
		, min
		, value
		, max
		, d.name		AS Dim
		, measureId		AS _measureId
	FROM bundle b
		JOIN step s			ON(b.stepId = s.id)	-- 3. step join 을 통해 min/max 획득 및 NG 판정
		JOIN measure m		ON(b.measureId = m.id)
		JOIN dimension d	ON(s.dim = d.id)
	WHERE b.ok = 0
		AND measureId
		IN ( -- 2. bundle 에서 해당 measure id 에 해당하는 bundle filtering
			-- 1. 입력 조건에 부합하는 measure id 찾기
			SELECT id
			FROM measure
			WHERE (in_ccsId IS NULL OR ccsId = in_ccsId)
				AND pdvId = in_pdvId
				AND (in_batchName IS NULL OR batchName=in_batchName)
				AND (day BETWEEN in_startDay AND in_endDay)
				AND type = 'NM'
		)
	ORDER BY s.step, day, _measureId
	;
END$$




-- CALL showStepHistoryView( '2016-08-07', '2016-08-010', l_ccsId, 'BATCH-11-7', 20);

/*
 * PHV : {position, CCS-sec, batch} 가 주어졌을 때, measure value 를 표시
 *  1. measure 에서 조건으로 filtering 하여 measure id 확보
 *  2. bundle 에서 해당 measure id 에 해당하는 bundle filtering
 *	3. step 정보와 join 해서 min/max 참조
 */
CREATE PROCEDURE showStepHistoryView(
	in_startDay DATE
	, in_endDay DATE
	, in_ccsId INT
	, in_pdvId INT
	, in_stepId INT
	, in_batchName VARCHAR(16)	-- if NULL, no batch filter
)
	COMMENT 'Shows measured value on fixed {position, CCS-sec, batch}'
BEGIN
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE('SQL error happend in showStepHistoryView.');
		RESIGNAL;
	END;

CALL addLogD(CONCAT('showStepHistoryView: step=', IFNULL(in_stepId, 'NULL')));
	SELECT 
		m.day			AS _startDay
		, m.time
		, s.position
		, s.id			AS _positionId
		-- , s.min
		, b.value
		-- , s.max
		-- , s.modName
		, NOT b.ok		AS NG
		, b.measureId	AS _measureId
		, d.name		AS Dim
	FROM bundle b
		LEFT JOIN measure m ON (b.measureId = m.id)
		LEFT JOIN step s ON (b.stepId = s.id)
		JOIN dimension d ON (s.dim = d.id)
	WHERE b.stepId = in_stepId
		AND b.measureId
		IN ( -- 2. bundle 에서 해당 measure id 에 해당하는 bundle filtering
			-- 1. 입력 조건에 부합하는 measure id 찾기
			SELECT id
			FROM measure
			WHERE ccsId=in_ccsId
				AND pdvId = in_pdvId
				AND (in_batchName IS NULL OR batchName=in_batchName)
				AND (day BETWEEN in_startDay AND in_endDay)
				AND type = 'NM'			-- (=normal measure)only ????
				-- AND NOT duration IS NULL		
		)
	;
END$$



DELIMITER ;

