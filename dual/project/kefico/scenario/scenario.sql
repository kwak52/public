USE kefico;


SET @quickViewMode = True;

SELECT id, _ccsId, _pdvId, _isFromDynamic, `day`
INTO @tsvId, @ccsId, @pdvId, @fromDynamic, @day
FROM topSummary_vw
ORDER BY `day` DESC
LIMIT 1
;

SELECT id, position, step
INTO @stepId, @position, @step
FROM stepFinal_vw
WHERE pdvId = @pdvId
LIMIT 33, 1;

-- Total Summary View 에서 사용할 row 정보 추출
CALL _extractInfoFromTSVRow(@tsvId, @fromDynamic, @dayV, @ccsIdV, @pdvIdV, @batchNameV);

SELECT id
INTO @measureId
FROM measure
WHERE ccsId = @ccsId
  AND pdvId = @pdvId
  AND type = 'NM'
LIMIT 1
;


-- show variables
SELECT @tsvId, @ccsId, @pdvId, @fromDynamic, @day
	, @stepId, @position, @step
	, @dayV, @ccsIdV, @pdvIdV, @batchNameV, @measureId;

SELECT
	@measureId, @stepId
  , @ccsId = @ccsIdV  AS EqCCS
  , @pdvId = @pdvIdV  AS EqPDV
  , @day = @dayV      AS EqDay
  ;



-- Top summary view 
SELECT * FROM topSummary_vw;
-- Sum overview
SELECT * FROM sumOverview_vw;

-- Positional error detail view
CALL showSPEDV(@tsvId, @fromDynamic);
-- Positional error view
CALL showSPEV(@tsvId, @fromDynamic);
-- Positional summary view
CALL showSPSV(@tsvId, @fromDynamic);
-- Top summary view, by measure
CALL showSTSV(@tsvId, @fromDynamic);
-- Show positional history view
CALL showPHV(@tsvId, @fromDynamic, @stepId);
-- Show total cycle time
CALL showTCT(@tsvId, @fromDynamic);
-- Show measurement
CALL showBundle(@measureId);
-- Show re-test
CALL showRT(@tsvId, @fromDynamic);


CALL showTotalSummaryView('2016-08-07', CURDATE(), @quickViewMode);
CALL showStepSummaryView('2016-08-07', CURDATE(), @ccsId, @pdvId, NULL);
CALL showStepErrorView('2016-08-07', CURDATE(), @ccsId, @pdvId, NULL);
CALL showStepErrorDetailView('2016-08-07', CURDATE(), @ccsId, @pdvId, NULL);
CALL showStepErrorDetailView('2016-08-07', CURDATE(), NULL, @pdvId, NULL);

CALL showTestSummary('2016-08-07', CURDATE(), @ccsId, @pdvId, NULL);
CALL showTestSummary('2016-08-07', CURDATE(), @ccsId, NULL, NULL);    -- On fixed CCS
CALL showTestSummary('2016-08-07', CURDATE(), NULL, @pdvId, NULL);    -- On fixed PDV

CALL showTestSummary('2016-08-07', CURDATE(), NULL, NULL, NULL);      -- FOR ALL ccs/pdv, create temporary table tt_measureSummary
SELECT * FROM tt_measureSummary WHERE type = 'PN';    -- POWER ON for all ccs/pdv
SELECT * FROM tt_measureSummary WHERE NOT ok;    -- ALL result which contains NG

CALL showTestAndCycleTimes('2016-08-07', CURDATE(), @ccsId, @pdvId, NULL);
CALL showRetest('2016-08-07', CURDATE(), @ccsId, @pdvId, NULL);

CALL showPartition('bundle');
CALL showAll();



SELECT id, position, step
INTO @updateStepId, @updatePosition, @updateStep
FROM step
WHERE pdvId = @pdvId
LIMIT 11, 1;

SELECT @updateStepId, @updatePosition, @updateStep;

