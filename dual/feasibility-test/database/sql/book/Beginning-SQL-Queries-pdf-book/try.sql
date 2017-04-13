/* 멤버들이 출전한 tour 횟수 출력 */








/* 멤버별로 출전한 tour id 모두 출력 */
SELECT m.MemberID, e.TourID, e.Year
FROM Member m, Entry e
WHERE m.MemberID = e.MemberID
ORDER by m.MemberID

SELECT m.MemberID, e.TourID, e.Year
FROM Member m JOIN Entry e ON m.MemberID = e.MemberID
ORDER by m.MemberID

/* MySQL INNER JOIN */
SELECT m.MemberID, e.TourID, e.Year
FROM Member m JOIN Entry e USING(MemberID)
ORDER by m.MemberID

SELECT m.MemberID, e.TourID
FROM Member m, Entry e
WHERE m.MemberID = e.MemberID
GROUP by m.MemberID

SELECT m.MemberID, count(e.TourID)
FROM Member m LEFT JOIN Entry e ON m.MemberID = e.MemberID
GROUP by m.MemberID


/*
 * Tournament 에 한번이라도 참석한 적이 있는 member 에 대해서, 참여한 tournamenet 정보를 출력하기
 *
 * https://www.percona.com/blog/2013/10/22/the-power-of-mysql-group_concat/ 
 * GROUP_CONCAT 은 mysql 에만 있는 함수 입니다.
 */
/*
+----------+-------+----------------------------------------------------------------------------------+
| MemberID | Count | Detail                                                                           |
+----------+-------+----------------------------------------------------------------------------------+
|      118 |     1 | 24(2005)                                                                         |
|      228 |     3 | 24(2006),25(2006),36(2006)                                                       |
|      235 |     4 | 38(2004),38(2006),40(2005),40(2006)                                              |
|      239 |     2 | 25(2006),40(2004)                                                                |
|      258 |     2 | 24(2005),38(2005)                                                                |
|      286 |     3 | 24(2004),24(2005),24(2006)                                                       |
|      415 |     9 | 24(2006),25(2004),36(2005),36(2006),38(2004),38(2006),40(2004),40(2005),40(2006) |
+----------+-------+----------------------------------------------------------------------------------+
7 rows in set (0.00 sec)
*/
SELECT m.MemberID, 
 count(*) as Count, 
 GROUP_CONCAT(e.TourID, '(', e.Year, ')') AS Detail
FROM Member m, Entry e
WHERE m.MemberID = e.MemberID
GROUP by m.MemberID


/*
 * TourID 36 에 참여하지 않은 모든 member는?
 */
SELECT m.MemberID
FROM Member m
WHERE m.MemberID NOT IN
 (SELECT DISTINCT e.MemberID from Entry e where TourID = 36)


/*  find the names of everyone who entered an Open tournament in 2006. */
SELECT DISTINCT m.MemberID, m.FirstName, m.LastName, t.TourID, t.TourType
FROM Member m, Entry e, Tournament t
WHERE m.MemberID = e.MemberID
 AND e.TourID = t.TourID
 AND e.Year = 2006
 AND t.TourType = 'Open'
/*
+----------+-----------+----------+--------+----------+
| MemberID | FirstName | LastName | TourID | TourType |
+----------+-----------+----------+--------+----------+
|      228 | Sandra    | Burton   |     36 | Open     |
|      415 | William   | Taylor   |     36 | Open     |
|      235 | William   | Cooper   |     38 | Open     |
|      415 | William   | Taylor   |     38 | Open     |
|      235 | William   | Cooper   |     40 | Open     |
|      415 | William   | Taylor   |     40 | Open     |
+----------+-----------+----------+--------+----------+
6 rows in set (0.00 sec)
*/


SELECT m.MemberID, m.FirstName, m.LastName, count(*)
FROM Member m, Entry e, Tournament t
WHERE m.MemberID = e.MemberID
 AND e.TourID = t.TourID
 AND e.Year = 2006
 AND t.TourType = 'Open'
GROUP BY m.MemberID, m.FirstName, m.LastName
/*
+----------+-----------+----------+----------+
| MemberID | FirstName | LastName | count(*) |
+----------+-----------+----------+----------+
|      228 | Sandra    | Burton   |        1 |
|      235 | William   | Cooper   |        2 |
|      415 | William   | Taylor   |        3 |
+----------+-----------+----------+----------+
3 rows in set (0.01 sec)
*/


/*
Open tournament 에 참가한 멤버 목록
*/
SELECT e.MemberID
FROM Entry e 
WHERE e.TourID IN
 (SELECT t.TourID FROM Tournament t where t.TourType = 'Open')


SELECT e.MemberID
FROM Entry e INNER JOIN Tournament t ON e.TourID = t.TourID
WHERE t.TourType = 'Open'


/*
Open tournament 에 참가한 적이 없는 멤버 목록
 - Open tournament 에 참가한 멤버 목록을 먼저 구한후,
 - Member 중에 해당 멤버가 아닌 모든 멤버를 구한다.
*/
SELECT m.MemberID, m.LastName, m.FirstName
FROM Member m
WHERE m.MemberID NOT IN(
 SELECT e.MemberID
 FROM Entry e 
 WHERE e.TourID IN
  (SELECT t.TourID FROM Tournament t where t.TourType = 'Open')
)

SELECT m.MemberID, m.LastName, m.FirstName
FROM Member m
WHERE NOT EXISTS
  (SELECT * FROM Entry e, Tournament t
    WHERE e.MemberID = m.MemberID AND e.TourID = t.TourID
    AND t.TourType = 'Open')
+----------+----------+-----------+
| MemberID | LastName | FirstName |
+----------+----------+-----------+
|      118 | McKenzie | Melissa   |
|      138 | Stone    | Michael   |
|      153 | Nolan    | Brenda    |
|      176 | Branch   | Helen     |
|      178 | Beck     | Sarah     |
|      286 | Pollard  | Robert    |
|      290 | Sexton   | Thomas    |
|      323 | Wilcox   | Daniel    |
|      331 | Schmidt  | Thomas    |
|      332 | Bridges  | Deborah   |
|      339 | Young    | Betty     |
|      414 | Gilmore  | Jane      |
|      461 | Reed     | Robert    |
|      469 | Willis   | Carolyn   |
|      487 | Kent     | Susan     |
+----------+----------+-----------+
15 rows in set (0.00 sec)

 




/* Tournament 에 한번이라도 참석한 적이 있는 member 의 이름 */
SELECT DISTINCT m.LastName, m.FirstName
FROM Member m INNER JOIN Entry e ON m.MemberID = e.MemberID

SELECT m.MemberID, m.LastName, m.FirstName
FROM Member m
WHERE EXISTS
(SELECT * FROM Entry e WHERE e.MemberID = m.MemberID)


/* Tournament 에 한번이라도 참석한 적이 없는 member 의 이름 */
... WHERE NOT EXISTS ...


/*  find all the members with a handicap less than Barbara Olson’s? */
SELECT m.MemberID, m.LastName, m.FirstName, m.Handicap
FROM Member m
WHERE m.Handicap <
 (SELECT Handicap FROM Member
  WHERE LastName = 'Olson' AND FirstName = 'Barbara')

/* find all the members who have a handicap less than the average */
SELECT m.MemberID, m.LastName, m.FirstName, m.Handicap
FROM Member m
WHERE m.Handicap < (SELECT AVG(Handicap) FROM Member) 


/* Juniors with Handicaps Less Than the Average Senior */
SELECT *
FROM Member m
WHERE m.MemberType = 'Junior' AND Handicap <
(SELECT AVG(Handicap)
FROM Member
WHERE MemberType = 'Senior')




/* Entries for Senior Members */
SELECT e.TourID, m.MemberID, m.LastName, m.FirstName
 FROM Entry e JOIN Member m ON e.MemberID = m.MemberID
 WHERE m.MemberType = 'Senior'

/* What are the names of the coaches? */
SELECT DISTINCT c.LastName, c.FirstName
FROM Member m, Member c
WHERE m.Coach = c.MemberID

/* Who is Jane Gilmore’s coach? */
SELECT c.LastName, c.FirstName
FROM Member m, Member c 
WHERE m.Coach = c.MemberID
AND m.LastName = 'Gilmore'
AND m.FirstName = 'Jane'

/* Is anyone being coached by someone with a higher handicap?
SELECT m.LastName, m.FirstName, m.Handicap, 
 c.Handicap AS CoachHandicap
FROM Member m, Member c 
WHERE m.Coach = c.MemberID
AND m.Handicap < c.Handicap


/* Are any women being coached by men?
SELECT m.LastName, m.FirstName, m.Gender, c.Gender AS CoachGender
FROM Member m, Member c 
WHERE m.Coach = c.MemberID
AND m.Gender = 'F' AND c.Gender = 'M'


/* List the Names of All the Members and the Names of Their Coaches */
SELECT m.LastName, m.FirstName, c.LastName, c.FirstName
FROM Member m LEFT JOIN Member c ON m.Coach = c.MemberID

/* Who Coaches the Coaches */
SELECT CONCAT(c2.LastName, '. ', c2.FirstName) AS GRAND,
 CONCAT(c1.LastName, '. ', c1.FirstName) AS COACH,
 CONCAT(m.LastName, '. ', m.FirstName) AS STUDENT
FROM Member m, Member c1, Member c2
WHERE m.Coach = c1.MemberID AND c1.Coach = c2.MemberID

/* Find Members Who Have Entered Both Tournaments 24 and 36 */
SELECT e1.MemberID
FROM Entry e1, Entry e2
WHERE e1.MemberID = e2.MemberID
AND e1.TourID = 24 AND e2.TourID = 36



/* Teams with Additional Information About Their Managers */
SELECT *
FROM Team t, Member m
WHERE t.Manager = m.MemberID

/* 자기가 속한 팀의 manager 인 member */
SELECT * 
FROM Member m INNER JOIN Team t
ON (m.MemberID = t.Manager) AND (m.Team = t.TeamName)

SELECT * 
FROM Member m, Team t
WHERE (m.MemberID = t.Manager) AND (m.Team = t.TeamName)


/* Listing 6-6. Information About Members, Their Team, and Their Team’s Manager */
SELECT m1.MemberID, m1.LastName, m1.FirstName, t.TeamName, m2.MemberID AS 'Team ManagerID'
FROM Member m1, Team t, Member m2
WHERE (m1.Team = t.TeamName) AND (t.Manager = m2.MemberID)

SELECT m1.MemberID, m1.LastName, m1.FirstName, t.TeamName, m2.MemberID AS 'Team ManagerID'
FROM Member m1 INNER JOIN(Team t, Member m2)
 ON (m1.Team = t.TeamName AND t.Manager = m2.MemberID)



/* Find Teams Where the Manager Is Not a Member of the Team */
SELECT t.teamname
FROM Member m, Team t
WHERE m.MemberID = t.Manager
AND (m.Team <> t.Teamname OR m.Team IS NULL)


/* IDs of Members Entered in Both Tournaments 25 and 36 */
SELECT DISTINCT e1.MemberID
FROM Entry e1, Entry e2
WHERE e1.MemberID = e2.MemberID
 AND e1.TourID = 25 AND e2.TourID = 36


SELECT m.MemberID, LastName, FirstName
FROM Member m INNER JOIN
 (SELECT DISTINCT e1.MemberID
  FROM Entry e1, Entry e2
  WHERE e1.MemberID = e2.MemberID
   AND e1.TourID = 25 AND e2.TourID = 36) NewTable
ON m.MemberID = NewTable.MemberID


SELECT m.MemberID, LastName, FirstName
FROM Member m 
WHERE MemberID IN
 (SELECT DISTINCT e1.MemberID
  FROM Entry e1, Entry e2
  WHERE e1.MemberID = e2.MemberID
   AND e1.TourID = 25 AND e2.TourID = 36)


/* Listing 8-17. Find How Many Tournaments Each Member Has Entered */
SELECT MemberID, COUNT(*) AS NumEntries
FROM Entry
GROUP BY MemberID

/* Listing 8-20. Find the Number of Entries in Each Tournament */
SELECT TourID, COUNT(*) AS NumEntries
FROM Entry
GROUP BY TourID

/* Listing 8-21. Find the Number of Entries in Each Tournament for the Year 2006 */
SELECT TourID, COUNT(*) AS NumEntries
FROM Entry
WHERE Year = 2006
GROUP BY TourID

/* Listing 8-22. Find the Number of Entries in Each Tournament for Each Year */
SELECT TourID, Year, COUNT(*) AS NumEntries
FROM Entry
GROUP BY TourID, Year

/* Listing 8-23. Find the Average Handicaps of Members Grouped by Gender */
SELECT Gender, AVG(Handicap)
FROM Member m
GROUP BY Gender

/* Listing 8-24. Find Tournaments with Three or More Entries */
SELECT TourID, COUNT(*)
FROM Entry
GROUP BY TourID
HAVING COUNT(*) >= 3













/* Listing 8-27. Count the Distinct Tournaments Entered by Each Member */
SELECT MemberID, count(DISTINCT TourID)
FROM Entry
GROUP BY MemberID

/* Listing 8-28. Find Members Who Have Entered Five Different Tournaments */
SELECT MemberID, count(DISTINCT TourID)
FROM Entry e 
GROUP BY MemberID
HAVING COUNT(DISTINCT TourID) = 5

Listing 8-29. Find Members Who Have Entered All the Different Tournaments in the Tournament Table

Listing 8-30. Return Members with a Handicap Greater Than Average

Listing 8-30. Return Members with a Handicap Greater Than Average

Listing 8-31. Find Members Who Have Entered More Than Three Tournaments

Listing 8-32. Find the Number of Entries for Each Member

Listing 8-33. Find the Average Number of Tournaments Entered by Members

