SELECT EXISTS(SELECT * FROM user WHERE user = 'NonExist') AS NonExist;
SELECT EXISTS(SELECT * FROM user WHERE user = 'kwak') AS Exist;

and 0 = (SELECT COUNT(*) ...)		-- BAD
and NOT EXISTS ( SELECT NULL ... )	-- GOOD

SELECT * FROM measure WHERE (id, ccsId) IN ( SELECT id, ccsId FROM measure);

INSERT INTO ... ON DUPLICATE KEYS SET ...
== REPLACE INTO ...

-- replace 는 해당 row 가 이미 존재하면 삭제 하고 새로 insert 하는 듯..
-- AUTO_INCREMENT key 가 있으면 해당 key 를 삭제 후, 재생성하므로, FK constraint 에 위배되어 fail 날 수도 있다.
-- 이때는 ON DUPLICATE KEY SET ... 구문이 답이다.
REPLACE INTO books		-- REPLACE 의 구문은 INSERT 와 동일.  없으면 insert, 있으면 replace
(title, author_id, isbn, genre, pub_year)
VALUES('Brighton Rock',1,'0099478471','novel','1938'),
('The Quiet American',1,'0099478393','novel','1955');



CREATE SERVER s
FOREIGN DATA WRAPPER mysql
OPTIONS (USER 'securekwak', PASSWORD 'kwak', HOST 'localhost', DATABASE 'tmp')
;

CREATE table p ENGINE=FEDERATED CONNECTION='s';
select * from person 

CREATE TABLE t (s1 INT) ENGINE=FEDERATED CONNECTION='s';


# http://stackoverflow.com/questions/1262786/mysql-update-query-based-on-select-query
update tableA a
left join tableB b on
    a.name_a = b.name_b
set
    validation_check = if(start_dts > end_dts, 'VALID', '')

UPDATE bundle b
	JOIN step s ON b.stepId = s.id
	JOIN dimension d ON s.dim = d.id
	SET message = d.name
	WHERE b.day = '20160909'
;

INSERT INTO user (id, name, username, opted_in)
(	-- 괄호는 optional
	SELECT id, name, username, opted_in 
	FROM user
	LEFT JOIN user_permission AS userPerm ON user.id = userPerm.user_id
)	-- 괄호는 optional
;
