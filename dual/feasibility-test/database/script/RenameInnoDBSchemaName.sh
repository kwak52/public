#!/bin/sh

# http://stackoverflow.com/questions/67093/how-do-i-quickly-rename-a-mysql-database-change-schema-name

if [ $# -ne 2 ]; then
 echo "Usage: $0 <old-db-name> <new-db-name>"
 exit
fi

source ./setenv
old_db=$1
new_db=$2

#
# InnoDB만 지원함
#
non_inno_db=$($mysql -sNe "SELECT ENGINE FROM information_schema.tables WHERE TABLE_SCHEMA='$old_db'" | grep -v InnoDB);
if [ "x$non_inno_db" != "x" ]; then
 echo "There is non-inno-db tables."
 exit
fi


$mysql -sNe "create database if not exists $new_db"
$mysql $old_db -sNe 'show tables' | while read table; do $mysql -sNe "rename table $old_db.$table to $new_db.$table"; done
$mysql -sNe "drop database $old_db"

