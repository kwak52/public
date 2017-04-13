#!/bin/sh

show_comment()
{
cat<<EOF
==================================================================
pdv
	- pdv 관련 table 수정 권한.  step table 의 update 권한은 제거.
ccs 
	- 시험기.  measure 및 bundle table 에 관한 추가 권한
mws
	- 읽기 권한
stored_program
	- server 에서 stored procedure/function 의 creator
backup
	- system backup user
admin
	- system admin
==================================================================
EOF
}


schema="kefico"

get_cmd_grant_privleges_to_user()
{
	user=$1
	eval privs="$2"

	tables=$(mysql -sNe "SELECT table_name FROM information_schema.tables WHERE table_schema='$schema'" | grep -v '^sample')
	for table in $tables; do
		echo "GRANT $privs ON $schema.$table TO '$user'@'%';"
	done
	echo ""
}


get_cmds()
{
	for user in pdv ccs mws stored_program; do
		echo "DROP USER IF EXISTS '$user'@'%';"
	done

cat<<EOF
	CREATE USER IF NOT EXISTS 'pdv'@'%' IDENTIFIED BY 'pdv';
	CREATE USER IF NOT EXISTS 'mws'@'%' IDENTIFIED BY 'mws';
	CREATE USER IF NOT EXISTS 'ccs'@'%' IDENTIFIED BY 'ccs';
	CREATE USER IF NOT EXISTS 'stored_program'@'%' IDENTIFIED BY 'stored_program';
	CREATE USER IF NOT EXISTS 'backup'@'%' IDENTIFIED BY 'backup';
	CREATE USER IF NOT EXISTS 'admin'@'%' IDENTIFIED BY 'admin';
	GRANT ALL PRIVILEGES ON *.* TO 'stored_program'@'%';
	GRANT ALL PRIVILEGES ON *.* TO 'admin'@'%';
	GRANT ALL PRIVILEGES ON *.* TO 'pdv'@'%';
EOF
	r="SELECT"
	cr="$r, INSERT"
	cru="$cr, UPDATE"
	crud="$cr, UPDATE, DELETE"
	get_cmd_grant_privleges_to_user pdv "\${crud}"
	get_cmd_grant_privleges_to_user mws "\${r}"
	get_cmd_grant_privleges_to_user backup "\${r}"
	get_cmd_grant_privleges_to_user ccs "\${cr}"
	get_cmd_grant_privleges_to_user stored_program "\${crud}"

	for user in pdv ccs mws stored_program; do
		echo "GRANT CREATE TEMPORARY TABLES, EXECUTE, TRIGGER ON $schema.* TO '$user'@'%';"
	done

	echo ""
	echo "FLUSH PRIVILEGES;"
	echo "REVOKE UPDATE ON kefico.step FROM 'pdv'@'%';"
	echo "REVOKE DELETE ON kefico.step FROM 'pdv'@'%';"
	echo "REVOKE INSERT ON kefico.bundle FROM 'ccs'@'%';"
	echo ""
	echo "FLUSH PRIVILEGES;"
}



cat << EOF | mysql
`get_cmds`
EOF

exit

---------------------------------------------------------------------------


-- show privileges;		-- lists all possible privileges
-- select * from mysql.user\G
-- select * from mysql.db\G
-- select * from mysql.tables_priv\G

GRANT SELECT, INSERT, UPDATE, DELETE ON @schema.ccs TO 'pdv'@'%';

-- GRANT ALL PRIVILEGES ON kefico.* TO 'pdv'@'%';
FLUSH PRIVILEGES;

UPDATE mysql.db
SET
	Update_priv = 'N'
	, Delete_priv = 'N'
WHERE
	Db = @schema
	AND User = 'pdv'
;
