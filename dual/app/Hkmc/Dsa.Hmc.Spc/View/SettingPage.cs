using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Base;
using System.Data;
using System.Drawing;
using System;
using System.Linq;
using Microsoft.Win32;
using System.Windows.Forms;

namespace Dsa.Hmc.Spc
{
    public delegate void UEventHandlerTagChanged(object sender, TagData tagData);
    public delegate void UEventHandlerMonitoringChanged(object sender, ConfigData configDatam);
    public delegate void UEventHandlerConfigChanged(object sender, ConfigData configDatam);

    public partial class SettingPage : XtraUserControl
    {
        public event UEventHandlerTagChanged UEventTagChanged;
        public event UEventHandlerMonitoringChanged UEventMonitoringChanged;
        public event UEventHandlerConfigChanged UEventConfigChanged;
        private ConfigData configData;
        private TagDataSource tagData;


        public SettingPage(ConfigData config, TagDataSource tagDataSource)
        {
            InitializeComponent();
            labelTitle.Text = "TAG 설정";
            labelSubtitle.Text = "주소 입력 및 Sensor offset 설정";
            //imageControl.Image = (Image)Properties.Resources.ResourceManager.GetObject("PLC");

            configData = new ConfigData();
            tagData = tagDataSource;
            configData = config;

//             textEdit_Line.EditValue = configData.Name;
//             textEdit_Ip.EditValue = configData.Ip;
//             textEdit_Ip.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
//             textEdit_Ip.Properties.Mask.EditMask = "(([0-1]?[0-9]{1,2}\\.)|(2[0-4][0-9]\\.)|(25[0-5]\\.)){3}(([0-1]?[0-9]{1,2})|(2[0-4][0-9])|(25[0-5]))";
//             textEdit_Port.EditValue = configData.Port;
//             textEdit_Min_IN.EditValue = configData.MinIN;
//             textEdit_Max_IN.EditValue = configData.MaxIN;
//             textEdit_Min_Out.EditValue = configData.MinOut;
//             textEdit_Max_Out.EditValue = configData.MaxOut;
//             foreach (TagData tagData in tagDataSource.Items)
//             {
//                 if (tagData.Name.Contains("PART CAR"))
//                     comboBoxEdit_Car.Properties.Items.Add(tagData.Name);
//             }
//             comboBoxEdit_Car.SelectedIndex = 0;
//             textEdit_Port.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
//             textEdit_Min_IN.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
//             textEdit_Max_IN.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
//             textEdit_Min_Out.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
//             textEdit_Max_Out.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
//             textEdit_ExportPath.EditValue = configData.ExportPath;

        }

        private void action1_Update(object sender, EventArgs e)
        {
//             textEdit_Line.Enabled = !toggleSwitch1.IsOn;
//             textEdit_Ip.Enabled = !toggleSwitch1.IsOn;
//             textEdit_Port.Enabled = !toggleSwitch1.IsOn;
        }
      

        private void toggleSwitch1_Toggled(object sender, EventArgs e)
        {
            
            UEventMonitoringChanged?.Invoke(sender, configData);
        }

        private void textEdit_DataPath_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;
            textEdit_ExportPath.EditValue = folderBrowserDialog1.SelectedPath;
            ConfigChangeEvent();
        }

      
      

        private void ConfigChangeEvent()
        {
            if (UEventConfigChanged == null)
                return;

            configData.Name = textEdit_Line.EditValue?.ToString();
            configData.Ip = textEdit_Ip.EditValue?.ToString();
            configData.Port = (int)textEdit_Port.EditValue;
            configData.MinIN = (float)textEdit_Min_IN.EditValue;
            configData.MaxIN = (float)textEdit_Max_IN.EditValue;
            configData.MinOut = (float)textEdit_Min_Out.EditValue;
            configData.MaxOut = (float)textEdit_Max_Out.EditValue;
            configData.DisplayCar = comboBoxEdit_Car.EditValue.ToString();
            configData.ExportPath = textEdit_ExportPath.EditValue?.ToString();
            configData.LHPath = textEdit_LHPath.EditValue?.ToString();
            configData.RHPath = textEdit_RHPath.EditValue?.ToString(); 

            UEventConfigChanged?.Invoke(this, configData);
        }

        private void textEdit_Line_EditValueChanged(object sender, EventArgs e)
        {
            ConfigChangeEvent();
        }

        private void textEdit_Ip_EditValueChanged(object sender, EventArgs e)
        {
            ConfigChangeEvent();
        }

        private void textEdit_Port_EditValueChanged(object sender, EventArgs e)
        {
            ConfigChangeEvent();
        }

        private void textEdit_Resolution_EditValueChanged(object sender, EventArgs e)
        {
            ConfigChangeEvent();
        }

        private void comboBoxEdit_Car_SelectedIndexChanged(object sender, EventArgs e)
        {
            ConfigChangeEvent();
        }

        private void textEdit_Max_In_EditValueChanged(object sender, EventArgs e)
        {
            ConfigChangeEvent();
        }

        private void textEdit_Min_Out_EditValueChanged(object sender, EventArgs e)
        {
            ConfigChangeEvent();
        }

        private void textEdit_Max_Out_EditValueChanged(object sender, EventArgs e)
        {
            ConfigChangeEvent();
        }
    }
}

