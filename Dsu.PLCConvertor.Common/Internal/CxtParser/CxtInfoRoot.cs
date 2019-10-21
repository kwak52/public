using Dsu.Common.Utilities.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Dsu.PLCConvertor.Common.Internal
{
    public abstract class CxtInfo
    {
        public string Key { get; private set; }
        protected CxtInfo(string key)
        {
            Key = key;
        }

        public abstract IEnumerable<CxtInfo> Children { get; }

        public IEnumerable<T> EnumerateType<T>()
        {
            return _enumerate(this).Cast<T>();

            IEnumerable<CxtInfo> _enumerate(CxtInfo start)
            {
                if (start.GetType() == typeof(T))
                    yield return start;

                var xs = start.Children.SelectMany(ch => _enumerate(ch)).ToArray();
                foreach (var x in xs)
                    yield return x;
            }
        }
    }


    public class CxtInfoRung : CxtInfo
    {
        public string Name { get; private set; }
        public string Comment { get; internal set; }
        public string[] ILs { get; internal set; }
        internal CxtInfoRung(string name)
            : base("Rung")
        {
            Name = name;
        }
        public override IEnumerable<CxtInfo> Children { get { return Enumerable.Empty<CxtInfo>(); } }
    }


    public class CxtInfoSection : CxtInfo
    {
        public string Name { get; internal set; }
        public List<CxtInfoRung> Rungs { get; } = new List<CxtInfoRung>();
        internal CxtInfoSection(string name)
            : base("Section")
        {
            Name = name;
        }
        public override IEnumerable<CxtInfo> Children { get { return Rungs; } }

        public IEnumerable<string> Convert(PLCVendor targetType)
        {
#if DEBUG
            Global.Logger.Info($"SecName={Name}");
            this.EnumerateType<CxtInfoRung>()
                .Where(rung => rung.ILs.NonNullAny())
                .Where(rung => !rung.ILs[0].StartsWith("END"))
                .Iter(rung => {
                    rung.ILs.Iter(il => Global.Logger.Debug($"{il}"));
                });
#endif

            var secConversion =
                this.EnumerateType<CxtInfoRung>()
                    .Where(rung => rung.ILs.NonNullAny())
                    .Where(rung => !rung.ILs[0].StartsWith("END"))
                    .SelectMany(rung =>
                    {
                        var ils = rung.ILs.Where(il => !il.StartsWith("'") && !il.StartsWith("//"));
                        return Rung2ILConvertor.ConvertFromMnemonics(ils, PLCVendor.Omron, PLCVendor.LSIS);
                    });

            var xs = secConversion.Select((line, n) => $"{n}\t{line}").ToArray();
            if (xs.NonNullAny())
            {

                switch (targetType)
                {
                    case PLCVendor.LSIS:
                        yield return $"[PROGRAM FILE] {Name}";

                        foreach (var x in xs)
                            yield return x;

                        yield return "[PROGRAM FILE END]";
                        break;

                    default:
                        Debug.Assert(false);
                        break;
                }

            }
        }
    }


    public class CxtInfoProgram : CxtInfo
    {
        public string Name { get; private set; }
        public List<CxtInfoSection> Sections { get; internal set; } = new List<CxtInfoSection>();
        internal CxtInfoProgram(string name)
            : base("Program")
        {
            Name = name;
        }
        public override IEnumerable<CxtInfo> Children { get { return Sections; } }
    }


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
        public override IEnumerable<CxtInfo> Children { get { return Programs; } }


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

            string getValue(string key) => rootBlock[key].Value.ToString().TrimStart('"').TrimEnd('"');

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
                            program.Sections.Add(section);
                            break;
                        case "Sections":
                            program.Sections = new List<CxtInfoSection>();
                            break;

                        //case "ProgramData":
                        //    rung =  new CxtInfoRungs()
                        //    section.Rungs.Add(;
                        //    break;

                        case "SL" when start.Lines != null:
                            rung.ILs = start.Lines.ToArray();
                            break;

                        case "Com":
                            break;
                        case "SecName":
                            section.Name = $"{start.Value}";
                            break;

                        default:
                            if (key.StartsWith("Program["))
                            {
                                program = new CxtInfoProgram(key);
                                Programs.Add(program);
                            }

                            else if (key.StartsWith("Sec["))
                            {
                                section = new CxtInfoSection(key);
                                program.Sections.Add(section);
                            }
                            else if (key.StartsWith("R["))
                            {
                                rung = new CxtInfoRung(key);
                                section.Rungs.Add(rung);
                            }

                            break;

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


            structure.PrintAll();
            Trace.WriteLine($"Num. Structures:{structure.SubStructures.Count}");
            structure.PrintCodeStuffs().ToArray();


            var cxt = new CxtInfoRoot(structure);
            return cxt;
        }
    }
}
