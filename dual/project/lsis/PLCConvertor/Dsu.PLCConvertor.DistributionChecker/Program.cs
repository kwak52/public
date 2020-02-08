using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;

namespace Dsa.LSIS.DistributionChecker
{
    /// 패키징을 위해서 MakeDistribution.bat 에서 호출 됨.
    /// DEBUG 혹은, FUTURE flag 가 정의된 채로 build 되어서 배포되는 것을 방지하기 위함
    class Program
    {
        static void Main(string[] args)
        {
            var folder = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var assemblies = new[] {
                "PLCConvertor.exe",
                "Dsu.Common.Utilities.Core.dll",
            };
            var errors = new List<string>();
            foreach ( var asmName in assemblies)
            {
                var asm = Assembly.LoadFile(Path.Combine(folder, asmName));
                foreach ( var t in new [] { "DistributionChecker.DebugEnabled" })
                {
                    var type = asm.GetType(t);
                    if (type != null)
                        errors.Add(asmName);
                }
            }
            if (errors.Any())
            {
                var lists = String.Join(", ", errors);
                System.Windows.Forms.MessageBox.Show($"{lists} built with DEBUG enabled!!!");
                Environment.Exit(1);
            }
            Environment.Exit(0);
        }
    }
}
