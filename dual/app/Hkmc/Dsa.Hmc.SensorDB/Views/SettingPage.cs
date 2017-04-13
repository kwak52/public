using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Base;
using System.Data;
using System.Drawing;
using System;
using System.Linq;
using Microsoft.Win32;

namespace Dsa.Hmc.SensorDB
{
    public delegate void UEventHandlerTagChanged(object sender, TagData tagData);
    public delegate void UEventHandlerMonitoringChanged(object sender, ConfigData configDatam);
    public delegate void UEventHandlerConfigChanged(object sender, ConfigData configDatam);

    public partial class SettingPage : XtraUserControl
    {
        public event UEventHandlerTagChanged UEventTagChanged;
        public event UEventHandlerMonitoringChanged UEventMonitoringChanged;
        public event UEventHandlerConfigChanged UEventConfigChanged;
        private TagDataSource tagData;
        private ConfigData configData;

        private string COL_SENSOR_TAG = "Tag";
        private string COL_SENSOR_ADDRESS = "Address";
        private string COL_SENSOR_OFFSET = "Offset";
        private string COL_SENSOR_LIMIT = "Limit";

        private string COL_CAR_TAG = "Tag";
        private string COL_CAR_ADDRESS = "Address";
        private string COL_CAR_TYPE = "CarType";

        public SettingPage(ConfigData config , TagDataSource tagDataSource)
        {
            InitializeComponent();
            labelTitle.Text = "TAG 설정";
            labelSubtitle.Text = "주소 입력 및 Sensor offset 설정";
            imageControl.Image = (Image)Properties.Resources.ResourceManager.GetObject("PLC");

            configData = new ConfigData();
            tagData = tagDataSource;
            configData = config;

            textEdit_Line.EditValue = configData.Name;
            textEdit_Ip.EditValue = configData.Ip;
            textEdit_Ip.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            textEdit_Ip.Properties.Mask.EditMask = "(([0-1]?[0-9]{1,2}\\.)|(2[0-4][0-9]\\.)|(25[0-5]\\.)){3}(([0-1]?[0-9]{1,2})|(2[0-4][0-9])|(25[0-5]))";
            textEdit_Port.EditValue = configData.Port;
            textEdit_Min_IN.EditValue = configData.MinIN;
            textEdit_Max_IN.EditValue = configData.MaxIN;
            textEdit_Min_Out.EditValue = configData.MinOut;
            textEdit_Max_Out.EditValue = configData.MaxOut;
            foreach (TagData tagData in tagDataSource.Items)
            {
                if (tagData.Name.Contains("PART CAR"))
                    comboBoxEdit_Car.Properties.Items.Add(tagData.Name);
            }
            comboBoxEdit_Car.SelectedIndex = 0;
            textEdit_Port.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            textEdit_Min_IN.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            textEdit_Max_IN.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            textEdit_Min_Out.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            textEdit_Max_Out.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            textEdit_ExportPath.EditValue = configData.ExportPath;

        }

        private void action1_Update(object sender, EventArgs e)
        {
            textEdit_Line.Enabled = !toggleSwitch1.IsOn;
            textEdit_Ip.Enabled = !toggleSwitch1.IsOn;
            textEdit_Port.Enabled = !toggleSwitch1.IsOn;
        }


        private DataTable CreateTableSensor(TagDataSource tagDataSource)
        {
            DataTable dtSetting = new DataTable();
            dtSetting.Columns.Add(COL_SENSOR_TAG, typeof(string));
            dtSetting.Columns.Add(COL_SENSOR_ADDRESS, typeof(string));
            dtSetting.Columns.Add(COL_SENSOR_OFFSET, typeof(float));
            dtSetting.Columns.Add(COL_SENSOR_LIMIT, typeof(float));

            foreach (TagData tagData in tagDataSource.Items)
            {
                if (!tagData.IsSensor)
                    continue;

                DataRow dr = dtSetting.NewRow();
                dr[COL_SENSOR_TAG] = tagData.Name;
                dr[COL_SENSOR_ADDRESS] = tagData.Address;
                dr[COL_SENSOR_OFFSET] = tagData.Offset;
                dr[COL_SENSOR_LIMIT] = tagData.Limit;
                dtSetting.Rows.Add(dr);
            }

            return dtSetting;
        }

        private DataTable CreateTableCar(TagDataSource tagDataSource)
        {
            DataTable dtSetting = new DataTable();
            dtSetting.Columns.Add(COL_CAR_TAG, typeof(string));
            dtSetting.Columns.Add(COL_CAR_ADDRESS, typeof(string));
            dtSetting.Columns.Add(COL_CAR_TYPE, typeof(string));

            foreach (TagData tagData in tagDataSource.Items)
            {
                if (tagData.IsSensor)
                    continue;

                DataRow dr = dtSetting.NewRow();
                dr[COL_CAR_TAG] = tagData.Name;
                dr[COL_CAR_ADDRESS] = tagData.Address;
                dr[COL_CAR_TYPE] = tagData.CarType;
                dtSetting.Rows.Add(dr);
            }

            return dtSetting;
        }

        private void ucGrid_Sensor_Load(object sender, System.EventArgs e)
        {
            ucGrid_Sensor.RowFont = new System.Drawing.Font("Tahoma", 18F, FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            ucGrid_Sensor.ColumnFont = new System.Drawing.Font("Tahoma", 18F, FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            ucGrid_Sensor.Editable = true;
            ucGrid_Sensor.ShowAutoFilterRow = false;
            ucGrid_Sensor.GridView.OptionsView.EnableAppearanceOddRow = true;

            ucGrid_Sensor.DataSource = CreateTableSensor(tagData);
            ucGrid_Sensor.GridView.Columns[COL_SENSOR_TAG].OptionsColumn.AllowEdit = false;
            ucGrid_Sensor.UEventCellValueChanged += ucGrid_Sensor_UEventCellValueChanged;
        }

        private void ucGrid_Car_Load(object sender, EventArgs e)
        {
            ucGrid_Car.RowFont = new System.Drawing.Font("Tahoma", 18F, FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            ucGrid_Car.ColumnFont = new System.Drawing.Font("Tahoma", 18F, FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            ucGrid_Car.Editable = true;
            ucGrid_Car.ShowAutoFilterRow = false;
            ucGrid_Car.GridView.OptionsView.EnableAppearanceOddRow = true;

            ucGrid_Car.DataSource = CreateTableCar(tagData);
            ucGrid_Car.GridView.Columns[COL_CAR_TAG].OptionsColumn.AllowEdit = false;
            ucGrid_Car.UEventCellValueChanged += UcGrid_Car_UEventCellValueChanged;
        }

        private void UcGrid_Car_UEventCellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            DataRowView dr = (DataRowView)ucGrid_Car.GridView.GetRow(e.RowHandle);
            TagData tagDataEdit = tagData.Items.Single(tag => dr[COL_SENSOR_TAG].ToString() == tag.Name);

            tagDataEdit.Address = dr[COL_CAR_ADDRESS].ToString();
            tagDataEdit.CarType = dr[COL_CAR_TYPE].ToString();
            UEventTagChanged?.Invoke(sender, tagDataEdit);
        }

        private void ucGrid_Sensor_UEventCellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            DataRowView dr = (DataRowView)ucGrid_Sensor.GridView.GetRow(e.RowHandle);
            TagData tagDataEdit = tagData.Items.Single(tag => dr[COL_SENSOR_TAG].ToString() == tag.Name);

            tagDataEdit.Address = dr[COL_SENSOR_ADDRESS].ToString();
            tagDataEdit.Offset = (float)dr[COL_SENSOR_OFFSET];
            tagDataEdit.Limit = (float)dr[COL_SENSOR_LIMIT];
            UEventTagChanged?.Invoke(sender, tagDataEdit);
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

        private void textEdit_LHPath_Click(object sender, EventArgs e)
        {
            string path = OpenImage();
            if (path == "")
                return;

            textEdit_LHPath.EditValue = path;
            ConfigChangeEvent();
        }

        private void textEdit_RHPath_Click(object sender, EventArgs e)
        {
            string path = OpenImage();
            if (path == "")
                return;

            textEdit_RHPath.EditValue = path;
            ConfigChangeEvent();
        }

        private string OpenImage()
        {
            OpenFileDialog open = new OpenFileDialog();
            open.DefaultExt = ".jpg";
            open.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
            if (open.ShowDialog() != true) return "";
            return open.FileName;
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

