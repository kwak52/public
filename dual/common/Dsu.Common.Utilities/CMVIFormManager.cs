using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace Dsu.Common.Utilities
{
    public class CControlInfo
    {
        protected Control m_Control = null;        // the control itself
        protected FormContainer m_Form = null;
        protected bool m_bToplevel = false;        // toplevel window 가 되면 true 로 설정된다.
        protected string m_strDesc = null;
        protected string m_strFormGeometry = null;

        public Control Control { get { return m_Control; } }
        public FormContainer FormContainer { get { return m_Form; } set { m_Form = value; } }
        public bool Toplevel { get { return m_bToplevel; } set { m_bToplevel = value; } }
        public string Description { get { return m_strDesc; } }
        public string Geometry { get { return m_strFormGeometry; } set { m_strFormGeometry = value; } }

        public CControlInfo(Control ctrl, string strDesc, string initialLayout)
        {
            m_strDesc = strDesc;
            m_Control = ctrl;
            m_strFormGeometry = initialLayout;
        }
    }

    public class CMVIFormManager
    {
        private Panel m_Panel = null;           // 모든 control 들을 포함하는 panel
        private List<CControlInfo> m_lstControlInfo = new List<CControlInfo>();
        private bool m_bMultiWindowMode = false;
        private Control m_Owner = null;     // form manager 의 소유주

        public Panel Panel { get { return m_Panel; } }
        public Control Owner { get { return m_Owner; } }
        public bool MultiWindowMode { get { return m_bMultiWindowMode; } set { m_bMultiWindowMode = value; } }
        public List<CControlInfo> ControlInfoList { get { return m_lstControlInfo; } }


        public CMVIFormManager(Control owner, Panel panel, Control[] controls, string[] descs, string[] initialLayouts)
        {
            Debug.Assert(controls.Length == descs.Length);
            m_Owner = owner;
            m_Panel = panel;
            for (int i = 0; i < controls.Length; i++)
            {
                if ( controls[i] == null )
                    continue;

                Debug.Assert(controls[i].Parent == panel);
                AddControl(controls[i], descs[i], (initialLayouts != null && initialLayouts[i] != null) ? initialLayouts[i] : null);
            }
        }

        public void AddControl(Control c, string desc, string initialLayout) { m_lstControlInfo.Add(new CControlInfo(c, desc, initialLayout)); }

        public void UpdateControlGeometry(Control c, string strGeometry)
        {
            if (c == null || String.IsNullOrEmpty((strGeometry)))
                return;

            var vQuery = m_lstControlInfo.Where(ci => { return ci.Control == c; });
            if ( vQuery.Count() == 0 )
                return;
            
            Debug.Assert(vQuery.Count() == 1);
            vQuery.ElementAt(0).Geometry = strGeometry;
        }

        public void GoMultiWindowMode() { GoMultiWindowMode(null); }
        public void GoMultiWindowMode(Control ctrlTop/*=null*/)
        {
            m_bMultiWindowMode = true;
            foreach (CControlInfo ci in m_lstControlInfo)
            {
                FormContainer frm = new FormContainer(ci.Control, m_Panel, false);
                frm.FormManager = this;
                frm.ControlBox = false;     // disables minimize / maximize / close button
                ComponentGeometrySerializer.ApplyGeometryString(frm, ci.Geometry);
                frm.SizeChanged += new EventHandler((sender, e) =>
                {
                    //FormContainer frmC = sender as FormContainer;
                    frm.FormManager.UpdateControlGeometry(frm.Inner, ComponentGeometrySerializer.GetGeometryString(frm));
                    DEBUG.WriteLine("Size Changed!!");
                });
                frm.Move += new EventHandler((sender, e) =>
                {
                    //FormContainer frmC = sender as FormContainer;
                    frm.FormManager.UpdateControlGeometry(frm.Inner, ComponentGeometrySerializer.GetGeometryString(frm));
                    DEBUG.WriteLine("Size Changed!!");
                });
                frm.Text = ci.Description;
                ci.FormContainer = frm;
                frm.Show();
            }

            BringControlToFront(ctrlTop);
        }

        public void GoSingleWindowMode() { GoSingleWindowMode(null); }
        public void GoSingleWindowMode(Control ctrlTop/*=null*/)
        {
            m_bMultiWindowMode = false;
            foreach (CControlInfo ci in m_lstControlInfo)
            {
                if (ci.FormContainer == null)
                {
                    Tools.ShowMessageOnce("Error on GoSingleWindowMode()");
                    ci.Geometry = null;
                }
                else
                {
                    ci.Geometry = ComponentGeometrySerializer.GetGeometryString(ci.FormContainer);
                    ci.FormContainer.Close();
                    ci.FormContainer = null;
                }
            }

            BringControlToFront(ctrlTop);
        }

        public bool BringControlToFront(Control ctrl)
        {
            if (ctrl == null)
                return false;

            if (m_bMultiWindowMode)            // multi-window mode 에서는 control 을 포함하는 form 을 top 으로 이동시켜야 한다.
            {
                foreach (CControlInfo ci in m_lstControlInfo)
                {
                    if (ci.Control != null && ci.Control == ctrl)
                    {
                        ci.FormContainer.BringToFront();
                        return true;
                    }
                }

                return false;
            }
            else  // single window mode 에서는 control 자체를 앞으로 이동시킨다.
            {
                ctrl.BringToFront();
                ctrl.Visible = true;

                return true;
            }
        }

        public CControlInfo FindTopmostControl()
        {
            CControlInfo ctrlTopmost = null;
            int nMaxZOrder = -1;
            foreach (CControlInfo ci in m_lstControlInfo)
            {
                if (ci.Toplevel)   // control 이 이미 toplevel 이면 skip
                    continue;

                Control ctrl = m_bMultiWindowMode ? ci.FormContainer : ci.Control;
                int n = m_Panel.Controls.IndexOf(ctrl);
                if (nMaxZOrder == -1 || n < nMaxZOrder)
                {
                    nMaxZOrder = n;
                    ctrlTopmost = ci;
                    //DEBUG.WriteLine("Control {0} : {1}", ctrlTopmost, n);
                }
            }

            return ctrlTopmost;
        }

        public CControlInfo FindControlInfo(Control ctrl)
        {
            foreach (CControlInfo ci in m_lstControlInfo)
                if ( ci.Control == ctrl )
                    return ci;
            return null;
        }

        public virtual FormContainer MoveFrontToplevel()
        {
            FormContainer frm = null;
            CControlInfo ciTopmost = FindTopmostControl();

            if (ciTopmost == null)
                return null;

            Control ctrlTopmost = ciTopmost.Control;
            if (ciTopmost.FormContainer == null)
            {
                frm = new FormContainer(ctrlTopmost, null, false);
                frm.FormManager = this;
                frm.Show();
            }
            else
            {
                frm = ciTopmost.FormContainer;
                m_Panel.Controls.Remove(ctrlTopmost);
                frm.Parent = null;
                frm.TopLevel = true;
                frm.ControlBox = true;     // enables minimize / maximize / close button
                ciTopmost.Toplevel = true;
            }

            return frm;
        }

        public int CountToplevelForms()
        {
            int n = 0;
            foreach (CControlInfo ci in m_lstControlInfo)
                if (ci.Toplevel)
                    n++;
            return n;
        }

        public int CountNonToplevelForms()
        {
            int n = 0;
            foreach (CControlInfo ci in m_lstControlInfo)
                if (! ci.Toplevel)
                    n++;
            return n;
        }
    }
}
