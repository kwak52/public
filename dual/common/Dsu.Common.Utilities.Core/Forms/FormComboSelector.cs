using System;
using System.Windows.Forms;

namespace Dsu.Common.Utilities
{
    public partial class FormComboSelector : FormOKCancel
    {
        protected object[] m_aobj = null;
        protected string m_strTitle = string.Empty;
        protected object m_ItemSelected = null;

        static public void ShowExample()
        {
            FormComboSelector frmcs = new FormComboSelector(typeof(DialogResult), "Select Dialog Result");
            object oSelected = frmcs.DoModalGetResult();
            if (oSelected != null)
            {
                DialogResult e = Tools.ParseEnum<DialogResult>(oSelected.ToString());
                DEBUG.WriteLine("Your selected {0}", e);
            }
        }

        public FormComboSelector(Type t)
            : this(Tools.ToObjects(Enum.GetValues(t)))
        { }

        public FormComboSelector(Type t, string strTitle)
            : this(Tools.ToObjects(Enum.GetValues(t)), strTitle)
        {}

        public FormComboSelector(object[] objs)
            : this(objs, "Select item")
        { }

        public FormComboSelector(object[] objs, string strTitle)
        {
            m_aobj = objs;
            m_strTitle = strTitle;

            InitializeComponent();
            InitializeFormOKCancel(btnOK, btnCancel);
        }

        private void FormSelector_Load(object sender, EventArgs e)
        {
            combo.Items.AddRange(m_aobj);
            combo.SelectedIndex = 0;
            Text = m_strTitle;
            combo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
        }

        public object DoModalGetResult()
        {
            return (ShowDialog() == DialogResult.OK) ? combo.SelectedItem : null;
        }
    }
}
