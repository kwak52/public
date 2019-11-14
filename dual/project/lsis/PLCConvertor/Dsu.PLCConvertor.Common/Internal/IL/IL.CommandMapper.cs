using Dsu.Common.Utilities.ExtensionMethods;
using Dsu.PLCConvertor.Common.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsu.PLCConvertor.Common.Internal
{
    /// <summary>
    /// 사용자 정의 명령어 mapping 도구
    /// </summary>
    public class UserDefinedCommandMapper
    {
        public Dictionary<string, UserDefinedILCommand> Map { get; }
        public UserDefinedCommandMapper(IEnumerable<UserDefinedILCommand> udcs)
        {
            Map = udcs.ToDictionary(udc => udc.Command);
        }

        [JsonConstructor]
        private UserDefinedCommandMapper() { }


        public static UserDefinedCommandMapper LoadFromJsonFile(string jsonFile)
            => LoadFromJsonString(File.ReadAllText(jsonFile, Encoding.GetEncoding("ks_c_5601-1987")));
        public static UserDefinedCommandMapper LoadFromJsonString(string json)
        {
            var udILs = JsonConvert.DeserializeObject<UserDefinedILCommand[]>(json);
            udILs.Iter(ud =>
            {
                ud.Arity = ud.PerInputProc.Length;
            });
            return new UserDefinedCommandMapper(udILs);
        }

        public void SaveToJsonFile(string jsonFile)
        {
            var json = JsonConvert.SerializeObject(Map.Values.ToArray(), MyJsonSerializer.JsonSettingsSimple);
            File.WriteAllText(jsonFile, json);
        }

        public UserDefinedILCommand this[string commandKey]
        {
            get { return Map.ContainsKey(commandKey) ? Map[commandKey] : null; }
            set { Map[commandKey] = value; }
        }
    }
}
