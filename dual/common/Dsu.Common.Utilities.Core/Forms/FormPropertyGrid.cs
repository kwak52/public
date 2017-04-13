using System;
using System.Diagnostics.Contracts;
using System.Windows.Forms;

namespace Dsu.Common.Utilities
{
    public partial class FormPropertyGrid : Form
    {
        public System.Windows.Forms.PropertyGrid PropertyGrid { get { return propertyGrid; } }

        public FormPropertyGrid()
        {
            InitializeComponent();
        }

        public FormPropertyGrid(object o, string strTitle)
        {
            Contract.Requires(o != null && ! String.IsNullOrEmpty(strTitle));
            InitializeComponent();

            propertyGrid.SelectedObject = o;
            Text = strTitle;
        }

        private void FormPropertyGrid_Load(object sender, EventArgs e)
        {
            Contract.Requires(propertyGrid.SelectedObject != null);

            //Array arr = propertyGrid.SelectedObject as Array;
            //if (arr != null)
            //{
            //    if (arr.Length == 1)
            //        propertyGrid.SelectedObject = arr.GetValue(0);
            //    else if (arr.Length > 1)
            //        propertyGrid.SelectedObjects = SelectedObject as object[];
            //}
        }
    }
}
