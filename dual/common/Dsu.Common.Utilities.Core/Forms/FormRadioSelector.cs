using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Dsu.Common.Utilities
{
    public partial class FormRadioSelector : FormOKCancel
    {
        protected object[] m_aobj = null;
        protected string m_strTitle = string.Empty;
        protected object m_ItemSelected = null;

        private const int m_nRadioX = 10;
        private const int m_nFirstRadioY = 10;
        private int m_nLastRadioY = -1;
        private const int m_nRadioOffsetY = 20;
        private List<KeyValuePair<RadioButton, object>> m_RadioButtons = new List<KeyValuePair<RadioButton, object>>();
        private int m_nInitialSelection = 0;

        static public void ShowExample()
        {
            FormRadioSelector frmcs = new FormRadioSelector(typeof(DialogResult), "Select Dialog Result");
            object oSelected = frmcs.DoModalGetResult();
            if (oSelected != null)
            {
                DialogResult e = Tools.ParseEnum<DialogResult>(oSelected.ToString());
                DEBUG.WriteLine("Your selected {0}", e);
            }
        }

        public FormRadioSelector(Type t)
            : this(Tools.ToObjects(Enum.GetValues(t)))
        { }

        public FormRadioSelector(Type t, string strTitle)
            : this(Tools.ToObjects(Enum.GetValues(t)), strTitle, 0)
        {}

        public FormRadioSelector(object[] objs)
            : this(objs, "Select item", 0)
        { }

        public FormRadioSelector(object[] objs, string strTitle, int nInitialSelection/*=0*/)
        {
            m_aobj = objs;
            m_strTitle = strTitle;
            m_nInitialSelection = nInitialSelection;

            InitializeComponent();
            InitializeFormOKCancel(btnOK, btnCancel);
        }

        private void FormRadioSelector_Load(object sender, EventArgs e)
        {

            Text = m_strTitle;
            //combo.Items.AddRange(m_aobj);
            //combo.SelectedIndex = 0;
            //combo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;

            m_nLastRadioY = m_nFirstRadioY;
            int nMaxRadioStringLength = 0;
            foreach (object obj in m_aobj)
            {
                string str = obj.ToString();
                nMaxRadioStringLength = (int)Tools.max(nMaxRadioStringLength, str.Length);
                RadioButton btn = new RadioButton();
                btn.AutoSize = true;
                btn.Location = new Point(m_nRadioX, m_nLastRadioY);
                m_nLastRadioY += m_nRadioOffsetY;
                btn.Name = str;
                btn.Size = new Size(388, 16);
                btn.TabIndex = 10;
                btn.TabStop = true;
                btn.Text = str;
                btn.UseVisualStyleBackColor = true;
                panel.Controls.Add(btn);
                if (m_RadioButtons.Count == m_nInitialSelection)
                    btn.Checked = true;
                m_RadioButtons.Add(new KeyValuePair<RadioButton, object>(btn, obj));
            }
        }

        public void RemoveRadioButton(object obj)
        {
            m_aobj = m_aobj.SkipWhile(ele => ele.Equals(obj)).ToArray();
        }

        public void SelectRadioButton(object obj)
        {
            m_nInitialSelection = Array.FindIndex(m_aobj, ele => ele.Equals(obj));
        }

        public object DoModalGetResult()
        {
            if (ShowDialog() == DialogResult.OK)
            {
                foreach (var pr in m_RadioButtons)
                {
                    RadioButton btn = pr.Key;
                    if (btn.Checked)
                        return pr.Value;
                }
            }

            return null;
        }
    }
}
