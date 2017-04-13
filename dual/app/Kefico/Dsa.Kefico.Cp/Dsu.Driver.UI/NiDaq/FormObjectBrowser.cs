using System.Windows.Forms;
using Dsu.Common.Utilities;

namespace Dsu.Driver.UI.NiDaq
{
    public partial class FormObjectBrowser : Form
    {
        public FormObjectBrowser()
        {
            InitializeComponent();
        }

        public void Initialize(object obj)
        {
            var props = FsReflection.GetPropertyInfoPair(obj);
            gridControl1.DataSource = props;
        }
    }
}
