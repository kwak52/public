using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Reflection;

namespace Dsu.Common.Utilities
{
    public partial class FormContainer : Form
    {
        /*
         * Inner control[m_ControlInner] 을 포함하는 form 을 하나 생성하고, 
         * parent 가 존재한다면 그 form 을 parent 에 포함시킨다.   parent 가 null 이면 top-level 로 생성한다.
         */

        /*
         * Mode 1
         */
        private CMVIFormManager m_MVIFormManager = null;
        private Control m_ControlInner = null;
        private Control m_Parent = null;
        private bool m_bDestructInnerWhenClosing = true;
        public Control m_BackupInnerControlParent = null;
        public DockStyle m_BackupInnerControlDockStyle = DockStyle.None;
        public object m_BackupInnerControlBorderStyle = null;

        public bool DestructInnerWhenClosing { get { return m_bDestructInnerWhenClosing; } set { m_bDestructInnerWhenClosing = value; } }
        public CMVIFormManager FormManager { get { return m_MVIFormManager; } set { m_MVIFormManager = value; } }
        public Control Inner { get { return m_ControlInner; } set { m_ControlInner = value; } }
        private const BindingFlags InvokeFlagPropertyGet = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty;
        private const BindingFlags InvokeFlagPropertySet = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty;

        public FormContainer(Control inner) : this(inner, null, true) { }
        public FormContainer(Control inner, Control parent/*=null*/, bool bDestructInnerWhenClosing/*=true*/)
        {
            m_ControlInner = inner;
            m_Parent = parent;
            m_bDestructInnerWhenClosing = bDestructInnerWhenClosing;

            m_BackupInnerControlParent = m_ControlInner.Parent;
            m_BackupInnerControlDockStyle = m_ControlInner.Dock;

            InitializeComponent();
        }

        /*
         * Mode 2
         */
        public delegate void DelegatorSnatch(FormContainer me, Control inner, Control parent);
        public delegate bool DelegatorRelease(FormContainer me, Control inner, Control parent);
        private DelegatorSnatch OnSnatch = null;
        public DelegatorRelease OnRelease = null;
        public Panel Panel { get { return panelContainer; } }
        public FormContainer(Control inner, Control parent, DelegatorSnatch onSnatch, DelegatorRelease onRelease)
        {
            m_ControlInner = inner;
            m_Parent = parent;
            OnSnatch = onSnatch;
            OnRelease = onRelease;

            InitializeComponent();
        }


        private void FormContainer_Load(object sender, EventArgs e)
        {
            if (OnSnatch != null)
            {
                OnSnatch(this, m_ControlInner, m_Parent);
                return;
            }

            panelContainer.Controls.Add(m_ControlInner);

            try
            {
                Type innerControl_t = m_ControlInner.GetType();
                if (RTTI.HasProperty_p(innerControl_t, "TopLevel"))
                {
                    //m_ControlInner.TopLevel = false;
                    innerControl_t.InvokeMember("TopLevel", BindingFlags.Public | BindingFlags.NonPublic, null, m_ControlInner, new object[1] { false });
                }

                if (RTTI.HasProperty_p(innerControl_t, "FormBorderStyle"))
                {
                    //m_BackupInnerControlBorderStyle = m_ControlInner.FormBorderStyle
                    m_BackupInnerControlBorderStyle = innerControl_t.InvokeMember("FormBorderStyle", InvokeFlagPropertyGet, null, m_ControlInner, null);

                    //m_ControlInner.FormBorderStyle = FormBorderStyle.None;
                    innerControl_t.InvokeMember("FormBorderStyle", InvokeFlagPropertySet, null, m_ControlInner, new object[1] { FormBorderStyle.None });
                }
                else if (RTTI.HasProperty_p(innerControl_t, "BorderStyle"))
                {
                    //m_BackupInnerControlBorderStyle = m_ControlInner.BorderStyle; 
                    m_BackupInnerControlBorderStyle = innerControl_t.InvokeMember("BorderStyle", InvokeFlagPropertyGet, null, m_ControlInner, null);

                    FormBorderStyle bs = (FormBorderStyle)m_BackupInnerControlBorderStyle;
                    if ( bs != FormBorderStyle.None)
                    {
                        //m_ControlInner.BorderStyle = FormBorderStyle.None;
                        innerControl_t.InvokeMember("BorderStyle", InvokeFlagPropertySet, null, m_ControlInner, new object[1] { FormBorderStyle.None });
                    }
                }
            }
            catch (System.Exception ex)
            {
                Tools.ShowMessageOnce("Exception on FormContainer_Load() :\r\n{1}", ex.Message);            	
            }


            m_ControlInner.Dock = DockStyle.Fill;
            m_ControlInner.Visible = true;
            if (m_Parent != null)
            {
                TopLevel = false;
                Dock = DockStyle.None;
                Parent = m_BackupInnerControlParent;
            }
        }

        private void FormContainer_FormClosing(object sender, FormClosingEventArgs e)
        {
            DEBUG.WriteLine("FormContainer_FormClosing()");
            if (OnRelease != null)
            {
                if (!OnRelease(this, m_ControlInner, m_Parent))
                    e.Cancel = true;
                return;
            }

            if (!m_bDestructInnerWhenClosing)
            {
                panelContainer.Controls.Remove(m_ControlInner);

                bool bRollbackSucceededUsingRtti = false;
                try
                {
                    Type innerControlParentPrev_t = m_BackupInnerControlParent.GetType();
                    if (RTTI.HasProperty_p(innerControlParentPrev_t, "Controls"))
                    {
                        // Equivalent to : m_BackupInnerControlParent.Controls.Add(m_ControlInner);

                        //foreach (string st in RTTI.GetProperties(innerControlParentPrev_t))
                        //    DEBUG.WriteLine("TYPE: {0}", st);

                        object oControls = innerControlParentPrev_t.InvokeMember("Controls", InvokeFlagPropertyGet, null, m_BackupInnerControlParent, new object[0] { });
                        if (oControls != null)
                        {
                            Type Controls_t = oControls.GetType();
                            Controls_t.InvokeMember("Add", BindingFlags.InvokeMethod, null, oControls, new object[1] { m_ControlInner });
                            bRollbackSucceededUsingRtti = true;
                        }
                    }

                    Type innerControl_t = m_ControlInner.GetType();
                    if (RTTI.HasProperty_p(innerControl_t, "FormBorderStyle"))
                    {
                        innerControl_t.InvokeMember("FormBorderStyle", InvokeFlagPropertySet, null, m_ControlInner, new object[1] { m_BackupInnerControlBorderStyle });
                    }
                    else if (RTTI.HasProperty_p(innerControl_t, "BorderStyle"))
                    {
                        innerControl_t.InvokeMember("BorderStyle", InvokeFlagPropertySet, null, m_ControlInner, new object[1] { m_BackupInnerControlBorderStyle });
                    }
                }
                catch (System.Exception ex)
                {
                    Tools.ShowMessageOnce("Exception on FormContainer_FormClosing() :\r\n{1}", ex.Message);            	
                }


                if (bRollbackSucceededUsingRtti)
                    Debug.Assert(m_ControlInner.Parent.Equals(m_BackupInnerControlParent));
                else
                    m_ControlInner.Parent = m_BackupInnerControlParent;

                //m_ControlInner.BorderStyle = FormBorderStyle.None;
                m_ControlInner.Dock = DockStyle.Fill;   //  m_BackupInnerControlDockStyle;
            }
        }
    }
}
