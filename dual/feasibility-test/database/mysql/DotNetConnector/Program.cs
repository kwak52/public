using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// - MySQL/Net Connector 설치 프로그램 다운로드
//  http://dev.mysql.com/downloads/connector/net/6.4.html
// - 설치 후, dll 참조
//  C:\Program Files (x86)\MySQL\MySQL Connector Net 6.4.6\Assemblies\v4.0\MySql.Data.dll
using MySql.Data;
using MySql.Data.MySqlClient;


namespace DotNetConnector
{
    class Program
    {
        static void Main(string[] args)
        {
            string connStr = "server=dualsoft.co.kr;user=securekwak;database=mysql;port=3306;password=kwak;";
            MySqlConnection conn = new MySqlConnection(connStr);


            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                // Perform database operations


                string sql = "SELECT host, user FROM user";
                MySqlCommand cmd = new MySqlCommand(sql, conn);

                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    // http://www.daveoncsharp.com/2009/11/retrieving-data-from-a-mysql-database/
                    var s1 = rdr.GetString("host");
                    var s2 = rdr.GetString("user");
                    var s3 = s1 + s2;
                    Console.WriteLine(rdr[0] + " -- " + rdr[1]);
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
            Console.WriteLine("Done.");
        }
    }
}
