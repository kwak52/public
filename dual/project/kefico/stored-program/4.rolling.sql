USE kefico

DROP FUNCTION IF EXISTS hasPartition_p;

DROP PROCEDURE IF EXISTS rollOutBeforeDateLine;
DROP PROCEDURE IF EXISTS rollOutAfterDateLine;
DROP PROCEDURE IF EXISTS showPartition;
DROP PROCEDURE IF EXISTS _updateDailySectionalPositionalSummary;
DROP PROCEDURE IF EXISTS _addBundlePartitionForDay;
DROP PROCEDURE IF EXISTS _dropBundlePartitionForDay;
DROP PROCEDURE IF EXISTS _applyBundlePartition;

DELIMITER $$


/*
 * 날짜 변경 시점 기준,  충분한 시간(1시간 정도) 이전에 수행할 작업
 */
CREATE PROCEDURE rollOutBeforeDateLine()
	COMMENT 'Before day change, prepares next day.'
BEGIN
	-- bundle 의 partition 생성
	CALL _addBundlePartitionForDay(DATE_ADD(CURDATE(), INTERVAL 1 DAY));
END$$

/*
 * 날짜 변경 시점 기준, 충분한 시간(1시간 정도) 이후에 수행할 작업
 */
CREATE PROCEDURE rollOutAfterDateLine()
	COMMENT 'After day change, drops rolled out day information.'
BEGIN
	DECLARE dayOneYearAgo	DATE		DEFAULT getRolloutDay();
	DECLARE partitionDrop	VARCHAR(32)	DEFAULT DATE_FORMAT(dayOneYearAgo, "p%Y%m%d");
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE('SQL error happend in rollOutAfterDateLine.');
		RESIGNAL;
	END;


	START TRANSACTION;

CALL addLogL(CONCAT('rollOutAfterDateLine(): ', dayOneYearAgo));

		DELETE FROM measure
		WHERE day < dayOneYearAgo
		;


		DELETE FROM staticTopSummary
		WHERE day < dayOneYearAgo
		;

		DELETE FROM staticDailyStepSummary
		WHERE day < dayOneYearAgo
		;

		DELETE FROM log
		WHERE eventDt < CURDATE() - INTERVAL 10 DAY
		;

	COMMIT;

	-- partition truncate 는 어차피 transaction 과 무방하므로, transaction loop 밖에서 or 맨 나중에 수행한다.
	CALL _dropBundlePartitionForDay(dayOneYearAgo);
END$$


CREATE FUNCTION hasPartition_p(tbl VARCHAR(255), part VARCHAR(255))
	RETURNS BOOLEAN
	COMMENT 'Check whether given partition exists or not'
BEGIN
	RETURN EXISTS(
		SELECT NULL
		FROM information_schema.partitions
		WHERE table_schema = Database()
			AND table_name = tbl
			AND partition_name = part
	);
END$$

CREATE PROCEDURE showPartition(tableName VARCHAR(255))
	COMMENT 'Shows partition for the given table.'
BEGIN
	SELECT PARTITION_ORDINAL_POSITION as posi
		, partition_name AS name, table_rows AS rows, partition_method AS method
	FROM information_schema.partitions
	WHERE table_schema = Database()
		AND table_name = tableName
		AND partition_name IS NOT NULL
	;
END$$



CREATE PROCEDURE _applyBundlePartition()
	COMMENT 'alter table by applying parition'
BEGIN
	DECLARE yearAgo DATE DEFAULT DATE_SUB(getRolloutDay(), INTERVAL 30 DAY);
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE('SQL error happend in _applyBundlePartition.');
		RESIGNAL;
	END;

    CALL executeStatement( CONCAT(
		'ALTER TABLE bundle PARTITION BY RANGE ( TO_DAYS(day) ) ( PARTITION p'
		, DATE_FORMAT(yearAgo, '%Y%m%d')
		, ' VALUES LESS THAN ('
		, 1 + TO_DAYS(yearAgo)
		, '));')
	);
END$$



CREATE PROCEDURE _addBundlePartitionForDay(day DATE)
	COMMENT 'add partition in bundle table for given day'
BEGIN
	DECLARE newPartition VARCHAR(32) DEFAULT DATE_FORMAT(day, 'p%Y%m%d');
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE('SQL error happend in _addBundlePartitionForDay.');
		RESIGNAL;
	END;

	IF hasPartition_p('bundle', newPartition) THEN
		CALL addLogW(CONCAT('  Partition already exists: ', newPartition));
	ELSE
		CALL executeStatement(
			CONCAT('ALTER TABLE bundle ADD PARTITION (PARTITION '
				, newPartition
				, ' VALUES LESS THAN (1 + TO_DAYS('
				, QUOTE(day)
				, ')));')
		);
		CALL addLogL(CONCAT('  Partition added: ', newPartition));
	END IF;
END$$



CREATE PROCEDURE _dropBundlePartitionForDay(day DATE)
	COMMENT 'drop partition in bundle table for given day'
BEGIN
	DECLARE partToDrop VARCHAR(32) DEFAULT DATE_FORMAT(day, 'p%Y%m%d');
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE('SQL error happend in _dropBundlePartitionForDay.');
		RESIGNAL;
	END;

	IF hasPartition_p('bundle', partToDrop) THEN
		CALL executeStatement(
			CONCAT('ALTER TABLE bundle DROP PARTITION ', partToDrop, ';')
		);
		CALL addLogL(CONCAT('  Partition dropped: ', partToDrop));
	ELSE
		CALL addLogD(CONCAT('  No such partition for drop: ', partToDrop));
	END IF;
END$$



/*
 * 하루가 지나는 시점에 호출되므로, in_day 에 해당하는 data는 staticDailyStepSummary 에 
 * 존재하지 않아야 한다.
 */
CREATE PROCEDURE _updateDailySectionalPositionalSummary(
	in_day DATE
)
	COMMENT 'Update daily sectional position summary on date change.'
BEGIN
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE('SQL error happend in _updateDailySectionalPositionalSummary.');
		RESIGNAL;
	END;

CALL addLogD(CONCAT('_updateDailySectionalPositionalSummary(', in_day, ')'));

	INSERT INTO staticDailyStepSummary(day, ccsId, pdvId, stepId, total, ngCount, valSum, valSqSum)
	SELECT
		-- b.measureId, b.day, b.message,
		in_day					AS day
		, m.ccsId				AS ccsId
		, m.pdvId				AS pdvId
		, b.stepId				AS stepId
		, count(*)				AS Total
		, SUM(NOT b.ok)			AS ngCount
		, SUM(b.value)			AS valSum
		, SUM(POW(b.value, 2))	AS valSqSum
	FROM bundle b
		, measure m
		-- , step st
	WHERE 
		b.measureId = m.id
		-- AND b.stepId = st.id
		AND m.day = in_day
		AND m.type = 'NM'
	GROUP BY ccsId, pdvId, stepId
	;

END$$




DELIMITER ;
