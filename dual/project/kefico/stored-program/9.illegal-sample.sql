USE kefico

DELIMITER $$

DROP PROCEDURE IF EXISTS illegalSQL;
CREATE PROCEDURE illegalSQL(in_tableName VARCHAR(255))
BEGIN
	SELECT * FROM in_tableName;		-- Table 'kefico.in_tableName' doesn't exist
	-- CALL executeStatement(CONCAT('SELECT * FROM ', in_tableName, ' ;'));
END$$



/*
 * function, trigger 에서는 prepare 문을 사용할 수 없다.!!!
 * procedure 로 wrapping 을 하여도 마찬가지 결과.
 */
DROP FUNCTION IF EXISTS illegalSQL2;
CREATE FUNCTION illegalSQL2(
	in_sql VARCHAR(65535)
)
        RETURNS BOOLEAN
        COMMENT 'ERROR 1336 (0A000) at line 15: Dynamic SQL is not allowed in stored function or trigger' 
BEGIN
	-- PREPARE pStmt FROM 'SELECT * FROM pdv;';
	RETURN True;
END$$



DELIMITER ;

/*
 * ERROR 1351 (HY000): View's SELECT contains a variable or parameter
 */
CREATE OR REPLACE VIEW _measureToday AS
SELECT * FROM measure
    WHERE day = @startDay
;

