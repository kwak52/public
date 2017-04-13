DROP PROCEDURE IF EXISTS updateStep;
DROP FUNCTION IF EXISTS _is_bundle_ok;

CREATE PROCEDURE updateStep(
	in_id INT UNSIGNED
	, in_min DECIMAL(17, 6)
	, in_max DECIMAL(17, 6)
	, in_modName VARCHAR(45)
	, in_dim SMALLINT(6) UNSIGNED 
)
	COMMENT 'Update step.  see deleteStep, showDeletedSteps'
	SQL SECURITY DEFINER
BEGIN
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE('SQL error happend in updateStep.');
		RESIGNAL;
	END;

CALL addLogD(CONCAT('updateStep: id=', in_id));
	START TRANSACTION;

	-- 기존 step 은 out-of-date 상태로 변경
	UPDATE step SET isActive=0 WHERE id = in_id;

	-- 기존 step 과 동일한 position/step 정보를 이용해서 새로운 step 생성
	INSERT INTO step (isActive, pdvId, position, revision, step, min, max, modName, fncId, dim)
		SELECT True, pdvId, position, revision+1, step, in_min, in_max, in_modName, fncId, in_dim
		FROM step
		WHERE id = in_id
	;

	COMMIT;
END$$


CREATE PROCEDURE deleteStep(
	in_id INT UNSIGNED
)
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE('SQL error happend in deleteStep.');
		RESIGNAL;
	END;

	COMMENT 'Delete step.  see updateStep, showDeletedSteps'
	SQL SECURITY DEFINER
BEGIN
	-- 기존 step 은 out-of-date 상태로 변경
	UPDATE step SET isActive=0 WHERE id = in_id;

	-- 새로운 step 을 만들지 않으므로 delete 효과 발생
END$$




CREATE PROCEDURE showStepChangeHistory(in_id INT)
	COMMENT 'Shows step change history for the given step id.'
BEGIN
	DECLARE l_pdvId INT;
	DECLARE l_position INT;
	DECLARE l_step INT;
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE('SQL error happend in showStepChangeHistory.');
		RESIGNAL;
	END;


	SELECT pdvId, position, step
	INTO l_pdvId, l_position, l_step
	FROM step
	WHERE id=in_id
	;

	SELECT *
	FROM step
	WHERE
		pdvId = l_pdvId
		AND position = l_position
		AND step = l_step
	ORDER BY revision
	;
END$$


CREATE PROCEDURE showDeletedSteps()
	COMMENT 'Shows deleted steps.'
BEGIN
	SELECT *
	FROM step
	WHERE
		isActive = 0
	ORDER BY pdvId, position, step, revision
	;
END$$



/*
ALTER TABLE step
PARTITION BY LIST( isActive )
(
	PARTITION pOutOfDate VALUES IN (0),
	PARTITION pActive VALUES IN (1)
)$$
*/



CREATE FUNCTION _get_ng_count(
	in_measureId INT
)
	RETURNS INT
	COMMENT 'For debug.  Returns number of NG\'s for the bundle.'
BEGIN
	DECLARE l_pdvId INT;
	DECLARE l_ng_count INT;
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE('SQL error happend in _get_ng_count.');
		RESIGNAL;
	END;


	-- PDV id for the given measure
	SELECT pdvId INTO l_pdvId FROM measure WHERE id=in_measureId;

	SELECT COUNT(*) -- > 0
	INTO l_ng_count
	FROM (
		SELECT value BETWEEN min AND max AS OK
		FROM bundle b, step s
		WHERE
			b.measureId = in_measureId
			AND b.stepId = s.id
			AND s.isActive = True
	) bs
	WHERE NOT bs.OK
	;

	return l_ng_count;
END$$





CREATE FUNCTION _is_bundle_ok(
	in_measureId INT
)
	RETURNS BOOLEAN
	COMMENT 'For debug.  Returns ok/ng status for the bundle.'
BEGIN
	return _get_ng_count(in_measureId) = 0;
END$$




CREATE PROCEDURE _showUseCase()
BEGIN
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE('SQL error happend in _showUseCase.');
		RESIGNAL;
	END;

	SELECT * FROM topSummary_vw ORDER BY day, id;
	
	SELECT id, _isFromDynamic, day
	INTO @tsvId, @dynamic, @day
	FROM topSummary_vw LIMIT 1
	;

	CALL showSPSV(@tsvId, @dynamic);

	SELECT id
	INTO @stepId
	FROM step
	WHERE isActive = True
	LIMIT 1
	;

	CALL showPHV(@tsvId, @dynamic, @stepId);

	SET @measureId=10;
	CALL showBundle(@measureId);
END$$
