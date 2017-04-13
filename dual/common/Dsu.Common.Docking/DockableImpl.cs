using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Dsu.Common.Interfaces;
using Dsu.Common.Utilities.ExtensionMethods;
using WeifenLuo.WinFormsUI.Docking;

namespace Dsu.Common.Utilities
{
    /// <summary>
    /// DockPanelSuite 를 적용하기 위한 기본 기능 구현 class
    /// <br/> - http://sourceforge.net/projects/dockpanelsuite
    /// </summary>
    public class DockableImpl : IDockable
    {
        private IApplication _application;
        private DockPanel _dockPanel;

        private SupportedCultures Language
        {
            get
            {
                if (CommonConfiguration.TheCommonConfiguration == null)
                    return default(SupportedCultures);

                return CommonConfiguration.TheCommonConfiguration.Language;
            }
        }

        public Dictionary<string, DockContent> DockInstanceMap { get { return _dockInstanceMap;} }
        private Dictionary<string, DockContent> _dockInstanceMap = new Dictionary<string, DockContent>();

        public DockableImpl(IApplication application, DockPanel dockPanel)
        {
            _application = application;
            _dockPanel = dockPanel;
        }

        public void SaveDockLayout(string xml)
        {
            using (new CwdChanger(CommonApplication.GetProfilePath()))
                _dockPanel.SaveAsXml(xml);

        }

        public void LoadDockLayout(string xml)
        {
            using (new CwdChanger(CommonApplication.GetProfilePath()))
            {
                if ( File.Exists(xml) )
                    _dockPanel.LoadFromXml(xml, LoadDockPanelSuite);
            }

        }
        public void RemoveNamedDockContent(string persistString)
        {
            DockInstanceMap.Remove(persistString);
        }


        protected virtual IDockContent LoadDockPanelSuite(string persistString)
        {
            return CreateDockableForm(persistString);
        }

        /* requires public for DotFuscator */
        public void SetLanguage(Form form, SupportedCultures lang)
        {
            try
            {
                form.SetLanguage(Language);
            }
            catch (Exception)
            {
                CommonApplication.Logger.ErrorFormat("Failed to change language {0} for form {1}",
                    Language.ToString(), form.Name);
            }            
        }

        /// <summary> Dockable form 생성 </summary>
        /// <param name="persistString">"Dsu.UI.WinForm.FormDockablePropertyWindow", "...FormDockableS3NodeTreeView", ... </param>
        /// <param name="singleton"></param>
        /// <param name="show"></param>
        /// <returns></returns>
        public DockContent CreateDockableForm(string persistString, bool singleton = true, bool show=true)
        {
            if (!persistString.Contains(".FormDockable") && !persistString.Contains(".DXFormDockable"))
                return null;

            if (singleton && DockInstanceMap.ContainsKey(persistString))
            {
                var form = DockInstanceMap[persistString];
                if ( show )
                    form.Do(() =>
                    {
                        SetLanguage(form, Language);
                        form.Show();
                    });
                return form;
            }
            else
            {
                Type t = Type.GetTypeFromProgID(persistString);
                dynamic form = Activator.CreateInstance(t);
                form.Initialize(_application, _dockPanel);
                SetLanguage(form, Language);

                if (show)
                    form.Show();
                DockInstanceMap.Add(persistString, form as DockContent);
                return form;
            }
        }

        public void ConfigurationChanged(object sender, ConfigurationChangedEventArgs e)
        {
            if (e.ChangedPropertyName == "Language")
            {
                var lang = CommonConfiguration.TheCommonConfiguration.Language;
                lang.Apply();

                _dockInstanceMap.Values.ForEach(d => d.SetLanguage(lang));
            }
        }

    }
}
