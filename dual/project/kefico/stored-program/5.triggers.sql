USE kefico

DROP PROCEDURE IF EXISTS _increment_table_revision;

-- DROP TRIGGER IF EXISTS step_bi;
DROP TRIGGER IF EXISTS step_ai;
DROP TRIGGER IF EXISTS step_au;
DROP TRIGGER IF EXISTS step_ad;

DROP TRIGGER IF EXISTS user_ai;
DROP TRIGGER IF EXISTS user_au;
DROP TRIGGER IF EXISTS user_ad;

DROP TRIGGER IF EXISTS dimension_ai;
DROP TRIGGER IF EXISTS dimension_au;
DROP TRIGGER IF EXISTS dimension_ad;

DROP TRIGGER IF EXISTS pdv_ai;
DROP TRIGGER IF EXISTS pdv_au;
DROP TRIGGER IF EXISTS pdv_ad;

DROP TRIGGER IF EXISTS pdvGroup_ai;
DROP TRIGGER IF EXISTS pdvGroup_au;
DROP TRIGGER IF EXISTS pdvGroup_ad;

DROP TRIGGER IF EXISTS preference_ai;
DROP TRIGGER IF EXISTS preference_au;
DROP TRIGGER IF EXISTS preference_ad;

DROP TRIGGER IF EXISTS command_ai;
DROP TRIGGER IF EXISTS command_au;
DROP TRIGGER IF EXISTS command_ad;

DROP TRIGGER IF EXISTS measure_ai;

DELIMITER $$

CREATE PROCEDURE _increment_table_revision(in_table_name VARCHAR(255))
	COMMENT 'Increment table revision number'
BEGIN
	UPDATE tableRevision
	SET revision=revision + 1
	WHERE name=in_table_name
	;	
END$$


/*
CREATE TRIGGER step_bi BEFORE INSERT ON step FOR EACH ROW
BEGIN
	DECLARE l_message VARCHAR(65535);

	IF NOT EXISTS(SELECT NULL FROM pdv WHERE id=new.pdvId) THEN
		SET l_message = CONCAT('FK Constraint error: pdvId ', new.pdvId, ' does not exist in pdv table.');
		SIGNAL SQLSTATE '99999'
			SET MESSAGE_TEXT = l_message;
	END IF;

	IF NOT EXISTS(SELECT NULL FROM pdv WHERE id=new.dim) THEN
		SET l_message = CONCAT('FK Constraint error: dimension ', new.dim, ' does not exist in dimension table.');
		SIGNAL SQLSTATE '99999'
			SET MESSAGE_TEXT = l_message;
	END IF;
END$$
*/

CREATE TRIGGER step_ai AFTER INSERT ON step FOR EACH ROW
BEGIN
	CALL _increment_table_revision('step');
END$$

CREATE TRIGGER step_au AFTER UPDATE ON step FOR EACH ROW
BEGIN
	CALL _increment_table_revision('step');
END$$

CREATE TRIGGER step_ad AFTER DELETE ON step FOR EACH ROW
BEGIN
	CALL _increment_table_revision('step');
END$$



CREATE TRIGGER user_ai AFTER INSERT ON user FOR EACH ROW
BEGIN
	CALL _increment_table_revision('user');
END$$

CREATE TRIGGER user_au AFTER UPDATE ON user FOR EACH ROW
BEGIN
	CALL _increment_table_revision('user');
END$$

CREATE TRIGGER user_ad AFTER DELETE ON user FOR EACH ROW
BEGIN
	CALL _increment_table_revision('user');
END$$




CREATE TRIGGER dimension_ai AFTER INSERT ON dimension FOR EACH ROW
BEGIN
	CALL _increment_table_revision('dimension');
END$$

CREATE TRIGGER dimension_au AFTER UPDATE ON dimension FOR EACH ROW
BEGIN
	CALL _increment_table_revision('dimension');
END$$

CREATE TRIGGER dimension_ad AFTER DELETE ON dimension FOR EACH ROW
BEGIN
	CALL _increment_table_revision('dimension');
END$$


CREATE TRIGGER pdv_ai AFTER INSERT ON pdv FOR EACH ROW
BEGIN
	CALL _increment_table_revision('pdv');
END$$

CREATE TRIGGER pdv_au AFTER UPDATE ON pdv FOR EACH ROW
BEGIN
	CALL _increment_table_revision('pdv');
END$$

CREATE TRIGGER pdv_ad AFTER DELETE ON pdv FOR EACH ROW
BEGIN
	CALL _increment_table_revision('pdv');
END$$


CREATE TRIGGER pdvGroup_ai AFTER INSERT ON pdvGroup FOR EACH ROW
BEGIN
	CALL _increment_table_revision('pdvGroup');
END$$

CREATE TRIGGER pdvGroup_au AFTER UPDATE ON pdvGroup FOR EACH ROW
BEGIN
	CALL _increment_table_revision('pdvGroup');
END$$

CREATE TRIGGER pdvGroup_ad AFTER DELETE ON pdvGroup FOR EACH ROW
BEGIN
	CALL _increment_table_revision('pdvGroup');
END$$



CREATE TRIGGER preference_ai AFTER INSERT ON preference FOR EACH ROW
BEGIN
	CALL _increment_table_revision('preference');
END$$

CREATE TRIGGER preference_au AFTER UPDATE ON preference FOR EACH ROW
BEGIN
	CALL _increment_table_revision('preference');
END$$

CREATE TRIGGER preference_ad AFTER DELETE ON preference FOR EACH ROW
BEGIN
	CALL _increment_table_revision('preference');
END$$




CREATE TRIGGER command_ai AFTER INSERT ON command FOR EACH ROW
BEGIN
	CALL _increment_table_revision('command');
END$$

CREATE TRIGGER command_au AFTER UPDATE ON command FOR EACH ROW
BEGIN
	CALL _increment_table_revision('command');
END$$

CREATE TRIGGER command_ad AFTER DELETE ON command FOR EACH ROW
BEGIN
	CALL _increment_table_revision('command');
END$$


-- measure 는 insert 시에만 신경쓰면 된다.
-- update 는 존재하지 않고, delete 시는 rollout 이므로 무시하면 된다.
CREATE TRIGGER measure_ai AFTER INSERT ON measure FOR EACH ROW
BEGIN
	CALL _increment_table_revision('measure');
END$$

DELIMITER ;

