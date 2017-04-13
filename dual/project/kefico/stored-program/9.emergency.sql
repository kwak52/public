USE kefico

DROP TRIGGER IF EXISTS emergencyRecoveryStatus_au;
DROP PROCEDURE IF EXISTS emergencyFillRecovery;
DROP PROCEDURE IF EXISTS emergencyAbortFillRecovery;

DELIMITER $$

CREATE TRIGGER emergencyRecoveryStatus_au AFTER UPDATE ON emergencyRecoveryStatus FOR EACH ROW
proc_label:
BEGIN
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE('SQL error happend in emergencyRecoveryStatus_au.');
		RESIGNAL;
	END;

	/*
	 * udpate 후, 임의의 ccs 가 recovery data 전송을 마쳤으면 
	 * - 다른 ccs 도 모두 마쳤는지 검사해서 emergency recovery 종료 순간 결정
	 */
    IF new.total = new.done THEN
		IF EXISTS(
			SELECT NULL
			FROM emergencyRecoveryStatus
			WHERE total IS NULL OR total <> done
		) THEN
			-- 다른 CCS 장비에서 더 처리할 작업이 남은 경우.
			LEAVE proc_label;
		ELSE
			UPDATE command SET val = 'N' WHERE name = 'EmergencyRecoveryMode';
		END IF;
	
	END IF;
END$$


/*
 * Database 가 특정 시점(일주일 이내)으로 roll back 하고 난 후, 
 * 그 시점부터 현재까지 backup 에 포함되지 않은 내용을 recovery 하기 위한 routine
 */
CREATE PROCEDURE emergencyFillRecovery(
	in_startDay DATE	-- date after last db back
)
	COMMENT 'Fills ccs local backup test result after database rollback.'
BEGIN
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE('SQL error happend in emergencyFillRecovery.');
		RESIGNAL;
	END;

	TRUNCATE emergencyRecoveryStatus;

	INSERT INTO emergencyRecoveryStatus(type, clientId, total, done)
	SELECT 'CCS', id, NULL, 0
	FROM ccs
	;

	INSERT INTO command(name, val, type, comment)
	VALUES('EmergencyRecoveryMode', 'Y', 'YESNO', 'Recovery mode')
	ON DUPLICATE KEY UPDATE
		val = 'Y'
	;

END$$


/*
 * Recovery 가 관리자의 판단하에, 강제 종료시킴.
 * - emergencyFillRecovery 수행시, 모든 ccs 에 대해 recovery 명령을 내리나,
 *	현재 시점에 실제 존재하지 않는 ccs 도 있을 수 있으므로, 이런 ccs 는 recovery 명령에 응답하지 않는다.
 *  이런 상황에서는 trigger emergencyRecoveryStatus_au 에 의한 recovery 자동 종료가 되지 않으므로 
 *	다음 procedure 를 이용해서 강제 종료한다.
 */
CREATE PROCEDURE emergencyAbortFillRecovery()
	COMMENT 'After human/manual verification of recovery finish, finishes recovery process.'
BEGIN
	TRUNCATE emergencyRecoveryStatus;
	DELETE FROM command WHERE name = 'EmergencyRecoveryMode';
END$$



DELIMITER ;

