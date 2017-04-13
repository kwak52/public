#!/bin/sh

# SEE ALSO ../stored-program/9.backup.sql
#
# IDEA
#	measure 및 bundle 에 대해서 SQL 문으로 partial backup 하고, 
#	local PC 로 해당 파일들을 옮겨서 local PC 상의 mysql+application 을 이용하여 
#	backup 된 내용들을 browsing 할 수 있도록 한다.
#
usage()
{
	cat<<EOF
# Backup 하기
#	$0 -s 2016-08-07
#	$0 -s 2016-08-01 -e 2016-08-31		# 한달치 backup
#	$0 --start-day=2016-08-07
#
# Restore 하기 (local PC상에서 수행할 것)
#	./backup-restore.sh -r tmpKefico -R backup-2016-08-01~2016-08-31

backup
	$0 <options>

restore
	$0 -r <options> or $0 --restore <options>

OPTIONS

 BACKUP-OPTIONS
	-b, --backup-db=<dbname> : default=kefico
	
 RESTORE-OPTIONS
	-r, --restore-db=<dbname> : temporary database name.  default=tmpKefico
	-R, --restore-data-dir=<dir> : backup'ed data dir.
	--restore-allow-append : if specified, allow appending 

	-p=<dir prefix>, --dir-prefix=<dir prefix> : default=sqlbackup-
	-s=<start day>, --start-day=<start day> : specify start day
	-e=<end day>, --end-day=<end day> : specify end day

CONSTRAINTS
	- start-day should be specified.
	- For period, both start-day and end-day should be specified.  In addition, start-day <= end-day
EOF
}

if [ $# == 0 ]; then
	usage
	exit
fi


BACKUP_DATABASE=
BACKUP_DATA_DIR=

RESTORE_DATA_DIR=
RESTORE_DATABASE=
RESTORE_ALLOW_APPEND=0


OPTS=`getopt -o hb:B:r:R:s:e: \
	--long help,backup-db:,backup-data-dir:,restore-db:,restore-allow-append,restore-data-dir:,start-day:,end-day:	\
	 -n 'parse-options' -- "$@"`
eval set -- "$OPTS"



while true; do
  case "$1" in
    -h | --help )				usage; exit;			shift ;;
    -b | --backup-db)			BACKUP_DATABASE="$2";	shift; shift ;;
    -B | --backup-data-dir )	BACKUP_DATA_DIR="$2";	shift; shift ;;
    -r | --restore-db )			RESTORE_DATABASE="$2";	shift; shift ;;
    -R | --restore-data-dir )	RESTORE_DATA_DIR="$2";	shift; shift ;;
    --restore-allow-append )	RESTORE_ALLOW_APPEND=1; shift ;;
    -s | --start-day )			STARTDAY="$2";			shift; shift ;;
    -e | --end-day )			ENDDAY="$2";			shift; shift ;;
    -- ) shift; break ;;
    * ) break ;;
  esac
done

ambiguous()
{
	echo "You specified both backup and restore related option.  It's ambiguous."
	exit
}


[[ -z $RESTORE_DATA_DIR ]] && isBackup=1 || isBackup=0

if [ $isBackup == 1 ]; then
	if ! [ -z $RESTORE_DATABASE ] || ! [ -z $RESTORE_DATA_DIR ]; then
		ambiguous;
	fi

	[[ -z $BACKUP_DATABASE ]] && BACKUP_DATABASE=kefico

	if [ -z $BACKUP_DATA_DIR ]; then
		[[ -z $ENDDAY ]] && BACKUP_DATA_DIR="sqlbackup-$STARTDAY" || BACKUP_DATA_DIR="sqlbackup-$STARTDAY~$ENDDAY"
	fi
else
	if ! [ -z $BACKUP_DATABASE ] || ! [ -z $BACKUP_DATA_DIR ]; then
		ambiguous
	fi

	[[ -z $RESTORE_DATABASE ]] && RESTORE_DATABASE=tmpKefico
fi




if [ -z $ENDDAY ]; then
	ENDDAY=$STARTDAY
fi

conf_tables=(pdv pdvGroup dimension function step user preference)
data_tables=(measure bundle)
tables=(${conf_tables[@]} ${data_tables[@]})

function backup()
{
	dir=$BACKUP_DATA_DIR
	rm -rf $dir
	mkdir $dir

	dump="mysqldump $BACKUP_DATABASE --extended-insert=FALSE"
	#dump="mysqldump $BACKUP_DATABASE"

	$dump --no-data  > $dir/schema.sql
	$dump ${conf_tables[@]} --no-create-info  > $dir/data1.sql
	$dump ${data_tables[@]} --no-create-info --where "startDay BETWEEN '$STARTDAY' AND '$ENDDAY'" > $dir/data2.sql
}


function restore()
{
	dir=$RESTORE_DATA_DIR

	[[ -z `mysql -sNe "show databases like '$RESTORE_DATABASE';"` ]] && restoreDbExist=0 || restoreDbExist=1

	if [ $restoreDbExist == 0 ]; then
		mysql -sNe "CREATE DATABASE $RESTORE_DATABASE;";
		mysql $RESTORE_DATABASE < $dir/schema.sql
	elif [ $RESTORE_ALLOW_APPEND == 0 ]; then
		echo "Database '$RESTORE_DATABASE' alread exists.  Aborting restore."
		exit
	fi

	mysql $RESTORE_DATABASE < $dir/data1.sql
	mysql $RESTORE_DATABASE < $dir/data2.sql
}


if [ $isBackup == 1 ]; then
	backup
else
	restore
fi
