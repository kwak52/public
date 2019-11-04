using Dsu.PLCConvertor.Common.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        //static Dictionary<string, RuleType> _ruleNameDic = new Dictionary<string, RuleType>()
        //{
        //    ["One to one"] = RuleType.OneToOne,
        //    ["Formatted ranges"] = RuleType.Range,
        //    ["Named formatted range"] = RuleType.Named,
        //};

        static Tuple<string, RuleType>[] _ruleNameDic = new Tuple<string, RuleType>[]
        {
            Tuple.Create("One to one", RuleType.OneToOne),
            Tuple.Create("Formatted ranges", RuleType.Range),
            Tuple.Create("Named formatted range", RuleType.Named),
        };



        public IAddressConvertRule _rule;
        public IAddressConvertRule Rule { get; private set; }
        public FormAddAddressMappingRule()
        {
            InitializeComponent();
        }

        private void FormAddAddressMappingRule_Load(object sender, EventArgs args)
        {
            listBoxRuleType.DataSource = _ruleNameDic.ToArray();
            listBoxRuleType.DisplayMember = "Item1";
            listBoxRuleType.SelectedValueChanged += (s, e) =>
            {
                var tpl = listBoxRuleType.SelectedValue as Tuple<string, RuleType>;
                var type = tpl.Item2;
                switch(type)
                {
                    case RuleType.OneToOne:
                        _rule = new AddressConvertRuleSpecialRelay("s", "t");
                        break;
                    //case RuleType.Range:
                    //    _rule = new AddressConvertRule("s", "t");
                    //    break;
                    //case RuleType.Named:
                    //    break;
                }
                Console.WriteLine(type);
            };
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Rule = _rule;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
