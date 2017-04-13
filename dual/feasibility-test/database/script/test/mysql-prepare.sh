#!/bin/sh


show_simple_usage()
{
	cat<<EOF | mysql kefico
		PREPARE stmt FROM 'SELECT * from pdv WHERE id=?;';
		SET @a=4
		EXECUTE stmt USING @a;

		DEALLOCATE PREPARE stmt;
EOF
}

generate_sql()
{
	day="'$(date +"%Y-%m-%d")'"
	cat<<EOF #| mysql kefico
		
		SET @insert=CONCAT(
			'INSERT INTO bundle(startDay, measureId, positionId, value) VALUES(',
				  QUOTE($day),
				  ', ?, ?, ?);'
		);
		PREPARE stmt FROM @insert;
EOF
	counter=0;
	while [ $counter -lt 10 ]; do
		echo "SET @measureId=$counter;"
		echo "EXECUTE stmt USING @measureId, @measureId, @measureId;"
		((counter++))
	done
	echo "DEALLOCATE PREPARE stmt;"
}


generate_sql
