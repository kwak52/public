using System;
using System.Windows.Forms;
using System.ComponentModel;
using Dsu.Common.Utilities.PropertyGrids;

namespace Dsu.Common.Utilities
{
    [TypeConverter(typeof(PropertySorter))]
    [DefaultProperty("Properties")]
    public class CSDI
    {
        private string m_strFileName = null;
        private bool m_bDirty = false;
        private string m_strFilter = null;
        private object m_ContainerApp = null;

        public string FileName { get { return m_strFileName; } set { m_strFileName = value; } }
        
        [Browsable(false)]
        public bool DocDirty { get { return m_bDirty; } set { m_bDirty = value; } }

        [Browsable(false)]
        public object ContainerApp { get { return m_ContainerApp; } set { m_ContainerApp = value; } }

        [Browsable(false)]
        public string Filter { get { return m_strFilter; } set { m_strFilter = value; } }

        public CSDI(object ContainerApp, string strFilter) { m_ContainerApp = ContainerApp; m_strFilter = strFilter; }

        public delegate bool DelegatorSDIWithFile(string strFileName);
        public delegate bool DelegatorNoParam();

        public DelegatorSDIWithFile OnSave;
        public DelegatorSDIWithFile OnOpen;
        public DelegatorNoParam OnNew;
        public DelegatorNoParam OnClose;


        public bool DocumentLoaded_p() { return !String.IsNullOrEmpty(m_strFileName); }

        private void ValidateDelegate()
        {
            if ( Tools.HasAnyNull_p(OnClose, OnOpen, OnSave, OnNew) )
                throw new System.MethodAccessException("One of OnOpen/OnClose/OnSave/OnNew method not implemented");
        }

        public virtual bool SaveAs()
        {
            using(SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = m_strFilter;
                sfd.RestoreDirectory = true;
                if (sfd.ShowDialog() != DialogResult.OK)
                    return false;

                ValidateDelegate();
                bool bSucceeded = OnSave(sfd.FileName);
                if (m_bDirty && bSucceeded)
                    m_bDirty = false;

                return bSucceeded;
            }
        }

        public virtual bool Save()
        {
            ValidateDelegate();
            bool bSucceeded = OnSave(m_strFileName);
            if (m_bDirty && bSucceeded)
                m_bDirty = false;

            return bSucceeded;
        }

        public virtual bool Open()
        {
            if (!Close())
                return false;

            using(OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = m_strFilter;
                ofd.RestoreDirectory = true;
                if (ofd.ShowDialog() != DialogResult.OK)
                    return false;

                return Open(ofd.FileName);
            }
        }

        private bool Open(string strFileName)
        {
            ValidateDelegate();
            if (String.IsNullOrEmpty(strFileName))
                return false;

            if (DocumentLoaded_p())
            {
                if (m_strFileName == strFileName)       // 현재 열린 문서와 동일 문서를 열려고 하는 경우
                    return true;

                if (!OnClose())
                    return false;
            }


            return OnOpen(strFileName);
        }

        public virtual bool Close()
        {
            ValidateDelegate();
            if (m_bDirty)
            {
                DialogResult response = MessageBox.Show(String.Format("Document {0} is modified.   Do you want to save?"
                    + "\r\nIf you select <NO>, current changes will be lost.", m_strFileName), "Save Changes:", MessageBoxButtons.YesNoCancel);
                if (response == DialogResult.Cancel)
                    return false;
                else if (response == DialogResult.Yes)
                {
                    if (String.IsNullOrEmpty(m_strFileName))
                    {
                        if (!SaveAs())
                            return false;
                    }
                    else
                    {
                        if (!OnSave(m_strFileName))
                            return false;
                    }
                }
            }

            bool bResult = OnClose();
            m_strFileName = null;
            m_bDirty = false;
            return bResult;
        }

        public virtual bool New()
        {
            ValidateDelegate();

            bool bDocLoaded = DocumentLoaded_p();
            bool bColseSucceeded = Close();         // anyway, Close() should be executed!!!
            if (bDocLoaded && !bColseSucceeded)
                return false;

            return OnNew();
        }

        public virtual void Reset()
        {
            m_strFileName = null;
            m_bDirty = false;
        }
    }
}