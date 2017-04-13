#!/bin/sh

if [ $# -ne 1 ]; then
  echo ""
  echo "Usage: $0 <partition name>"
  echo ""
  exit
fi

mydir="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"

if [ "x$database" = "x" -o "x$table" = "x" ]; then
  source $mydir/../setenv
fi


num_rows=$($mysql -Ne "select format(count(*),1) from $database.$table partition ($1);")
echo "$database.$table.$1 = $num_rows"
