#!/bin/sh

mydir="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"

counter=0
while [ $counter -lt 30 ]; do
  $mydir/insertData.sh $counter
  echo '---------DONE'
  sleep 20
  let counter=counter+1
done

