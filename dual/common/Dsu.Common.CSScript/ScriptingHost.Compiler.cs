using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using csscript;
//using csscript;
using Microsoft.CSharp;
using Dsu.Common.Utilities.ExtensionMethods;

namespace Dsu.Common.Utilities
{
    partial class ScriptingHost
    {
        // http://www.codeproject.com/Tips/715891/Compiling-Csharp-Code-at-Runtime
        protected CSharpCodeProvider _provider;

        /// <summary> Compiled assembly using scripts </summary>
        public Assembly Assembly { get; private set; }

        public virtual string[] DefaultReferences { get { return GetDefaultReferences().ToArray(); } }
        public static IEnumerable<string> GetDefaultReferences()
        {
            return new[]
                {
                    "System.dll", "System.Core.dll", "System.Drawing.dll",
                    "System.Windows.Forms.dll", "System.Xml.dll", "System.Xml.Linq.dll",
                    "System.Threading.dll",
                    "Microsoft.CSharp.dll",
                    "log4net.dll",
					"Dsu.Common.Interfaces.dll", "Dsu.Common.Resources.dll", "Dsu.Common.Utilities.dll",
                };
        }

        private void InitializeCompiler()
        {
            /*
             * provider 를 매번 새로 생성하면, 두번째부터 컴파일 오류가 발생한다. (원인은 모름)
             */
            if (_provider == null)
                _provider = new CSharpCodeProvider();

            ScriptingTargetProject = new ScriptingTargetProject(ScriptProjectFileName);
        }

        CompilerParameters CompilerParameters { get { return ScriptingTargetProject.CompilerParameters; } }
        protected virtual bool AssemblyLoading()
        {
            /*
             * Compile 이전에 code 에서 필요로 하는 reference 가 먼저 정의되어야 한다.
             */

            /*
             * Add CSharpSimpleScripting.exe as a reference to Scripts.dll to expose interfaces
             * http://headsigned.com/article/csharp-scripting-example-using-csharpcodeprovider
             */
            CompilerParameters.ReferencedAssemblies.Add(Assembly.GetEntryAssembly().Location);
            ScriptingTargetProject.References.ForEach(r => CompilerParameters.ReferencedAssemblies.Add(r));

            return true;
        }


        /// <summary>
        /// reference dll 목록이 변경될 때에 수행할 작업 지정
        /// </summary>
        public Action ActionReferenceListChanged { get; set; }

        /// <summary> Delegate to CodeDomProvider.CompileAssemblyFrom{File,Source} </summary>
        private delegate CompilerResults CompileAssemblyDelegate(CompilerParameters options, params string[] fileNamesOrSnippets);

        private Assembly CompileAssembly(CompileAssemblyDelegate compiler, params string[] snippets)
        {
            using (new CwdChanger(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)))
            {
                CompilerResults results = compiler(CompilerParameters, snippets);
                bool hasErrors = results.Errors.HasErrors;
                bool hasWarnings = results.Errors.HasWarnings;
                string errorMessage = null;

                if (hasErrors || hasWarnings)
                {
                    errorMessage = String.Join("\r\n", results.Errors.SelectEx<CompilerError, string>(er => er.ToString()));
                    if (hasErrors)
                        throw new CompilerException(errorMessage);

                    /* Error 는 없으나, warning 은 존재하는 경우 */
                    logger.EnableAll();
                    results.Errors.ForEach<CompilerError>(er => CommonApplication.Logger.Warn(er.ToString()));
                }

                return results.CompiledAssembly;
            }
        }

        private Assembly CompileAssemblyFromSource(params string[] snippets)
        {
            return CompileAssembly(_provider.CompileAssemblyFromSource, snippets);
        }
        private Assembly CompileAssemblyFromFile(params string[] files)
        {
            return CompileAssembly(_provider.CompileAssemblyFromFile, files);
        }
    }
}
