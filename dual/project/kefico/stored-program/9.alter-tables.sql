USE kefico

DELIMITER $$

ALTER TABLE bundle				ROW_FORMAT = COMPRESSED		KEY_BLOCK_SIZE=4;
ALTER TABLE measure				ROW_FORMAT = COMPRESSED		KEY_BLOCK_SIZE=4;
ALTER TABLE step				ROW_FORMAT = COMPRESSED		KEY_BLOCK_SIZE=4;
ALTER TABLE staticTopSummary	ROW_FORMAT = COMPRESSED		KEY_BLOCK_SIZE=4;
ALTER TABLE staticDailyStepSummary	ROW_FORMAT = COMPRESSED		KEY_BLOCK_SIZE=4;

/*
 * bundle table 에서 외래키 제약조건을 없앤다.  table partition 기능은 외래키가 있으면 지원되지 않기 때문이다.
 */
CREATE PROCEDURE _removeConstraint(in_table VARCHAR(255))
BEGIN
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE('SQL error happend in _removeConstraint.');
		RESIGNAL;
	END;

	SELECT GROUP_CONCAT(constraint_name SEPARATOR ',')
		INTO @fks
		FROM information_schema.table_constraints T
		WHERE constraint_schema = DATABASE()
			AND table_name = in_table
			AND constraint_type = 'FOREIGN KEY'
		;

	SET @c=1;
	lable1: LOOP
		SET @fk = splitString(@fks, ',', @c);

		IF @fk IS NULL THEN
			LEAVE lable1;
		END IF;

		CALL executeStatement(CONCAT('ALTER TABLE ', in_table, ' DROP FOREIGN KEY ', @fk));
		SET @c=@c+1;
	END LOOP lable1;
END$$

DELIMITER ;




/*
 * _removeConstraint 함수는 한번 사용하고 더이상 사용할 일이 없으므로 호출 후, 삭제한다.
 */
CALL _removeConstraint('bundle');
-- CALL _removeConstraint('step');

/*
 * bundle 에서 FK constraint 삭제 이후 남아있는 index 를 삭제한다.
 */
/*
ALTER TABLE bundle
	DROP INDEX fk_bundle_step1_idx
	-- , DROP INDEX bundle_ok_idx
		-- ok index 는 showStepErrorDetailView 에서 유용하게 쓰임
	-- , DROP INDEX fk_bundle_measure1_idx;
		-- measureId 에 대한 index 는 Positional Summary 에서 유용하게 쓰이므로 남겨 둘 것.
	-- bundle 에서 step 은 staticDailyStepSummary 에 이미 summary 해 두므로, index 가 크게 유용하지 않음.
;
*/

DROP PROCEDURE _removeConstraint;

CALL _applyBundlePartition();	-- 1년 1개월 이전
SET @yearAgo=DATE_SUB(CURDATE(), INTERVAL 1 YEAR);
	CALL _addBundlePartitionForDay(DATE_SUB(@yearAgo, INTERVAL 10 DAY));
	CALL _addBundlePartitionForDay(DATE_SUB(@yearAgo, INTERVAL  9 DAY));
	CALL _addBundlePartitionForDay(DATE_SUB(@yearAgo, INTERVAL  8 DAY));
	CALL _addBundlePartitionForDay(DATE_SUB(@yearAgo, INTERVAL  7 DAY));
	CALL _addBundlePartitionForDay(DATE_SUB(@yearAgo, INTERVAL  6 DAY));
	CALL _addBundlePartitionForDay(DATE_SUB(@yearAgo, INTERVAL  5 DAY));
	CALL _addBundlePartitionForDay(DATE_SUB(@yearAgo, INTERVAL  4 DAY));
	CALL _addBundlePartitionForDay(DATE_SUB(@yearAgo, INTERVAL  3 DAY));
	CALL _addBundlePartitionForDay(DATE_SUB(@yearAgo, INTERVAL  2 DAY));
	CALL _addBundlePartitionForDay(DATE_SUB(@yearAgo, INTERVAL  1 DAY));
	CALL _addBundlePartitionForDay(@yearAgo);
	CALL _addBundlePartitionForDay(DATE_ADD(@yearAgo, INTERVAL  1 DAY));
	CALL _addBundlePartitionForDay(DATE_ADD(@yearAgo, INTERVAL  2 DAY));
	CALL _addBundlePartitionForDay(DATE_ADD(@yearAgo, INTERVAL  3 DAY));
	CALL _addBundlePartitionForDay(DATE_ADD(@yearAgo, INTERVAL  4 DAY));




