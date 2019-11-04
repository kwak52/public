using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.PLCConvertor.Common.Internal;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace PLCConvertor.Forms
{
    /// <summary>
    /// 하나의 rule 을 추가하기 위한 form
    /// - AddressConvertRuleSpecialRelay(1:1 mapping rule),
    /// - AddressConvertRule(range rule)
    /// - NamedAddressConvertRule
    /// </summary>
    public partial class FormAddAddressMappingRule : Form
    {
        enum RuleType
        {
            OneToOne,
            Range,
            Named,
        };

        static Tuple<string, RuleType>[] _ruleNameDic = new Tuple<string, RuleType>[]
        {
            Tuple.Create("One to one", RuleType.OneToOne),
            Tuple.Create("Formatted ranges", RuleType.Range),
            Tuple.Create("Named formatted range", RuleType.Named),
        };



        public IAddressConvertRule Rule { get; private set; }
        public FormAddAddressMappingRule()
        {
            InitializeComponent();
        }

        TextBox[] _textBoxSrcArgs => new[] { textBoxSourceArg0, textBoxSourceArg1, textBoxSourceArg2, textBoxSourceArg3 };
        TextBox[] _textBoxTgtArgs => new[] { textBoxTargetArg0, textBoxTargetArg1, textBoxTargetArg2, textBoxTargetArg3 };
        private void FormAddAddressMappingRule_Load(object sender, EventArgs args)
        {
            listBoxRuleType.DataSource = _ruleNameDic.ToArray();
            listBoxRuleType.DisplayMember = "Item1";
            listBoxRuleType.SelectedValueChanged += (s, e) =>
            {
                var tpl = listBoxRuleType.SelectedValue as Tuple<string, RuleType>;
                var type = tpl.Item2;

                var oneToOne = type == RuleType.OneToOne;
                
                textBoxNumArgs.Visible =
                labelNumArgs.Visible = !oneToOne;
                
                labelRuleName.Visible =
                textBoxRuleName.Visible = type == RuleType.Named;

                if (oneToOne)
                    textBoxNumArgs.Text = "0";

                Console.WriteLine(type);
            };


            var labels = new[] { labelArg0, labelArg1, labelArg2, labelArg3 };
            var allWidgets = new Control[][] { labels, _textBoxSrcArgs, _textBoxTgtArgs };
            textBoxNumArgs.TextChanged += (s, e) =>
            {
                int numArgs = 0;
                if (int.TryParse(textBoxNumArgs.Text, out numArgs))
                {
                    allWidgets.Iter(widgets => widgets.Iter((w, n) => w.Visible = n < numArgs));
                }
            };

            listBoxRuleType.SetSelected(0, true);
            textBoxNumArgs.Text = "0";
        }


        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            var tpl = listBoxRuleType.SelectedValue as Tuple<string, RuleType>;
            var type = tpl.Item2;

            var s = textBoxSource.Text;
            var t = textBoxTarget.Text;

            var srcRanges =
                _textBoxSrcArgs
                    .Where(tb => tb.Visible)
                    .Select(tb =>
                    {
                        var range =
                            tb.Text
                                .Split(new[] { ' ', ',', '\t' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(str => int.Parse(str))
                                .ToArray()
                                ;
                        return Tuple.Create(range[0], range[1]);
                    });

            var tgtFormulas =
                _textBoxTgtArgs
                    .Where(tb => tb.Visible)
                    .Select(tb => tb.Text)
                    ;

            switch (type)
            {
                case RuleType.OneToOne:
                    Rule = new AddressConvertRuleSpecialRelay(s, t);
                    break;
                case RuleType.Range:
                    Rule = new AddressConvertRule(s, srcRanges, t, tgtFormulas);
                    break;
                case RuleType.Named:
                    Rule = new NamedAddressConvertRule(textBoxRuleName.Text, s, srcRanges, t, tgtFormulas);
                    break;
            }
            Close();
        }
    }
}
