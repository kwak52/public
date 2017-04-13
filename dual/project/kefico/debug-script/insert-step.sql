USE kefico

DROP TABLE IF EXISTS t1;
DROP TABLE IF EXISTS t2;

CREATE TABLE t1 (
	id INT PRIMARY KEY NOT NULL AUTO_INCREMENT
	, pdvId INT
	, step INT
	, isActive BOOLEAN
	, revision INT
	, val VARCHAR(255)
);
INSERT INTO t1(pdvId, step, isActive, revision, val)
VALUES
	(1, 1, True, 0, "one")
	, (1, 2, True, 0, "two")
	, (1, 3, True, 0, "three")
	, (1, 4, True, 0, "four")
	, (2, 1, True, 0, "one")
	, (2, 2, True, 0, "two")
	, (2, 3, True, 0, "three")
	, (2, 4, True, 0, "four")
;



CREATE TABLE t2 (
	id INT PRIMARY KEY NOT NULL AUTO_INCREMENT
	, pdvId INT
	, step INT
	, isActive BOOLEAN
	, revision INT
	, val VARCHAR(255)
);
INSERT INTO t2(pdvId, step, isActive, revision, val)
VALUES
	(1, 3, True, 0, "three")			-- remain same
	, (1, 4, True, 0, "four-modified")	-- modified
	, (1, 5, True, 0, "five")			-- added
	, (1, 6, True, 0, "six")
;

SET @pdvId = 1;




-- Modification, first
DROP TEMPORARY TABLE IF EXISTS tt_step_natural_join;
CREATE TEMPORARY TABLE tt_step_natural_join ENGINE=MEMORY
AS
	SELECT t1.id, t1.pdvId, t1.step
		, True AS isActive
		, t1.revision+1 AS revision
		, t2.val
	FROM t1
	JOIN t2 ON t1.pdvId = t2.pdvId AND t1.step = t2.step
	WHERE t1.val <> t2.val
;

/*
UPDATE t1
SET isActive=False
WHERE id IN ( SELECT id FROM tt_step_natural_join )
;
*/

UPDATE t1
SET isActive=False
WHERE id IN (
	SELECT id FROM tt_step_natural_join
	UNION ALL
		(SELECT id FROM(		-- http://stackoverflow.com/questions/4471277/mysql-delete-from-with-subquery-as-condition
			SELECT t1.id FROM t1
			LEFT JOIN t2 ON t1.pdvId = t2.pdvId AND t1.step = t2.step
			WHERE t2.id IS NULL
			AND t1.pdvId = @pdvId
			) x
		)
)
;


INSERT INTO t1(pdvId, step, isActive, revision, val)
SELECT pdvId, step, isActive, revision, val
FROM tt_step_natural_join
;


--
-- New addition
--
INSERT INTO t1(pdvId, step, isActive, revision, val)
SELECT t2.pdvId, t2.step, True AS isActive, 0 AS revision, t2.val
FROM t1
RIGHT JOIN t2 ON t1.pdvId = t2.pdvId AND t1.step = t2.step
WHERE t1.id IS NULL
;


