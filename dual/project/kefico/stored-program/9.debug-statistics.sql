USE kefico;

DROP PROCEDURE IF EXISTS testLongLoop;


DELIMITER $$


/*
 * 약 28.51 초 소요됨.
 * 	- generator 를 사용하는 경우, 1.23초 소요됨 (select * from generator_1000000;)
 */
CREATE PROCEDURE testLongLoop()
BEGIN
	DECLARE n INT DEFAULT 0;
	REPEAT
		SET n = n + 1;
	UNTIL n > 1000000
	END REPEAT;
END$$


DELIMITER ;

