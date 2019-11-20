using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dsu.Common.Utilities.Core.ExtensionMethods;
using Dsu.Common.Utilities.ExtensionMethods;

namespace Dsu.PLCConvertor.Common.Internal
{
    /// <summary>
    /// PLC 변환에 사용될 parameters
    /// </summary>
    public class ConvertParams
    {
        /// <summary>
        /// 원본 PLC type
        /// </summary>
        public PLCVendor SourceType { get; private set; }
        /// <summary>
        /// target PLC type
        /// </summary>
        public PLCVendor TargetType { get; private set; }
        /// <summary>
        /// Source PLC 의 시작 step.  Message txt file 에 source step 을 기록하기 위해서 사용
        /// 직접 file 에 write 하지 않는 이유는 추후 병렬화 등을 고려해서 임
        /// </summary>
        public int SourceStartStep { get; set; }
        /// <summary>
        /// Target PLC 의 시작 step.  Message txt file 에 source step 을 기록하기 위해서 사용
        /// 직접 file 에 write 하지 않는 이유는 추후 병렬화 등을 고려해서 임
        /// </summary>
        public int TargetStartStep { get; set; }


        /// <summary>
        /// 변환에 실패한 rung 만을 따로 모아서 review 용 project 생성하기 위한 용도
        /// </summary>
        public CxtGenerator ReviewProjectGenerator;


        /// <summary>
        /// Source 측 PLC 변수.  Device comment 및 type 등의 정보가 담겨 있다.
        /// </summary>
        public static Dictionary<string, PLCVariable> SourceVariableMap { get; internal set; } = new Dictionary<string, PLCVariable>();


        /// Program 의 local varaible map
        public static Dictionary<CxtInfoProgram, Dictionary<string, PLCVariable>> ProgramLocalVariableMap { get; internal set; } = new Dictionary<CxtInfoProgram, Dictionary<string, PLCVariable>>();

        /// <summary>
        /// 변환에 실제 사용된 source PLC 의 device address 들
        /// </summary>
        public static Dictionary<string, PLCVariable> UsedSourceDevices { get; } = new Dictionary<string, PLCVariable>();


        /// <summary>
        /// 변수 정의를 주어진 program 에서 우선 검색하고, 없으면 global 에서 검색한다. 
        /// </summary>
        /// <param name="program"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static PLCVariable SearchVariable(CxtInfoProgram program, string key)
        {
            if (ProgramLocalVariableMap[program].ContainsKey(key))
                return ProgramLocalVariableMap[program][key];

            if (SourceVariableMap.ContainsKey(key))
                return SourceVariableMap[key];

            return null;
        }
        public static PLCVariable SearchVariable(string key)
        {
            if (SourceVariableMap.ContainsKey(key))
                return SourceVariableMap[key];

            foreach (var lv in ProgramLocalVariableMap.Values)
            {
                if (lv.ContainsKey(key))
                    return lv[key];
            }

            return null;
        }


        /// <summary>
        /// CXT 파일 내의 global/local 변수 정의부 추출
        /// </summary>
        /// <param name="cxt"></param>
        public void BuildSymbolTables(CxtInfoRoot cxt)
        {
            // global 변수 선언부
            var globals = cxt.EnumerateType<CxtInfoGlobalVariables>().ToArray();

            var variables = globals.SelectMany(g => g.VariableList.Variables);
            variables.Iter(v =>
            {
                if (!SourceVariableMap.ContainsKey(v.Device))
                    SourceVariableMap.Add(v.Device, v);
            });

            foreach (var v in SourceVariableMap.Values.ToArray())
            {
                if (v.Name.NonNullAny() && !SourceVariableMap.ContainsKey(v.Name))
                    SourceVariableMap.Add(v.Name, v);
            }


            var programs = cxt.EnumerateType<CxtInfoProgram>().ToArray();

            // program 별 local 변수 쌍을 생성
            var query =
                from prog in programs
                let localVars = prog.EnumerateType<CxtInfoLocalVariables>()
                where localVars.Any()
                let localDic =
                    localVars.First().VariableList.Variables
                    .ToDictionary(v => v.Name)
                select (prog, localDic)
                ;

            ProgramLocalVariableMap = query.ToDictionary(q => q.prog, q => q.localDic);


            // 디버깅용 파일 생성 : 사용된 변수를 출력
            using (StreamWriter msgStream = new StreamWriter("dic.txt", false))
            {
                msgStream.WriteLine("==================Global Dictionary");
                foreach (var kv in SourceVariableMap)
                {
                    var k = kv.Key;
                    var v = kv.Value;
                    msgStream.WriteLine($"{k}\t{v.Name}:{v.Device}:{v.Comment}:{v.Type}:{v.Variable}");
                }

                foreach (var ld in ProgramLocalVariableMap)
                {
                    msgStream.WriteLine($"=================={ld.Key.Name} Local Dictionary");
                    foreach (var kv in ld.Value)
                    {
                        var k = kv.Key;
                        var v = kv.Value;
                        msgStream.WriteLine($"{k}\t{v.Name}:{v.Device}:{v.Comment}:{v.Type}:{v.Variable}");
                    }

                }

            }
        }

        /// <summary>
        /// 하나의 section/program 에 대한 변환이 종료되면, source/target 의 시작 step 을 reset 함
        /// </summary>
        public void ResetStartStep()
        {
            SourceStartStep = 0;
            TargetStartStep = 0;
        }

        /// <summary>
        /// 강제로 section 에 의해서 구분할 지의 여부.
        /// </summary>
        public bool SplitBySection { get => Cx2Xg5kOption.SplitBySection; set { Cx2Xg5kOption.SplitBySection = value; } }

        public ConvertParams(PLCVendor sourceType, PLCVendor targetType, int soruceStartStep = 0, int targetStartStep = 0)
        {
            SourceType = sourceType;
            TargetType = targetType;
            SourceStartStep = soruceStartStep;
            TargetStartStep = targetStartStep;
            ReviewProjectGenerator = new CxtGenerator(this);
        }

        public static void Reset()
        {
            SourceVariableMap.Clear();
            ProgramLocalVariableMap.Clear();
            UsedSourceDevices.Clear();
        }
    }
}
