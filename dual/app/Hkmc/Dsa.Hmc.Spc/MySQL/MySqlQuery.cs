using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dsu.DB.MySQL;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace Dsa.Hmc.Spc
{
    public static class MySqlQuery
    {
        public static string InsertMeasure(DateTime dt, string lot, bool result, List<float> lstMeasure)
        {
            string SQL = string.Format("insert into measure({0}, {1}, {2}"
                , "time"
                , "lot"
                , "result");

            for (int i = 1; i <= lstMeasure.Count; i++)
                SQL += string.Format(", m{0}", i);

            SQL += ")";
            SQL += string.Format("values('{0}', '{1}', {2}"
               , dt.ToString("yyyy-MM-dd HH:mm:ss.fff")
               , lot
               , result);

            for (int i = 0; i < lstMeasure.Count; i++)
                SQL += string.Format(", {0}", lstMeasure[i]);
            SQL += ")";

            return SQL;
        }

        public static string SelectMeasure()
        {
            string SQL = string.Format("select * from measure");

            return SQL;
        }

    }
}
