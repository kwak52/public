use kefico

/*
 * 5000 개의 step data 에 대해서 10개의 dimension 항목을 random 으로 붙이고자 한다.
 * 	1. 10개의 dimension 항목을 random 으로 5000 개 생성한다.  이때 row id 는 순차적으로 생성
 * 	1. 생성된 random 5000개의 dimension 항목을 row id 와 step table 의 id 를 이용해서 join 한다.
 */

SET @row1=0, @row2=0;
SELECT @row1 := @row1 + 1 as row, r.name, r.id
FROM (
	SELECT * FROM generator_1k, dimension d		-- 1024 * 10 random dimensions
	ORDER BY RAND()
) r


SET @row2=0;
SELECT @row2 := @row2 + 1 as row, r.min, r.max
FROM (
	SELECT * FROM generator_1k, sampleMinMax 	-- 1024 * 10 random min/max
	ORDER BY RAND()
) r



SET @row1=0, @row2=0;
SELECT t.*
	, CASE getRandomN(0, 3) % 4
		WHEN 0 THEN 'R'
		WHEN 1 THEN 'I'
		WHEN 2 THEN 'H'
		WHEN 3 THEN 'M'
		ELSE NULL
	END AS displayType
	FROM (
		SELECT row1, min, max, dimId
		FROM (
			SELECT
				@row1 := @row1 + 1 as row1,
				rd.id AS dimId
			FROM (
				SELECT * FROM generator_1k, dimension		-- 1024 * 10 random dimensions
				ORDER BY RAND()
			) rd
		) rr1
		JOIN
		(
			SELECT
				@row2 := @row2 + 1 as row2,
				rmm.min, rmm.max
			FROM (
				SELECT * FROM generator_1k, sampleMinMax 	-- 1024 * 10 random min/max
				ORDER BY RAND()
			) rmm
		) rr2
		ON rr1.row1 = rr2.row2
	) t


