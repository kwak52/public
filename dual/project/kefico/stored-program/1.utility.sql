USE kefico

DROP FUNCTION IF EXISTS getDayOneYearAgo;
DROP FUNCTION IF EXISTS getDay;
DROP FUNCTION IF EXISTS getRolloutDay;
DROP FUNCTION IF EXISTS getSigmaRange;
DROP FUNCTION IF EXISTS getRandomN;
DROP FUNCTION IF EXISTS getRunLevel;
DROP FUNCTION IF EXISTS getColumnNamesFromTable;
DROP FUNCTION IF EXISTS getPartitions;
DROP FUNCTION IF EXISTS getSecondsFromTime;
DROP FUNCTION IF EXISTS splitString;
DROP FUNCTION IF EXISTS randomString;
DROP FUNCTION IF EXISTS getDeviationInfo;
DROP FUNCTION IF EXISTS IfZeroN;
DROP FUNCTION IF EXISTS IfZeroR;

DROP PROCEDURE IF EXISTS executeStatement;
DROP PROCEDURE IF EXISTS addLogE;
DROP PROCEDURE IF EXISTS addLogW;
DROP PROCEDURE IF EXISTS addLogD;
DROP PROCEDURE IF EXISTS addLogD9;
DROP PROCEDURE IF EXISTS addLogL;
DROP PROCEDURE IF EXISTS verify;
DROP PROCEDURE IF EXISTS queueSendMail;
DROP PROCEDURE IF EXISTS showStoredPrograms;
DROP PROCEDURE IF EXISTS showViews;
DROP PROCEDURE IF EXISTS showTables;
DROP PROCEDURE IF EXISTS showProgInfo;
DROP PROCEDURE IF EXISTS showTriggers;
DROP PROCEDURE IF EXISTS showEventSchedulers;;
DROP PROCEDURE IF EXISTS showAll;
DROP PROCEDURE IF EXISTS dropPartition;
DROP PROCEDURE IF EXISTS unPartition;
DROP PROCEDURE IF EXISTS selectTableWithGeneratedRow;
DROP PROCEDURE IF EXISTS copyAsTemporaryTable;
DROP PROCEDURE IF EXISTS copyAsNormalTable;


DELIMITER $$

CREATE PROCEDURE executeStatement(statement VARCHAR(65535))
	COMMENT	'Execute statement'
BEGIN
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE(CONCAT('SQL error happend in executeStatement: ', statement));
		RESIGNAL;
	END;

	SET @stmt=statement;
	PREPARE pStmt FROM @stmt;

	EXECUTE pStmt;
	DEALLOCATE PREPARE pStmt;
END$$

CREATE PROCEDURE addLogD(in_message TEXT)
	COMMENT	'Inserts debug log line into log table'
BEGIN
	SELECT val INTO @enableLogD FROM preference WHERE name = 'enableLogD';
	IF @enableLogD = 'Y' THEN
		INSERT INTO log(eventDt, type, message) VALUES(NOW(), 'D', in_message);
	END IF;
END$$

CREATE PROCEDURE addLogD9(in_message TEXT)
	COMMENT	'Inserts debug log line into log table if runLevel = 9'
BEGIN
	IF getRunLevel() = 9 THEN
		CALL addLogD(in_message);
	END IF;
END$$

CREATE PROCEDURE addLogW(in_message TEXT)
	COMMENT	'Inserts warning log line into log table'
BEGIN
	SELECT val INTO @enableLogW FROM preference WHERE name = 'enableLogW';
	IF @enableLogW = 'Y' THEN
		INSERT INTO log(eventDt, type, message) VALUES(NOW(), 'W', in_message);
	END IF;
END$$

CREATE PROCEDURE addLogE(in_message TEXT)
	COMMENT	'Inserts error log line into log table'
BEGIN
    INSERT INTO log(eventDt, type, message) VALUES(NOW(), 'E', in_message);
END$$

CREATE PROCEDURE addLogL(in_message TEXT)
	COMMENT	'Inserts log line into log table'
BEGIN
    INSERT INTO log(eventDt, type, message) VALUES(NOW(), 'L', in_message);
END$$


CREATE PROCEDURE verify(in_checkCondition BOOL, in_message TEXT)
	COMMENT	'Verifies condition and signals on check failure.'
BEGIN
	IF NOT in_checkCondition THEN
		SIGNAL SQLSTATE '99999'
			SET MESSAGE_TEXT = in_message;
	END IF;
END$$



CREATE FUNCTION IfZeroN(a INT, b INT)
	RETURNS INT
	NO SQL
	COMMENT	'Similiar implementation for IFNULL.  Test zero for integer value.'
BEGIN
	RETURN IF(a = 0, b, a);
END$$


CREATE FUNCTION IfZeroR(a DOUBLE, b DOUBLE)
	RETURNS DOUBLE
	NO SQL
	COMMENT	'Similiar implementation for IFNULL.  Test zero for double value.'
BEGIN
	RETURN IF(a = 0, b, a);
END$$





CREATE FUNCTION getDayOneYearAgo()
	RETURNS DATE
	NO SQL
	COMMENT	'Get the day one year ago from today.'
BEGIN
	RETURN SUBDATE(CURDATE(), INTERVAL 1 YEAR);
END$$

CREATE FUNCTION getDay(offsetDay INT)
	RETURNS DATE
	NO SQL
	COMMENT	'Get the day offset\'ed from today.'
BEGIN
	RETURN ADDDATE(CURDATE(), INTERVAL offsetDay DAY);
END$$

CREATE FUNCTION getRolloutDay()
	RETURNS DATE
	NO SQL
	COMMENT	'Get the day for rollout removal'
BEGIN
	RETURN getDayOneYearAgo();
END$$


CREATE FUNCTION getColumnNamesFromTable(
	in_tableName VARCHAR(255)
)
	RETURNS VARCHAR(65535)
	COMMENT	'Get column names.'
BEGIN
	DECLARE l_colNames VARCHAR(65535);

	-- MySQL does not support using separator as input parameter.
	-- so, GROUP_CONCAT(column_name SEPARATOR in_separator) is illegal
	--
	SELECT GROUP_CONCAT(column_name SEPARATOR ' | ')
	INTO l_colNames
    FROM information_schema.columns
	WHERE table_name=in_tableName
	;

	RETURN l_colNames;
END$$


CREATE DEFINER=`stored_program`@`%` FUNCTION `getDeviationInfo`(
	n DOUBLE
	, v DOUBLE
	, x DOUBLE	
) RETURNS varchar(16) CHARSET latin1
    NO SQL
    COMMENT 'Get Deviation range for [miN, Value, maX] if can not caculate = ?' 
BEGIN
	DECLARE k DOUBLE DEFAULT 0;
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE('SQL error happend in getDeviationInfo.');
		RESIGNAL;
	END;

	
	IF(x = n)
    THEN RETURN -1;
    END IF;
	
    SET k = 14 * (v - (n+x)*0.5) / (x-n);
	IF(k > 7) THEN
		RETURN 'X F+' ;
	ELSEIF(k < -7) THEN
		RETURN 'X F-' ;
	ELSEIF(k < 0) THEN
		RETURN CONCAT(ABS(CEIL(k)),'-'); -- for sorting
	ELSE
		RETURN CONCAT(FLOOR(k),'+');
    END IF;  
END$$

CREATE FUNCTION getSigmaRange(
	n DOUBLE
	, x DOUBLE
    , a DOUBLE
	, s DOUBLE
	, c DOUBLE
) RETURNS double
    NO SQL
    COMMENT 'Get cpk for [miN, maX, Avg, SqSum, Count] if can not caculate = -1'
BEGIN
	DECLARE k DOUBLE DEFAULT 0;
	DECLARE sigma DOUBLE DEFAULT 0;
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE('SQL error happend in getSigmaRange.');
		RESIGNAL;
	END;

    
    IF(c < 2 or x = n)
    THEN RETURN -1;
    END IF;

	SET k = ABS( ( (x + n) / 2 - a  ) / (x - n) * 2);
	SET sigma =  ( s - c * a * a )  / (c - 1);

	RETURN IF( k > 1 or sigma <= 0, -1, (1 - k) * (x - n) / ( 6 * SQRT(sigma) ) );
END$$


CREATE FUNCTION getRandomN(
	n INT	-- miN
	, x INT	-- maX
)
	RETURNS INT
	NO SQL
	COMMENT	'Get value in range [miN..maX]'
BEGIN
	RETURN n + (x - n) * RAND();
END$$


CREATE FUNCTION getRunLevel()
	RETURNS INT
	COMMENT	'Get database system run level.'
BEGIN
	DECLARE l_runLevel INT;

	SELECT val INTO l_runLevel
	FROM preference
	WHERE name = 'runLevel'
	;
	
	RETURN l_runLevel + 0;
END$$


/*
 * mysql 을 통한 메일을 전송하기 위한 용도.
 * 전송해야 할 메일 내용을 text file 로 dump 하고, text file 을 모니터링하는 다른 프로세스가 메일을 전송하도록 한다.
 * mysql server 에서 직접 메일을 전송하는 것은 서버 부하 문제로 비추천...
 * http://stackoverflow.com/questions/387483/how-to-send-email-from-mysql-5-1
 */
CREATE PROCEDURE queueSendMail(
	in_from VARCHAR(255)
	, in_to VARCHAR(255)
	, in_cc VARCHAR(255)
	, in_bcc VARCHAR(255)
	, in_subject VARCHAR(255)
	, in_body TEXT
)
	COMMENT	'Queue mail contents for later processing.'
BEGIN
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE('SQL error happend in queueSendMail.');
		RESIGNAL;
	END;

      /* START THE WRITING OF THE EMAIL FILE HERE*/      
      SELECT  CONCAT('To: ', in_to),
              CONCAT('From: ', in_from),
              CONCAT('Subject: ', in_subject),
              in_body
          INTO OUTFILE 
                   'C:\\inetpub\\mailroot\\pickup\\mail.txt' 
              FIELDS TERMINATED by '\r\n' ESCAPED BY '';            
END$$


CREATE PROCEDURE showStoredPrograms()
	COMMENT	'Shows stored programs list.'
BEGIN
	/*
	SHOW PROCEDURE STATUS WHERE DB=DATABASE();
	SHOW FUNCTION STATUS WHERE DB=DATABASE();

	SELECT specific_name, routine_type
	FROM information_schema.routines WHERE routine_schema=DATABASE()
	;
	*/

	SELECT * from storedProcedure_vw
	ORDER BY type, specific_name
	;
END$$

CREATE PROCEDURE showViews()
	COMMENT	'Shows views list.'
BEGIN
	SELECT
		table_name AS View
		, vi.comment AS Comment
	FROM information_schema.views sv
	LEFT JOIN viewInfo vi ON(sv.table_name = vi.name)
	WHERE table_schema = DATABASE()
	;
END$$


CREATE PROCEDURE showTables()
	COMMENT	'Shows tables list.'
BEGIN
	SELECT
		table_name AS `Table`
		, REPLACE(LEFT(table_comment, 80), '\n', '  ') AS comment
	FROM information_schema.tables
	WHERE table_schema = DATABASE()
	AND table_type = 'BASE TABLE'
	;
END$$



CREATE PROCEDURE showProgInfo(in_prog VARCHAR(255))
	COMMENT	'Shows program information.'
BEGIN
	SELECT
		specific_name AS Name
		, type
		, param_list
		, returns
		, comment
		, definer
		, body
	FROM mysql.proc
       WHERE db=DATABASE()
               AND specific_name=in_prog
       ;
END$$

CREATE PROCEDURE showTriggers()
	COMMENT	'Shows triggers information.'
BEGIN
	SELECT
		TRIGGER_NAME AS 'Trigger Name'
		, EVENT_OBJECT_TABLE AS 'Table'
		, ACTION_TIMING	AS Time
		, EVENT_MANIPULATION AS Oper
	FROM information_schema.triggers
	WHERE trigger_schema=DATABASE()
	;
END$$


-- select * from information_schema.events\G
--
CREATE PROCEDURE showEventSchedulers()
    COMMENT 'Shows event schedulers information.'
BEGIN
	SELECT
		event_name AS 'Event Name'
		, event_type AS 'TYPE'
		, starts AS 'START'
		, CONCAT(interval_value, ' ', interval_field) AS 'INTERVAL'
		, event_comment AS 'COMMENT'
	FROM information_schema.events
	WHERE event_schema=DATABASE()
	;
	END$$



CREATE PROCEDURE showAll()
       COMMENT 'Shows list of tables, views, stored programs.'
BEGIN
	-- SELECT 'Table Summary';
	CALL showTables();

	-- SELECT 'View Summary';
	CALL showViews();

	-- SELECT 'Triggers Summary';
	CALL showTriggers();

	CALL showEventSchedulers();

	-- SELECT 'Stored Programs Summary';
	CALL showStoredPrograms();

END$$


CREATE FUNCTION getPartitions(
	in_tableName VARCHAR(255)
)
	RETURNS VARCHAR(65535)
	COMMENT	'Helper for getPartitions function.'
BEGIN
	DECLARE l_partitions VARCHAR(65535);
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE('SQL error happend in getPartitions.');
		RESIGNAL;
	END;

	SELECT GROUP_CONCAT(partition_name SEPARATOR ',')
	INTO l_partitions
	FROM information_schema.partitions
	WHERE table_schema = Database()
	AND table_name = in_tableName
	ORDER BY partition_ordinal_position
	;

	RETURN l_partitions;
END$$





/*
 * Partition 된 table 을 partition 없이 재구성.  data 는 그대로 유지
 */
CREATE PROCEDURE unPartition( tableName VARCHAR(255) )
    COMMENT 'Un-partition partitioned table.'
BEGIN
	ALTER TABLE bundle REMOVE PARTITIONING;
END$$


CREATE PROCEDURE dropPartition(
	in_tableName VARCHAR(255)
	, in_partName VARCHAR(255)
)
    COMMENT 'Remove partition from table.  Data in the partition will be removed.'
thisproc:
BEGIN
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE(CONCAT('SQL error happend in unPartition(', tableName, ').' ));
		RESIGNAL;
	END;

	IF FIND_IN_SET(in_partName, getPartitions(in_tableName)) = 0 THEN
		CALL addLogD(CONCAT('Can\'t find partition ', in_partName, ' in table ', in_tableName));
		LEAVE thisproc; 
	END IF;

	ALTER TABLE in_tableName DROP PARTITION in_partName;
END$$


CREATE FUNCTION getSecondsFromTime(t TIME(2))
	RETURNS DECIMAL(9,2)
	NO SQL
BEGIN
	RETURN SECOND(t) + MICROSECOND(t) / 1000000.0;
END$$


/*
 * http://stackoverflow.com/questions/14950466/how-to-split-the-name-string-in-mysql
 * https://blog.fedecarg.com/2009/02/22/mysql-split-string-function/
 */
CREATE FUNCTION splitString (
	s VARCHAR(1024)
	, del CHAR(1)
	, i INT
)
	RETURNS VARCHAR(1024)
	DETERMINISTIC -- always returns same results for same input parameters
	COMMENT 'Returns N-th token after splitting given string with delimiter'
BEGIN
	-- get max number of items
	DECLARE n INT DEFAULT LENGTH(s) - LENGTH(REPLACE(s, del, '')) + 1;
	DECLARE result VARCHAR(255);

	IF i > n THEN
		RETURN NULL ;
	END IF;

	SET result = SUBSTRING_INDEX(SUBSTRING_INDEX(s, del, i) , del , -1 ) ;        
	IF result = '' THEN
		RETURN NULL;
	END IF;

	RETURN result;
END$$


/*
 * https://www.thingy-ma-jig.co.uk/blog/10-07-2008/generate-random-string-mysql
 */
CREATE FUNCTION randomString (in_length SMALLINT)
	RETURNS VARCHAR(128)
	COMMENT 'Returns randomly generated string with given length.'
BEGIN
	-- RETURN SUBSTRING(MD5(RAND()) FROM 1 FOR in_length);	-- 32 char max
	RETURN SUBSTRING(sha2(RAND(), 512) FROM 1 FOR in_length);	-- 128 char max
END$$


/*
 * http://stackoverflow.com/questions/304461/generate-an-integer-sequence-in-mysql
 */
CREATE PROCEDURE selectTableWithGeneratedRow(in_table VARCHAR(255))
	COMMENT 'Forces attach a row id column'
BEGIN
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE('SQL error happend in selectTableWithGeneratedRow.');
		RESIGNAL;
	END;

	SET @stmt = CONCAT('
		SELECT
		@row := @row + 1 as row, t.*
		FROM ', in_table, ' t, (SELECT @row := 0) r;');
	CALL executeStatement(@stmt);
END$$



CREATE PROCEDURE copyAsTemporaryTable(
	in_src VARCHAR(255)		-- source table name
	, in_tmp VARCHAR(255)	-- temporary table name
	, in_copyRows BOOLEAN	-- whether to copy the contens.
)
	COMMENT 'Copy table into temporary table'
BEGIN
	DECLARE l_limitStmt VARCHAR(255) DEFAULT IF(in_copyRows, '', ' LIMIT 0');
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE('SQL error happend in copyAsTemporaryTable.');
		RESIGNAL;
	END;

	CALL executeStatement(CONCAT(
		'DROP TEMPORARY TABLE IF EXISTS ', in_tmp, ';\n'));

	CALL executeStatement(CONCAT(
    	'CREATE TEMPORARY TABLE ', in_tmp, ' ENGINE=MEMORY\n',
    	'AS SELECT * FROM ', in_src, l_limitStmt, ';\n'))
	;
END$$


/*
	-- Temporary Table 을 일반 table 로 복사 : e.g
    DROP TABLE IF EXISTS tts_stepSummary;
    CREATE TABLE tts_stepSummary ENGINE=MEMORY
    AS SELECT * FROM tt_stepSummary
    ;
 */
CREATE PROCEDURE copyAsNormalTable(
	in_src VARCHAR(255)		-- source table name
	, in_tgt VARCHAR(255)		-- target table name
	, in_copyRows BOOLEAN	-- whether to copy the contens.
)
	COMMENT 'Copy table into normal table'
BEGIN
	DECLARE l_limitStmt VARCHAR(255) DEFAULT IF(in_copyRows, '', ' LIMIT 0');
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE('SQL error happend in copyAsNormalTable.');
		RESIGNAL;
	END;

	CALL executeStatement(CONCAT(
		'DROP TABLE IF EXISTS ', in_tgt, ';\n'));

	CALL executeStatement(CONCAT(
    	'CREATE TABLE ', in_tgt, ' ENGINE=MEMORY\n',
    	'AS SELECT * FROM ', in_src, l_limitStmt, ';\n'));
END$$




DELIMITER ;
