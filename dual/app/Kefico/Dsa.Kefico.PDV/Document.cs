using DevExpress.XtraBars.Docking2010.Views;
using Dsa.Kefico.PDV.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Dsa.Kefico.PDV
{
    public class DocumentMWS
    {
        private Dictionary<string, BaseDocument> Documents = new Dictionary<string, BaseDocument>();
        private bool _IsCreating = false;
        public DocumentMWS()
        {
        }

        public bool Creating { get { return _IsCreating; } set { _IsCreating = value; } }

        public void AddDocument(string Key, BaseDocument document)
        {
            if (document.Caption == ViewPDV.selectPdvView)
                document.Caption = ViewPDV.CaptionPdv;
            else if (document.Caption == ViewPDV.selectPdvGroup)
                document.Caption = ViewPDV.CaptionGroup;
            else if (document.Caption == ViewPDV.selectPdvTestList)
                document.Caption = ViewPDV.CaptionTestList;

            if (!Documents.ContainsKey(Key))
                Documents.Add(Key, document);
            _IsCreating = false;
        }

        public void RemoveDocument(string Key)
        {
            if (Documents.ContainsKey(Key))
            {
                Documents.Remove(Key);
            }
        }
        public bool GetDocument(string Key, out BaseDocument document)
        {
            document = null;
            if (Documents.ContainsKey(Key))
            {
                document = Documents[Key];
                return true;
            }
            else
                return false;
        }

        public bool ExistDoc(string Key)
        {
            if (Documents.ContainsKey(Key))
                return true;
            else
                return false;
        }

       
    }
}
