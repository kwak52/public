CREATE DATABASE rookery;
-- SHOW databases;

USE rookery;
CREATE TABLE birds (
  bird_id INT AUTO_INCREMENT PRIMARY KEY,
  scientific_name VARCHAR(255) UNIQUE,
  common_name VARCHAR(50),
  family_id INT,
  description TEXT);

-- SHOW tables;
-- DESCRIBE birds;

INSERT INTO birds (scientific_name, common_name)
VALUES ('Charadrius vociferus', 'Killdeer'),
  ('Gavia immer', 'Great Northern Loon'),
  ('Aix sponsa', 'Wood Duck'),
  ('Chordeiles minor', 'Common Nighthawk'),
  ('Sitta carolinensis', ' White-breasted Nuthatch'),
  ('Apteryx mantelli', 'North Island Brown Kiwi');

-- SELECT * FROM birds;

CREATE DATABASE birdwatchers;
CREATE TABLE birdwatchers.humans (
  human_id INT AUTO_INCREMENT PRIMARY KEY,
  formal_title VARCHAR(25),
  name_first VARCHAR(25),
  name_last VARCHAR(25),
  email_address VARCHAR(255));

INSERT INTO birdwatchers.human (formal_title, name_first, name_last, email_address)
VALUES ('Mr.', 'Russell', 'Dyer', 'russell@mysqlresources.com'),
  ('Mr.', 'Richard', 'Stringer', 'richard@mysqlresources.com'),
  ('Ms.', 'Rusty', 'Osborne', 'rusty@mysqlresources.com'),
  ('Ms.', 'Lexi', 'Hollar', 'alexandra@mysqlresources.com');

-- SHOW CREATE TABLE birds \G

CREATE TABLE bird_families (
  family_id INT AUTO_INCREMENT PRIMARY KEY,
  scientific_name VARCHAR(255) UNIQUE,
  brief_description VARCHAR(255));

CREATE TABLE bird_orders (
 order_id INT AUTO_INCREMENT PRIMARY KEY,
 scientific_name VARCHAR(255) UNIQUE,
 brief_description VARCHAR(255),
 order_image BLOB
) DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

ALTER TABLE bird_families
ADD COLUMN order_id INT;





CREATE TABLE test.birds_new LIKE birds;
USE test;
DESCRIBE birds_new;

INSERT INTO birds_new
SELECT * FROM rookery.birds;

