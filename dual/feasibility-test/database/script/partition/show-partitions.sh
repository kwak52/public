#!/bin/sh

mydir="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"

if [ "x$database" = "x" -o "x$table" = "x" ]; then
  source $mydir/../setenv
fi


sql=$(cat << EOF
  SELECT PARTITION_ORDINAL_POSITION as posi,
   PARTITION_NAME as name, TABLE_ROWS as rows, PARTITION_METHOD as method
   FROM information_schema.PARTITIONS
   WHERE TABLE_SCHEMA = '$database' AND TABLE_NAME = '$table';
EOF
)

echo "====================================================="
echo "Partition statisitics"
echo " - Number of rows is not exact. It's just estimation!"
echo "====================================================="
mysql -e "$sql"

