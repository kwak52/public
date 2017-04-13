#!/bin/sh

show_while_loop()
{
	counter=0
	while [ $counter -lt 10 ]; do
		echo $counter
		((counter++))
	done
}


show_array_usage()
{
	my_array=("ONE" "TWO")
	echo ${my_array[0]}
	echo ${my_array[@]}		# "ONE" "TWO"

	declare -A PERSON		# array decl
	PERSON["FNAME"]='John'
	PERSON["LNAME"]='Andrew'
	echo "${!PERSON[@]}";	# prints FNAME LNAME.  shows the index key strings for array

}


show_strlen()
{
	str="0123456789"
	echo ${#str}
}

show_substr()
{
	str="0123456789"
	echo ${str:0:3}		# 012 : from index 0, 3 chars
	echo ${str:3:4}		# 3456 : from index 3, 4 chars
}



show_eval_to_variable()
{
	now=$(date)		# save result of date into variable now
}


show_date_usage()
{
	yesterday=$(date +"%Y-%m-%d" -d "`date`-1 days")
	an_hour_ago=$(date +"%H:%M:%S" -d "`date`-1 hours")
	noon=$(date +"%H:%M:%S" -d "11:59:00 KST + 1 minutes")
}


show_function_returning_value()
{
	__=`whoami`
}

show_function_call()
{
	show_function_returning_value
	echo ${__}
}


show_ternary_conditional_operator()
{
	b=5
	[[ $b = 5 ]] && a="OK" || a="NG"
	echo $a
}

test_empty()
{
	[[ -z "" ]] && echo "empty" || echo "non-empty"
	[[ -z "xx" ]] && echo "empty" || echo "non-empty"
}


show_seq_generator()
{
	for r in $(seq 1 100); do
		echo $r
	done
}
