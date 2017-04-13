USE kefico;

DROP PROCEDURE IF EXISTS createTemporaryStepTable;
DROP PROCEDURE IF EXISTS mergeStepFromTemporaryTable;

/*
 * Step parsing 을 위한 flow 및 temporary table 들
 *

 1. CCS actor 에서 gaudi file 위치를 MWS server actor 에게 전송
 1. MWS server actor 가 
	- gaudi file 을 parsing 하고 
	- sql sesssion 을 통해
		. CALL createTemporaryStepTable() 호출해서 tt_step_parsed 를 생성
		. tt_step_parsed 에 parsing 된 step 정보를 insert
		. CALL mergeStepFromTemporaryTable() 호출을 통해서 tt_step_parsed 와 기존 step 을 merge
			- tt_step_added, tt_step_report 등의 temporary table 들을 생성함
		. tt_step_report table 을 통해서 주어진 gaudi file 에 대한 최종 step 정보(revision 포함)를 획득하고 CCS actor 에게 반환

 - tt_step_parsed	: gaudi file 을 parsing 해서 얻은 step 정보
 - tt_step_report	: 최종적으로 CCS 가 가져가야할 step 정보들
 - tt_step_pdv		: 기존 step 파일에서 pdv id 가 현재 gaudi file 에 해당하는 active step 들만 모은 것(internal)
 - tt_step_added	: 기존 step 대비 새로 추가된 step 들(internal)
 */


DELIMITER $$


--
-- step table 에서 id column 만 auto increment column 으로 변경한 빈 temporary table 을 생성한다.
--
CREATE PROCEDURE createTemporaryStepTable()
	COMMENT 'Create temporary step table for further processing.'
BEGIN
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE('SQL error happend in createTemporaryStepTable.');
		RESIGNAL;
	END;


CALL addLogD('createTemporaryStepTable() called.');
    DROP TEMPORARY TABLE IF EXISTS tt_step_parsed;

    CREATE TEMPORARY TABLE tt_step_parsed
        ENGINE=MEMORY
    AS
        SELECT * FROM step LIMIT 0;

	ALTER TABLE tt_step_parsed 
		CHANGE id id INT(10) UNSIGNED NOT NULL PRIMARY KEY AUTO_INCREMENT
	;

	DELETE FROM tt_step_parsed;
END$$


--
-- Gaudi file 수정에 따른 step 정보 갱신
-- 선행 조건: tt_step_parsed table 이 생성되어 있어야 한다.
--
CREATE PROCEDURE mergeStepFromTemporaryTable(
	in_pdvId INT	-- `id` column of pdv
)
	COMMENT 'Merge steps from temporary step table(tt_step_parsed).'
BEGIN
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE('SQL error happend in mergeStepFromTemporaryTable.');
		RESIGNAL;
	END;


	SELECT val INTO @runLevel FROM preference WHERE name = 'runLevel';

IF @runLevel = 9 THEN
	CALL addLogD(CONCAT('mergeStepFromTemporaryTable() called with pdvId=', in_pdvId));
	SELECT count(*) INTO @totalSteps FROM tt_step_parsed;
	CALL addLogD(CONCAT('	Number of parsed steps =', @totalSteps));
END IF;

	-- 본 session 에서 추가된 step 을 확인하기 위해서 마지막 step id 저장.
	SELECT IFNULL(MAX(id),0) INTO @lastStepId FROM step;

	-- temporary table 은 하나의 query 에서 두번이상 조회 안됨.  ERROR 1137: Can't reopen table
	DROP TEMPORARY TABLE IF EXISTS tt_step_pdv;
	CREATE TEMPORARY TABLE tt_step_pdv ENGINE=MEMORY
	AS
		SELECT * FROM step WHERE pdvId = in_pdvId
	;
IF @runLevel = 9 THEN
	CALL addLogD(CONCAT('	last step id before operation=', @lastStepId));
	CALL copyAsNormalTable('tt_step_pdv', 'ttstep_pdv', True);
	CALL addLogD('	Generating join table..');
END IF;

	-- 기존 step 내용 중 수정된 step 만 추출
	-- id 는 그대로 유지한 채, revision 만 하나 증가 시킴.
	--
	DROP TEMPORARY TABLE IF EXISTS tt_step_natural_join;
	CREATE TEMPORARY TABLE tt_step_natural_join ENGINE=MEMORY
	AS
		SELECT id, pdvId, position, step
			, revision
			, o_min, o_max, o_modName, o_fncId, o_dim, o_mM, o_parameter, o_comment
			, min, max, modName, fncId, dim, mM, parameter, comment
			, _changed
		FROM(
			SELECT t1.id
				, t1.pdvId
				, t1.position
				, t1.step
				, t1.revision
				, t1.min		AS o_min
				, t1.max		AS o_max
				, t1.modName		AS o_modName
				, t1.fncId		AS o_fncId
				, t1.dim		AS o_dim
				, t1.mM			AS o_mM
				, t1.parameter	AS o_parameter
				, t1.comment	AS o_comment
				, t3.min
				, t3.max
				, t3.modName
				, t3.fncId
				, t3.dim
				, t3.mM
				, t3.parameter
				, t3.comment
				, (((t1.min IS NULL) <> (t3.min IS NULL))	OR ((t1.min IS NOT NULL) AND (t1.min <> t3.min)))
					OR (((t1.max IS NULL) <> (t3.max IS NULL))	OR ((t1.max IS NOT NULL) AND (t1.max <> t3.max)))
					OR t1.dim <> t3.dim
					OR (((t1.modName IS NULL) <> (t3.modName IS NULL))	OR ((t1.modName IS NOT NULL) AND (t1.modName <> t3.modName)))
					OR (((t1.parameter IS NULL) <> (t3.parameter IS NULL))	OR ((t1.parameter IS NOT NULL) AND (t1.parameter <> t3.parameter)))
					OR (((t1.comment IS NULL) <> (t3.comment IS NULL))	OR ((t1.comment IS NOT NULL) AND (t1.comment <> t3.comment)))
					OR t1.fncId <> t3.fncId
				  AS _changed	-- 기존 step 대비 변경 여부
			FROM tt_step_pdv t1
			JOIN tt_step_parsed t3 
				ON t1.pdvId = t3.pdvId
					AND t1.step = t3.step
					AND t1.position = t3.position
		) t;

IF @runLevel = 9 THEN
	CALL addLogD('	Generated join table..');
	SELECT count(*) INTO @totalSteps FROM tt_step_natural_join;
	CALL addLogD(CONCAT('	Number of interesting steps =', @totalSteps));
	SELECT count(*) INTO @totalSteps FROM tt_step_natural_join WHERE _changed;
	CALL addLogD(CONCAT('	Number of changed steps =', @totalSteps));

	CALL copyAsNormalTable('tt_step_natural_join', 'ttstep_natural_join', True);
END IF;
	

IF @runLevel = 9 THEN
CALL addLogD(CONCAT('	mergeStepFromTemporaryTable(): upadting.'));
END IF;


	DROP TEMPORARY TABLE IF EXISTS tt_step_added;
	CREATE TEMPORARY TABLE tt_step_added ENGINE=MEMORY
	AS
		SELECT t1.pdvId, t1.position, t1.step, 
			0 AS revision, t1.min, t1.max, t1.modName, t1.fncId, t1.dim, t1.mM, t1.parameter, t1.comment
		FROM tt_step_parsed t1
		LEFT JOIN tt_step_pdv t2
			ON t1.step = t2.step
			AND t1.position = t2.position
		WHERE t2.pdvId IS NULL
	;

IF @runLevel = 9 THEN
	CALL copyAsNormalTable('tt_step_added', 'ttstep_added', True);
END IF;


	-- START TRANSACTION;
CALL addLogD(CONCAT('	mergeStepFromTemporaryTable(): changing.'));
	CALL copyAsTemporaryTable('tt_step_natural_join', 'tt_step_j2', True);

	INSERT INTO step(pdvId, position, step, revision, min, max, modName, fncId, dim, mM, parameter, comment)
	SELECT j1.pdvId, j1.position, j1.step, j1.revision+1, j1.min, j1.max, j1.modName, j1.fncId, j1.dim, j1.mM, j1.parameter, j1.comment
	FROM tt_step_natural_join j1
	LEFT OUTER JOIN tt_step_j2 j2
		ON j1.pdvId = j2.pdvId
		AND j1.step = j2.step
		AND j1.position = j2.position
		AND j1.revision < j2.revision
	WHERE j2.id IS NULL and j1._changed
	;

	-- 이미 변경된 부분을 marking 함 : already added
	DROP TABLE IF EXISTS tt_step_alread_added;
	CREATE TEMPORARY TABLE tt_step_alread_added
	AS
		SELECT step, position
		FROM tt_step_natural_join
		WHERE _changed
	;

		
	DELETE FROM tt_step_natural_join
		WHERE (step, position) IN (SELECT step, position FROM tt_step_alread_added)
	;

		
IF @runLevel = 9 THEN
SELECT count(*) INTO @totalSteps FROM tt_step_added;
CALL addLogD(CONCAT('	Number of added steps =', @totalSteps));
END IF;
	--
	-- New addition
	--
CALL addLogD('	adding added step().');
	IF EXISTS (SELECT NULL FROM tt_step_added) THEN
		INSERT INTO step(pdvId, position, step, revision, min, max, modName, fncId, dim, mM, parameter, comment)
			SELECT *
			FROM tt_step_added
		;
	END IF;


	-- COMMIT;


IF @runLevel = 9 THEN
	CALL addLogD('	Generating reports...');
END IF;
-- reports changed/new steps
	DROP TEMPORARY TABLE IF EXISTS tt_step_report;
	CREATE TEMPORARY TABLE tt_step_report ENGINE=MEMORY
	AS
		SELECT id, pdvId, position, step, revision, min, max, modName, fncId, dim, mM, parameter, comment
		FROM tt_step_natural_join -- WHERE _changed IS NOT NULL
		UNION ALL
		SELECT id, pdvId, position, step, revision, min, max, modName, fncId, dim, mM, parameter, comment
		FROM step WHERE id > @lastStepId
	;
	
IF @runLevel = 9 THEN
	SELECT count(*) INTO @totalSteps FROM tt_step_report;
	CALL addLogD(CONCAT('	Number of reported steps =', @totalSteps));
	CALL copyAsNormalTable('tt_step_report', 'ttstep_report', True);
END IF;


END$$




DELIMITER ;

