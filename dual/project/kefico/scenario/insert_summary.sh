#!/bin/sh

mydir="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
source $mydir/../bash/bash_functions

MYSQL_PWD=ccs;
export MYSQL_PWD;
function sql()
{
	# mysql kefico -sNe "$1"
	__=`mysql kefico -u ccs -sNe "$1"`
}


UseRANDOM=0
HOST=9000
PDVID=4
#PDVID=`sql "SELECT partNumber from pdv limit 1;"`
OPTS=`getopt -o -rh:p: --long use-random,host:,pdv:\
     -n 'parse-options' -- "$@"`
eval set -- "$OPTS"

while true; do
  case "$1" in
    -r | --use-random)    UseRANDOM=1;  shift ;;
    -h | --host)          HOST="$2";   shift; shift ;;
    -p | --pdv)           PDVID="$2";   shift; shift ;;
    -- ) shift; break ;;
    * ) break ;;
  esac
done

function generateDailyRandomSummary()
{
	local offset=$1
	local count=$(getRandomN 5 25)
	echo "$HOST;$PDVID;+;ECUIDZ0000;$offset"
	local n=0
	local ok="G"
	while [ $n -lt $count ]; do
		[[ $(getRandomN 0 10) = 0 ]] && ok="X" || ok="G";
		echo "$HOST;$PDVID;$ok;ECUIDZ0000;$offset"
		((n++))
	done
	echo "$HOST;$PDVID;-;ECUIDZ0000;$offset"
}

function generateRandomSummary()
{
	local count=20;
	while [ $count -gt 0 ]; do
		generateDailyRandomSummary -$count
		((count--))
	done
}

# HOST;PDV;{+-cGX};ECUID;DAYOFFSET
summary=$(cat<<EOF | sed -e "s/#.*//"
#	$HOST;$PDVID;+;ECUIDZ0000;-372
#	$HOST;$PDVID;+;ECUIDZ0000;-371
#	$HOST;$PDVID;+;ECUIDZ0000;-370
#	$HOST;$PDVID;+;ECUIDZ0000;-369
#	$HOST;$PDVID;+;ECUIDZ0000;-368
#	$HOST;$PDVID;+;ECUIDZ0000;-367
#	$HOST;$PDVID;+;ECUIDZ0000;-366
#	$HOST;$PDVID;+;ECUIDZ0000;-365
#	$HOST;$PDVID;G;ECUIDZ1001;-365
#	$HOST;$PDVID;-;ECUIDZ0000;-365
#	$HOST;$PDVID;+;ECUIDZ0000;-364
#	$HOST;$PDVID;G;ECUIDZ1002;-364
#	$HOST;$PDVID;-;ECUIDZ0000;-364
#	$HOST;$PDVID;+;ECUIDZ0000;-363
#	$HOST;$PDVID;G;ECUIDZ1003;-363
#	$HOST;$PDVID;-;ECUIDZ0000;-363
	$HOST;$PDVID;+;ECUIDZ0000;-7
	$HOST;$PDVID;G;ECUIDZ1004;-7
	$HOST;$PDVID;G;ECUIDZ1005;-7
	$HOST;$PDVID;G;ECUIDZ1006;-7
	$HOST;$PDVID;G;ECUIDZ1007;-7

	$HOST;$PDVID;+;ECUIDZ0000;-2
	$HOST;$PDVID;G;ECUIDZ1008;-2
	$HOST;$PDVID;G;ECUIDZ1009;-2
	$HOST;$PDVID;G;ECUIDZ1010;-2
	$HOST;$PDVID;G;ECUIDZ1011;-2
	$HOST;$PDVID;G;ECUIDZ1012;-2
	$HOST;$PDVID;G;ECUIDZ1013;-2
	$HOST;$PDVID;X;ECUIDZ1014;-2
	$HOST;$PDVID;X;ECUIDZ1014;-2
	$HOST;$PDVID;G;ECUIDZ1015;-2
	$HOST;$PDVID;G;ECUIDZ1016;-2
	$HOST;$PDVID;G;ECUIDZ1017;-2
	$HOST;$PDVID;G;ECUIDZ1018;-2
	$HOST;$PDVID;G;ECUIDZ1019;-2
 #	$HOST;$PDVID;c;ECUIDZ0000;-2
	$HOST;$PDVID;G;ECUIDZ1020;-2
	$HOST;$PDVID;X;ECUIDZ1021;-2
	$HOST;$PDVID;G;ECUIDZ1021;-2

	$HOST;$PDVID;+;ECUIDZ0000;0
	$HOST;$PDVID;+;ECUIDZ0000;-1
	$HOST;$PDVID;G;ECUIDZ1051;-1
	$HOST;$PDVID;X;ECUIDZ1052;-1
	$HOST;$PDVID;G;ECUIDZ1052;-1		# 2차 판정에서 성공
	$HOST;$PDVID;G;ECUIDZ1053;-1
#	$HOST;$PDVID;c;ECUIDZ0000;-1
	$HOST;$PDVID;G;ECUIDZ1054;-1
	$HOST;$PDVID;X;ECUIDZ1055;-1
	$HOST;$PDVID;X;ECUIDZ1055;-1

	$HOST;$PDVID;+;ECUIDZ0000;0
	$HOST;$PDVID;G;ECUIDZ1061;0
	$HOST;$PDVID;X;ECUIDZ1062;0
	$HOST;$PDVID;X;ECUIDZ1062;0
	$HOST;$PDVID;G;ECUIDZ1063;0
	$HOST;$PDVID;G;ECUIDZ1064;0
	$HOST;$PDVID;G;ECUIDZ1065;0
	$HOST;$PDVID;G;ECUIDZ1066;0
#	$HOST;$PDVID;c;ECUIDZ0000;0
	$HOST;$PDVID;G;ECUIDZ1067;0
	$HOST;$PDVID;G;ECUIDZ1068;0
	$HOST;$PDVID;G;ECUIDZ1069;0
	$HOST;$PDVID;X;ECUIDZ1070;0
	$HOST;$PDVID;X;ECUIDZ1070;0
	$HOST;$PDVID;X;ECUIDZ1071;0
	$HOST;$PDVID;X;ECUIDZ1071;0

EOF
)

minutesOffset=0
function procSummary {
	host=$1
	sec="'1-1'"
	pdvId=$2
	oper=$3
	ecuId="'$4'"
	dayOffset=$5
	date=$(date +"%Y-%m-%d" -d "`date`+$dayOffset days")
	time=$(date +"%H:%M:%S" -d "$time KST +$((++minutesOffset)) minutes")
	batchName=NULL
	echo "HOST=$host, pdvId=$pdvId, oper=$oper, day=$date, time=$time"

	eprom="'FFFFFFFFFFXXXXXXXXXX'"
	fixture="'4A-126'"

	sql "set @ccsId=getCcsIdFromHost(9000, '1-1'); select @ccsId;"
	ccsId=${__}

	case $oper in
	G|X)
		[[ $oper = 'G' ]] && ok=1 || ok=0
		sql "CALL _addBundlePartitionForDay('$date');"
		sql "CALL _generateBundleDataIntoTemporaryTable($pdvId, $ok);
			 CALL insertMeasure('$date', '$time', 33.2, $ccsId, $pdvId, 
				$ecuId, $eprom, $fixture, $batchName, $ok);"
		;;
	+)
		sql "CALL notifyPowerOnOffStatusChange('$date $time', $host, $sec, 1, $fixture, $batchName);"
		;;
	-)
		sql "CALL notifyPowerOnOffStatusChange('$date $time', $host, $sec, 0, $fixture, $batchName);"
		;;
	c)
		sql "CALL notifyBatchChange('$date $time', $ccsId, $pdvId, $ecuId, $eprom, $fixture, $batchName);"
		;;
	*)
		echo "ERROR: Invalid spec $oper"
		;;
	esac
	
}

function procSummaries {
	for s in $*; do
		procSummary `echo $s | tr ';' " "`
	done
}

function addDayPartitions {
	day=$1
	today=$(date +"%Y-%m-%d")

	while [ $day != $today ]; do
		sql "CALL _addBundlePartitionForDay('$day');"
		day=$(date +"%Y-%m-%d" -d "$day + 1 days")
	done
	sql "CALL _addBundlePartitionForDay('$today');"
}

addDayPartitions '2016-12-05';
if [ $UseRANDOM == 1 ]; then
	procSummaries `generateRandomSummary`
else
	procSummaries $summary
fi

