using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.PLCConvertor.Common.Internal;
using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PLCConvertor.Forms
{
    public partial class FormTestAddressMapping : Form
    {
        public static ILog Logger => FormPLCConverter.Logger;
        public FormTestAddressMapping()
        {
            InitializeComponent();
        }

        private void FormAddressMapping_Load(object sender, EventArgs e)
        {
            textBoxSourcePattern.Text = "(%d).(%2d)";
            textBoxTargetPattern.Text = "P(%4d)(%x)";
            textBoxSourceArgs.Text = "0 1\r\n0 15";
            textBoxTargetArgs.Text = "$0 * 2\r\n$1";
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            var srcPattern = textBoxSourcePattern.Text;
            var tgtPattern = textBoxTargetPattern.Text;
            var srcArgsRange =
                textBoxSourceArgs.Text
                    .SplitByLines()
                    .Select(ln =>
                    {
                        var nums =
                            ln.Split(new[] { ' ', '\t', ',' }, StringSplitOptions.RemoveEmptyEntries)
                              .Select(num => int.Parse(num))
                              .ToArray()
                              ;
                        Debug.Assert(nums.Length == 2);
                        return Tuple.Create(nums[0], nums[1]);
                    });

            var tgtArgsExpr = textBoxTargetArgs.Text.SplitByLines();

            var rule = new AddressConvertRule(srcPattern, srcArgsRange, tgtPattern, tgtArgsExpr);
            rule.GenerateTranslations().Iter(pr => Logger.Info($"{pr.Item1} => {pr.Item2}"));
        }
    }
}
