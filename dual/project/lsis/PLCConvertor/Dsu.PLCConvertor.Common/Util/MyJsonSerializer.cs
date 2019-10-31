using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsu.PLCConvertor.Common.Util
{
    public class MyJsonSerializer
    {
        public static JsonSerializerSettings JsonSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,

            // Newtonsoft.Json.JsonSerializationException: Self referencing loop detected for property ....  https://stackoverflow.com/questions/13510204/json-net-self-referencing-loop-detected
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
            PreserveReferencesHandling = PreserveReferencesHandling.All,


            Formatting = Formatting.Indented,
        };
    }
}
