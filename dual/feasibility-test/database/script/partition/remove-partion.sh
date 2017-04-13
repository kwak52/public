#!/bin/sh


mydir="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
source $mydir/../bash_functions

select_partition; partition=${__}

if [ "x$partition" = "x" ]; then
  echo "No valid partion selected."
  exit
fi

echo "Do you really drop partition $partition from $database.$table?"
echo -n "Choice(y/n): "

read choice
if [ "$choice" = "y" ]; then
  mysql -e "alter table $database.$table drop partition $partition;"
fi

