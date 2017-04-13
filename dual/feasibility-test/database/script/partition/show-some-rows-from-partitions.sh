#!/bin/sh

mydir="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"

if [ "x$database" = "x" -o "x$table" = "x" ]; then
  source $mydir/../setenv
fi

limit=5

partitions=$(cat << EOF | mysql -N
  SELECT PARTITION_NAME 
   FROM information_schema.PARTITIONS
   WHERE TABLE_SCHEMA = '$database' AND TABLE_NAME = '$table';
EOF
)

for p in $partitions; do
  echo "Partition $p"
  mysql -e "SELECT * FROM $database.$table PARTITION ($p) limit $limit;"
  echo ""
done
