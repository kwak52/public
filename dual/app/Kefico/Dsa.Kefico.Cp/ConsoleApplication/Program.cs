using System;
using System.Diagnostics;
using System.Configuration;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using static GaudiFileParserApi;
using static GaudiFileDBApi;
using Gnu.Getopt;
using System.Management;
using Dsu.Common.Utilities.Core;
using Dsu.Common.Utilities.ExtensionMethods;

namespace ConsoleApplication
{
    class Program
    {
        private static string _gaudiFile => "../pruef_ccs/pruef_s_mmxx/p9001270067.v01e";       // p9001270003
        private static MySqlConnection _conn;

        private static void Initialize()
        {
            _conn = new MySqlConnection(MwsConfig.getConnectionString());
            _conn.Open();
        }


        private static void TestParse()
        {
            var steps = ParseGaudiFile(_gaudiFile, "9001270003", "HT").ToArray();
            var step8480 = steps.Where(s => s.stepNumber == 8500);

            foreach (var s in steps)
                Trace.WriteLine(s.ToString() + "\\n");


            //foreach (var s in steps)
            //{
            //    Trace.WriteLine($"Step={s.stepNumber}");
            //    Trace.WriteLine($"  Position={s.positionNumber}");
            //    Trace.WriteLine($"  Name={s.@enum}");
            //    Trace.WriteLine($"  Dim={s.dim}");
            //    if (s.@enum == PsCCSDefineEnumSTDFunction.PRINTOUT)
            //        Trace.WriteLine($"  Params={s.p0}");
            //    else
            //        Trace.WriteLine($"  Min={s.min}, Max={s.max}");

            //    Trace.WriteLine($"  Mo={s.m0}");
            //    Trace.WriteLine($"  STDFuncType={s.stdFuncType}");
            //}
        }
        private static void TestDB()
        {
            Initialize();
            var pdvId = 1u;
            UploadStepFromGaudiFile(_conn, pdvId);
        }


        static void Main(string[] args)
        {
            HardwareId.ShowHardwareInformation();


            MwsConfig.loadFromAppConfig();

            string mwsPeers = MwsConfig.mwsPeers;
            int mwsPort = MwsConfig.mwsPort;
            string mwsNonExistent = ConfigurationManager.AppSettings["mwsNonExistent"];
            string dbDatabase = ConfigurationManager.AppSettings["dbDatabase"];
            Console.WriteLine($"mwsPeers={mwsPeers}, mwsPort={mwsPort}, dbDatabase={dbDatabase}, mwsNonExistent={mwsNonExistent}");



            //TestParse();
            //TestDB();

            Console.WriteLine("Bye!");
            System.Console.ReadKey();
        }
    }
}
