using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Some.Name.Space
{
    enum Month
    {
        일월 = 1,
        정월 = 1,
        Jan = 1,

        Feb = 2,
        이월 = 2,

        Mar = 3,
        삼월 = 3,

        Apr = 4,
        사월 = 4,

        May,
        Jun,
    }
}

namespace EnumTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Type t = Type.GetType("Some.Name.Space.Month");
            Console.WriteLine(t.ToString());

            var months = new[] {"Jan", "Feb", "Mar", "Apr", "일월", "이월", "삼월", "사월", "정월", "없는달", "May", "Jun"};
            foreach (var month in months)
            {
                try
                {
                    var parsed = Enum.Parse(t, month);
                    Console.WriteLine($"{month}: Enum value = {parsed}, int value = {(int)parsed}");
                }
                catch (Exception)
                {
                    Console.WriteLine($"{month}: Failed to parse!!!");
                }
            }
            Console.ReadKey();
        }
    }
}
