using System;
using DevExpress.XtraEditors;
using Dsu.PLC.Melsec;
using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.XtraBars.Docking2010.Views.WindowsUI;
using DevExpress.XtraBars.Docking2010.Views;

namespace Dsa.Hmc.Spc
{
    public partial class MainForm : XtraForm
    {
        private PlcMXComp _PlcMX;
        private MySqlClient _conn;
        private SerialLVDT _serial;
        private ConfigData _configData;
        private TagDataSource _tagDataS;
        private LogPage _logPage;

        public MainForm()
        {
            InitializeComponent();
            _PlcMX = new PlcMXComp();
            _conn = new MySqlClient();
            _serial = new SerialLVDT();
        }

        private void MainForm_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            if (PreClosingConfirmation() == DialogResult.Yes)
            {
                //SaveConfig();
                //_PlcMX?.DisConnect(_subscription);
                //t.Stop();

                //Thread.Sleep(1000);
                Dispose(true);
                Application.Exit();
            }
            else
                e.Cancel = true;
        }

        private DialogResult PreClosingConfirmation()
        {
            DialogResult dialogResult;
            dialogResult = XtraMessageBox.Show(this, "Exit Application?", "Sensor Monitoring", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return dialogResult;
        }

        private void MainForm_Load(object sender, System.EventArgs e)
        {
            mainTileContainer.ButtonClick += MainTileContainer_ButtonClick;
            _serial.UEventDataReceived += Serial_UEventDataReceived;
            _conn.Open();

            CreateLayoutLogPage();
            CreateLayoutSettingPage();

            windowsUIView.ActivateContainer(mainTileContainer);

        }

        private void MainTileContainer_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            PageGroup pageGroup = (e.Button.Properties.Tag as PageGroup);
            if (pageGroup != null)
                windowsUIView.ActivateContainer(pageGroup);
        }

        private void CreateLayoutLogPage()
        {
            PageGroup pageGroup = new PageGroup();
            pageGroup.Parent = mainTileContainer;
            pageGroup.Caption = "LOG";
            windowsUIView.ContentContainers.Add(pageGroup);
            _logPage = new LogPage(_tagDataS, _conn.GetDataFromDBView(MySqlQuery.SelectMeasure()));
            _logPage.Dock = System.Windows.Forms.DockStyle.Fill;
            BaseDocument document = windowsUIView.AddDocument(_logPage);
            document.Caption = "LOG Export";
            mainTileContainer.Buttons["DB"].Properties.Tag = pageGroup;
            pageGroup.Items.Add(document as Document);
        }

        private void CreateLayoutSettingPage()
        {
            PageGroup pageGroup = new PageGroup();
            pageGroup.Parent = mainTileContainer;
            pageGroup.Caption = "SETTING";
            windowsUIView.ContentContainers.Add(pageGroup);

            SettingPage settingPage = new SettingPage(_configData, _tagDataS);

           // windowsUIView.Caption = _configData.Name;
//             settingPage.UEventTagChanged += SettingPage_UEventTagChanged;
//             settingPage.UEventMonitoringChanged += SettingPage_UEventMonitoringChanged;
//             settingPage.UEventConfigChanged += SettingPage_UEventConfigChanged;
            settingPage.Dock = DockStyle.Fill;
            BaseDocument document = windowsUIView.AddDocument(settingPage);
            document.Caption = "SETTING";
            mainTileContainer.Buttons["SETTING"].Properties.Tag = pageGroup;
            pageGroup.Items.Add(document as Document);

        }


        private void Serial_UEventDataReceived(object sender, string msg)
        {
            List<float> lstMeasure = new List<float>();
            int start = Convert.ToInt32(msg.Split(',')[0]);
            int end = Convert.ToInt32(msg.Split(',')[1]);
            for (int n = 2; n < end - start + 3; n++)
                lstMeasure.Add(Convert.ToSingle(msg.Split(',')[n]));


            _conn.Execute(MySqlQuery.InsertMeasure(DateTime.Now, "Lot"/*타각기 tcp/ip*/, false, lstMeasure));
        }
    }
}