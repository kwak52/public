using System;
using System.IO;
using System.Net;
using System.Text;
using Dsu.Driver;
using NationalInstruments.DAQmx;
//using static Dsu.Driver.Plc.Melsec;
//using static Dsu.Driver.Plc.Melsec.MxPlcManagerModule;
//using Dsu.Driver.Plc;

namespace TestConsoleApplicationCS
{

    class Program
    {

        static string GetCookies()
        {
            string postData = $"login_username=apc&login_password=apc&submit=Log+On";
            CookieCollection cookies = new CookieCollection();
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://192.168.0.25/logon.htm");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://192.168.0.25/Forms/login1");
            request.CookieContainer = new CookieContainer();
            request.CookieContainer.Add(cookies); //recover cookies First request
            request.Method = WebRequestMethods.Http.Post;
            request.UserAgent = "Kefico-CPT/1.0";
            request.AllowWriteStreamBuffering = true;
            request.ProtocolVersion = HttpVersion.Version11;
            request.AllowAutoRedirect = false;
            request.ContentType = "application/x-www-form-urlencoded";

            byte[] byteArray = Encoding.ASCII.GetBytes(postData);
            request.ContentLength = byteArray.Length;
            Stream newStream = request.GetRequestStream(); //open connection
            newStream.Write(byteArray, 0, byteArray.Length); // Send the data.
            newStream.Close();

            HttpWebResponse response1 = (HttpWebResponse)request.GetResponse();
            var redirect = response1.Headers["Location"];



            using (StreamReader sr = new StreamReader(response1.GetResponseStream()))
            {
                string sourceCode = sr.ReadToEnd();
                File.WriteAllText("bbb.html", sourceCode);
                //return sourceCode;
            }


            request.CookieContainer = new CookieContainer();
            request.CookieContainer.Add(cookies);
            //Get the response from the server and save the cookies from the first request..
            //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            cookies = response1.Cookies;
            //}

            //static void Post()
            //{
            //string getUrl = "http://192.168.0.25/uiotemp.htm";

            //string getUrl = "http://192.168.0.25/NMC/ReP0sh9Syz+DY0CG3d1ltw/uiotemp.htm";
            string getUrl = redirect + "uiotemp.htm";
            //string getUrl = "https://www.facebook.com/login.php?login_attempt=1";
            HttpWebRequest getRequest = (HttpWebRequest)WebRequest.Create(getUrl);
            getRequest.CookieContainer = new CookieContainer();
            getRequest.CookieContainer.Add(cookies); //recover cookies First request
            getRequest.Method = WebRequestMethods.Http.Post;
            getRequest.UserAgent = "Kefico-CPT/1.0";
            getRequest.AllowWriteStreamBuffering = true;
            getRequest.ProtocolVersion = HttpVersion.Version11;
            getRequest.AllowAutoRedirect = true;
            getRequest.ContentType = "application/x-www-form-urlencoded";

            byte[] byteArray2 = Encoding.ASCII.GetBytes(postData);
            getRequest.ContentLength = byteArray2.Length;
            Stream newStream2 = getRequest.GetRequestStream(); //open connection
            newStream2.Write(byteArray2, 0, byteArray2.Length); // Send the data.
            newStream2.Close();

            HttpWebResponse getResponse = (HttpWebResponse)getRequest.GetResponse();
            using (StreamReader sr = new StreamReader(getResponse.GetResponseStream()))
            {
                string sourceCode = sr.ReadToEnd();
                return sourceCode;
            }

        }

        /*
         * 다음 함수 포함되면 linking 오류 발생
         * error CS0012: The type 'AICoupling' is defined in an assembly that is not referenced. 
         * You must add a reference to assembly 'NationalInstruments.DAQmx, Version=15.5.45.109, Culture=neutral, PublicKeyToken=4febd62461bf11a4'.
         */
        static void CreateDaqManager()
        {
            //// Type1 : channel 별 parameter 조정없이, 사전 정의된 parameter 로 모든 channel 을 사용할 경우.
            //NiDaqMcAi.createManagerSimple(4);     // manager 생성

            // Type2: channel 별 parameter 를 조정하여 각 channel 별로 다르게 사용할 경우.
            var parameters = NiDaq.CreateDefaultMcParameter(new[] { "Dev5/ai0", "Dev5/ai1", "Dev5/ai2", "Dev5/ai3" });
            var ch0 = parameters.AiChannelParameters[0];
            ch0.AICoupling = AICoupling.AC;
            var ch1 = parameters.AiChannelParameters[1];
            ch1.AICoupling = AICoupling.DC;
            var chmin = ch1.Min;
            ch1.Min = -5;
            ch1.Max = +5;
            chmin = ch1.Min;

            NiDaq.CreateMcManager(parameters);  // channel 별 parameter 를 다르게 적용한 manager 생성
        }

        static async void TestDaq()
        {
            // !!! Single channel manager 를 생성해서 사용하려면, 반드시 사전에 Multi channel manager 객체가 생성되어야 한다.
            // CreateDaqManager() 참고

            // DAQ 해당 channel 을 관장하는 manager 를 생성
            var daqch = NiDaq.CreateScManager("Dev5/ai0");

            // 해당 channel 로부터 비동기적으로 주어진 샘플 갯수만큼 수집하는 task를 await 를 통해 데이터 수집
            var data = await daqch.CollectAsync(100000);
            Console.WriteLine($"Got {data.Length} data");


            // 해당 channel 에 데이터가 수집될 때마다 수행하고 싶은 action 이 있다면 등록.
            // IDisposable 객체를 반환하므로, 해당 객체가 dispose 되지 않는 동안은 계속 action 수행됨.
            daqch.Subscribe(sample => { Console.WriteLine($"I got data {sample.Length}"); });
        }

//        static void TestPLC()
//        {
//            var manager = new MxPlcManager();
//            var info = manager.GetServerInfo();
//            Console.WriteLine($"== MxPlcServer Information ==\n{info}");

//            manager.SetDevice("D777", 66);
//            var d777 = manager.GetDevice("D777");

//            var read1 = manager.ReadDeviceRandom("x0", 1);
//            Console.WriteLine($"X0={read1}");

//            for (int i = 0; i < 4; i++)
//            {
//                var value = manager.GetDevice($"X{i}");
//                Console.WriteLine($"X{i} = {value}");
//            }

//            manager.SetDevice("D777", 66);
//            Console.WriteLine($"D777 = {manager.GetDevice("D777")}");



//            var read = manager.ReadDeviceRandom("Y0\nY1\nY2\nY3", 4);
//            manager.WriteDeviceRandom("Y0\nY1\nY2\nY3", new [] { 0, 0, 1, 0 });
//            read = manager.ReadDeviceRandom("Y0\nY1\nY2\nY3", 4);



////            var cpuType = MelsecType.CpuType.NewQnACpu(MelsecType.QnACpuType.Q02H);
////#if TCP
////            var parameters = new PlcParametersTcp(cpuType, "192.168.0.102")
////#else
////            var parameters = new PlcParametersUdp(cpuType, "192.168.0.102")
////#endif
////            {
////                ActNetworkNumber = 1,
////                ActStationNumber = 1,
////                ActSourceNetworkNumber = 1,
////                ActSourceStationNumber = 2,
////                ActTimeOut = 1000,
////                ActConnectUnitNumber = 0,

////#if TCP
////                ActDestinationPortNumber = 5002,
////#else
////                ActPortNumber = 11111,
////#endif
////            };
//            //var plcDriver = new PlcDriver(parameters);
//            //int[] onValues = { 1, 1, 1, 0 };
//            //int iResultValue = 1;
//            //plcDriver.WriteDeviceRandom("Y0\nY1\nY2\nY3", onValues);
//            //var read = plcDriver.ReadDeviceRandom("Y0\nY1\nY2\nY3", 4);

//            //for (int i = 0; i < 4; i++)
//            //{
//            //    var value = plcDriver.GetDevice($"Y{i}");
//            //    Console.WriteLine($"Y{i} = {value}");
//            //}

//            //plcDriver.WriteDeviceRandom("D777", new [] {77});
//            //read = plcDriver.ReadDeviceRandom("D777", 1);

//            //plcDriver.WriteDeviceRandom("M700", new[] { 77 });
//        }


        private static void TestVisaRs232C()
        {
            var rs232c = new NiVISA.Rs232cManager("ASRL3::INSTR");


        }
        [STAThread]
        static void Main(string[] args)
        {
            TestVisaRs232C();
            //TestPLC();
            //Login();
            //var html = GetCookies();
            //File.WriteAllText("aaa.html", html.ToString());

            //CreateDaqManager();
            //TestDaq();
            Console.Write("Hit any key..");
            Console.ReadKey();
        }
    }
}
