using Dsu.PLCConvertor.Common.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCConvertor.Forms
{
    partial class FormEditAddressMappingRule
    {
        /// <summary>
        /// 구조체를 UI 에서 변경하도록 하기 위해서 참조 형태로 임시 변경하기 위한 용도
        /// </summary>
        class MinMaxRange
        {
            public int Min { get; set; }
            public int Max { get; set; }
            public MinMaxRange(Tuple<int, int> tpl)
            {
                Min = tpl.Item1;
                Max = tpl.Item2;
            }
        }

        /// <summary>
        /// 변환 rule 하나가 선택 되었을 때, 그 rule 의 source 관련 detail 을 저정하기 위한 용도
        /// </summary>
        class SourceDetailWrapper : List<MinMaxRange>
        {
            AddressConvertRule _rule;
            public SourceDetailWrapper(AddressConvertRule rule)
            {
                _rule = rule;
                var entities = rule.SourceArgsMinMax.Select(tpl => new MinMaxRange(tpl));
                AddRange(entities);
            }

            public void Apply()
            {
                _rule.SourceArgsMinMax =
                    this.Select(range => Tuple.Create<int, int>(range.Min, range.Max))
                    .ToArray()
                    ;
            }
        }

        /// <summary>
        /// rule 의 target 의 expression 하나를 저장하기 위한 용도
        /// </summary>
        class ExpressionHolder
        {
            public string Expression { get; set; }
            public ExpressionHolder(string expression)
            {
                Expression = expression;
            }
        }

        /// <summary>
        /// 변환 rule 하나가 선택 되었을 때, 그 rule 의 target 관련 detail 을 저정하기 위한 용도
        /// </summary>
        class TargetDetailWrapper : List<ExpressionHolder>
        {
            AddressConvertRule _rule;
            public TargetDetailWrapper(AddressConvertRule rule)
            {
                _rule = rule;
                var entities = rule.TargetArgsExpr.Select(exp => new ExpressionHolder(exp));
                AddRange(entities);
            }

            public void Apply()
            {
                _rule.TargetArgsExpr = this.Select(exp => exp.Expression).ToArray();
            }
        }
    }
}
