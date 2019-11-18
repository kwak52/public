using Dsu.Common.Utilities.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Dsu.PLCConvertor.Common.Internal
{


    /// <summary>
    /// 옴론 project(.cxt) 파일 전체의 내용을 표현하기 위한 구조
    /// 버젼, 생성일자 등의 기본 정보와 Programs 의 주정보를 포함.
    /// </summary>
    public class CxtInfoRoot : CxtInfo
    {
        public string CXProgVer { get; private set; }
        public string WrittenByCXPVersion { get; private set; }
        public string FileType { get; private set; }
        public string Created { get; private set; }
        public string Modified { get; private set; }

        /// <summary>
        /// Project name.  CX-One 의 project tab 이름
        /// </summary>
        public string Name { get; private set; }

        public List<CxtInfoProgram> Programs { get; } = new List<CxtInfoProgram>();
        /// <summary>
        /// 마지막 Global or Local variables
        /// </summary>

        CxtInfoVariables _lastVariables;
        public List<CxtInfoGlobalVariables> GlobalVariables { get; private set; } = new List<CxtInfoGlobalVariables>();
        public override IEnumerable<CxtInfo> Children { get { return Programs.Cast<CxtInfo>().Concat(GlobalVariables); } }
        internal override void ClearMyResult() { }


        CxtInfoRoot(CxtTextBlock rootBlock)
            : base("ROOT")
        {
            CXProgVer = getValue("CXProgVer");
            WrittenByCXPVersion = getValue("WrittenByCXPVersion");
            FileType = getValue("FileType");
            Created = getValue("Created");
            Modified = getValue("Modified");
            Name = getValue("Name");

            BuildCxtInfo();

            string unQuote(string str) => str.TrimStart('"').TrimEnd('"');
            string getValue(string key) => unQuote(rootBlock[key].Value.ToString());
            string getChildValueWithKey(CxtTextBlock block, string key) => block.SubStructures.FirstOrDefault(cb => cb.Key == key).Value?.ToString();

            void BuildCxtInfo()
            {
                CxtInfoProgram program = null;
                CxtInfoSection section = null;
                CxtInfoRung rung = null;



                Stack<CxtTextBlock> stack = new Stack<CxtTextBlock>();
                stack.Push(rootBlock);

                _build();

                void _build()
                {
                    var start = stack.Peek();
                    var key = start.Key;
                    switch (key)
                    {
                        case "Program":
                            program = new CxtInfoProgram(key);
                            Programs.Add(program);
                            break;
                        case "Programs":
                            break;


                        case "Section":
                            section = new CxtInfoSection(key);
                            program.AddSection(section);
                            break;
                        case "Sections":
                            Debug.Assert(program.Sections.Count() == 0);
                            //program.Sections = new List<CxtInfoSection>();
                            break;

                        case "SL":
                            rung.ILs = CxtParser.SplitBlock(start.Value.ToString(), StringSplitOptions.RemoveEmptyEntries);
                            break;

                        case "Com": // comment
                            if (rung == null)
                                Debug.Assert( ! start.Parent.Key.StartsWith("R["));
                            else if (Cx2Xg5kOption.CopySourceComment)
                            {
                                if (start.Value == null)
                                    rung.Comment = string.Join("\r\n", start.Lines);
                                else
                                    rung.Comment = unQuote((string)start.Value);
                            }
                            break;
                        case "SecName":
                            section.Name = unQuote(start.Value.ToString());
                            break;

                        case "GlobalVariables":
                            _lastVariables = new CxtInfoGlobalVariables();
                            GlobalVariables.Add(_lastVariables as CxtInfoGlobalVariables);
                            break;

                        case "LocalVariables":
                            Debug.Assert(program.LocalVariables == null);
                            program.LocalVariables = new CxtInfoLocalVariables();
                            program.LocalVariables.Parent = program;
                            _lastVariables = program.LocalVariables;
                            break;

                        case "VariableList" when start.Parent.Key.IsOneOf("GlobalVariables", "LocalVariables"):
                            _lastVariables.VariableList = new CxtInfoVariableList(start.SubStructures[0].Lines);
                            break;

                        default:
                            if (key.StartsWith("Program["))
                            {
                                program = new CxtInfoProgram(getName());
                                Programs.Add(program);
                            }

                            else if (key.StartsWith("Sec["))
                            {
                                section = new CxtInfoSection(getName("SecName"));
                                program.AddSection(section);
                            }
                            else if (key.StartsWith("R["))
                            {
                                rung = new CxtInfoRung(key);
                                section.AddRung(rung);
                            }

                            break;

                            string getName(string searchKey="Name") => unQuote(getChildValueWithKey(start, searchKey));

                    }

                    if (start.SubStructures.Any())
                    {
                        foreach (var s in start.SubStructures)
                        {
                            stack.Push(s);
                            _build();
                        }
                    }
                    else
                        stack.Pop();
                }
            }
        }



        public static CxtInfoRoot Parse(string cxtFile)
        {
            var parser = new CxtParser(cxtFile);
            var structure = new CxtTextBlock("ROOT", "ROOT") { SubStructures = parser.BuildStructures().ToList() };

#if DEBUG
            //structure.PrintAll();
            //Trace.WriteLine($"Num. Structures:{structure.SubStructures.Count}");
            //structure.PrintCodeStuffs().ToArray();
#endif

            var cxt = new CxtInfoRoot(structure);
            return cxt;
        }
    }
}
