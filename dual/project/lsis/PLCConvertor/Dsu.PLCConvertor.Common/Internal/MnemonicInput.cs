using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Dsu.PLCConvertor.Common
{
    public partial class MnemonicInput
    {
        public string Input { get; private set; }
        public List<string> DesiredOutputs { get; private set; }
        public string Comment { get; private set; }

        public static string CommentPrefix { get; set; } = "//";
        public static string[] MultilineString2Array(string input) =>
                input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(m => TrimSingle(m))
                .Select(m => Regex.Replace(m, $"{CommentPrefix}.*", ""))
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


        internal static MnemonicInput[] Inputs { get { return InputsOK.Concat(InputsNG).ToArray(); } }
    }
}
