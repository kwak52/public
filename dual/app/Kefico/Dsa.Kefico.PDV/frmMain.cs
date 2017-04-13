using System;
using DevExpress.LookAndFeel;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Docking2010.Views;
using DevExpress.XtraBars.Helpers;
using DevExpress.XtraBars.Ribbon;
using Dsa.Kefico.PDV.Enumeration;

using MySql.Data.MySqlClient;
using DevExpress.XtraBars.Docking2010.Views.Tabbed;
using System.Windows.Forms;
using Dsa.Kefico.PDV.Forms;
using System.Threading;
using System.Reflection;
using System.Configuration;

namespace Dsa.Kefico.PDV
{
    public partial class frmMain : RibbonForm
    {
        private MySqlMWSClient mySqlMWS = new MySqlMWSClient();
        private DocumentMWS docMWS = new DocumentMWS();
        private ViewQuery viewQuery = new ViewQuery();
        private FilterPDV filterMWS = new FilterPDV();
        private FrmEntry frmEntry;
        private FrmRelease frmRelease;
        private FrmTestList frmTestList;
        private PdvManagerModule.PdvManager _pdvManager = null;
        private int UserId = 3;  //test ahn default -1
        private bool Connected = false;

        public frmMain()
        {
            InitializeComponent();
            InitSkins();
            InitControl();
            InitManuTab();

            tabbedView.DocumentAdded += tabbedView_DocumentAdded;
            tabbedView.DocumentRemoved += tabbedView_DocumentRemoved;
            tabbedView.QueryControl += tabbedView_QueryControl;
            tabbedView.DocumentActivated += tabbedView_DocumentActivated;

            mySqlMWS.UEventSqlReadRecordCount += mySqlMWS_UEventSqlReadRecordCount;
            mySqlMWS.UEventSqlReadRecordPosition += mySqlMWS_UEventSqlReadRecordPosition;
            mySqlMWS.UEventSqlException += mySqlMWS_UEventSqlException;
            barStaticItem_Ver.Caption = string.Format("v{0} ({1})", Assembly.GetExecutingAssembly().FullName.Split(',')[1].Trim().Split('=')[1]
                , System.IO.File.GetLastWriteTime(Assembly.GetExecutingAssembly().Location).ToShortDateString());
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            try
            {
                MwsConfig.mwsServer = ConfigurationManager.AppSettings["mwsServer"];  //"112.170.136.46";
                _pdvManager = new PdvManagerModule.PdvManager(new PdvManagerModule.PdvClientConfig());
            }
            catch (System.Exception ex)
            {
                WriteOutput("Initial Connect", ex.Message);
                WriteOutput("Service", "MWS Service connection is failed");
                ex.Data.Clear();
            }

            try
            {
                for (int i = 0; i < 3; i++)
                {
                    Connected = mySqlMWS.Open();
                    if (Connected)
                    {
                        OpenDocument(ViewPDV.selectPdvGroup);
                        OpenDocument(ViewPDV.selectPdvTestList);
                        OpenDocument(ViewPDV.selectPdvView);
                        break;
                    }
                    Thread.Sleep(1000);
                }
            }
            catch (System.Exception ex)
            {
                WriteOutput("Initial Connect", ex.Message);
                ex.Data.Clear();
                Connected = mySqlMWS.Open();
            }
            finally
            {
                ConnectionUI(Connected);
            }
        }

        private void InitManuTab()
        {
            ribbonPage1.Text = RibbonMWS.GetName(EnumRibbon.Overview);
        }

        private void InitSkins()
        {
            SkinHelper.InitSkinGallery(skinGalleryBarItem, true);
            UserLookAndFeel.Default.SetSkinStyle("Office 2016 Colorful");
        }

        private void ConnectionUI(bool bConnected)
        {
            if (bConnected)
            {
                this.barButtonItem_Online.Caption = "Online (2)";  //ahn test 실제 연결된 Server 1 or Server 2  정보 표현
                this.barButtonItem_Online.Glyph = global::Dsa.Kefico.PDV.Properties.Resources.show_32x32;
            }
            else
            {
                this.barButtonItem_Online.Caption = "Offline";
                this.barButtonItem_Online.Glyph = global::Dsa.Kefico.PDV.Properties.Resources.hide_16x16;
            }
        }

        private void mySqlMWS_UEventSqlException(MySqlException err)
        {
            memoEdit1.Text = memoEdit1.Text + string.Format("{0}: {1}{2}", DateTime.Now, err.Message, Environment.NewLine);
            barStaticItem_StatusText.Caption = EnumEventResult.SqlError.ToString();
            memoEdit1.SelectionStart = memoEdit1.Text.Length - 1;
            memoEdit1.ScrollToCaret();
            err.Data.Clear();
        }
        private void WriteOutputException(Exception ex, string functionName)
        {
            memoEdit1.Text = memoEdit1.Text + string.Format("{0}: [{1}] {2}{3}", DateTime.Now, functionName, ex.Message, Environment.NewLine);
            barStaticItem_StatusText.Caption = EnumEventResult.Error.ToString();
            memoEdit1.SelectionStart = memoEdit1.Text.Length - 1;
            memoEdit1.ScrollToCaret();
            ex.Data.Clear();
        }
        private void WriteOutput(string functionName, string message)
        {
            memoEdit1.Text = memoEdit1.Text + string.Format("{0}: [{1}] {2}{3}", DateTime.Now, functionName, message, Environment.NewLine);
            barStaticItem_StatusText.Caption = EnumEventResult.Error.ToString();
            memoEdit1.SelectionStart = memoEdit1.Text.Length - 1;
            memoEdit1.ScrollToCaret();
        }

        private void mySqlMWS_UEventSqlReadRecordPosition(object sender, int Position)
        {
        }

        private void mySqlMWS_UEventSqlReadRecordCount(object sender, int RowCount)
        {
            DisplayRecordCount(RowCount);
        }

        private void tabbedView_DocumentAdded(object sender, DocumentEventArgs e)
        {
            docMWS.AddDocument(e.Document.Caption, e.Document);
            if (e.Document.Container == null)
            {
//                 if (e.Document.Caption == ViewPDV.selectPdv)
//                     tabbedView.Controller.CreateNewDocumentGroup(e.Document as Document, Orientation.Vertical);
            }
        }

        private void tabbedView_DocumentRemoved(object sender, DocumentEventArgs e)
        {
            docMWS.RemoveDocument(e.Document.Caption);
        }

        private void tabbedView_QueryControl(object sender, QueryControlEventArgs e)
        {
            if (e.Control == null)  //Devexpress MDI Control
                e.Control = new System.Windows.Forms.Control();
        }

        private void tabbedView_DocumentActivated(object sender, DocumentEventArgs e)
        {

        }

        private void action1_Update(object sender, EventArgs e)
        {
            DisableBotton();

            barButtonItem_Summary_Apply.Enabled = Connected;

            BaseDocument document = null;
            if (docMWS.GetDocument(ViewPDV.selectPdvGroup, out document))
            {
                if (tabbedView.ActiveDocument == document)
                {
                    if (frmEntry == null || frmEntry.IsDisposed)
                    {
                        barButtonItem_NewEntry.Enabled = true;
                        barButtonItem_EditEntry.Enabled = true;
                        barButtonItem_SelectTestList.Enabled = true;
                        barButtonItem_SelectRelease.Enabled = true;
                    }
                }
            }
            if (docMWS.GetDocument(ViewPDV.selectPdvTestList, out document))
            {
                if (tabbedView.ActiveDocument == document)
                {
                    if (frmTestList == null || frmTestList.IsDisposed)
                    {
                        barButtonItem_NewTestList.Enabled = true;
                        barButtonItem_EditTestList.Enabled = true;
                        barButtonItem_SelectEntry.Enabled = true;
                        barButtonItem_SelectRelease.Enabled = true;
                    }
                }
            }
            if (docMWS.GetDocument(ViewPDV.selectPdvView, out document))
            {
                if (tabbedView.ActiveDocument == document)
                {
                    if (frmRelease == null || frmRelease.IsDisposed)
                    {
                        barButtonItem_NewRelease.Enabled = true;
                        barButtonItem_EditRelease.Enabled = true;
                        barButtonItem_SelectEntry.Enabled = true;
                        barButtonItem_SelectTestList.Enabled = true;
                    }
                }
            }
        }

        private void DisableBotton()
        {
            barButtonItem_NewEntry.Enabled = false;
            barButtonItem_EditEntry.Enabled = false;
            barButtonItem_NewTestList.Enabled = false;
            barButtonItem_EditTestList.Enabled = false;
            barButtonItem_NewRelease.Enabled = false;
            barButtonItem_EditRelease.Enabled = false;
            barButtonItem_Summary_Apply.Enabled = false;

            barButtonItem_SelectEntry.Enabled = false;
            barButtonItem_SelectTestList.Enabled = false;
            barButtonItem_SelectRelease.Enabled = false;

        }

        private void barButtonItem_Summary_Apply_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenDocument(ViewPDV.selectPdvGroup);
            OpenDocument(ViewPDV.selectPdvTestList);
            OpenDocument(ViewPDV.selectPdvView);
        }

        private void barEditItem_QuickView_EditValueChanged(object sender, EventArgs e)
        {
            FilterOn(ViewPDV.selectPdvGroup);
            FilterOn(ViewPDV.selectPdvTestList);
            FilterOn(ViewPDV.selectPdvView);
        }

        private void FilterOn(string doc)
        {
            BaseDocument document = null;
            if (docMWS.GetDocument(doc, out document))
            {
                FrmGrid frmGrid = document.Form as FrmGrid;
                frmGrid.SetFilter((bool)barEditItem_QuickView.EditValue);
            }
        }

       
    }
}