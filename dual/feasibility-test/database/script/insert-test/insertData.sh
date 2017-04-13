#!/bin/sh

mydir="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"

if [ "x$database" = "x" -o "x$table" = "x" ]; then
  source $mydir/../setenv
fi

counter=0
stat_table=statistics

iter=$1

iter=200
sql=tmp_insert_${iter}K.sql
rm -f $sql
$mydir/insertData.perl $iter > $sql
tsql=t$$.$sql

while [ $counter -lt 366 ]; do
  day=`date +"%Y-%m-%d" -d "2016-01-01 +$counter days"`
  sed -e "s/REPLACE_ME/$day/" < $sql >$tsql

  tts=$SECONDS
  ts=`date +"%Y-%m-%d %H:%M:%S"`
  $mysql < $tsql
  elapsed=$(($SECONDS - $tts))
  echo "[$iter-$counter-$day] $elapsed(sec)"
  rm -f $tsql

  rows=`echo "SELECT TABLE_ROWS FROM information_schema.TABLES where table_name = '$table';" | $mysql -N`
  dbsize=`du -s $dblocation | awk '{print $1}'`
  let counter=counter+1

  cat << EOF | $mysql
INSERT INTO $database.$stat_table(ts, elapsed, dbsize, num_rows)
VALUES('$ts', '$elapsed', $dbsize, $rows);
EOF


# Calculating time difference on mysql 
# SELECT DATE_FORMAT(TIMEDIFF(tf, ts) , "%H:%i:%s") AS epalsed FROM statistics;
# SELECT AVG(elapsed) FROM statistics;


done


