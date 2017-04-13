USE test

ALTER TABLE birds_new
ADD COLUMN wing_id CHAR(2);

-- DESCRIBE birds_new 

ALTER TABLE birds_new
DROP COLUMN wing_id;

ALTER TABLE birds_new
ADD COLUMN wing_id CHAR(2) AFTER family_id;

-- DESCRIBE birds_new

ALTER TABLE brids_new
ADD COLUMN body_id CHAR(2) AFTER wing_id,
ADD COLUMN bill_id CHAR(2) AFTER body_id,
ADD COLUMN endangered BIT DEFAULT b'1' AFTER bill_id,
CHANGE COLUMN common_name common_name VARCHAR(255);

UPDATE birds_new SET endangered = 0
WHERE bird_id IN(1, 2, 4, 5);


-- SELECT * FORM birds_new WHERE NOT endangered;


ALTER TABLE birds_new
MODIFY COLUMN endangered
ENUM(
 'Extinct',
 'Extinct in Wild',
 'Threatened - Critically Endangered',
 'Threatened - Endangered',
 'Threatened - Vulnerable',
 'Lower Risk - Conservation Dependent',
 'Lower Risk - Near Threatened',
 'Lower Risk - Least Concern')
AFTER family_id;

-- SHOW COLUMNS FROM birds_new LIKE 'endangered' \G

-- 모든 row 의 endangered column 값을 7 ('Lower Risk - Least Concern')로 바꿈.
-- UPDATE birds_new SET endangered = 7;











USE birdwatchers;

CREATE TABLE surveys (
 survey_id INT AUTO_INCREMENT KEY,		-- NOT PRIMARY???
 survey_name VARCHAR(255));

CREATE TABLE survey_questions (
 question_id INT AUTO_INCREMENT KEY,
 survey_id INT AUTO_INCREMENT PRIMARY KEY,
 question VARCHAR(255),
 choices BLOB);

CREATE TABLE survey_answers (
 answer_id INT AUTO_INCREMENT KEY,		-- NOT PRIMARY???
 human_id INT,
 question_id INT,
 date_answered DATETIME,
 answer VARCHAR(255));













INSERT INTO surveys (survey_name)
VALUES("Favorite Birding Location");

INSERT INTO survey_questions
(survey_id, question, choices)
VALUES(
 LAST_INSERT_ID(),
 "What's your favorite setting for bird-watching?",
 COLUMN_CREATE('1', 'forest', '2', 'shore', '3', 'backyard'));


/*
SELECT COLUMN_GET(choices, 3 AS CHAR)
AS 'Location'
FROM survey_questions
WHERE survey_id = 1;
+----------+
| Location |
+----------+
| backyard |
+----------+
*/

INSERT INTO survey_answers
(human_id, question_id, date_answered, answer)
VALUES
(29, 1, NOW(), 2),
(29,	2,	NOW(),	2),
(35,	1,	NOW(),	1),
(35,	2,	NOW(),	1),
(26,	1,	NOW(),	2),
(26,	2,	NOW(),	1),
(27,	1,	NOW(),	2),
(27,	2,	NOW(),	4),
(16,	1,	NOW(),	3),
(3,	1,	NOW(),	1),
(3,	2,	NOW(),	1);

/*
SELECT IFNULL(COLUMN_GET(choices, answer AS CHAR), 'total')
AS 'Birding Site', COUNT(*) AS 'Votes'
FROM survey_answers
JOIN survey_questions USING(question_id)
WHERE survey_id = 1
AND question_id = 1
GROUP BY answer WITH ROLLUP;
+--------------+-------+
| Birding Site | Votes |
+--------------+-------+
| forest       |     2 |
| shore        |     3 |
| backyard     |     1 |
| total        |     6 |
+--------------+-------+
*/

INSERT INTO surveys (survey_name)
VALUES("Preferred Birds");

INSERT INTO survey_questions
(survey_id, question, chocies)
VALUES(
 LAST_INSERT_ID(),
 "Which type of birds do you like best?",
 COLUMN_CREATE('1', 'perching', '2', 'shore', '3', 'fowl', '4', 'rapture') );

