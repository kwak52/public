USE kefico;

DROP EVENT IF EXISTS evt_beforeDateLine;
DROP EVENT IF EXISTS evt_onDateLine;
DROP EVENT IF EXISTS evt_afterDateLine;


DELIMITER $$

CREATE EVENT evt_beforeDateLine
ON SCHEDULE EVERY 1 DAY
-- STARTS CURRENT_TIMESTAMP + INTERVAL 1 DAY
-- STARTS '2016-08-01 23:00:00'	-- from 11:00 PM, today
-- 9 시간 느린 UTC 로 표시되나, 올바르게 동작함. (mysql 에서 mysql.event table 내용 검색시)
STARTS CURDATE() + INTERVAL 1 DAY - INTERVAL 1 HOUR		-- from 11:00 PM, today
	ON COMPLETION PRESERVE
ENABLE
COMMENT 'event handler before date change'
DO BEGIN
	CALL addLogD(CONCAT('EventScheduler evt_beforeDateLine on ', now()));
	CALL rollOutBeforeDateLine();
END$$




CREATE EVENT evt_onDateLine
ON SCHEDULE EVERY 1 DAY
STARTS CURDATE() + INTERVAL 1 DAY + INTERVAL 1 MINUTE		-- from 00:01 AM, tomorrow
	ON COMPLETION PRESERVE
ENABLE
COMMENT 'event handler on date change'
DO BEGIN
	CALL addLogD(CONCAT('EventScheduler evt_onDateLine on ', now()));
	CALL _updateDailySectionalPositionalSummary( CURDATE() - INTERVAL 1 DAY );
END$$




CREATE EVENT evt_afterDateLine
ON SCHEDULE EVERY 1 DAY
STARTS CURDATE() + INTERVAL 1 DAY + INTERVAL 1 HOUR		-- from 01:00 AM, tomorrow
	ON COMPLETION PRESERVE
ENABLE
COMMENT 'event handler after date change'
DO BEGIN
	CALL addLogD(CONCAT('EventScheduler evt_afterDateLine on ', now()));
	CALL rollOutAfterDateLine();
END$$



DELIMITER ;

/*
mysql> show variables like 'event%'
+-----------------+-------+
| Variable_name   | Value |
+-----------------+-------+
| event_scheduler | OFF   |
+-----------------+-------+
1 row in set (0.01 sec)

mysql> set global event_scheduler='ON';
Query OK, 0 rows affected (0.01 sec)

mysql> show variables like 'event%'
+-----------------+-------+
| Variable_name   | Value |
+-----------------+-------+
| event_scheduler | ON    |
+-----------------+-------+
1 row in set (0.01 sec)

mysql> show processlist;
+-----+-----------------+----------------------+--------+---------+-------+------------------------+------------------+
| Id  | User            | Host                 | db     | Command | Time  | State                  | Info             |
+-----+-----------------+----------------------+--------+---------+-------+------------------------+------------------+
| 565 | kwak            | localhost            | kefico | Query   |     0 | starting               | show processlist |
| 566 | event_scheduler | localhost            | NULL   | Daemon  |     5 | Waiting on empty queue | NULL             |
+-----+-----------------+----------------------+--------+---------+-------+------------------------+------------------+

mysql> select * from mysql.event\G
*/


