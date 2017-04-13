#!/bin/sh

mydir="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"

source $mydir/../bash_functions
if [ "x$database" = "x" -o "x$table" = "x" ]; then
  source $mydir/../setenv
fi


num_rows=$(cat << EOF | $mysql -N
  SELECT TABLE_ROWS from information_schema.TABLES
  where TABLE_SCHEMA = '$database' AND TABLE_NAME = '$table';
EOF
)
printf "Rows: %'d\n" $num_rows
arithmatic "$num_rows/70000000"
days=${__}
printf "Days=%'.3f\n" $days

if [ -r $dblocation ];then
  du_data=`du -s $dblocation | awk '{print $0/(1024*1024)}'`
  du_binlog=`du $dblocation/../mysql-bin* | awk '{s+=$1} END {print s/(1024*1024)}'`

  arithmatic "($du_data + $du_binlog)"
  du_total=${__}

  echo "Disk Usage(total): $du_total GB"
  echo "  Data size: $du_data GB"
  echo "  Binlog size: $du_binlog GB"


  arithmatic "$du_total/$days"
  du_d=${__}
  arithmatic "($du_d * 30)"
  du_m=${__}
  arithmatic "($du_d * 365)"
  du_y=${__}

  echo "Disk quota requirement estimation"
  printf '  Daily = %.4f GB\n' $du_d
  printf '  Monthly = %.4f GB\n' $du_m
  printf '  Yearly = %.4f GB\n' $du_y

else
  echo "You do not have read permission on $dblocation."
fi

