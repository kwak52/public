USE kefico

DROP PROCEDURE IF EXISTS generateTemporaryBundleTable;
DROP PROCEDURE IF EXISTS _generateBundleDataIntoTemporaryTable;
DROP PROCEDURE IF EXISTS _buildInitialDailySectionalStepSummary;

DELIMITER $$

CREATE PROCEDURE generateTemporaryBundleTable()
BEGIN
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE('SQL error happend in generateTemporaryBundleTable.');
		RESIGNAL;
	END;

	/*
	 * temporary table 을 만드는 overhead 를 줄이기 위해서
	 * 없는 경우 생성하고, truncate 하는 방법을 이용
	 * http://stackoverflow.com/questions/84330/mysql-create-table-if-not-exists-else-truncate
	 */
	CALL copyAsTemporaryTable('bundle', 'tt_bundle', False);
	ALTER TABLE tt_bundle
		DROP COLUMN day
		, DROP COLUMN measureId
	;
END$$

/*
 * Debugging 용 1회 measure 에 대한 bundle insert routine.
 * 실제 구현에서는 temporary table tt_bundle 을 만들고, tt_bundle 에 insert 한 후,
 *  - tt_bundle 생성은 transaction 에 포함시키지 않는다.
 * tt_bundle 을 bundle 에 insert 하는 것이 좋을 듯하다.
 */
CREATE PROCEDURE _generateBundleDataIntoTemporaryTable(
	in_pdvId INT
	, in_OkTarget BOOLEAN
)
	COMMENT 'generate bundle data into tt_bundle for debugging session.'
BEGIN
	DECLARE l_po INT DEFAULT 1;
	DECLARE l_maxPo INT DEFAULT IF( in_OkTarget, 50000, 10*getRandomN(1, 4999) );
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE('SQL error happend in _generateBundleDataIntoTemporaryTable.');
		RESIGNAL;
	END;


	CALL generateTemporaryBundleTable();

	INSERT INTO tt_bundle(stepId, value, message, ok)
	SELECT
		st.id
		, IF(position=l_maxPo, -2*min, getRandomN(min, max)) AS value
		, NULL as message
		, position<>l_maxPo AS ok
	FROM step st
	WHERE pdvId = in_pdvId
		AND position <= l_maxPo
	;
END$$


CREATE PROCEDURE _buildInitialDailySectionalStepSummary()
	COMMENT 'builds initial daily sectional position summary table.'
BEGIN
	DECLARE l_date DATE;
	DECLARE l_last_row_fetched BOOLEAN;

	DECLARE csr_date CURSOR FOR
	SELECT DISTINCT day FROM measure WHERE day <> CURDATE()
	;

	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE('SQL error happend in _buildInitialDailySectionalStepSummary.');
		RESIGNAL;
	END;

	DECLARE CONTINUE HANDLER FOR NOT FOUND SET l_last_row_fetched=1;

	TRUNCATE staticDailyStepSummary;

	OPEN csr_date;
		crs_loop: LOOP
			FETCH csr_date INTO l_date;
			IF l_last_row_fetched = 1 THEN
				LEAVE crs_loop;
			END IF;
			CALL _updateDailySectionalPositionalSummary(l_date);
		END LOOP crs_loop;
	CLOSE csr_date;

END$$

DELIMITER ;

