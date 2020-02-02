using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars;
using log4net.Appender;
using System.Configuration;

namespace AddressMapper
{
    public partial class FormRibonApp
        : DevExpress.XtraBars.Ribbon.RibbonForm
        , IAppender
    {
        public FormRibonApp()
        {
            InitializeComponent();
        }

        private void FormRibonApp_Load(object sender, EventArgs e)
        {
            Logger.Info("FormRibonApp launched.");

            TestCustomAppConfig();

            void TestCustomAppConfig()
            {
                Logger.Info("Custom configuration section test:");
                var appConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var config = appConfig.GetSection("exportPdv") as CustomConfigurationSection;
                Logger.Debug($"Source: folder={config.SourceFolderPrefix}");
                Logger.Debug($"Destination: folder={config.DestinationFolderPrefix}");
                Logger.Debug($"Destination server IP: {config.DestinationDBServerIp}");
            }
        }
    }
}