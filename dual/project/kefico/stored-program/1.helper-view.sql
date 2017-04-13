USE kefico;


/*
 * http://use-the-index-luke.com/blog/2011-07-30/mysql-row-generator
 */

CALL registerViewInfo('generator_4', 'Number generator [0..3]');
CREATE OR REPLACE VIEW generator_4 AS
SELECT 0 n
	UNION ALL SELECT 1  UNION ALL SELECT 2  UNION ALL SELECT 3
ORDER BY n
;


CALL registerViewInfo('generator_10', 'Number generator [0..9]');
CREATE OR REPLACE VIEW generator_10 AS
SELECT 0 n 
	UNION ALL SELECT 1  UNION ALL SELECT 2
	UNION ALL SELECT 3   UNION ALL SELECT 4  UNION ALL SELECT 5
	UNION ALL SELECT 6   UNION ALL SELECT 7  UNION ALL SELECT 8
	UNION ALL SELECT 9
ORDER BY n
;

CALL registerViewInfo('generator_16', 'Number generator [0..15]');
CREATE OR REPLACE VIEW generator_16 AS
SELECT 0 n 
	UNION ALL SELECT 1  UNION ALL SELECT 2
	UNION ALL SELECT 3   UNION ALL SELECT 4  UNION ALL SELECT 5
	UNION ALL SELECT 6   UNION ALL SELECT 7  UNION ALL SELECT 8
	UNION ALL SELECT 9   UNION ALL SELECT 10 UNION ALL SELECT 11
	UNION ALL SELECT 12  UNION ALL SELECT 13 UNION ALL SELECT 14
	UNION ALL SELECT 15
ORDER BY n
;

CALL registerViewInfo('generator_64', 'Number generator [0..63]');
CREATE OR REPLACE VIEW generator_64 AS
SELECT ( hi.n * 4 + lo.n ) AS n
FROM generator_4 lo, generator_16 hi
ORDER BY n
;

CALL registerViewInfo('generator_256', 'Number generator [0..255]');
CREATE OR REPLACE VIEW generator_256 AS
SELECT ( hi.n * 16 + lo.n ) AS n
FROM generator_16 lo, generator_16 hi
ORDER BY n
;

CALL registerViewInfo('generator_1k', 'Number generator [0..1023]');
CREATE OR REPLACE VIEW generator_1k AS
SELECT ( hi.n * 4 + lo.n ) AS n
FROM generator_4 lo, generator_256 hi
ORDER BY n
;

CALL registerViewInfo('generator_4k', 'Number generator [0..4095]');
CREATE OR REPLACE VIEW generator_4k AS
SELECT ( ( hi.n << 8 ) | lo.n ) AS n
FROM generator_256 lo, generator_16 hi
ORDER BY n
;

CALL registerViewInfo('generator_64k', 'Number generator [0..65535]');
CREATE OR REPLACE VIEW generator_64k AS
SELECT ( ( hi.n << 8 ) | lo.n ) AS n
FROM generator_256 lo, generator_256 hi
ORDER BY n
;

CALL registerViewInfo('generator_1m', 'Number generator [0..1048575]');
CREATE OR REPLACE VIEW generator_1m AS
SELECT ( ( hi.n << 16 ) | lo.n ) AS n
FROM generator_64k lo, generator_16 hi
ORDER BY n
;




CALL registerViewInfo('generator_100', 'Number generator [0..99]');
CREATE OR REPLACE VIEW generator_100 AS
SELECT ( hi.n * 10 + lo.n ) AS n
FROM generator_10 lo, generator_10 hi
ORDER BY n
;

CALL registerViewInfo('generator_1000', 'Number generator [0..999]');
CREATE OR REPLACE VIEW generator_1000 AS
SELECT ( hi.n * 100 + lo.n ) AS n
FROM generator_100 lo, generator_10 hi
ORDER BY n
;

CALL registerViewInfo('generator_10000', 'Number generator [0..9999]');
CREATE OR REPLACE VIEW generator_10000 AS
SELECT ( hi.n * 1000 + lo.n ) AS n
FROM generator_1000 lo, generator_10 hi
ORDER BY n
;

CALL registerViewInfo('generator_100000', 'Number generator [0..99999]');
CREATE OR REPLACE VIEW generator_100000 AS
SELECT ( hi.n * 10000 + lo.n ) AS n
FROM generator_10000 lo, generator_10 hi
ORDER BY n
;

CALL registerViewInfo('generator_1000000', 'Number generator [0..999999]');
CREATE OR REPLACE VIEW generator_1000000 AS
SELECT ( hi.n * 100000 + lo.n ) AS n
FROM generator_100000 lo, generator_10 hi
ORDER BY n
;


CALL registerViewInfo('generator_alpha', 'Character generator [\'a\'..\'z\']');
CREATE OR REPLACE VIEW generator_alpha AS 
SELECT 'a' n 
	UNION ALL SELECT 'b'
	UNION ALL SELECT 'c'
	UNION ALL SELECT 'd'
	UNION ALL SELECT 'e'
	UNION ALL SELECT 'f'
	UNION ALL SELECT 'g'
	UNION ALL SELECT 'h'
	UNION ALL SELECT 'i'
	UNION ALL SELECT 'j'
	UNION ALL SELECT 'k'
	UNION ALL SELECT 'l'
	UNION ALL SELECT 'm'
	UNION ALL SELECT 'n'
	UNION ALL SELECT 'o'
	UNION ALL SELECT 'p'
	UNION ALL SELECT 'q'
	UNION ALL SELECT 'r'
	UNION ALL SELECT 's'
	UNION ALL SELECT 't'
	UNION ALL SELECT 'u'
	UNION ALL SELECT 'v'
	UNION ALL SELECT 'w'
	UNION ALL SELECT 'x'
	UNION ALL SELECT 'y'
	UNION ALL SELECT 'z'
ORDER BY n
;



CALL registerViewInfo('generator_ALPHA', 'Character generator [\'A\'..\'Z\']');
CREATE OR REPLACE VIEW generator_ALPHA AS
SELECT 'A' n 
	UNION ALL SELECT 'B'
	UNION ALL SELECT 'C'
	UNION ALL SELECT 'D'
	UNION ALL SELECT 'E'
	UNION ALL SELECT 'F'
	UNION ALL SELECT 'G'
	UNION ALL SELECT 'H'
	UNION ALL SELECT 'I'
	UNION ALL SELECT 'J'
	UNION ALL SELECT 'K'
	UNION ALL SELECT 'L'
	UNION ALL SELECT 'M'
	UNION ALL SELECT 'N'
	UNION ALL SELECT 'O'
	UNION ALL SELECT 'P'
	UNION ALL SELECT 'Q'
	UNION ALL SELECT 'R'
	UNION ALL SELECT 'S'
	UNION ALL SELECT 'T'
	UNION ALL SELECT 'U'
	UNION ALL SELECT 'V'
	UNION ALL SELECT 'W'
	UNION ALL SELECT 'X'
	UNION ALL SELECT 'Y'
	UNION ALL SELECT 'Z'
ORDER BY n
;
