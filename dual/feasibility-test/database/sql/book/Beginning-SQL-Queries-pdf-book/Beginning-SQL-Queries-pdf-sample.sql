/*
 Beginning SQL Queries- From Novice to Professional.pdf 의 Sample 
*/

CREATE TABLE IF NOT EXISTS Type (
        Type VARCHAR(20) Primary Key,
        Fee INT);

CREATE TABLE IF NOT EXISTS Member (
        MemberID INT Primary Key,
        LastName VARCHAR(20),
        FirstName VARCHAR(20),
        MemberType VARCHAR(20),
        Phone VARCHAR(20),
        Handicap INT,
        JoinDate DATETIME,
        Coach INT,
        Team VARCHAR(20),
        Gender VARCHAR(1),
        foreign key (MemberType) references Type(Type));

CREATE TABLE IF NOT EXISTS Tournament (
        TourID INT Primary Key,
        TourName VARCHAR(20),
        TourType VARCHAR(20));

CREATE TABLE IF NOT EXISTS Entry (
        MemberID INT,
        TourID INT,
        Year INT,
Primary Key (MemberID, TourID, Year),
foreign key(MemberID) references Member (MemberID),
foreign key(TourID) references Tournament (TourID));

CREATE TABLE IF NOT EXISTS Team (
        TeamName VARCHAR(20) Primary Key,
        PracticeNight VARCHAR(20),
        Manager INT,
        foreign key (Manager) references Member (MemberID));

Alter TABLE Member
ADD FOREIGN KEY (coach) References Member(MemberID);

Alter TABLE Member
ADD FOREIGN KEY (team) References Team(TeamName);

Insert into Type values ('Junior',150);
Insert into Type values ('Senior',300);
Insert into Type values ('Social',50);

Insert into Tournament values (24,'Leeston','Social');
Insert into Tournament values (25,'Kaiapoi','Social');
Insert into Tournament values (36,'WestCoast','Open');
Insert into Tournament values (38,'Canterbury','Open');
Insert into Tournament values (40,'Otago','Open');


Insert into Team values ('TeamA','Tuesday', null);
Insert into Team values ('TeamB','Monday', null);

Insert into Member values (118,'McKenzie','Melissa','Junior','963270',30,null,null,null,'F');
Insert into Member values (138,'Stone','Michael','Senior','983223',30,null,null,null,'M');
Insert into Member values (153,'Nolan','Brenda','Senior','442649',11,null,null,'TeamB','F');
Insert into Member values (176,'Branch','Helen','Social','589419',null,null,null,null,'F');
Insert into Member values (178,'Beck','Sarah','Social','226596',null,null,null,null,'F');
Insert into Member values (228,'Burton','Sandra','Junior','244493',26,null,null,null,'F');
Insert into Member values (235,'Cooper','William','Senior','722954',14,null,null,'TeamB','M');
Insert into Member values (239,'Spence','Thomas','Senior','697720',10,null,null,null,'M');
Insert into Member values (258,'Olson','Barbara','Senior','370186',16,null,null,null,'F');
Insert into Member values (286,'Pollard','Robert','Junior','617681',19,null,null,'TeamB','M');
Insert into Member values (290,'Sexton ','Thomas','Senior','268936',26,null,null,null,'M');
Insert into Member values (323,'Wilcox','Daniel','Senior','665393',3,null,null,'TeamA','M');
Insert into Member values (331,'Schmidt','Thomas','Senior','867492',25,null,null,null,'M');
Insert into Member values (332,'Bridges','Deborah','Senior','279087',12,null,null,null,'F');
Insert into Member values (339,'Young','Betty','Senior','507813',21,null,null,'TeamB','F');
Insert into Member values (414,'Gilmore','Jane','Junior','459558',5,null,null,'TeamA','F');
Insert into Member values (415,'Taylor','William','Senior','137353',7,null,null,'TeamA','M');
Insert into Member values (461,'Reed','Robert','Senior','994664',3,null,null,'TeamA','M');
Insert into Member values (469,'Willis','Carolyn','Junior','688378',29,null,null,null,'F');
Insert into Member values (487,'Kent','Susan','Social','707217',null,null,null,null,'F');





Insert into Entry values (118,24,2005);
Insert into Entry values (228,24,2006);
Insert into Entry values (228,25,2006);
Insert into Entry values (228,36,2006);
Insert into Entry values (235,38,2004);
Insert into Entry values (235,38,2006);
Insert into Entry values (235,40,2005);
Insert into Entry values (235,40,2006);
Insert into Entry values (239,25,2006);
Insert into Entry values (239,40,2004);
Insert into Entry values (258,24,2005);
Insert into Entry values (258,38,2005);
Insert into Entry values (286,24,2004);
Insert into Entry values (286,24,2005);
Insert into Entry values (286,24,2006);
Insert into Entry values (415,24,2006);
Insert into Entry values (415,25,2004);
Insert into Entry values (415,36,2005);
Insert into Entry values (415,36,2006);
Insert into Entry values (415,38,2004);
Insert into Entry values (415,38,2006);
Insert into Entry values (415,40,2004);
Insert into Entry values (415,40,2005);
Insert into Entry values (415,40,2006);

Update Team Set Manager = 239 where TeamName = 'TeamA';
Update Team Set Manager = 153 where TeamName = 'TeamB';

Update Member Set Coach = 153 where MemberID = 118;
Update Member Set Coach = 153 where MemberID = 228;
Update Member Set Coach = 153 where MemberID = 235;
Update Member Set Coach = 235 where MemberID = 286;
Update Member Set Coach = 235 where MemberID = 290;
Update Member Set Coach = 153 where MemberID = 331;
Update Member Set Coach = 235 where MemberID = 332;
Update Member Set Coach = 153 where MemberID = 414;
Update Member Set Coach = 235 where MemberID = 415;
Update Member Set Coach = 235 where MemberID = 461;


