<?php
function connect_sql($host, $user, $password, $database, $verbose=false)
{
	$mysqli = new mysqli("localhost", "securekwak", "kwak", "kefico");

	if (mysqli_connect_errno( )) {
		printf("Connect failed: %s\n", mysqli_connect_error( ));
		exit ( );
	} else {
		if ( $verbose )
			printf("Connect succeeded\n");
	}

	return $mysqli;
}
?>
