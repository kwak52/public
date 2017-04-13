using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dsa.Test.DbReader
{
    public partial class Form1 : Form
    {
        MySql mySql = new MySql();
        public Form1()
        {
            InitializeComponent();
        }

        public object selector(IDataRecord arg)
        {
            return arg;
        }

        public object selector(IDataReader arg)
        {
            object[] arrObj = new object[arg.FieldCount];
            arg.GetValues(arrObj);
            return arrObj;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string query = string.Format("SELECT * FROM {0}", mySql.DataTableName);
            DataTable dtTime = new DataTable();

            dtTime.Columns.Add(new DataColumn("Func"));
            dtTime.Columns.Add(new DataColumn("Time"));

            Stopwatch sw = new Stopwatch();

            // Field 7개, Records 100만건   "ahn_db.CUSTOMER_ORDER"
            // Read3,4,5  < Read2 < Read0 < Read1  
            // DataRecord < DataAdapter.Flll < DataReader < DataAdapter.Load
            // DataAdapter.Flll와 DataReader는 유사한 차이로 데이터 이벤트 및 가공을 위해 DataReader 사용
            for (int i = 0; i < 50; i++)
            {
                sw = Stopwatch.StartNew();
                mySql.Read0(query); // 4.28 ~ 4.59 sec
                dtTime.Rows.Add(new string[] { "Read0", sw.ElapsedMilliseconds.ToString("00:000") });

                sw = Stopwatch.StartNew();
                mySql.Read1(query); // 9.72 ~ 11.07 sec
                dtTime.Rows.Add(new string[] { "Read1", sw.ElapsedMilliseconds.ToString("00:000") });

                sw = Stopwatch.StartNew();
                mySql.Read2<MySqlDataAdapter>(query); // 4.14 ~ 4.27 sec
                dtTime.Rows.Add(new string[] { "Read2", sw.ElapsedMilliseconds.ToString("00:000") });

                sw = Stopwatch.StartNew();
                mySql.Read3(query, selector).ToArray(); // 3.25 ~ 4.6 sec
                dtTime.Rows.Add(new string[] { "Read3", sw.ElapsedMilliseconds.ToString("00:000") });

                sw = Stopwatch.StartNew();
                mySql.Read4(query, selector); // 3.38 ~ 4.58 sec
                dtTime.Rows.Add(new string[] { "Read4", sw.ElapsedMilliseconds.ToString("00:000") });

                sw = Stopwatch.StartNew();
                mySql.Read5(query, selector); // 3.29 ~ 4.42 sec
                dtTime.Rows.Add(new string[] { "Read5", sw.ElapsedMilliseconds.ToString("00:000") });

                Console.WriteLine(dtTime.Rows[i + 0].ItemArray[0] + "\t" + dtTime.Rows[i + 0].ItemArray[1]);
            }

            dataGridView1.DataSource = dtTime;
        }
    }

}
