#!/usr/bin/php

<?php
include 'common/connect.php';
$mysqli=connect_sql("localhost", "securekwak", "kwak", "kefico");


$tables = $mysqli->query("SELECT table_name FROM information_schema.tables WHERE table_schema='kefico'"); //pull tables from theh databsase
while ($table= $tables->fetch_row()) { 
	printf("Table %s\n\t", $table[0]);
	$rsFields = $mysqli->query("SHOW COLUMNS FROM ".$table[0]); 
	while ($field = $rsFields->fetch_assoc()) { 
		echo $field["Field"].", "; //prints out all columns 
	} 
	printf("\n");
	// $query = "SELECT * FROM ".$table[0]; //prints out tables name
	// $result = $mysqli->query($query); 
}


function transaction()
{
	$mysqli->autocommit(FALSE);
	$mysqli->rollback( );
	$mysqli->commit( );
}


function prepareStatement()
{
	# Preparing the statment
	$insert_stmt=$mysqli->prepare("INSERT INTO x VALUES(?,?)") or die($mysqli->error);

	# associate variables with the input parameters.  [idsb] for Integer, Double, String, Blob
	# http://php.net/manual/kr/mysqli-stmt.bind-param.php
	$insert_stmt->bind_param("is", $my_number, $my_string); #i=integer

	# Execute the statement multiple times....
	for ($my_number = 1; $my_number <= 10; $my_number++) {
		$my_string="row ".$my_number;
		$insert_stmt->execute( ) or die ($insert_stmt->error);
	}
	$insert_stmt->close( );
}


?>
