using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.CSharp;

namespace Dsu.Common.Utilities
{
    /// <summary>
    /// Dynamic Code Integration with CodeDom
    /// http://www.codeproject.com/Articles/26312/Dynamic-Code-Integration-with-CodeDom
    /// see UnitTestCodeDom.cs
    /// </summary>
    public class CodeDOMParser : IDisposable
    {
        private CodeDomProvider _codeDomProvider = new CSharpCodeProvider();

        public CompilerParameters CompilerParameters { get { return _compilerParameters; } set { _compilerParameters = value; } }
        private CompilerParameters _compilerParameters = new CompilerParameters();

        public bool GenerateExecutable { get { return CompilerParameters.GenerateExecutable; } set { CompilerParameters.GenerateExecutable = value; } }

        /// <summary>
        /// dll references.  e.g "System.dll", "System.Windows.Form.dll", ...
        /// </summary>
        public IEnumerable<string> References { get { return _references; } 
            set
            {
                if (value == null || value.Count() == 0)
                {
                    CompilerParameters.ReferencedAssemblies.Clear();
                    _references.Clear();
                }
                else
                {
                    _references = value.ToList();
                    foreach (var reference in _references)
                        CompilerParameters.ReferencedAssemblies.Add(reference);                    
                }
            }
        }
        private List<string> _references = new List<string>();       // dll references


        public Assembly CompileCode(string code)
        {
            // Invoke compilation.
            CompilerResults cr = _codeDomProvider.CompileAssemblyFromSource(CompilerParameters, code);
            return cr.CompiledAssembly;
        }

        public Assembly CompileFiles(IEnumerable<string> files)
        {
            // Invoke compilation.
            CompilerResults cr = _codeDomProvider.CompileAssemblyFromFile(CompilerParameters, files.ToArray());
            return cr.CompiledAssembly;
        }

        public void Dispose()
        {
            _codeDomProvider = null;
            _compilerParameters = null;
        }
    }
}
