using System;
using DevExpress.LookAndFeel;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Docking2010.Views;
using DevExpress.XtraBars.Helpers;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraNavBar;
using Dsa.Kefico.MWS.Enumeration;

using MySql.Data.MySqlClient;
using DevExpress.XtraBars.Docking2010.Views.Tabbed;
using System.Windows.Forms;
using System.Threading;
using System.Reflection;

namespace Dsa.Kefico.MWS
{
    public partial class frmMain : RibbonForm
    {
        private MySqlMWSClient mySqlMWS = new MySqlMWSClient();
        private DocumentMWS docMWS = new DocumentMWS();
        private ViewQuery viewQuery = new ViewQuery();
        private FilterMWS filterMWS = new FilterMWS();
        private ChartSPC chartSPC = new ChartSPC();

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
            bool Connected = false;
            for (int i = 0; i < 3; i++)
            {
                if (mySqlMWS.Open())
                {
                    Connected = true;
                    break;
                }
                Thread.Sleep(1000);
            }

            filterMWS.SetFilterSummary(TimeSummaryStart, TimeSummaryEnd, LineGroup);
            BaseDocument document = null;
            if (docMWS.GetDocument(docMWS.GetKey(EnumTable.showTotalSummaryView), out document))
            {
                FrmGrid frmGrid = document.Form as FrmGrid;
                if (frmGrid.DataSource.Rows.Count > 0)
                    UdateRowSelect(EnumTable.showTotalSummaryView, SchemaMWS.TSV_id, frmGrid.DataSource.Rows[0]);
            }

            if (Connected)
                OpenDocument(EnumTable.showTotalSummaryView);

            ConnectionUI(Connected);
        }

        private void InitManuTab()
        {
            ribbonPage1.Text = RibbonMWS.GetName(EnumRibbon.Overview);
            ribbonPage2.Text = RibbonMWS.GetName(EnumRibbon.History);
            ribbonPage3.Text = RibbonMWS.GetName(EnumRibbon.Analysis);
        }

        private void InitSkins()
        {
            SkinHelper.InitSkinGallery(skinGalleryBarItem, true);
            UserLookAndFeel.Default.SetSkinStyle("Office 2013 Dark Gray");
        }

        private void ConnectionUI(bool bConnected)
        {
            if (bConnected)
            {
                this.barButtonItem_Online.Caption = "Online";
                this.barButtonItem_Online.Glyph = global::Dsa.Kefico.MWS.Properties.Resources.show_32x32;
            }
            else
            {
                this.barButtonItem_Online.Caption = "Offline";
                this.barButtonItem_Online.Glyph = global::Dsa.Kefico.MWS.Properties.Resources.hide_16x16;
            }
        }



        private void InitNavBarGroup()
        {
            gridGroup.Caption = "TEST1";
            gridGroup.Expanded = true;
            gridGroup.GroupCaptionUseImage = DevExpress.XtraNavBar.NavBarImage.Large;
            gridGroup.LargeImage = global::Dsa.Kefico.MWS.Properties.Resources.cards_32x32;
            gridGroup.Name = "TEST1";
            gridGroup.SelectedLinkIndex = 0;
            gridGroup.SmallImage = global::Dsa.Kefico.MWS.Properties.Resources.cards_16x16;
            chartGroup.Caption = "TEST2";
            chartGroup.GroupCaptionUseImage = DevExpress.XtraNavBar.NavBarImage.Large;
            chartGroup.LargeImage = global::Dsa.Kefico.MWS.Properties.Resources.chart_32x32;
            chartGroup.Name = "TEST2";
            chartGroup.SmallImage = global::Dsa.Kefico.MWS.Properties.Resources.chart_16x16;
            othersGroup.Caption = "TEST3";
            othersGroup.GroupCaptionUseImage = DevExpress.XtraNavBar.NavBarImage.Large;
            othersGroup.LargeImage = global::Dsa.Kefico.MWS.Properties.Resources.converttoparagraphs_32x32;
            othersGroup.Name = "TEST3";
            othersGroup.SmallImage = global::Dsa.Kefico.MWS.Properties.Resources.converttoparagraphs_16x16;
        }

        private NavBarItem CreateItem(EnumTable value)
        {
            NavBarItem item = new NavBarItem();
            item.Tag = value;
            item.LinkClicked += new NavBarLinkEventHandler(OnLinkClicked);
            return item;
        }


        private void mySqlMWS_UEventSqlException(MySqlException err)
        {
            memoEdit1.Text = memoEdit1.Text + string.Format("{0}: {1}{2}", DateTime.Now, err.Message, Environment.NewLine) ;
            barStaticItem_StatusText.Caption = EnumEventResult.SqlError.ToString();
            memoEdit1.SelectionStart = memoEdit1.Text.Length -1;
            memoEdit1.ScrollToCaret();
        }
        private void WriteOutputException(Exception ex, string functionName)
        {
            memoEdit1.Text = memoEdit1.Text + $"{DateTime.Now}: [{functionName}] {ex.Message}{Environment.NewLine}";
            barStaticItem_StatusText.Caption = EnumEventResult.Error.ToString();
            memoEdit1.SelectionStart = memoEdit1.Text.Length -1;
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
            SetFilterEnableControl(e.Document.Caption, false);
            if (e.Document.Container == null)
            {
                if (e.Document.Caption == docMWS.GetKey(EnumTable.showPHV))
                    tabbedView.Controller.CreateNewDocumentGroup(e.Document as Document, Orientation.Vertical);
                else if (e.Document.Caption == docMWS.GetKey(EnumTable.showBundle))
                    tabbedView.Controller.MoveToDocumentGroup(e.Document as Document, false);
            }
        }

        private void tabbedView_DocumentRemoved(object sender, DocumentEventArgs e)
        {
            docMWS.RemoveDocument(e.Document.Caption);
            SetFilterEnableControl(e.Document.Caption, true);
        }

        private void tabbedView_QueryControl(object sender, QueryControlEventArgs e)
        {
            if (e.Control == null)  //Devexpress MDI Control
                e.Control = new System.Windows.Forms.Control();
        }
        private void tabbedView_DocumentActivated(object sender, DocumentEventArgs e)
        {
            FrmGrid frmGrid = e.Document.Form as FrmGrid;
            if (frmGrid == null)
                return;

            if (frmGrid.ViewTable == EnumTable.showTotalSummaryView)
                mainRibbon.SelectedPage = ribbonPage1;
            else if (frmGrid.ViewTable == EnumTable.showSPSV
                || frmGrid.ViewTable == EnumTable.showSTSV
                || frmGrid.ViewTable == EnumTable.showPHVChart
                || frmGrid.ViewTable == EnumTable.showPHV
                || frmGrid.ViewTable == EnumTable.showBundle)
                mainRibbon.SelectedPage = ribbonPage2;
            else
                mainRibbon.SelectedPage = ribbonPage3;
        }

     
    }
}