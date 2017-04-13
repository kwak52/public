USE kefico;


DROP PROCEDURE IF EXISTS obsolete_showStepSummaryView;
DROP PROCEDURE IF EXISTS obsolete_showStepErrorView;

DELIMITER $$

/*
 * TSV 에서 특정 row {= ccsid, pdvid, 날짜}  를 지정하였을 때, 
 * 해당 hw-sec 에서 진행한 모든 검사 결과를 position 별로 summary 해서 보여줌.
 *  1. measure 에서 조건으로 filtering 하여 measure id 확보
 *  2. bundle 에서 해당 measure id 에 해당하는 bundle filtering
 *	3. step 정보와 join 해서 min/max 참조
 * 	4. NG 정보 생성(measure 당 주어진 step 의 NG)
 * 	5. summary 생성해서 결과 출력
 */
CREATE PROCEDURE obsolete_showStepSummaryView(
	in_startDay DATE
	, in_endDay DATE
	, in_ccsId INT
	, in_pdvId INT
	, in_batchName VARCHAR(16)	-- if NULL, no batch filter
)
	COMMENT 'Shows section position summary on fixed {hw-sec, date, batch}.'
BEGIN
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE('SQL error happend in showStepSummaryView.');
		RESIGNAL;
	END;

	-- bundle 에서 measure id 로 filter
	-- SELECT day, measureId, stepId, value, message
	SELECT
		stepId AS _stepId
		, s.position, s.step, s.modName, s.min, j.Average, s.max
		, d.name AS Dim
		, j.Total, j.NG
		, getSigmaRange(s.min, s.max, j.Average, j.Std) AS cpk
	FROM (
		SELECT
			f.stepId
			, AVG(f.value) AS Average
            , STDDEV_SAMP(f.value) AS Std
			, count(*) AS Total
			, SUM(f.NG) AS NG
			, SUM(f.value) AS valueSum
		FROM (
			SELECT
				bm.stepId
				, bm.value
				, NOT bm.ok AS NG	-- 4. measure 당 주어진 step 의 NG
			FROM (
				SELECT stepId, value, ok
				FROM bundle
				WHERE measureId
				IN ( -- 2. bundle 에서 해당 measure id 에 해당하는 bundle filtering
					-- 1. 입력 조건에 부합하는 measure id 찾기
					SELECT id
					FROM measure
					WHERE ccsId = in_ccsId
						AND pdvId = in_pdvId
						AND (in_batchName IS NULL OR batchName=in_batchName)
						AND (day BETWEEN in_startDay AND in_endDay)
						AND type = 'NM'		-- NM(=normal measure) only ????
				)
			) AS bm		-- Bundle filtered with Measure
		) AS f			-- filtered
		GROUP BY stepId
	) AS j				-- joinable
		JOIN step s ON(j.stepId = s.id)	-- 3. step join 을 통해 min/max 획득
		JOIN dimension d ON(s.dim = d.id)
	ORDER BY stepId
	;
END$$






/*
 * TSV 에서 특정 row {= ccsid, pdvid, 날짜}  를 지정하였을 때, 
 * 해당 hw-sec 에서 진행한 모든 검사 중, error 를 포함하는 position 만을 summary 해서 보여줌.
 * 
 *  1. measure 에서 조건으로 filtering 하여 measure id 확보
 *  2. bundle 에서 해당 measure id, NG 에 해당하는 bundle filtering
 *		-> 성능상의 문제로 bundle 에서 NG, OK 결과를 가져야 할 듯.
 *	3. step 정보와 join 해서 min/max 참조
 * 	4. NG 정보 생성(measure 당 주어진 step 의 NG)
 * 	5. summary 생성해서 결과 출력
 */
CREATE PROCEDURE obsolete_showStepErrorView(
	in_startDay DATE
	, in_endDay DATE
	, in_ccsId INT
	, in_pdvId INT
	, in_batchName VARCHAR(16)	-- if NULL, no batch filter
)
	COMMENT 'Shows sectional position error summary on fixed {hw-sec, date, batch}.'
BEGIN
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE('SQL error happend in showStepErrorView.');
		RESIGNAL;
	END;

	-- bundle 에서 measure id 로 filter
	-- SELECT day, measureId, stepId, value, message
	SELECT
		stepId AS _stepId
		, s.position, s.step, s.modName, s.min, j.Average, s.max
		, d.name AS Dim
		, j.Total, j.NG
		, getSigmaRange(s.min, s.max, j.Average, j.Std) AS cpk
	FROM (
		SELECT
			f.stepId
			, AVG(f.value) AS Average
            , STDDEV_SAMP(f.value) AS Std
			, count(*) AS Total
			, SUM(f.NG) AS NG
			, SUM(f.value) AS valueSum
		FROM (
			SELECT
				stepId
				, value
				, NOT ok AS NG	-- 4. measure 당 주어진 step 의 NG
			FROM bundle
			WHERE ok = 0	-- showStepSummaryView 와 유일하게 다른 부분.  WHERE NOT ok 로 사용하면 index 를 이용할 수 없다.
				AND measureId
				IN ( -- 2. bundle 에서 해당 measure id 에 해당하는 bundle filtering
					-- 1. 입력 조건에 부합하는 measure id 찾기
					SELECT id
					FROM measure
					WHERE ccsId = in_ccsId
						AND pdvId = in_pdvId
						AND (in_batchName IS NULL OR batchName=in_batchName)
						AND (day BETWEEN in_startDay AND in_endDay)
						AND type = 'NM'		-- NM(=normal measure) only ????
				)
		) AS f		-- Bundle filtered with Measure
		GROUP BY stepId
	) AS j				-- joinable
		JOIN step s ON(j.stepId = s.id)	-- 3. step join 을 통해 min/max 획득
		JOIN dimension d ON(s.dim = d.id)
	ORDER BY stepId
	;
END$$



DELIMITER ;
