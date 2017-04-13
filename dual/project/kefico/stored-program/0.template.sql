USE kefico

DROP FUNCTION IF EXISTS _myfunction;
DROP PROCEDURE IF EXISTS _myprocedure;
DROP PROCEDURE IF EXISTS _myShortProcedure;
DROP TRIGGER IF EXISTS _mytrigger_bi;


DELIMITER $$


CREATE FUNCTION _myfunction(in_message TEXT)
	RETURNS BOOLEAN
	COMMENT	'You should not use HANGUL, here'
BEGIN
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE('SQL error happend in _myfunction.');
		RESIGNAL;
	END;

	INSERT INTO log(eventDt, type, message) VALUES(NOW(), 'D', in_message);
	RETURN True;
END$$


CREATE PROCEDURE _myShortProcedure()
BEGIN
END$$

CREATE PROCEDURE _myprocedure(in_checkCondition BOOL, in_message TEXT)
	COMMENT	'You should not use HANGUL, here'
thisproc:	-- use LEAVE thisproc; to exit this routine
BEGIN
	DECLARE denied INT DEFAULT 0;
	DECLARE command_denied CONDITION FOR 1142;
	DECLARE CONTINUE HANDLER FOR command_denied SET denied=1;

    DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
    BEGIN
        CALL addLogE('SQL error happend in myprocedure.');
        RESIGNAL;
    END;

	IF NOT in_checkCondition THEN
		SIGNAL SQLSTATE '99999'
			SET MESSAGE_TEXT = in_message;
	END IF;

	IF NOT in_checkCondition THEN
		BEGIN
		END;
	END IF;
END$$

CREATE TRIGGER _mytrigger_bi
    BEFORE INSERT -- {BEFORE|AFTER} {INSERT|UPDATE|DELETE}
    ON bundle
    FOR EACH ROW
	-- COMMENT	'You can not add comment clause on trigger'
BEGIN
END$$


DELIMITER ;
