-- http://mysqlserverteam.com/generated-columns-in-mysql-5-7-5/

CREATE TABLE squareroot (
	val DOUBLE
	, V_squareroot DOUBLE AS (sqrt(val))
);

INSERT INTO squareroot(val) VALUES(2),(3),(4),(5);
