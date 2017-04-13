using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;

namespace Dsu.Common.Utilities.DX
{
    /// <summary>
    /// 범용 DevXpress grid view
    /// </summary>
    public partial class DXFormGridView : Form
    {
        public GridView GridView { get { return gridView1; } }
        public object DataSource { get { return gridControl1.DataSource; } }
        public DXFormGridView(object dataSource)
        {
            InitializeComponent();
            gridControl1.DataSource = dataSource;
        }
    }
}
