using System;
using System.IO;
using System.Threading;
using System.Timers;
using Akka.Actor;
using Dsu.Common.Utilities.Core;
using log4net;
using log4net.Config;
using Topshelf;
using Topshelf.Hosts;
using Timer = System.Timers.Timer;
using System.Windows.Forms;

namespace MwsServiceApp
{
    /// <summary>
    /// MWS service
    /// http://getakka.net/docs/deployment-scenarios/Windows%20Service
    /// 서비스 관련 명령 수행 방법
    ///     - MwsServiceApp.exe install     설치
    ///     - MwsServiceApp.exe start       시작
    ///     - MwsServiceApp.exe stop        중지
    ///     - MwsServiceApp.exe uninstall   제거
    /// 서비스 실행시 필요한 사항
    ///     - 실행 파일(MwsServiceApp.exe) 및 실행 파일이 참조하는 folder 의 권한 부여
    /// services.msc
    /// 권한 설정
    ///     http://superuser.com/questions/315705/how-to-assign-permissions-to-manage-windows-service-when-uac-is-enabled
    ///     https://technet.microsoft.com/en-us/sysinternals/bb896653 process explorer
    /// </summary>
    public class MwsService : ServiceControl
    {
        public static ILog Logger;
        /// Debugging 용으로 console mode 로 동작하면 true, Windows Service 로 동작하면 false 값을 가짐.
        public static bool IsRunningAsConsole { get; private set; }

        private ActorSystem _actorSystem;
        private IActorRef _mwsServerActor;
        private Timer _timer; // Start() 함수 호출 이후에 작업을 수행하기 위한 persistence 용도의 timer
        private int _counter = 0;
        private HostControl _hostControl;

        public MwsService()
        {
            Logger = LogManager.GetLogger(typeof(MwsService));
            Logger.Info("===================================");
            Logger.Info("MWS Service object created.");
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


        protected void OnStartService(object sender, ElapsedEventArgs e)
        {
            try
            {
                MwsServer.MwsServerFailedMessageSubject.Subscribe(o =>
                {
                    var msg = $"Fatal excetion on server:\r\n{o}";
                    Logger.Error(msg);
                    Logger.Error("\r\n\r\nMWS Service exiting...");
                    if (IsRunningAsConsole)
                    {
                        MessageBox.Show(msg, "FATAL ERROR");
                    }
                    else
                        Stop(_hostControl);

                    Environment.Exit(-1);
                });

                IsRunningAsConsole = _hostControl is ConsoleRunHost;
                MwsConfig.mwsAsWindowsService = !IsRunningAsConsole;
                Logger.InfoFormat("--------Starting mws service as {0}.------", IsRunningAsConsole ? "console mode" : "Windows Service");
                if (Interlocked.Increment(ref _counter) == 1)
                {
                    if (IsRunningAsConsole)
                    {
                        Console.Title = "MWS Service: " + System.Diagnostics.Process.GetCurrentProcess().Id.ToString();
                        Console.SetBufferSize(Console.BufferWidth, 3000);
                    }

                    _timer.Elapsed -= OnStartService;
                    _timer.Stop();

                    Logger.Info("Creating mws server actor.");
                    var tpl = MwsServer.CreateMwsServerActor(Logger);
                    _actorSystem = tpl.Item1;
                    _mwsServerActor = tpl.Item2;

                    Logger.Info("actor created.");
                    Logger.Info("------ MWS system ready.");
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

            Logger.Info("--------Stopped mws service.--------");
            Interlocked.Decrement(ref _counter);
            return true;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Security.RunAsAdmin();

            MwsConfig.loadFromAppConfig();

            // Configure log4net, reading the TopshelfLog4net.config file
            XmlConfigurator.ConfigureAndWatch(new FileInfo(MwsConfig.log4netConfigFile));
            XmlConfigurator.Configure(new FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile));


            try
            {
                HostFactory.Run(hc =>
                {
                    hc.Service<MwsService>(sc =>
                    {
                        sc.ConstructUsing(() => new MwsService());
                        sc.WhenStarted((s, c) => s.Start(c));
                        sc.WhenStopped((s, c) => s.Stop(c));
                    });

                    hc.SetDisplayName("MWS Windows Service");
                    hc.SetDescription("MWS Windows Service with Topshelf");
                    hc.SetServiceName("MWSWindowsService");

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
                MwsService.Logger.Error($"EXCEPTION: {ex.Message}");
                throw;
            }

            if (MwsService.IsRunningAsConsole)
                Console.ReadKey();
        }
    }
}
