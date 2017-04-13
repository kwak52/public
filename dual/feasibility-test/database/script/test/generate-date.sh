#!/bin/sh

counter=0
while [ $counter -lt 365 ]; do
  echo $(date +"%Y-%m-%d %H:%M:%S" -d "-1 years +$counter days")
  let counter=counter+1
done

