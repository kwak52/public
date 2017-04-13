using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using Dsu.Common.Utilities.ExtensionMethods;

namespace Dsu.Common.Utilities
{
    public class ScriptingTargetProject
    {
        public static readonly LogProxy logger = LogProxy.CreateLoggerProxy(MethodBase.GetCurrentMethod().DeclaringType);
        public string FileName { get; protected set; }
        public CompilerParameters CompilerParameters { get; protected set; }

        public ScriptingTargetProject() { }
        public ScriptingTargetProject(string projName)
        {
            FileName = projName;
            CompilerParameters = new CompilerParameters()
            {
                GenerateInMemory = true,
                GenerateExecutable = false,
                WarningLevel = 4,
                //IncludeDebugInformation = true,                
            };

            ReadProjectFile(projName);
        }


        private void ReadProjectFile(string projName)
        {
            if (Path.GetExtension(projName).ToLower() != ".cssproj")
                throw new UnexpectedCaseOccurredException("Expecting ShaprStudio C# project file with extension .cssproj, but provided " + projName);

            FileName = projName;
            var doc = new XmlDocument();
            
            doc.Load(projName);

            _sourceFiles = doc.SelectNodes("//*/Sources/Source")
                .SelectEx<XmlNode, string>(n => n.Attributes["File"].Value).ToList();

            _references = doc.SelectNodes("//*/References/Reference")
                .SelectEx<XmlNode, string>(n => n.Attributes["Assembly"].Value).ToList();

            doc.SelectNodes("//*/CompilerParameters/*").ForEach<XmlNode>(n =>
            {
                var text = n.InnerText;
                switch (n.Name)
                {
                    case "GenerateInMemory": CompilerParameters.GenerateInMemory = Boolean.Parse(text);
                        break;
                    case "GenerateExecutable": CompilerParameters.GenerateExecutable = Boolean.Parse(text);
                        break;
                    case "WarningLevel": CompilerParameters.WarningLevel = Int32.Parse(text);
                        break;
                    case "IncludeDebugInformation": CompilerParameters.IncludeDebugInformation = Boolean.Parse(text);
                        break;

                    default:
                        logger.WarnFormat("Unknown compile option {0} on project file {1}", n.Name, FileName);
                        break;
                }
            });
        }

        private List<String> _sourceFiles = new List<string>();
        private List<String> _references = new List<string>();

        public string[] SourceFiles { get { return _sourceFiles.ToArray(); } }
        /// <summary>
        /// source file 들을 project 파일 위치에 대해서 상대 path 로 구함
        /// </summary>
        public string[] AbsolutePathSourceFiles { get { return SourceFiles.Select(s => FileSpec.Relative2Absolute(s, Path.GetDirectoryName(FileName))).ToArray(); }         }
        public string[] References { get { return _references.ToArray(); } } 
        public void AddReference(string reference) { _references.Add(reference); }
        public bool RemoveReference(string reference) { return _references.Remove(reference); }

        public override string ToString() { return FileName; }


        private static IEnumerable<Pair<string, object>> CreateDefaultCompilerParameters()
        {
            yield break;
            yield return new Pair<string, object>("GenerateInMemory", true);
            yield return new Pair<string, object>("GenerateExecutable", false);
            yield return new Pair<string, object>("WarningLevel", 4);
            yield return new Pair<string, object>("IncludeDebugInformation", false);
        }
        /// <summary>
        /// 단일 c# source 를 지정하던 legacy version 을 C# project 를 사용하는 형태로 변환한다.
        /// a.cs 에 해당하는 a.cssproj 파일을 생성하고, 그 이름을 반환한다.
        /// </summary>
        /// <param name="soureFileName"></param>
        /// <param name="references"></param>
        /// <returns></returns>
        public static string ConvertLegacySingleFileScript(string soureFileName, IEnumerable<string> references)
        {
            var projFile = Path.ChangeExtension(soureFileName, ".cssproj");
            var relativeSourceFile = Path.GetFileName(soureFileName);       // 현재 project 와 single source *.cs 는 동일 위치에 존재한다.
            return CreateProjectFile(projFile, new[] { relativeSourceFile }, references, CreateDefaultCompilerParameters());
        }

        public static string CreateProjectFile(string projFileName, IEnumerable<string> sourceFileNames, IEnumerable<string> references)
        {
            return CreateProjectFile(projFileName, sourceFileNames, references, CreateDefaultCompilerParameters());
        }
        public static string CreateProjectFile(string projFileName, IEnumerable<string> sourceFileNames, IEnumerable<string> references, IEnumerable<Pair<string, object>> compilerParameters)
        {
            var project = new XElement("Project",
                new[]
                {
                    new XElement("Sources",
                        from f in sourceFileNames select new XElement("Source", new XAttribute("File", f))),
                    new XElement("References",
                        from r in references select new XElement("Reference", new XAttribute("Assembly", r))),
                    new XElement("CompilerParameters",
                        from pr in compilerParameters select new XElement(pr.First, new XText(pr.Second.ToString()))),
                });

            new XDocument(project).Save(projFileName);
            return projFileName;            
        }
    }


}
