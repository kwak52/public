#!/bin/sh

#
# Workbench 를 통한 modeling 작업을 forward engineering 을 통해 database 생성하고 난 후에 수행할 작업들
#

mydir="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
source $mydir/bash/bash_functions


1.db-user-permission.sh || die "Error on file 1.db-user-permission.sh"


function sql()
{
	__=`$mysql kefico -sNe "$1"`
}



MYSQL_PWD=stored_program
export MYSQL_PWD
mysql="mysql -u stored_program"

echo -n "---- Checking mysqld event scheduler.."
event_scheduler=$($mysql -sNe "SELECT @@event_scheduler;")
if [ $event_scheduler = "ON" ]; then
	echo "  working!"
else
	echo "  not working."
	die "  You should added 'event_scheduler = on' in /etc/my.cnf."
fi


for f in \
	1.initial-DDL.sql	\
	2.create-view.sql	\
	stored-program/1.utility.sql			\
	stored-program/1.helper-view.sql		\
	stored-program/1.event-scheduler.sql	\
	stored-program/2.notify.sql				\
	stored-program/3.operation.sql			\
	stored-program/4.rolling.sql			\
	stored-program/5.triggers.sql			\
	stored-program/6.query-view.sql			\
	stored-program/6.query-step.sql			\
	stored-program/6.step-parse.sql			\
	stored-program/7.pdv.sql				\
	stored-program/9.alter-tables.sql		\
	stored-program/9.backup.sql				\
	stored-program/9.debug-insert-bundle.sql\
	stored-program/9.debug.sql				\
	stored-program/9.emergency.sql			\
	stored-program/9.obsolete.sql			\
	4.initial-DML.sql	\
	insert-sample-tiny.sql;	\
do
	echo "---- processing $f"
	$mysql < $f || die "Error on file $f"
done

echo "---- updating tableRevision"

cat<<EOF | $mysql kefico
	UPDATE tableRevision SET revision=0;
	INSERT INTO preference(name, val, type, comment)
		VALUES('enableTestDataInsert', 'Y', 'YESNO', 'enable test data insert.  Y/N');

	CALL _removeData();
	DELETE FROM preference where name = 'nextMinimalStartTime';
EOF

InsertTestSample=0
if [ $InsertTestSample == 1 ]; then
	echo "---- processing scenario/insert_summary.sh"
	scenario/insert_summary.sh || die "Error on file scenario/insert_summary.sh"
	# scenario/insert_summary.sh -c 2 -p 5 || die "Error on file scenario/insert_summary.sh"
else
	# insert today's partition on bundle table
	echo "CALL _addBundlePartitionForDay(DATE_ADD(CURDATE(), INTERVAL 1 DAY));" | $mysql kefico
	echo "DELETE FROM step; ALTER TABLE step AUTO_INCREMENT = 1" | $mysql kefico
fi



echo "---- Cleaning up."
cat<<EOF | $mysql kefico
	CALL rollOutAfterDateLine();

	-- clean up
	DELETE FROM preference WHERE name IN ('enableTestDataInsert', 'nextMinimalStartTime');

	CALL _buildInitialDailySectionalStepSummary();
EOF


