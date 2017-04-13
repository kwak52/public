#define TCP

using System;
using System.IO;
using System.Threading;
using System.Timers;
using Akka.Actor;
using log4net;
using log4net.Config;
using Topshelf;
using Topshelf.Hosts;
using Dsu.Common.Utilities.Core;
using Dsu.Driver.Plc.Melsec;
using static Dsu.Driver.Plc.Melsec.Type;
using static Dsu.Driver.Plc.Melsec.MxPlcParameters;

using Timer = System.Timers.Timer;

namespace MxPlcServiceApp
{
    /// <summary>
    /// MWS service
    /// http://getakka.net/docs/deployment-scenarios/Windows%20Service
    /// 서비스 관련 명령 수행 방법
    ///     - MxPlcServiceApp.exe install     설치
    ///     - MxPlcServiceApp.exe start       시작
    ///     - MxPlcServiceApp.exe stop        중지
    ///     - MxPlcServiceApp.exe uninstall   제거
    /// 서비스 실행시 필요한 사항
    ///     - 실행 파일(MxPlcServiceApp.exe) 및 실행 파일이 참조하는 folder 의 권한 부여
    /// services.msc
    /// 권한 설정
    ///     http://superuser.com/questions/315705/how-to-assign-permissions-to-manage-windows-service-when-uac-is-enabled
    ///     https://technet.microsoft.com/en-us/sysinternals/bb896653 process explorer
    /// </summary>
    public class MxPlcService : ServiceControl
    {
        public static ILog Logger;
        /// Debugging 용으로 console mode 로 동작하면 true, Windows Service 로 동작하면 false 값을 가짐.
        public static bool IsRunningAsConsole { get; private set; }

        private ActorSystem _actorSystem;
        private IActorRef _mxPlcServerActor;
        private Timer _timer; // Start() 함수 호출 이후에 작업을 수행하기 위한 persistence 용도의 timer
        private int _counter = 0;
        private HostControl _hostControl;

        public MxPlcService()
        {
            Logger = LogManager.GetLogger(typeof(MxPlcService));
            Logger.Info("===================================");
            Logger.Info("Melsec PLC Service object created.");
            Logger.Info("COLUMNS: date time [thread] messages");
            Logger.Info("===================================");
            _timer = new Timer(1000);
        }

        /// <summary>
        /// Windows Service 를 시작.
        /// 매우 빠른 시간안에 Start() 함수가 종료되지 않으면 서비스 시작이 불가능하다.
        /// Timer 를 이용하여, 나중에 필요한 작업을 수행한다.
        /// ServiceControl.Start() 구현.
        /// </summary>
        public bool Start(HostControl hostControl)
        {
            _hostControl = hostControl;
            _timer.Elapsed += OnStartService;
            _timer.Start();

            return true;
        }

        private PlcParametersBase GetPlcParameters()
        {
            var plcIp = "192.168.0.99";
            var cpuType = CpuType.NewRCpu(RCpuType.Q06UDV);
            var networkCardInterface = ActType.QNUDECPUTCP;
            //var cpuType = CpuType.NewQnACpu(QnACpuType.Q02H);
            //var plcIp = "192.168.0.102";
            ////var networkCardInterface = ActType.QJ71E71TCP;


#if TCP
            //var parameters = new PlcParametersQJ71E71TCP(cpuType, plcIp)
            var parameters = new PlcParametersQNUDECPUTCP(cpuType, plcIp)
#else
            var parameters = new PlcParametersQJ71E71UDP(cpuType, plcIp)
#endif
            {
                //ActNetworkNumber = 1,
                //ActStationNumber = 1,
                //ActSourceNetworkNumber = 1,
                //ActSourceStationNumber = 2,
                //ActTimeOut = 1000,
                //ActConnectUnitNumber = 0,

                ActNetworkNumber = 0,
                ActStationNumber = 255,
                ActTimeOut = 1000,

//#if TCP
//                ActDestinationPortNumber = 5002,
//#else
//                ActPortNumber = 11111,
//#endif
            };

            Logger.Info(parameters.ToString());

            return parameters;
        }
        protected void OnStartService(object sender, ElapsedEventArgs e)
        {
            try
            {
                IsRunningAsConsole = _hostControl is ConsoleRunHost;
                Logger.InfoFormat("--------Starting Melsec PLC service as {0}.------", IsRunningAsConsole ? "console mode" : "Windows Service");
                if (Interlocked.Increment(ref _counter) == 1)
                {
                    if (IsRunningAsConsole)
                    {
                        Console.Title = "MxPLC Service: " + System.Diagnostics.Process.GetCurrentProcess().Id.ToString();
                        Console.SetBufferSize(Console.BufferWidth, 3000);
                    }

                    _timer.Elapsed -= OnStartService;
                    _timer.Stop();

                    Logger.Info("Creating mx plc server actor.");
                    var tpl = MxPlcServer.CreateMxPlcServerActor(GetPlcParameters(), Logger);
                    _actorSystem = tpl.Item1;
                    _mxPlcServerActor = tpl.Item2;

                    Logger.Info("actor created.");
                    Logger.Info("------ Mx PLC system ready.");
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to start service with exception: {ex.ToString()}");
                throw;
            }
        }
        /// <summary>
        /// Windows Service 를 중지.
        /// ServiceControl.Stop() 구현.
        /// </summary>
        /// <param name="hostControl"></param>
        /// <returns></returns>
        public bool Stop(HostControl hostControl)
        {
            //this is where you stop your actor system
            _actorSystem.Terminate();

            Logger.Info("--------Stopped Melsec PLC service.--------");
            Interlocked.Decrement(ref _counter);
            return true;
        }
    }
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Security.RunAsAdmin();

            MxPlcConfig.loadFromAppConfig();

            // Configure log4net, reading the TopshelfLog4net.config file
            XmlConfigurator.ConfigureAndWatch(new FileInfo(MxPlcConfig.log4netConfigFile));
            XmlConfigurator.Configure(new FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile));


            try
            {
                HostFactory.Run(hc =>
                {
                    hc.Service<MxPlcService>(sc =>
                    {
                        sc.ConstructUsing(() => new MxPlcService());
                        sc.WhenStarted((s, c) => s.Start(c));
                        sc.WhenStopped((s, c) => s.Stop(c));
                    });

                    hc.SetDisplayName("MxPlc Windows Service");
                    hc.SetDescription("MxPlc Windows Service with Topshelf");
                    hc.SetServiceName("MxPlcWindowsService");

                    //hc.RunAs("Administrator", "kwak");
                    hc.RunAsLocalSystem();
                    //hc.RunAsNetworkService();
                    //hc.SetInstanceName("MWS Window Service Instance");

                    hc.StartAutomaticallyDelayed();
                    hc.UseLog4Net();
                });
            }
            catch (Exception ex)
            {
                MxPlcService.Logger.Error($"EXCEPTION: {ex.Message}");
                throw;
            }

            if (MxPlcService.IsRunningAsConsole)
                Console.ReadKey();
        }
    }
}
