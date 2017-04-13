/*
 * Beginning SQL Queries.pdf
 */

-- Listing 7-32. SQL to Find the Names of Members Who Have Entered Every Tournament
SELECT m.LastName, m.FirstName FROM Member m
WHERE NOT EXISTS
(
  SELECT * FROM Tournament t
  WHERE NOT EXISTS
  (
    SELECT * FROM Entry e
    WHERE e.MemberID = m.MemberID AND e.TourID = t.TourID
  )
);

-- Listing 8-29. Find Members Who Have Entered All the Different Tournaments in the Tournament Table
SELECT MemberID
FROM Entry e
GROUP BY MemberID
HAVING COUNT(DISTINCT TourID) =
	(SELECT COUNT(DISTINCT TourID) FROM Tournament)
;


-- Listing 8-31. Find Members Who Have Entered More Than Three Tournaments
SELECT * FROM Member m
WHERE (
	SELECT COUNT(*)
		FROM Entry e
		WHERE e.MemberID = m.MemberID
	) > 3
;


-- Listing 10-13. Finding Members Who Have Never Entered a Tournament Using a Nested Query
SELECT m.MemberID FROM Member m
WHERE m.MemberID NOT IN
(SELECT e.MemberID FROM Entry e)
;
