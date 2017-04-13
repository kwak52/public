#!/bin/sh

# https://github.com/dumblob/mysql2sqlite

mydir="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
source $mydir/../bash/bash_functions


STARTDAY=`date +"%Y-%m-%d"`
ENDDAY=`date +"%Y-%m-%d"`

OPTS=`getopt -o hs:e: \
    --long help,start-day:,end-day:  \
     -n 'parse-options' -- "$@"`
eval set -- "$OPTS"

while true; do
  case "$1" in
    -h | --help )               usage; exit;            shift ;;
    -s | --start-day )          STARTDAY="$2";          shift; shift ;;
    -e | --end-day )            ENDDAY="$2";            shift; shift ;;
    -- ) shift; break ;;
    * ) break ;;
  esac
done

schema=kefico
dayFilter="day BETWEEN '$STARTDAY' AND '$ENDDAY'"

hasData=$(mysql $schema -sNe "SELECT EXISTS (SELECT NULL FROM measure WHERE $dayFilter)")

if [ $hasData = 0 ]; then
	die "No data for backup, in period [$STARTDAY .. $ENDDAY]"
fi


function dump()
{
	mysqldump $schema --compact --skip-triggers --skip-extended-insert "$@" \
		|| die "Failed on mysqldump $*"
}

rm -rf $schema.mysql $schema.sqlite $schema.db

echo "Backing up $schema schemas."
dump --no-data 											\
	| grep -v "GENERATED ALWAYS AS"						\
	| sed -e "s/^  UNIQUE KEY \`.*//"					\
	| sed -e "s/^  KEY \`.*//"							\
	| sed -e "s/^  CONSTRAINT \`.*//"					\
	| sed -e "s/DEFAULT b'0'/DEFAULT '0'/"				\
	| sed -e "s/COMMENT '[^']*'//"						\
	> $schema.mysql

data_tables=(measure bundle)

dump --no-create-info ${data_tables[@]} \
	--where "$dayFilter" >> $schema.mysql

ccsIds=$(mysql $schema -sNe "SELECT DISTINCT(ccsId) FROM measure WHERE $dayFilter;" | tr '\n' ',' | sed -e "s/,$//" )
pdvIds=$(mysql $schema -sNe "SELECT DISTINCT(pdvId) FROM measure WHERE $dayFilter;" | tr '\n' ',' | sed -e "s/,$//" )

if [ -z $ccsIds -o -z $pdvIds ]; then
	die "No corresponding CCS or PDV data."
fi

echo "  Backing up pdvs with id=$pdvIds.."
dump --no-create-info pdv \
	--where "id IN ($pdvIds)" >> $schema.mysql

echo "  Backing up ccss with id=$ccsIds.."
dump --no-create-info ccs \
	--where "id IN ($ccsIds)" >> $schema.mysql

echo "  Backing up step with pdvid=$pdvIds.."
dump --no-create-info step \
	--where "pdvId IN ($pdvIds)" >> $schema.mysql

echo "  Backing up dynamicTopSummary.."
dump --no-create-info dynamicTopSummary \
	--where "$dayFilter" >> $schema.mysql

echo "  Backing up staticTopSummary.."
dump --no-create-info staticTopSummary \
	--where "$dayFilter" >> $schema.mysql

echo "  Backing up dimension.."
dump --no-create-info dimension >> $schema.mysql

echo "  Backing up pdvGroup.."
dump --no-create-info pdvGroup >> $schema.mysql




echo "  Converting MySQL SQL into SQLite SQL.."
mysql2sqlite $schema.mysql > $schema.sqlite

echo "  Creating SQLite database using SQLite SQL.."
sqlite3 $schema.db < $schema.sqlite

# rm -rf $schema.mysql $schema.sqlite

cat<<EOF | sqlite3 $schema.db
	drop table Unknown;
	drop table command;
	drop table emergencyRecoveryStatus;
	drop table errorCode;
	drop table function;
	drop table log;
	drop table preference;
	drop table sampleBatchName;
	drop table sampleMinMax;
	drop table sampleMessage;
	drop table sampleModName;
	drop table staticDailyStepSummary;
	drop table tableRevision;
	drop table viewInfo;
EOF

echo "DONE!!"

exit 0
