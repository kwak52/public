#!/bin/sh

# http://stackoverflow.com/questions/11233825/multi-dimensional-arrays-in-bash
# multidimensional : should be separated with space for first dimension, with non-space for second dimension

array2d="1.1:1.2:1.3
	 2.1:2.2
	 3.1:3.2:3.3:3.4"

function process2ndDimension {
    for dimension2 in $*
    do
        echo -n $dimension2 "   "
    done
    echo
}

function process1stDimension {
    for dimension1 in $*
    do
        process2ndDimension `echo $dimension1 | tr : " "`
    done
}

process1stDimension $array2d




conf_tables=(pdv pdvGroup dimension function step user preference)
data_tables=(measure bundle)
tables=(${conf_tables[@]} ${data_tables[@]})

echo ${conf_tables[0]}
echo ${conf_tables[@]}
echo ${data_tables[@]}
echo ${tables[@]}


