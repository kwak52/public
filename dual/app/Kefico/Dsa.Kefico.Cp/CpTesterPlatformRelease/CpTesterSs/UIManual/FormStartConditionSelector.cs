using CpTesterPlatform.CpTester;
using Dsu.Common.Utilities.ExtensionMethods;
using System.Windows.Forms;

namespace CpTesterSs.UIManual
{
    public partial class FormStartConditionSelector : Form
    {
        public static bool UseTemperatureConstraint { get; private set; } = true;
        public static bool UseHumidityConstraint { get; private set; } = true;
        public static bool UseVoltageConstraint { get; private set; } = true;
        public static bool UseRpmConstraint { get; private set; } = true;

        public FormStartConditionSelector()
        {
            InitializeComponent();
        }

        public bool IsEnvirontmentalConditionOK()
        {
            var form = FormAppSs.TheMainForm;
            if (UseTemperatureConstraint && !form.CurrentTemerature.InClosedRange(form.TemperatureMin, form.TemperatureMax))
                return false;
            if (UseHumidityConstraint && !form.CurrentHumidity.InClosedRange(form.HumidityMin, form.HumidityMax))
                return false;

            return true;
        }
    }
}
