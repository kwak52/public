using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Dsu.Common.Utilities.ExtensionMethods;

namespace Dsu.Common.Utilities.DX
{
    public class DXFormAppCommon : FormAppCommon, IFormProgressbarCancelable
    {
        protected SplashScreenProgressWaitor _progressbarImpl;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public string ProgressCaption { get { return _progressbarImpl.ProgressCaption; } set { _progressbarImpl.ProgressCaption = value; } }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public string ProgressDescription { get { return _progressbarImpl.ProgressDescription; } set { _progressbarImpl.ProgressDescription = value; } }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public CancellationToken CancellationToken { get { return _progressbarImpl.CancellationToken; } }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public int ProgressTotal { get { return _progressbarImpl.ProgressTotal; } set { _progressbarImpl.ProgressTotal = value; } }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public int ProgressPortion { get { return _progressbarImpl.ProgressPortion; } set { _progressbarImpl.ProgressPortion = value; } }

        public void AddProgressPortion(int portion) { _progressbarImpl.AddProgressPortion(portion); }
        public void StartProgressbar()
        {
            _progressbarImpl.StartProgressbar();
        }

        public void FinishProgressbar()
        {
            _progressbarImpl.FinishProgressbar();
        }

        protected DXFormAppCommon()
        {
            if (!DesignMode)
                _progressbarImpl = new SplashScreenProgressWaitor(this);            
        }


        public void RunBackgroundAction(string actionName, Action action, bool withProgressUI)
        {
            Task.Run(() =>
            {
                try
                {
                    if ( withProgressUI )
                    {
                        StartProgressbar();
                        ProgressDescription = actionName;
                    }

                    TimeSpan elapsed = ActionStopWatch.GetExecutionTimeSpan(() => action());

                    logger.InfoFormat("Processing action {0} took {1} milliseconds.", actionName, elapsed.TotalMilliseconds);
                }
                finally
                {
                    if (withProgressUI)
                        FinishProgressbar();
                }
            });
        }


        /// <summary>
        /// http://stackoverflow.com/questions/29193669/how-to-check-if-type-from-assembly-is-comvisible
        /// Check if the given type is ComVisible
        /// </summary>
        /// <param name="type">the type to check</param>
        /// <returns>whether or not the given type is ComVisible</returns>
        private bool IsComVisible(Type type)
        {
            bool comVisible = false;
            //first check if the type has ComVisible defined for itself
            var typeAttributes = type.GetCustomAttributes(typeof(ComVisibleAttribute), false);
            var guidAttributes = type.GetCustomAttributes(typeof(GuidAttribute), false);
            if (typeAttributes.Length > 0 && guidAttributes.Length > 0)
            {
                comVisible = ((ComVisibleAttribute)typeAttributes[0]).Value;
            }
            //else
            //{
            //      /* 여기는 assembly 자체가 COM visible 인지 등의 여부 */
            //    //no specific ComVisible attribute defined, return the default for the assembly
            //    var assemblyAttributes = type.Assembly.GetCustomAttributes(typeof(ComVisibleAttribute), false);
            //    if (assemblyAttributes.Length > 0 && assemblyGuidAttributes.Length > 0 )
            //    {
            //        comVisible = ((ComVisibleAttribute)assemblyAttributes[0]).Value;
            //    }
            //}

            return comVisible;
        }
        protected void ClearRegistry(IEnumerable<string> dlls)
        {
            var dir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            using (new CwdChanger(dir))
            {
                /* 등록 해제할 dll 들 */
                var defaultDlls = new[] {
                    "Dsu.Common.Interfaces.dll",
                    "Dsu.Common.Utilities.dll",
                    "Dsu.Common.Utilities.DX.dll",
                    "Dsu.Common.UI.WinForm.dll",
                }.Union(dlls);

                var assemblies = Assembly.GetEntryAssembly().ToEnumerable()
                    .Union(defaultDlls.Select(d => Assembly.LoadFrom(Path.Combine(dir, d))));

                var COMVisible = assemblies.SelectMany(a => a.GetTypes())
                    .Where(t => IsComVisible(t))
                    .Distinct()
                    .Select(t => new { Type = t, Guid = (GuidAttribute)t.GetCustomAttributes(typeof(GuidAttribute), false)[0] })
                    ;
                COMVisible.ForEach(cv =>
                {
                    var guid = "{" + cv.Guid.Value + "}";
                    var t1 = RegistryHelper.DeleteKey(@"CLSID\" + guid, "HKCR");
                    var t2 = RegistryHelper.DeleteKey(@"Interface\" + guid, "HKCR");
                    var t3 = RegistryHelper.DeleteKey(@"Record\" + guid, "HKCR");           // COM visible enum
                    var t4 = RegistryHelper.DeleteKey(@"Wow6432Node\Interface\" + guid, "HKCR");
                    var t5 = RegistryHelper.DeleteKey(@"SOFTWARE\Classes\Interface\" + guid, "HKLM");

                    if (t1 || t2 || t3 || t4 || t5)
                        logger.InfoFormat("Unregistering {0} = {1}", cv.Type, guid);
                    else
                        logger.ErrorFormat("Failed to unregistering {0} = {1}", cv.Type, guid);
                });
            }            
        }
    }
}
