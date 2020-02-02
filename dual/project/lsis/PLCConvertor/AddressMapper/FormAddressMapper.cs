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
    public partial class FormAddressMapper
        : DevExpress.XtraBars.Ribbon.RibbonForm
        , IAppender
    {
        public FormAddressMapper()
        {
            InitializeComponent();
        }

        private void FormAddressMapper_Load(object sender, EventArgs e)
        {
            Logger.Info("FormAddressMapper launched.");
        }
    }
}