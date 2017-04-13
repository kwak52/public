#!/bin/bash

# 윤년 검사
# Usage : $0 year
# e.g $0 2016

mydir="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"

source $mydir/../bash_functions

is_leap_year $1; isleapyear=${__}
if [ "x$isleapyear" = "xtrue" ]; then
  echo "Yes, year $1 is leap year"
else
  echo "No, year $1 is not leap year"
fi
