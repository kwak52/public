using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Dsu.Common.Utilities
{
    public partial class FormListboxSelector : FormOKCancel
    {
        public bool IsAllowMultiSelection { get; set; } = true;
        public string Title { get; set; }
        public bool ShowOKButton { get; set; } = true;
        public bool ShowCancelButton { get; set; } = true;
        public int ListBoxFontSize { get; set; } = 9;
        public ListBox ListBox => listBox;

        protected object[] m_aobj = null;
        protected object m_ItemSelected = null;

        static public void ShowExample()
        {
            FormListboxSelector frm = new FormListboxSelector(typeof(DialogResult), "Select Dialog Result");
            List<object> lstSelected = frm.DoModalGetResults();
            if (lstSelected != null && lstSelected.Count > 0)
            {
                foreach (object o in lstSelected)
                {
                    DialogResult e = Tools.ParseEnum<DialogResult>(o.ToString());
                    DEBUG.WriteLine("Your selected {0}", e);
                }
            }
        }

        public FormListboxSelector(Type t)
            : this(Tools.ToObjects(Enum.GetValues(t)))
        { }

        public FormListboxSelector(Type t, string strTitle)
            : this(Tools.ToObjects(Enum.GetValues(t)), strTitle)
        {}


        public FormListboxSelector(object[] objs)
            : this(objs, true, "Select item")
        { }

        public FormListboxSelector(object[] objs, string strTitle)
            : this(objs, true, strTitle)
        { }

        public FormListboxSelector(object[] objs, bool bMultiSelection, string strTitle)
        {
            m_aobj = objs;
            Title = strTitle;

            InitializeComponent();
        }

        private void FormListboxSelector_Load(object sender, EventArgs e)
        {
            InitializeFormOKCancel(ShowOKButton ? btnOK : null, ShowCancelButton ? btnCancel : null);
            btnOK.Visible = ShowOKButton;
            btnCancel.Visible = ShowCancelButton;

            if (!ShowOKButton && !ShowCancelButton)
                listBox.Dock = DockStyle.Fill;
            else
                listBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;

            var fontFamily = listBox.Font.FontFamily;
            var font = new Font(fontFamily, ListBoxFontSize, FontStyle.Regular, GraphicsUnit.Pixel);
            listBox.Font = font;

            listBox.Items.AddRange(m_aobj);
            listBox.SelectionMode = IsAllowMultiSelection ? SelectionMode.MultiExtended : SelectionMode.One;
            //combo.SelectedIndex = 0;
            Text = Title;
        }

        public object DoModalGetResult()
        {
            return DoModalGetResults().FirstOrDefault();
        }

        public List<object> DoModalGetResults()
        {
            if ( (ShowDialog() != DialogResult.OK) )
                return null;

            List<object> lst = new List<object>();
            foreach (object o in listBox.SelectedItems)
                lst.Add(o);
            return lst;
        }

        private void checkedListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            DEBUG.Write("");
        }

    }
}
