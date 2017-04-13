using DevExpress.XtraEditors;
using DevExpress.XtraBars.Docking2010.Views.WindowsUI;
using DevExpress.XtraBars.Docking2010.Views;
using System.Drawing;
using Dsu.PLC.Melsec;
using Dsu.Common.Utilities.ExtensionMethods;
using System.Linq;
using Dsu.PLC.Common;
using System;
using System.Reactive.Linq;
using System.Windows.Forms;
using System.Data;
using Dsu.Common.Utilities;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Threading;

namespace Dsa.Hmc.SensorDB
{
    public partial class MainForm : XtraForm
    {
        private TagDataSource _tagDataS;
        private ConfigData _configData;
        private LogPage _logPage;
        private PlcMX _PlcMX;

        private System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();

        public MainForm()
        {
            InitializeComponent();

            _PlcMX = new PlcMX();

            LoadConfig();
            CreateLayout();
        }

        private void LoadConfig()
        {
            FileStream fsData;
            FileStream fsConfig;
            BinaryFormatter bf = new BinaryFormatter();
            if (File.Exists("data.dat"))
            {
                fsData = new FileStream("data.dat", FileMode.Open);
                if (fsData.Length != 0)
                {
                    _tagDataS = bf.Deserialize(fsData) as TagDataSource;
                    fsData.Close();
                }
            }
            if (_tagDataS == null)
                _tagDataS = new TagDataSource();

            if (File.Exists("config.dat"))
            {
                fsConfig = new FileStream("config.dat", FileMode.Open);
                if (fsConfig.Length != 0)
                {
                    _configData = bf.Deserialize(fsConfig) as ConfigData;
                    fsConfig.Close();
                }
            }
            if (_configData == null)
                _configData = new ConfigData();
        }

        private void SaveConfig()
        {
            FileStream fsData = new FileStream("data.dat", FileMode.Create);
            FileStream fsConfig = new FileStream("config.dat", FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fsData, _tagDataS);
            fsData.Close();
            bf.Serialize(fsConfig, _configData);
            fsConfig.Close();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            windowsUIView.ContentContainerHeaderClick += WindowsUIView_ContentContainerHeaderClick;
            DateTime time = DateTime.Now;
            t.Interval = ((23 - time.Hour) * 3600 + (59 - time.Minute) * 60) * 1000;
            t.Tick += T_Tick;
            t.Start();
        }

        private void T_Tick(object sender, EventArgs e)
        {
            t.Interval = 24 * 3600 * 1000;
            _logPage.UserExportCount = 0;
            _logPage.ExportExcel(_configData.ExportPath);
            _logPage.ClearData();
        }

        private void WindowsUIView_ContentContainerHeaderClick(object sender, DocumentHeaderClickEventArgs e)
        {
            try
            {
                if (e.Document.Control is LogPage)
                {
                    _logPage.UserExportCount = _logPage.UserExportCount + 1;
                    _logPage.ExportExcel(_configData.ExportPath);
                }
                else if (e.Document.Control is ItemDetailPage)
                {
                    ItemDetailPage item = e.Document.Control as ItemDetailPage;
                    item.CreateChart(e.Document.Caption, _logPage.TableLog, _tagDataS);
                }
            }
            catch (System.Exception ex) { DEBUG.WriteLine("Exception : {0}\n{1}", ex.Message, ex.StackTrace); ex.Data.Clear(); }
        }

        private void tile_Click(object sender, TileClickEventArgs e)
        {
            PageGroup page = ((e.Tile as Tile).ActivationTarget as PageGroup);
            Document doc = null;
            if (page != null)
            {
                page.Parent = tileContainer;
                doc = (e.Tile as Tile).Document;
                if (doc == null)
                {
                    TagData t = (TagData)(sender as Tile).Frames[0].Tag;
                    foreach (Document d in windowsUIView.Documents)
                    {
                        if (d.Caption == t.LogName)
                            doc = d;
                    }
                }

                page.SetSelected(doc);
                ItemDetailPage item = doc.Control as ItemDetailPage;
                if (item != null)
                    item.CreateChart(doc.Caption, _logPage.TableLog, _tagDataS);
            }
        }

        private void CreateLayout()
        {
            PageGroup pageGroup = null;
            BaseDocument document = null;
            List<string> groups = new List<string>();
            _tagDataS.Items.ForEach(t => { if (!groups.Contains(t.Group)) groups.Add(t.Group); });

            tileContainer.Buttons["CAR TYPE"].Properties.Tag = _tagDataS.PartCarTypeA;
            tileContainer.Buttons["CAR TYPE"].Properties.Checked = _configData.DisplaySensor;
            tileContainer.Buttons["CAR TYPE"].Properties.Caption = _tagDataS.PartCarTypeA.CarType;
            _tagDataS.SelectCarType = _tagDataS.PartCarTypeA;

            foreach (string group in groups)
            {
                if (group == "") continue;

                pageGroup = new PageGroup();
                pageGroup.Parent = tileContainer;
                pageGroup.Caption = group;
                windowsUIView.ContentContainers.Add(pageGroup);

                ItemDetailPage itemDetailPage = new ItemDetailPage();
                itemDetailPage.Dock = System.Windows.Forms.DockStyle.Fill;
                document = windowsUIView.AddDocument(itemDetailPage);
                document.Caption = group;
                pageGroup.Items.Add(document as Document);
                CreateTileGroup(document as Document, group).ActivationTarget = pageGroup;

                foreach (TagData t in _tagDataS.Items)
                {
                    if (t.Group == group && t.IsSensor)
                    {
                        if (_tagDataS.PositionA == t.Position)
                        {
                            itemDetailPage = new ItemDetailPage();
                            itemDetailPage.Dock = DockStyle.Fill;
                            document = windowsUIView.AddDocument(itemDetailPage);
                            document.Caption = t.LogName;

                            pageGroup.Items.Add(document as Document);
                            CreateTile(document as Document, t, group).ActivationTarget = pageGroup;
                        }
                        else
                            CreateTile(null, t, group).ActivationTarget = pageGroup;

                    }
                }
            }

            CreateLayoutLogPage();
            CreateLayoutSettingPage();

            windowsUIView.ActivateContainer(tileContainer);
            tileContainer.ButtonClick += new DevExpress.XtraBars.Docking2010.ButtonEventHandler(buttonClick);
            tileContainer.ButtonChecked += TileContainer_ButtonChecked;
            tileContainer.ButtonUnchecked += TileContainer_ButtonChecked;
        }

        private void CreateLayoutLogPage()
        {
            PageGroup pageGroup = new PageGroup();
            pageGroup.Parent = tileContainer;
            pageGroup.Caption = "LOG";
            windowsUIView.ContentContainers.Add(pageGroup);
            _logPage = new LogPage(_tagDataS);
            _logPage.Dock = System.Windows.Forms.DockStyle.Fill;
            BaseDocument document = windowsUIView.AddDocument(_logPage);
            document.Caption = "LOG Export";
            tileContainer.Buttons["LOG"].Properties.Tag = pageGroup;
            pageGroup.Items.Add(document as Document);
        }

        private void CreateLayoutSettingPage()
        {
            PageGroup pageGroup = new PageGroup();
            pageGroup.Parent = tileContainer;
            pageGroup.Caption = "SETTING";
            windowsUIView.ContentContainers.Add(pageGroup);

            SettingPage settingPage = new SettingPage(_configData, _tagDataS);

            windowsUIView.Caption = _configData.Name;
            settingPage.UEventTagChanged += SettingPage_UEventTagChanged;
            settingPage.UEventMonitoringChanged += SettingPage_UEventMonitoringChanged;
            settingPage.UEventConfigChanged += SettingPage_UEventConfigChanged;
            settingPage.Dock = DockStyle.Fill;
            BaseDocument document = windowsUIView.AddDocument(settingPage);
            document.Caption = "SETTING";
            tileContainer.Buttons["SETTING"].Properties.Tag = pageGroup;
            pageGroup.Items.Add(document as Document);

        }

        private void SettingPage_UEventConfigChanged(object sender, ConfigData configData)
        {
            try
            {
                windowsUIView.Caption = configData.Name;
                _configData = configData;
                if (configData.LHPath != null)
                    windowsUIView.Tiles[_tagDataS.GroupA].BackgroundImage = Image.FromFile(@configData.LHPath);
                if (configData.RHPath != null)
                    windowsUIView.Tiles[_tagDataS.GroupB].BackgroundImage = Image.FromFile(@configData.RHPath);

            }
            catch (System.Exception ex) { DEBUG.WriteLine("Exception : {0}\n{1}", ex.Message, ex.StackTrace); ex.Data.Clear(); }
        }

        private void SettingPage_UEventTagChanged(object sender, TagData tagData)
        {
            try
            {
                _PlcMX.ReCollect(_tagDataS);
            }
            catch (System.Exception ex) { DEBUG.WriteLine("Exception : {0}\n{1}", ex.Message, ex.StackTrace); ex.Data.Clear(); }
        }

        private IDisposable _subscription;
        private async void SettingPage_UEventMonitoringChanged(object sender, ConfigData configData)
        {
            try
            {
                if (((ToggleSwitch)sender).IsOn)
                {
                    SaveConfig();

                    //start monitoring
                    MxConnection conn = _PlcMX.Connect(configData, _tagDataS);
                    _subscription = conn.Subject.OfType<TagValueChangedEvent>().Subscribe(evt => EventTag((MxTag)evt.Tag));
                    await conn.StartDataExchangeLoopAsync();
                    conn.Disconnect();
                    ((ToggleSwitch)sender).Enabled = true;
                }
                else  //stop monitoring
                {
                    _PlcMX.DisConnect(_subscription);
                    ((ToggleSwitch)sender).Enabled = false;
                }
            }
            catch (Exception ex) { DEBUG.WriteLine("Exception : {0}\n{1}", ex.Message, ex.StackTrace); ex.Data.Clear(); }
            finally
            {
                ((ToggleSwitch)sender).Enabled = true;
            }
        }

        private void EventTag(MxTag tagEvt)
        {
            IEnumerable<TagData> tagData = _tagDataS.Items.Where(t => t.Address.ToUpper() == tagEvt.Name.ToUpper());
            tagData.ForEach(t =>
            {
                this.Do(() =>
                {
                    if (t.IsSensor)
                        t.Value = GetResolution(Convert.ToInt32(tagEvt.Value));
                    else
                        t.Value = tagEvt.Value;

                    if (t.IsLogging && Convert.ToInt32(tagEvt.Value) != 0)
                    {
                        _logPage.AddLog(t.Position);
                        Console.WriteLine(tagEvt.Name + "  " + tagEvt.Value);

                        if (_tagDataS.PositionA == t.Position)
                        {
                            tileContainer.Buttons[3].Properties.Caption = _tagDataS.PartCarType;
                            UpdateSensorTile(true);
                        }
                        else if (_tagDataS.PositionB == t.Position)
                        {
                            tileContainer.Buttons[3].Properties.Caption = _tagDataS.AssyCarType;
                            UpdateSensorTile(false);
                        }
                    }
                });

            });
        }

        private float GetResolution(int v)
        {
            Random r = new Random();
        
            float Ratio = (v - _configData.MinIN) / (_configData.MaxIN - _configData.MinIN);
            float Out = Ratio * (_configData.MaxOut - _configData.MinOut);

            return Out + _configData.MinOut    + r.Next(-10, 10); 
        }

        private void action1_Update(object sender, EventArgs e)
        {
            if (!_PlcMX.IsConneted) return;
            tileContainer.Buttons["PLC ON"].Properties.Checked = Convert.ToInt32(_tagDataS.PLCOn.Value) != 0;
        }

        private Tile CreateTile(Document document, TagData tag, string group)
        {
            Tile tile = new Tile();
            tile.Document = document;
            tile.Group = group;
            tile.Name = tag.LogName;

            tile.Properties.ItemSize = TileItemSize.Medium;
            TileItemFrame frame = new TileItemFrame();
            frame.Tag = tag;
            CreateFrame(frame);

            tile.Frames.Add(frame);
            tile.Click += new TileClickEventHandler(tile_Click);
            windowsUIView.Tiles.Add(tile);
            tileContainer.Items.Add(tile);
            return tile;
        }

        private void CreateFrame(TileItemFrame frame)
        {
            frame.Elements.Clear();
            if (_tagDataS.PositionA == ((TagData)frame.Tag).Position)
                frame.Elements.Add(CreateTileItemElement(((TagData)frame.Tag).Name, TileItemContentAlignment.TopCenter, Point.Empty, 18, Color.DeepSkyBlue));
            else
                frame.Elements.Add(CreateTileItemElement(((TagData)frame.Tag).Name, TileItemContentAlignment.TopCenter, Point.Empty, 18, Color.DarkOliveGreen));

            if (_configData.DisplaySensor)
            {
                frame.Elements.Add(CreateTileItemElement(string.Format("센서"), TileItemContentAlignment.Manual, new Point(0, 40), 15, Color.BlanchedAlmond));
                frame.Elements.Add(CreateTileItemElement(string.Format("기준"), TileItemContentAlignment.Manual, new Point(0, 80), 15, Color.BlanchedAlmond));
                frame.Elements.Add(CreateTileItemElement(string.Format("측정"), TileItemContentAlignment.Manual, new Point(0, 120), 15, Color.BlanchedAlmond));
                frame.Elements.Add(new TileItemElement());
                frame.Elements.Add(new TileItemElement());
                frame.Elements.Add(new TileItemElement());
            }
            else
            {
                frame.Elements.Add(CreateTileItemElement(string.Format("측정"), TileItemContentAlignment.Manual, new Point(0, 70), 15, Color.BlanchedAlmond));
                frame.Elements.Add(new TileItemElement());
            }
        }

        private void UpdateSensorTile(bool bNew)
        {
            foreach (Tile tile in windowsUIView.Tiles)
            {
                if (tile.Frames.Count == 0)
                    continue;

                TileItemFrame frame = tile.Frames[0];
                TagData t = frame.Tag as TagData;
                if (!bNew && _tagDataS.PositionA == t.Position)
                    continue;

                float sensorValue = (float)(t.Value);
                CreateFrame(frame);
                if (_configData.DisplaySensor)
                {
                    frame.Elements.RemoveTail();
                    frame.Elements.RemoveTail();
                    frame.Elements.RemoveTail();
                    if ((bNew && _tagDataS.PositionA == t.Position) || (!bNew && _tagDataS.PositionB == t.Position))
                    {
                        frame.Elements.Add(CreateTileItemElement(sensorValue.ToString("0.0"), TileItemContentAlignment.Manual, new Point(75, 40), 16, Color.GhostWhite));
                        frame.Elements.Add(CreateTileItemElement(t.Offset.ToString("0.0"), TileItemContentAlignment.Manual, new Point(75, 80), 16, Color.GhostWhite));
                        frame.Elements.Add(CreateTileItemElement((sensorValue - t.Offset).ToString("0.0"), TileItemContentAlignment.Manual, new Point(75, 120), 16, Color.GhostWhite));
                    }
                    else
                    {
                        frame.Elements.Add(new TileItemElement());
                        frame.Elements.Add(new TileItemElement());
                        frame.Elements.Add(new TileItemElement());
                    }
                }
                else
                {
                    frame.Elements.RemoveTail();
                    if ((bNew && _tagDataS.PositionA == t.Position) || (!bNew && _tagDataS.PositionB == t.Position))
                        frame.Elements.Add(CreateTileItemElement((sensorValue - t.Offset).ToString("0.0"), TileItemContentAlignment.Manual, new Point(75, 70), 18, Color.GhostWhite));
                    else
                        frame.Elements.Add(new TileItemElement());
                }
            }
        }

        private Tile CreateTileGroup(Document document, string group)
        {
            Tile tile = new Tile();
            tile.Document = document;
            tile.Group = group;
            tile.Name = group;

            tile.Properties.ItemSize = TileItemSize.Large;
            tile.BackgroundImage = (Image)Properties.Resources.ResourceManager.GetObject(group);
            tile.Properties.BackgroundImageScaleMode = TileItemImageScaleMode.Stretch;
            tile.Appearances.Selected.BackColor = tile.Appearances.Hovered.BackColor = tile.Appearances.Normal.BackColor = Color.FromArgb(88, 88, 88);
            tile.Appearances.Selected.BorderColor = tile.Appearances.Hovered.BorderColor = tile.Appearances.Normal.BorderColor = Color.FromArgb(88, 88, 88);


            tile.Click += new TileClickEventHandler(tile_Click);
            windowsUIView.Tiles.Add(tile);
            tileContainer.Items.Add(tile);
            return tile;
        }

        private TileItemElement CreateTileItemElement(string text, TileItemContentAlignment alignment, Point location, float fontSize, Color color)
        {
            TileItemElement element = new TileItemElement();
            element.TextAlignment = alignment;
            if (!location.IsEmpty) element.TextLocation = location;
            element.Appearance.Normal.Options.UseFont = true;
            element.Appearance.Normal.Font = new System.Drawing.Font(new FontFamily("현대하모니 M"), fontSize, FontStyle.Regular);
            element.Appearance.Normal.ForeColor = color;
            element.Text = text;
            return element;
        }

        private void buttonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            PageGroup pageGroup = (e.Button.Properties.Tag as PageGroup);
            if (pageGroup != null)
                windowsUIView.ActivateContainer(pageGroup);
        }

        private void TileContainer_ButtonChecked(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            if (e.Button.Properties.Tag is TagData)
            {
                _configData.DisplaySensor = e.Button.Properties.Checked;
                foreach (Tile tile in windowsUIView.Tiles)
                {
                    if (tile.Frames.Count == 0)
                        continue;

                    tile.BeginUpdate();
                    CreateFrame(tile.Frames[0]);
                    tile.EndUpdate();
                }
            }
        }

        private DialogResult PreClosingConfirmation()
        {
            DialogResult dialogResult;
            dialogResult = XtraMessageBox.Show(this, "Exit Application?", "Sensor Monitoring", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return dialogResult;
        }

        private void MainForm_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {

            if (PreClosingConfirmation() == DialogResult.Yes)
            {
                SaveConfig();
                //_PlcMX?.DisConnect(_subscription);
                //t.Stop();

                Thread.Sleep(1000);
                Dispose(true);
                Application.Exit();
            }
            else
                e.Cancel = true;
        }
    }
}
