DROP PROCEDURE IF EXISTS legalSQL;

DELIMITER $$
CREATE PROCEDURE legalSQL(in_tableName VARCHAR(255))
BEGIN
    CALL nonExistingFunction();		-- compile time OK, but error on runtime
END$$
DELIMITER ;

