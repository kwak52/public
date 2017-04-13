USE kefico

/*
 * View 작성시, ORDER BY 구문이나 GROUP BY 구문을 사용하게되면, 더 이상 relational set 이 아니므로
 * 해당 view 를 포함하는 query 의 optimize 가 어려워진다.   The Art of SQL.pdf
 */

/*
DROP VIEW IF EXISTS stepFinal_vw;
DROP VIEW IF EXISTS measure_vw;
DROP VIEW IF EXISTS storedProcedure_vw;
DROP VIEW IF EXISTS dynamicTopSummary_vw;
DROP VIEW IF EXISTS topSummary_vw;
*/
DROP PROCEDURE IF EXISTS registerViewInfo;


DELIMITER $$
CREATE PROCEDURE registerViewInfo(
    in_viewName VARCHAR(255)
    , in_comment VARCHAR(1023)
)
	COMMENT 'Registers our own view information, because MySQL does not support view comment.'
BEGIN
	INSERT INTO viewInfo(name, comment)
	VALUES(in_viewName, in_comment)
	ON DUPLICATE KEY
		UPDATE comment = in_comment;
END$$
DELIMITER ;


CALL registerViewInfo('storedProcedure_vw', 'Shows stored procedures');
CREATE OR REPLACE VIEW storedProcedure_vw AS
	SELECT
		specific_name
		, type
		-- , param_list
		, returns
		, comment
	FROM mysql.proc
	WHERE db='kefico'
	-- ORDER BY type, specific_name	-- NOT relational.
;


/*
 * step 정보 중에서 revision 이 가장 최신인 것들만 골라서 보여준다.
 */
CALL registerViewInfo('stepFinal_vw', 'Shows steps with most recent revision number.');
CREATE OR REPLACE VIEW stepFinal_vw AS
	SELECT s.*
		, d.name AS dimName
		, f.name AS funcName
	FROM step s
	LEFT OUTER JOIN step s2
		ON s.pdvId = s2.pdvId
			AND s.step = s2.step
			AND s.position = s2.position
			AND s.revision < s2.revision
	JOIN dimension d ON s.dim = d.id
	JOIN function f ON s.fncId = f.id
;
/*
CREATE OR REPLACE VIEW stepFinal_vw AS
	SELECT a.*, d.name AS _dimensionName
	FROM step a
	LEFT OUTER JOIN step b
		ON a.pdvId = b.pdvId
			AND a.step = b.step
			AND a.position = b.position
			AND a.revision < b.revision
	JOIN dimension d ON a.dim = d.id
	JOIN function f ON a.fncId = f.id
	WHERE b.id IS NULL
	;
*/

/*
 * measure 의 ccsId 를 host 와 section 으로 표시
 */
CALL registerViewInfo('measure_vw', 'Shows host and section explicitly, instead of ccsId');
CREATE OR REPLACE VIEW measure_vw AS
	SELECT m.id _id, m.day, m.time, m.duration, m.pdvId, ccs.host, ccs.sec, m.ok, m.batchName, m.type
	FROM measure m
	JOIN ccs ON (m.ccsId = ccs.id)
	-- ORDER BY m.id		-- NOT relational.
;


CALL registerViewInfo('bundle_vw', 'Shows bundle');
CREATE OR REPLACE VIEW bundle_vw AS
    SELECT
        b.measureId AS _measureId
        , s.position AS posNr
        , s.step AS Teststep
        , s.modName AS Modname
        , s.min AS Min
        , b.value AS Value
        , s.max AS Max
        , d.name AS Dim
        , b.message
		, b.value BETWEEN s.min AND s.max AS OK
    FROM bundle b
        JOIN step s ON (b.stepId = s.id)
        JOIN dimension d ON(s.dim = d.id)
    -- ORDER BY stepId	-- NOT relational.
;


 
/*
 * TSV 중에서 dynamic 만 보여준다.
 */
CALL registerViewInfo('dynamicTopSummary_vw', 'Shows TSV info from dynamic summary table');
CREATE OR REPLACE VIEW dynamicTopSummary_vw AS
	SELECT
		ds.id -- 삭제시 사용하기 위해서..
		, _batchName
		, ccsId AS _ccsId
		, pdvId AS _pdvId
		, testListId AS _testListId
		, True As _isFromDynamic
		, day
		, ccs.host, ccs.sec
		, pdvTestList.product
		, partNumber AS partNr
		, startTime, endTime
		, duration_gc AS duration
		, total
		, ngCount AS NG
		, percentGood_gc AS '%Good'
		, percentGood100_gc AS '%Good100'
		, lastECUs_gc AS LastECUs	-- Shows only the last 12 results
	FROM dynamicTopSummary ds
	JOIN ccs ON (ccsId = ccs.id)
	JOIN pdv ON (pdvId = pdv.id)
	JOIN pdvTestList ON (testListId = pdvTestList.id)
	
	
;


/*
 * TSV(Top Summary View, 시작시 최초화면) 을 보여준다.
 */
CALL registerViewInfo('topSummary_vw', 'Shows TSV(Top Summary View, the first view on MWS).');
CREATE OR REPLACE VIEW topSummary_vw AS
	SELECT *
		FROM dynamicTopSummary_vw
	UNION
		SELECT ss.id
			, _batchName
			, ccsId AS _ccsId
			, pdvId AS _pdvId
			, testListId AS _testListId
			, False As _isFromDynamic
			, day
			, ccs.host, ccs.sec
			, pdvTestList.product
			, partNumber AS partNr
			, startTime, endTime
			, durationAvg AS duration
			, total
			, ngCount AS NG
			, percentageGood AS '%Good'
			, percentageGood100 AS '%Good100'
			, LastECUs
		FROM staticTopSummary ss
		JOIN ccs ON (ccsId = ccs.id)
		JOIN pdv ON (pdvId = pdv.id)
		JOIN pdvTestList ON (testListId = pdvTestList.id)
	-- ORDER BY day, id		-- NOT relational.
;

CALL registerViewInfo('topSummaryToday_vw', 'Shows today\'s TSV(Top Summary View).');
CREATE OR REPLACE VIEW topSummaryToday_vw AS
	SELECT *
		FROM dynamicTopSummary_vw
		WHERE day=CURDATE()
	UNION
		SELECT ss.id
			, _batchName
			, ccsId AS _ccsId
			, pdvId AS _pdvId
			, testListId AS _testListId
			, False As _isFromDynamic
			, day
			, ccs.host, ccs.sec
			, pdvTestList.product
			, partNumber AS partNr
			, startTime, endTime
			, durationAvg AS duration
			, total
			, ngCount AS NG
			, percentageGood AS '%Good'
			, percentageGood100 AS '%Good100'
			, LastECUs
		FROM staticTopSummary ss
		JOIN ccs ON (ccsId = ccs.id)
		JOIN pdv ON (pdvId = pdv.id)
		JOIN pdvTestList ON (testListId = pdvTestList.id)
		WHERE day=CURDATE()
	ORDER BY day, id		-- NOT relational.  see The Art of SQL.pdf
;


CALL registerViewInfo('topSummaryQuick_vw', 'Shows quick TSV(Top Summary View).');
CREATE OR REPLACE VIEW topSummaryQuick_vw AS
	SELECT t1.*
		FROM topSummary_vw t1
		LEFT OUTER JOIN topSummary_vw t2
		ON t1._ccsId = t2._ccsId
			AND t1._pdvId = t2._pdvId
			AND t1.day = t2.day
			AND t1.startTime < t2.startTime
		WHERE t2.id IS NULL
;


CALL registerViewInfo('topSummaryTodayQuick_vw', 'Shows today\'s quick TSV(Top Summary View, the first view on MWS).');
CREATE OR REPLACE VIEW topSummaryTodayQuick_vw AS
	SELECT t1.*
		FROM topSummaryToday_vw t1
		LEFT OUTER JOIN topSummaryToday_vw t2
		ON t1._ccsId = t2._ccsId
			AND t1._pdvId = t2._pdvId
			AND t1.startTime < t2.startTime
		WHERE t2.id IS NULL
;

CALL registerViewInfo('sumOverview_vw', 'Sum overview for all period');
CREATE OR REPLACE VIEW sumOverview_vw AS
	SELECT t.*
		, TRUNCATE(IF(total=0, NULL, 100 * (total - ng) / total), 2) AS `%Good`
		, TRUNCATE(IF(total=0, NULL, 100 * ng / total), 2) AS `%Bad`
	FROM (
		SELECT
			day
			, host AS Hosts
			, SUM(total) AS total
			, SUM(NG) As ng
		FROM topSummary_vw
		GROUP BY day, host
	) t
;

/*
 * pdv 의 PDV View 데이터  표시
 */
CALL registerViewInfo('pdv_vw', 'Shows pdv View field');
CREATE OR REPLACE VIEW pdv_vw AS
	SELECT 
		p.id AS _id
		, CONCAT(pdvGroup.productGroup, ' ', pdvGroup.productModel)  as groupEntry
		, p.partNumber
		, p.pamType
		, p.pamGroup
		, CONCAT(pdvTestList.productNumber, ' ',pdvTestList.product, ' ', pdvTestList.productType ,' (', pdvTestList.version, ')')  as testList
		, p.dataConfig
		, p.dataVariant
		, p.changeNumber
		, p.createdDt
		, user.username
		, p.comment
	FROM pdv p
		JOIN pdvGroup ON (pdvGroup.id = p.groupId)
		JOIN pdvTestList ON (pdvTestList.id = p.testListId)
		JOIN user ON (user.id = p.userId)
;

CALL registerViewInfo('pdvt_vw', 'Shows pdv/pdvTestList View field');
CREATE OR REPLACE VIEW pdvj_vw AS
	SELECT 
		p.id AS pdvId
		, p.partNumber
		, p.pamType
		, p.isProduction
		, pdvTestList.id AS testListId
		, pdvTestList.productNumber
		, CONCAT(pdvTestList.product, pdvTestList.productType) AS productType
		, pdvTestList.version
		, pdvTestList.fileStem
		, pdvTestList.testListPathHint_gc AS pathHint
	FROM pdv p
		JOIN pdvTestList ON (pdvTestList.id = p.testListId)
;
