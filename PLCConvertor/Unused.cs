using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCConvertor
{
#if false
    class FormPLCConverter
    {
        void TestConversion()
        {
            var cvtParam = new ConvertParams(PLCVendor.Omron, PLCVendor.LSIS);
            var inputs = MnemonicInput.Inputs[0].Input.SplitByLines();
            var rung = Rung.CreateRung(inputs, "TestRung", cvtParam);
            var graph = rung.GraphViz();
            var _pictureBox = new PictureBox() { Image = graph, Dock = DockStyle.Fill };
            var _formGraphviz = new Form() { Size = new Size(800, 500) };
            _formGraphviz.Controls.Add(_pictureBox);
            _formGraphviz.Show();
        }



        // 사용자로부터 추가적인 symbol table 정보를 입력받는다.
        DialogResult acceptSymbolsByUserPaste()
        {
            var form = new FormSymbolPaste();
            var dialogResult = form.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                var map = ConvertParams.SourceVariableMap;
                form.SymbolTableText
                    .SplitByLines(StringSplitOptions.RemoveEmptyEntries)
                    .Select(line => generatePlcVariable(line))
                    .Iter(v => {
                        if (map.ContainsKey(v.Device))
                        {
                            var existingV = map[v.Device];
                            if (v.Name.NonNullAny() && existingV.Name.IsNullOrEmpty())
                                existingV.Name = v.Name;
                            if (v.Variable.NonNullAny() && existingV.Variable.IsNullOrEmpty())
                                existingV.Variable = v.Variable;
                            if (v.Comment.NonNullAny() && existingV.Comment.IsNullOrEmpty())
                                existingV.Comment = v.Comment;
                        }
                        else
                            map.Add(v.Device, v);
                    });
            }
            return dialogResult;

            // <TAB> 에 의해 구분되는 symbol table 의 하나의 line 을 분석하여 PLCVariable 로 반환
            // line 구조 : 이름 <TAB> 데이터type <TAB> address <TAB> 주석
            PLCVariable generatePlcVariable(string line)
            {
                var t = line.Split('\t').ToArray();
                var name = t[0];
                var typeStr = t[1];
                var device = t[2];  // address
                var comment = t[3];

                var type =
                        (PLCVariable.DeviceType)Enum.Parse(
                            typeof(PLCVariable.DeviceType), typeStr.Replace(" ", "_"), true);

                return new PLCVariable(name, device, type, comment, "");
            }
        }
    }
#endif
}
