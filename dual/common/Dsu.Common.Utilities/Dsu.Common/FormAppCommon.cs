using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dsu.Common.Interfaces;
using Dsu.Common.Utilities.Designer;
using Dsu.Common.Utilities.ExtensionMethods;

namespace Dsu.Common.Utilities
{
    /// <summary>
    /// 모든 applicaiton form 의 공통 base class
    /// </summary>
    [TypeDescriptionProvider(typeof(AbstractControlDescriptionProvider<FormAppCommon, Form>))]
    public abstract class FormAppCommon : Form, ITopLevelHelpProvider
    {
        public static readonly LogProxy logger = LogProxy.CreateLoggerProxy(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary> Main UI SynchronizationContext </summary>
        public static SynchronizationContext UISynchronizationContext { get; private set; }
        /// <summary> Main UI TaskScheduler(System.Threading.Tasks.TaskScheduler) </summary>
        public static TaskScheduler UITaskScheduler { get; private set; }
        protected IApplication ApplicationImpl
        {
            get { return _applicationImpl; } 
            set
            {
                Contract.Requires(value is ITopLevelHelpProvider);
                _applicationImpl = value;
            }
        }

        private IApplication _applicationImpl;

        public string HelpFilePath { get { return ((ITopLevelHelpProvider)_applicationImpl).HelpFilePath; } }
        public HelpProvider ContextHelp { get { return _contextHelp; } }
        private HelpProvider _contextHelp = new ExHelpProvider();

        protected virtual void OnApplicationFormShown(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                ContextHelp.HelpNamespace = HelpFilePath;

                if ( ! ContextHelp.GetShowHelp(this) )
                    ContextHelp.SetHelp(this, "Welcome.htm");

                this.SetLanguage(CommonConfiguration.TheCommonConfiguration.Language.ConvertToString());
                CommonConfiguration.TheCommonConfiguration.ConfigurationChangedHook += ConfigurationChanged;
                Closed += (s, args) =>
                {
                    CommonConfiguration.TheCommonConfiguration.ConfigurationChangedHook -= ConfigurationChanged;
                };
            }
        }
        public FormAppCommon()
        {
            UISynchronizationContext = SynchronizationContext.Current;
            UITaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();

            Shown += OnApplicationFormShown;
        }

        protected virtual void ConfigurationChanged(object sender, ConfigurationChangedEventArgs e)
        {
            if (e.ChangedPropertyName == "Language")
                this.SetLanguage(CommonConfiguration.TheCommonConfiguration.Language.ConvertToString());
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // FormAppCommon
            // 
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Name = "FormAppCommon";
            this.ResumeLayout(false);

        }

    }
}
