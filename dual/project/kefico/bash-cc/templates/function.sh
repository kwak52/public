
value1=1
value2=2

func1()
{
	value1=10;
	echo "in func1 value1 = $value1"
}

func2()
{
	local value2=20;
	echo "in func2 value2 = $value2"
}


func1
func2

echo "in global, value1 = $value1"
echo "in global, value2 = $value2"
