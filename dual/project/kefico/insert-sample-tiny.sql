USE kefico

INSERT INTO user(username, password, email)
VALUES
	  ('User1',	'kefico1',	'guest@dualsoft.co.kr')
	, ('User2',	'kefico2',	'guest@dualsoft.co.kr')
	, ('User3',	'kefico3',	'guest@dualsoft.co.kr')
	, ('User4',	'kefico4',	'guest@dualsoft.co.kr')
	, ('User5',	'kefico5',	'guest@dualsoft.co.kr')
	
;

SELECT min(id), max(id)
INTO @minUserId, @maxUserId
FROM user
;



INSERT INTO sampleMinMax(min, max)
VALUES
	(-0.01, +0.01)
	, (-0.1, +0.1)
	, (-1, +1)
	, (-5, +5)
	, (-10, +10)
	, (-50, +50)
	, (-100, +100)
	, (-500, +500)
	, (-1000, +1000)
	, (-5000, +5000)
	, (-10000, +10000)
	;

SELECT min(id), max(id)
INTO @minMMId, @maxMMId
FROM sampleMinMax
;



INSERT INTO pdvGroup(productGroup, productModel, comment)
VALUES
	('MOTRONIC / M', 'SIM2K-260', 'comment')
	, ('MOTRONIC / M', 'SIM2K-261', 'comment')
	, ('MOTRONIC / M', 'SIM2K-262', 'comment')
	, ('MOTRONIC / M', 'SIM2K-263', 'comment')
	, ('MOTRONIC / M', 'SIM2K-264', 'comment')
	, ('MOTRONIC / M', 'SIM2K-265', 'comment')
	, ('MOTRONIC / M', 'SIM2K-266', 'comment')
	, ('MOTRONIC / M', 'SIM2K-267', 'comment')
	, ('MOTRONIC / M', 'SIM2K-268', 'comment')
	, ('MOTRONIC / M', 'SIM2K-269', 'comment')
	, ('MOTRONIC / M', 'SIM2K-270', 'comment')
	, ('MOTRONIC / M', 'SIM2K-271', 'comment')
	, ('MOTRONIC / M', 'SIM2K-272', 'comment')
	, ('MOTRONIC / M', 'SIM2K-510', 'comment')
	, ('MOTRONIC / M', 'CPEDG1.1', 'comment')
	, ('MOTRONIC / M', 'CPEDG1.2', 'comment')
	, ('MOTRONIC / M', 'CPEDG1.3', 'comment')
	, ('MOTRONIC / M', 'CPEDG2.1', 'comment')
	, ('MOTRONIC / M', 'CPEDG2.2', 'comment')
	, ('MOTRONIC / M', 'CPEDG2.3', 'comment')
	, ('MOTRONIC / M', 'CPEDG2.4', 'comment')

	, ('MOTRONIC / M', 'CPDGSH1.1', 'comment')
	, ('MOTRONIC / M', 'CPDGSH1.2', 'comment')
	, ('MOTRONIC / M', 'CPDGSH1.3', 'comment')
	, ('MOTRONIC / M', 'CPDGSH1.4', 'comment')
	, ('MOTRONIC / M', 'CPDGSH1.5', 'comment')
	, ('MOTRONIC / M', 'CPDGSH1.6', 'comment')

	, ('GETRIEBESTEUERUNG / GS', 'CPTSH1.1', 'comment')
	, ('GETRIEBESTEUERUNG / GS', 'CPTSH1.2', 'comment')
	, ('GETRIEBESTEUERUNG / GS', 'CPTSH1.3', 'comment')
	;


SELECT min(id), max(id)
INTO @minPdvGroupId, @maxPdvGroupId
FROM pdvGroup
;

INSERT INTO pdvTestList(productNumber, product, productType, version, fileStem, createdDt)
VALUES
	  ('9001270001', 'MM', 'XX', 1,		'p9001270001', '2016-08-09 23:15:00')		/* 1 */
	, ('9001270001', 'MM', 'XX', 2,		'p9001270001', '2016-08-09 23:15:00')		/* 2 */
	, ('9001270001', 'MM', 'XX', 3,		'p9001270001', '2016-08-09 23:15:00')		/* 3 */
	, ('9001270001', 'MM', 'XF', 1,		'p9001270001', '2016-08-09 23:15:00')		/* 4 */
	, ('9001270001', 'MM', 'XF', 2,		'p9001270001', '2016-08-09 23:15:00')		/* 5 */
	, ('9001270001', 'MM', 'XF', 3,		'p9001270001', '2016-08-09 23:15:00')		/* 6 */
	, ('9001270001', 'MM', 'SF', 1,		'p9001270001', '2016-08-09 23:15:00')		/* 7 */
	, ('9001270001', 'MM', 'MF', 1,		'p9001270001', '2016-08-09 23:15:00')		/* 8 */
	, ('9001270002', 'MX', 'MF', 1,		'p9001270002', '2016-08-09 23:15:00')		/* 9 */
	, ('9001270003', 'MX', 'SF', 1,		'p9001270003', '2016-08-09 23:15:00')		/* 10 */
	, ('9001270003', 'MM', 'XX', 1,		'p9001270003', '2017-02-25 23:15:00')		/* 11 */
	, ('9001270003', 'MM', 'XX', 2,		'p9001270003', '2017-02-25 23:15:00')		/* 12 */
	, ('9001270003', 'MM', 'XX', 3,		'p9001270003', '2017-02-25 23:15:00')		/* 13 */
	, ('9001270003', 'MM', 'XF', 1,		'p9001270003', '2017-02-25 23:15:00')		/* 14 */
	, ('9001270003', 'MM', 'XF', 77,	'p9001270003', '2017-02-25 23:15:00')		/* 15 */
	, ('9001270004', 'MX', 'XF', 1, 	'p9001270004', '2016-08-09 23:15:00')		/* 16 */
	, ('9001270005', 'MX', 'XX', 1, 	'p9001270005', '2016-08-09 23:15:00')		/* 17 */
	, ('9001270014', 'MM', 'XF', 1, 	'p9001270003', '2017-02-25 23:15:00')		/* 18 */
	, ('9001270014', 'MM', 'XF', 77,	'p9001270003', '2017-02-25 23:15:00')		/* 19 */
	, ('9001270014', 'MM', 'XX', 1, 	'p9001270003', '2017-02-25 23:15:00')		/* 20 */
	, ('9001270067', 'MM', 'XX', 1, 	'p9001270510', '2017-02-25 23:15:00')		/* 21 */
	, ('9001270510', 'MM', 'XX', 1, 	'p9001270510', '2017-02-25 23:15:00')		/* 22 */
	, ('9001270510', 'MM', 'XX', 2, 	'p9001270510', '2017-02-25 23:15:00')		/* 23 */
	, ('9001270510', 'MM', 'XF', 1, 	'p9001270510', '2017-02-25 23:15:00')		/* 24 */
	, ('9001270999', 'MM', 'XX', 1, 	'p9001270003', '2017-02-25 23:15:00')		/* 25 */
	, ('9001270999', 'MM', 'XF', 1, 	'p9001270003', '2017-02-25 23:15:00')		/* 26 */
	, ('9001270999', 'MM', 'XF', 77,	'p9001270003', '2017-02-25 23:15:00')		/* 27 */
	, ('9001270999', 'MM', 'SF', 1,		'p9001270003', '2017-02-25 23:15:00')		/* 28 */
	, ('9001270999', 'MM', 'SF', 2,		'p9001270003', '2017-02-25 23:15:00')		/* 29 */
	
	


	, ('9001200066', 'MM', 'XX', 0,	'p9001200066', '2017-02-25 23:15:00')		/* 30 */
	, ('9001200067', 'MM', 'XX', 0,	'p9001200066', '2017-02-25 23:15:00')		/* 31 */
	, ('9001280029', 'MM', 'XX', 0,	'p9001200066', '2017-02-25 23:15:00')		/* 32 */
	, ('9001200068', 'MM', 'XX', 0,	'p9001200066', '2017-02-25 23:15:00')		/* 33 */
	, ('9001280034', 'MM', 'XX', 0,	'p9001200066', '2017-02-25 23:15:00')		/* 34 */
	, ('9001200069', 'MM', 'XX', 0,	'p9001200066', '2017-02-25 23:15:00')		/* 35 */
	, ('9001280032', 'MM', 'XX', 0,	'p9001200066', '2017-02-25 23:15:00')		/* 36 */
	, ('9001200079', 'MM', 'XX', 0,	'p9001200066', '2017-02-25 23:15:00')		/* 37 */
	, ('9001200080', 'MM', 'XX', 0,	'p9001200066', '2017-02-25 23:15:00')		/* 38 */
	, ('900120XX68', 'MM', 'XX', 0,	'p9001200066', '2017-02-25 23:15:00')		/* 49 */
	, ('900120AI66', 'MM', 'XX', 0,	'p9001200066', '2017-02-25 23:15:00')		/* 40 */
	, ('900120AI67', 'MM', 'XX', 0,	'p9001200066', '2017-02-25 23:15:00')		/* 41 */
	, ('900120AI68', 'MM', 'XX', 0,	'p9001200066', '2017-02-25 23:15:00')		/* 42 */
	, ('900120AI69', 'MM', 'XX', 0,	'p9001200066', '2017-02-25 23:15:00')		/* 43 */
	, ('900120AI79', 'MM', 'XX', 0,	'p9001200066', '2017-02-25 23:15:00')		/* 44 */
	, ('900120AI80', 'MM', 'XX', 0,	'p9001200066', '2017-02-25 23:15:00')		/* 45 */
                                                                                    
	, ('9001200066', 'MM', 'XX', 1,	'p9001200066', '2017-02-25 23:15:00')		/* 46 */
	, ('9001200067', 'MM', 'XX', 1,	'p9001200066', '2017-02-25 23:15:00')		/* 47 */
	, ('9001280029', 'MM', 'XX', 1,	'p9001200066', '2017-02-25 23:15:00')		/* 48 */
	, ('9001200068', 'MM', 'XX', 1,	'p9001200066', '2017-02-25 23:15:00')		/* 49 */
	, ('9001280034', 'MM', 'XX', 1,	'p9001200066', '2017-02-25 23:15:00')		/* 50 */
	, ('9001200069', 'MM', 'XX', 1,	'p9001200066', '2017-02-25 23:15:00')		/* 51 */
	, ('9001280032', 'MM', 'XX', 1,	'p9001200066', '2017-02-25 23:15:00')		/* 52 */
	, ('9001200079', 'MM', 'XX', 1,	'p9001200066', '2017-02-25 23:15:00')		/* 53 */
	, ('9001200080', 'MM', 'XX', 1,	'p9001200066', '2017-02-25 23:15:00')		/* 54 */
	, ('900120XX68', 'MM', 'XX', 1,	'p9001200066', '2017-02-25 23:15:00')		/* 55 */
	, ('900120AI66', 'MM', 'XX', 1,	'p9001200066', '2017-02-25 23:15:00')		/* 56 */
	, ('900120AI67', 'MM', 'XX', 1,	'p9001200066', '2017-02-25 23:15:00')		/* 57 */
	, ('900120AI68', 'MM', 'XX', 1,	'p9001200066', '2017-02-25 23:15:00')		/* 58 */
	, ('900120AI69', 'MM', 'XX', 1,	'p9001200066', '2017-02-25 23:15:00')		/* 69 */
	, ('900120AI79', 'MM', 'XX', 1,	'p9001200066', '2017-02-25 23:15:00')		/* 60 */
	, ('900120AI80', 'MM', 'XX', 1,	'p9001200066', '2017-02-25 23:15:00')		/* 61 */

	
	;



SELECT min(id), max(id)
INTO @minPdvTestListId, @maxPdvTestListId
FROM pdvTestList
;



SET @partNumber = 9001270001;		-- pdvId
SET @isProduction = False;

INSERT INTO pdv(testListId, partNumber, pamType, isProduction, createdDt, revision, comment, dataConfig, dataVariant, changeNumber, groupId, userId, pamGroup)
	VALUES 
	  (9 , '9001270002', 'HT', 0,		'2017-02-26 4:15:00', 0, 'comment', 'SaW_T [10]', 'FABRIK', 'XXSAXXXX', 5, 5, 'Alle-CCS')
	, (9 , '9001270002', 'HT', 1,		'2017-02-26 4:15:00', 0, 'comment', 'SaW_T [10]', 'FABRIK', 'XXSAXXXX', 5, 5, 'Alle-CCS')
	, (9 , '9001270002', 'CT', 1,		'2017-02-26 4:15:00', 0, 'comment', 'SaW_T [10]', 'FABRIK', 'XXSAXXXX', 5, 5, 'Alle-CCS')
	, (11, '9001270003', 'HT', 0,		'2017-02-26 4:15:00', 0, 'comment', 'SaW_T [10]', 'FABRIK', 'XXSAXXXX', 5, 5, 'Alle-CCS')
	, (11, '9001270003', 'HT', 1,		'2017-02-26 4:15:00', 0, 'comment', 'SaW_T [10]', 'FABRIK', 'XXSAXXXX', 5, 5, 'Alle-CCS')
	, (11, '9001270003', 'FT', 0,		'2017-02-26 4:15:00', 0, 'comment', 'SaW_T [10]', 'FABRIK', 'XXSAXXXX', 5, 5, 'Alle-CCS')
	, (11, '9001270003', 'FT', 1,		'2017-02-26 4:15:00', 0, 'comment', 'SaW_T [10]', 'FABRIK', 'XXSAXXXX', 5, 5, 'Alle-CCS')
	, (11, '9001270013', 'CT', 0,		'2017-02-26 4:15:00', 0, 'comment', 'SaW_T [10]', 'FABRIK', 'XXSAXXXX', 5, 5, 'Alle-CCS')
	, (11, '9001270013', 'CT', 1,		'2017-02-26 4:15:00', 0, 'comment', 'SaW_T [10]', 'FABRIK', 'XXSAXXXX', 5, 5, 'Alle-CCS')
	, (17, '9001270013', 'HT', 0,		'2017-02-26 4:15:00', 0, 'comment', 'SaW_T [10]', 'FABRIK', 'XXSAXXXX', 5, 5, 'Alle-CCS')
	, (20, '9001270014', 'HT', 0,		'2017-02-26 4:15:00', 0, 'comment', 'SaW_T [10]', 'FABRIK', 'XXSAXXXX', 5, 5, 'Alle-CCS')
	, (20, '9001270014', 'HT', 1,		'2017-02-26 4:15:00', 0, 'comment', 'SaW_T [10]', 'FABRIK', 'XXSAXXXX', 5, 5, 'Alle-CCS')
	, (20, '9001270014', 'FT', 0,		'2017-02-26 4:15:00', 0, 'comment', 'SaW_T [10]', 'FABRIK', 'XXSAXXXX', 5, 5, 'Alle-CCS')
	, (20, '9001270014', 'FT', 1,		'2017-02-26 4:15:00', 0, 'comment', 'SaW_T [10]', 'FABRIK', 'XXSAXXXX', 5, 5, 'Alle-CCS')
	, (19, '9001270998', 'HT', 0,		'2017-02-26 4:15:00', 0, 'comment', 'SaW_T [10]', 'FABRIK', 'XXSAXXXX', 5, 5, 'Alle-CCS')
	, (16, '9001270998', 'HT', 1,		'2017-02-26 4:15:00', 0, 'comment', 'SaW_T [10]', 'FABRIK', 'XXSAXXXX', 5, 5, 'Alle-CCS')
	, (22, '9001270998', 'CT', 0,		'2017-02-26 4:15:00', 0, 'comment', 'SaW_T [10]', 'FABRIK', 'XXSAXXXX', 5, 5, 'Alle-CCS')
	, (25, '9001270999', 'HT', 0,		'2017-02-26 4:15:00', 0, 'comment', 'SaW_T [10]', 'FABRIK', 'XXSAXXXX', 5, 5, 'Alle-CCS')
	, (25, '9001270999', 'HT', 1,		'2017-02-26 4:15:00', 0, 'comment', 'SaW_T [10]', 'FABRIK', 'XXSAXXXX', 5, 5, 'Alle-CCS')
	, (25, '9001270999', 'FT', 0,		'2017-02-26 4:15:00', 0, 'comment', 'SaW_T [10]', 'FABRIK', 'XXSAXXXX', 5, 5, 'Alle-CCS')
	, (25, '9001270999', 'FT', 1,		'2017-02-26 4:15:00', 0, 'comment', 'SaW_T [10]', 'FABRIK', 'XXSAXXXX', 5, 5, 'Alle-CCS')
	, (25, '9001270999', 'CT', 0,		'2017-02-26 4:15:00', 0, 'comment', 'SaW_T [10]', 'FABRIK', 'XXSAXXXX', 5, 5, 'Alle-CCS')
	, (25, '9001270999', 'CT', 1,		'2017-02-26 4:15:00', 0, 'comment', 'SaW_T [10]', 'FABRIK', 'XXSAXXXX', 5, 5, 'Alle-CCS')

	
	, (30, '9001200066', 'HT', 1,		'2017-02-26 4:15:00', 0, 'comment', 'SaW_T [10]', 'FABRIK', 'XXSAXXXX', 5, 5, 'Alle-CCS')
	, (31, '9001200067', 'HT', 1,		'2017-02-26 4:15:00', 0, 'comment', 'SaW_T [10]', 'FABRIK', 'XXSAXXXX', 5, 5, 'Alle-CCS')
	, (32, '9001280029', 'HT', 1,		'2017-02-26 4:15:00', 0, 'comment', 'SaW_T [10]', 'FABRIK', 'XXSAXXXX', 5, 5, 'Alle-CCS')
	, (33, '9001200068', 'HT', 1,		'2017-02-26 4:15:00', 0, 'comment', 'SaW_T [10]', 'FABRIK', 'XXSAXXXX', 5, 5, 'Alle-CCS')
	, (34, '9001280034', 'HT', 1,		'2017-02-26 4:15:00', 0, 'comment', 'SaW_T [10]', 'FABRIK', 'XXSAXXXX', 5, 5, 'Alle-CCS')
	, (35, '9001200069', 'HT', 1,		'2017-02-26 4:15:00', 0, 'comment', 'SaW_T [10]', 'FABRIK', 'XXSAXXXX', 5, 5, 'Alle-CCS')
	, (36, '9001280032', 'HT', 1,		'2017-02-26 4:15:00', 0, 'comment', 'SaW_T [10]', 'FABRIK', 'XXSAXXXX', 5, 5, 'Alle-CCS')
	, (37, '9001200079', 'HT', 1,		'2017-02-26 4:15:00', 0, 'comment', 'SaW_T [10]', 'FABRIK', 'XXSAXXXX', 5, 5, 'Alle-CCS')
	, (38, '9001200080', 'HT', 1,		'2017-02-26 4:15:00', 0, 'comment', 'SaW_T [10]', 'FABRIK', 'XXSAXXXX', 5, 5, 'Alle-CCS')
	, (49, '900120XX68', 'HT', 1,		'2017-02-26 4:15:00', 0, 'comment', 'SaW_T [10]', 'FABRIK', 'XXSAXXXX', 5, 5, 'Alle-CCS')
	, (40, '900120AI66', 'HT', 1,		'2017-02-26 4:15:00', 0, 'comment', 'SaW_T [10]', 'FABRIK', 'XXSAXXXX', 5, 5, 'Alle-CCS')
	, (41, '900120AI67', 'HT', 1,		'2017-02-26 4:15:00', 0, 'comment', 'SaW_T [10]', 'FABRIK', 'XXSAXXXX', 5, 5, 'Alle-CCS')
	, (42, '900120AI68', 'HT', 1,		'2017-02-26 4:15:00', 0, 'comment', 'SaW_T [10]', 'FABRIK', 'XXSAXXXX', 5, 5, 'Alle-CCS')
	, (43, '900120AI69', 'HT', 1,		'2017-02-26 4:15:00', 0, 'comment', 'SaW_T [10]', 'FABRIK', 'XXSAXXXX', 5, 5, 'Alle-CCS')
	, (44, '900120AI79', 'HT', 1,		'2017-02-26 4:15:00', 0, 'comment', 'SaW_T [10]', 'FABRIK', 'XXSAXXXX', 5, 5, 'Alle-CCS')
	, (45, '900120AI80', 'HT', 1,		'2017-02-26 4:15:00', 0, 'comment', 'SaW_T [10]', 'FABRIK', 'XXSAXXXX', 5, 5, 'Alle-CCS')

	, (46, '9001200066', 'HT', 0,		'2017-02-26 4:15:00', 0, 'comment', 'SaW_T [10]', 'FABRIK', 'XXSAXXXX', 5, 5, 'Alle-CCS')
	, (47, '9001200067', 'HT', 0,		'2017-02-26 4:15:00', 0, 'comment', 'SaW_T [10]', 'FABRIK', 'XXSAXXXX', 5, 5, 'Alle-CCS')
	, (48, '9001280029', 'HT', 0,		'2017-02-26 4:15:00', 0, 'comment', 'SaW_T [10]', 'FABRIK', 'XXSAXXXX', 5, 5, 'Alle-CCS')
	, (49, '9001200068', 'HT', 0,		'2017-02-26 4:15:00', 0, 'comment', 'SaW_T [10]', 'FABRIK', 'XXSAXXXX', 5, 5, 'Alle-CCS')
	, (50, '9001280034', 'HT', 0,		'2017-02-26 4:15:00', 0, 'comment', 'SaW_T [10]', 'FABRIK', 'XXSAXXXX', 5, 5, 'Alle-CCS')
	, (51, '9001200069', 'HT', 0,		'2017-02-26 4:15:00', 0, 'comment', 'SaW_T [10]', 'FABRIK', 'XXSAXXXX', 5, 5, 'Alle-CCS')
	, (52, '9001280032', 'HT', 0,		'2017-02-26 4:15:00', 0, 'comment', 'SaW_T [10]', 'FABRIK', 'XXSAXXXX', 5, 5, 'Alle-CCS')
	, (53, '9001200079', 'HT', 0,		'2017-02-26 4:15:00', 0, 'comment', 'SaW_T [10]', 'FABRIK', 'XXSAXXXX', 5, 5, 'Alle-CCS')
	, (54, '9001200080', 'HT', 0,		'2017-02-26 4:15:00', 0, 'comment', 'SaW_T [10]', 'FABRIK', 'XXSAXXXX', 5, 5, 'Alle-CCS')
	, (55, '900120XX68', 'HT', 0,		'2017-02-26 4:15:00', 0, 'comment', 'SaW_T [10]', 'FABRIK', 'XXSAXXXX', 5, 5, 'Alle-CCS')
	, (56, '900120AI66', 'HT', 0,		'2017-02-26 4:15:00', 0, 'comment', 'SaW_T [10]', 'FABRIK', 'XXSAXXXX', 5, 5, 'Alle-CCS')
	, (57, '900120AI67', 'HT', 0,		'2017-02-26 4:15:00', 0, 'comment', 'SaW_T [10]', 'FABRIK', 'XXSAXXXX', 5, 5, 'Alle-CCS')
	, (58, '900120AI68', 'HT', 0,		'2017-02-26 4:15:00', 0, 'comment', 'SaW_T [10]', 'FABRIK', 'XXSAXXXX', 5, 5, 'Alle-CCS')
	, (59, '900120AI69', 'HT', 0,		'2017-02-26 4:15:00', 0, 'comment', 'SaW_T [10]', 'FABRIK', 'XXSAXXXX', 5, 5, 'Alle-CCS')
	, (60, '900120AI79', 'HT', 0,		'2017-02-26 4:15:00', 0, 'comment', 'SaW_T [10]', 'FABRIK', 'XXSAXXXX', 5, 5, 'Alle-CCS')
	, (61, '900120AI80', 'HT', 0,		'2017-02-26 4:15:00', 0, 'comment', 'SaW_T [10]', 'FABRIK', 'XXSAXXXX', 5, 5, 'Alle-CCS')
	;


SELECT min(id), max(id)
INTO @minPdvId, @maxPdvId
FROM pdv
;


DELIMITER $$

DROP PROCEDURE IF EXISTS initializeStep;
CREATE PROCEDURE initializeStep()
BEGIN
	SET @pdvId = @minPdvId;

	SELECT min(id), max(id)
	INTO @minDimId, @maxDimId
	FROM dimension
	;

	pdv_loop: REPEAT

CALL addLogD(CONCAT('Initializing step for pdv: ', @pdvId));

		SET @row1=0, @row2=0, @fncId=0;
		INSERT INTO step(pdvId, position, revision, step, min, max, dim, fncId)
		SELECT 
			t.*
			, @fncId
		FROM (
			SELECT
				@pdvId AS pdvId
				, row1 * 10 AS position,
				0 AS revision,
				row1 AS step,
				min, max, dim
			FROM (
				SELECT
					@row1 := @row1 + 1 as row1,
					rd.id AS dim
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
					WHERE n < 5000
					ORDER BY RAND()
				) rmm
			) rr2
			ON (rr1.row1 = rr2.row2 AND rr1.row1 <= 5000 )		-- Limiting total # of step's
		) t
		;


	/*
		INSERT INTO step(pdvId, position, revision, step, min, max, modName, dim)
		VALUES
			(@pdvId, 10000, 0, 5000, -999.999, 999.999, 'My Module', 1)
			, (@pdvId, 10000, 1, 5000, -99.999, 99.999, 'My Module', 1)
			, (@pdvId, 20000, 0, 5010, -100, 100, 'My Int', 9)
			, (@pdvId, 30000, 0, 5020, -100000, 100000, 'My Hex', 10)
			;
	*/

		SET @pdvId = @pdvId+1;
	UNTIL @pdvId > @maxPdvId
	END REPEAT pdv_loop;

END$$
DELIMITER ;

	/*  Field 'isActive' doesn't have a default value  수정 필요 */
CALL initializeStep();
DROP PROCEDURE initializeStep;
