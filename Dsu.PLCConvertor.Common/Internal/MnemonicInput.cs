using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Dsu.PLCConvertor.Common
{
    public partial class MnemonicInput
    {
        public string Input { get; private set; }
        public List<string> DesiredOutputs { get; private set; }
        public string Comment { get; private set; }

        public static string CommentPrefix { get; set; } = "//";

        /// <summary>
        /// multiline 문자열 입력을 라인단위로 쪼갠후, 빈 문자열 제거하고 앞뒤 공백문자나 comment 문자로 시작하는 라인 제거하여
        /// array 문자열로 반환
        /// </summary>
        public static string[] MultilineString2Array(string input) =>
                input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Where(m => !string.IsNullOrWhiteSpace(m))
                .Select(m => TrimSingle(m))
                .Select(m => m.Replace('\t', ' '))
                .Select(m => Regex.Replace(m, $"{CommentPrefix}.*", ""))    // 코멘트 이후 라인 끝까지 제거
                .ToArray()
                ;

        public static string CommentOutMultiple(string mns) => String.Join("\r\n", MultilineString2Array(mns));
        public static string CommentOutSingle(string mn) => Regex.Replace(mn, $"{CommentPrefix}.*", "");
        public static string TrimSingle(string mn) => Regex.Replace(mn.TrimStart(new[] { ' ', '\t' }), $"{CommentPrefix}.*", "").TrimEnd(new[] { ' ', '\t', '\r', '\n' });

        public MnemonicInput(string comment, string input)
        {
            Input = input;
            Comment = comment;
        }
        public MnemonicInput(string comment, string input, string output)
            : this(comment, input, new[] { output })
        {
        }
        public MnemonicInput(string comment, string input, IEnumerable<string> outputs)
            : this(comment, input)
        {
            DesiredOutputs = outputs.ToList();
        }


        internal static MnemonicInput[] Inputs { get {
                return 
                    new[] { InputsOK, InputsTR, InputsComplex, InputsSpecial, InputsBUG, InputsNG, }
                    .SelectMany(inp => inp)
                    .ToArray()
                ;
            }
        }
    }
}
