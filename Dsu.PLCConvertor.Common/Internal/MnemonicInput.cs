using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsu.PLCConvertor.Common
{
    public class MnemonicInput
    {
        public string Input { get; private set; }
        public string DesiredOutput { get; private set; }
        public string Comment { get; private set; }

        public static string[] MultilineString2Array(string input) =>
                input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(m => m.TrimStart(new[] { ' ', '\t' }))
                .ToArray()
                ;

        public MnemonicInput(string comment, string input)
        {
            Input = input;
            DesiredOutput = "";
            Comment = comment;
        }
        public MnemonicInput(string comment, string input, string output)
            : this(comment, input)
        {
            DesiredOutput = output;
        }



        internal static MnemonicInput[] Inputs = new MnemonicInput[]
        {
            new MnemonicInput("Basic",
                @"LD A
AND B
OR C
OUT D
",
                @"LD A
AND B
LD C
ORLD
OUT D"),
            new MnemonicInput("Basic2",
                @"LD A
AND B
LD C
ORLD
OUT D
"),
            new MnemonicInput("Basic3",
                @"LD A
LD B
ORLD
AND C
OUT D
"),

            new MnemonicInput("TR Basic1",
                @"LD A
OUT TR0
AND B
OUT O1
LD TR0
AND C
OUT O2
",
                @"LD A
MPUSH
AND B
OUT O1
MPOP
AND C
OUT O2
"),

        new MnemonicInput("TR Basic2",
            @"LD 0.00
OUT TR0
AND 0.01
OUT 110.00
LD TR0
AND 110.00
OUT 102.10
"),
        new MnemonicInput("TR Basic3",
            @"LD 0.00
LD 0.01
OUT TR0
AND 0.02
ORLD
AND 0.03
OUT 102.11
LD TR0
AND 0.04
OUT 102.12
"),
        new MnemonicInput("산전 변환 불가 case: TR Basic4",
            @"LD 0.00
LD 0.01
OUT TR0
AND 0.02
ORLD
AND 0.03
OUT 102.11
LD TR0
AND 0.04
OUT 102.12
"),
    };
    }
}
