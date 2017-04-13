/*
 * SEE ALSO ../bash/backup-restore.sh
 *
 * backup 을 text 로 생성하고, (Backup To Text : btt)
 * text file 을 browsing 할 수 있도록 local PC 에 win32-mysql or sqlite 을 생성한 후,
 * 이 내용을 browsing 할 수 있도록 하는 방법은 어떨까?
 *	- local pc 의 mysql 도 server 와 동일한 schema 를 이용해서 db 생성
 *	- SELECT .. INTO OUTFILE 구문은 mysql priviledge 에 의해서 정해진 directory 에만 write 가능하므로 사용에 약간 번거로움.
 *	- backup routine (btt_day 등)을 shell 에서 호출하여 redirection 을 통해서 일자별 단일 text backup file 생성
 *	- text backup file 은 table 단위로 쉽게 잘라 낼 수 있도록 formatting
 *		# TABLE <table1>
 *		SELECT table1 결과문
 *		# TABLE <table2>
 *		SELECT table2 결과문
 */



USE kefico

DROP PROCEDURE IF EXISTS btt_measure;
DROP PROCEDURE IF EXISTS btt_day;

DELIMITER $$



CREATE PROCEDURE btt_measure(measureId INT)
	COMMENT	'backup to text : measure'
BEGIN
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE('SQL error happend in btt_measure.');
		RESIGNAL;
	END;

	CALL showBundle(measureId);
END$$


CREATE PROCEDURE btt_day(in_day DATE)
	COMMENT	'backup to text : measure'
BEGIN
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE('SQL error happend in btt_day.');
		RESIGNAL;
	END;

	SELECT CONCAT('#\n'
				, '# DATE: ', in_day, '\n'
				, '# Backup created on: ', now()
				, '#\n'
	) AS DECRIPTION;

	CREATE TEMPORARY TABLE IF NOT EXISTS tt_mr ENGINE=MEMORY
	AS(		-- tt_mr : measure in Range
		SELECT id, day, time, duration, pdvId, ccsId, ok, batchName, m.type
		FROM measure m
		WHERE day=in_day
	);

	
	-- SELECT '# --------- pdv info';
	-- SELECT CONCAT('# ', getColumnNamesFromTable('pdv'));

	SELECT *
	FROM pdv p JOIN pdvGroup g ON(p.groupId = g.id)
	WHERE p.id IN ( SELECT DISTINCT pdvId FROM tt_mr )
	; 


	SELECT '# --------- measure info';
	SELECT CONCAT('# ', GROUP_CONCAT(column_name SEPARATOR ' | '))
	FROM information_schema.columns WHERE table_name='measure';
	SELECT * FROM tt_mr ORDER BY id;

	SELECT '';
	SELECT '# --------- bundles info';

	SELECT *
	FROM bundle
	WHERE day=in_day
	ORDER by measureId, stepId
	;
END$$



DELIMITER ;


