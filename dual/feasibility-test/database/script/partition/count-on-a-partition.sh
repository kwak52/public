#!/bin/sh

# Counts exact number of rows on a given partition
# Usage : $0 <partition_name>
#


mydir="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
source $mydir/../bash_functions

if [ $# = 1 ]; then
  partition=$1
  is_exists_partition $1
  if [ ${__} != "true" ]; then
    "No partition named $1"
  fi
else
  select_partition; partition=${__}
fi

if [ "x$partition" = "x" ]; then
  echo "No valid partion selected."
  exit
fi



rows=$(mysql -Ne "select count(*) from $database.$table partition ($partition)")
printf "%'3d rows on %s\n" $rows $database.$table.$partition

