#!/bin/sh

usage()
{
	echo "$0 <old db> <new db>"
}

if [ $# != 2 ]; then
    usage
    exit
fi

old_db=$1
new_db=$2

if [ -z `mysql -sNe "SELECT schema_name FROM information_schema.schemata WHERE schema_name='$old_db';"` ]; then
	echo "Source database '$old_db' does exists."
	exit
fi

if [ ! -z `mysql -sNe "SELECT schema_name FROM information_schema.schemata WHERE schema_name='$new_db';"` ]; then
	echo "Target database '$new_db' already exists."
	exit
fi

mysql -sNe "create database $new_db;"
mysqldump --routines $old_db | mysql $new_db
