USE kefico

DROP FUNCTION IF EXISTS getPdvId;
DROP PROCEDURE IF EXISTS getTestDetails;
DROP PROCEDURE IF EXISTS getTestDetailsFromPdvId;


DELIMITER $$


/*
 * e.g getPdvId('9001270003', 'CH', false, 1);
 */
CREATE FUNCTION getPdvId(
	in_partNumber TEXT
	, in_pamType TEXT
	, in_isProduction BOOLEAN
	, in_version INT
)
	RETURNS INT UNSIGNED
	COMMENT	'Get pdv id with given arguments.'
BEGIN
	DECLARE l_pdvId INT DEFAULT null;
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE('SQL error happend in getPdvId.');
		RESIGNAL;
	END;


	-- SELECT pdv.id, pdv.partNumber, pdv.pamType, pdv.pamGroup, pdv.isProduction, tl.productNumber
	SELECT pdv.id INTO l_pdvId
	FROM pdv
	JOIN pdvTestList tl ON pdv.testListId = tl.id
	WHERE pdv.partNumber = in_partNumber
		AND pdv.pamType = in_pamType
		AND pdv.isProduction = in_isProduction
		AND tl.version = in_version
		-- AND tl.productNumber = 
	;

	RETURN l_pdvId;
END$$


/*
 * e.g getTestDetails('9001270003', 'CH', false);
 */
CREATE PROCEDURE getTestDetails(
	in_partNumber TEXT
	, in_pamType TEXT
	, in_isProduction BOOLEAN
)
	COMMENT	'Get test details with given arguments.'
BEGIN
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE('SQL error happend in getTestDetails.');
		RESIGNAL;
	END;

	SELECT pdv.id, tl.productNumber, tl.product, tl.productType, tl.fileStem, tl.version, pdv.pamGroup,
			tl.testListPathHint_gc AS pathHint
	FROM pdv
	JOIN pdvTestList tl ON pdv.testListId = tl.id
	WHERE pdv.partNumber = in_partNumber
		AND pdv.pamType = in_pamType
		AND pdv.isProduction = in_isProduction
	;
END$$



/*
 * e.g getTestDetails('9001270003', 'CH', false);
 */
CREATE PROCEDURE getTestDetailsFromPdvId(
	in_pdvId INT
)
	COMMENT	'Get test details with given pdvId.'
BEGIN
	DECLARE CONTINUE HANDLER FOR SQLEXCEPTION
	BEGIN
		CALL addLogE('SQL error happend in getTestDetailsFromPdvId.');
		RESIGNAL;
	END;

	SELECT
		p.id
		, p.partNumber
		, p.pamType
		, p.isProduction
		, t.productNumber
		, t.product
		, t.productType
		, t.version
		, t.fileStem
		, t.testListPathHint_gc
	FROM pdv p
	JOIN pdvTestList t ON p.testListId = t.id
	WHERE p.id = in_pdvId
	;
END$$



DELIMITER ;

