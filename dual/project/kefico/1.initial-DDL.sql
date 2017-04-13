USE kefico;

/*
 * dynamicTopSummary 에 generated column 생성 : mysql workbench 에서 아직 해당 기능을 제공하지 않는 듯하다.
 * - 초기 버젼은 dynamicTopSummary_vw 에 column 을 추가 했었다.
 */
ALTER TABLE dynamicTopSummary
	ADD COLUMN duration_gc DECIMAL(18, 2)
		AS (IF(total=0, NULL, durationSum/total))
	, ADD COLUMN percentGood_gc DECIMAL(18, 2)
		AS (IF(total=0, NULL, 100 * (total - ngCount)/total))
	, ADD COLUMN percentGood100_gc DECIMAL(18, 2)
		AS (IF(total=0, NULL, 100 * ((BIT_COUNT(lastEcu100First) + BIT_COUNT(lastEcu100Last)) / LEAST(100, total))))
	, ADD COLUMN lastECUs_gc CHAR(12)
		AS (IF(total=0, NULL, BIN(lastEcu100Last & CONV('FFF', 16, 10))))
;

/*ALTER TABLE pdv ADD UNIQUE unique_index(partNumber, `version`, pamType, revision);*/



ALTER TABLE pdvTestList
	ADD COLUMN testListPathHint_gc VARCHAR(256)
		AS (CONCAT('pruef_cp/testList/', product, productType))
;
