USE kefico

DROP PROCEDURE IF EXISTS _removeData;
DROP PROCEDURE IF EXISTS _showUseCase;
DROP FUNCTION IF EXISTS _get_ng_count;
DROP FUNCTION IF EXISTS _alloc_measure_id;

DELIMITER $$

/*
 * Debugging 을 위해서 추가된 data 삭제
 */
CREATE PROCEDURE _removeData()
	COMMENT 'For debug.  Cleans stored data, for new test.'
BEGIN
	DECLARE l_runLevel INT DEFAULT getRunLevel();

	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE('SQL error happend in _removeData.');
		RESIGNAL;
	END;


	CALL verify(l_runLevel <> 0, 'Removing data just allowed for debugging purpose.');

	DELETE FROM log;
	DELETE FROM dynamicTopSummary;
	DELETE FROM staticTopSummary;
	DELETE FROM bundle;
	DELETE FROM measure;
END$$





DELIMITER ;

