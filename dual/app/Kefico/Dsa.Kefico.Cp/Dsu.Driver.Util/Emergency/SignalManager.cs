using Dsu.Common.Utilities.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Xml.Linq;

namespace Dsu.Driver.Util.Emergency
{
    public class SignalManager
    {
        public static bool IsEmergency { get; protected set; }
        public static SignalManager TheSignalManager;
        /// { DI/DO, INDEX, DEVICEID } => SignalEnum map.  e.g "DI;1;UDIO_CPU1" => SignalEnum.UEmergency
        public static Dictionary<string, ParsedSignal> Dictionary = new Dictionary<string, ParsedSignal>();
        protected string FindKey(SignalEnum signal) => Dictionary.First(pr => pr.Value.SignalEnum == signal).Key;
        public static Subject<ISignal> RawSignalSubject = new Subject<ISignal>();
        public static Subject<FilteredSignal> FilteredSignalSubject = new Subject<FilteredSignal>();

        public SignalManager(string configXmlPath)
        {
            LoadFromXml(configXmlPath);
            TheSignalManager = this;
        }

        public void LoadFromXml(string configXmlPath)
        {
            XDocument doc = XDocument.Load(configXmlPath);
            var signals = from signalXml in doc.Descendants("Signals").Elements("Signal")
                          let att = signalXml.Attributes().ToDictionary(at => at.Name.ToString(), at => at.Value)
                          let name = att["Name"]
                          let signal = (SignalEnum)Enum.Parse(typeof(SignalEnum), name)
                          let address = att["Address"]
                          let message = att["Message"]
                          let comment = att["Comment"]
                          let type = att["Type"]
                          let deviceId = att["DeviceId"]
                          select new ParsedSignal(signal, address, deviceId, type, message, comment)
                          ;
            signals.ForEach(s =>
            {
                var key = s.Type == "PLC" ? $"{s.Address};{s.DeviceId}" : $"{s.Type};{s.Address};{s.DeviceId}";
                Dictionary.Add(key, s);

            });
        }

        public static ParsedSignal GetParsedSignal(SignalEnum sigEnum) => Dictionary.Values.FirstOrDefault(ps => ps.SignalEnum == sigEnum);
        public static ParsedSignal GetParsedSignal(string addressValue)
        {
            var addressTokens = 
                    addressValue
                    .Split(';')
                    .Select((t, i) => i == 2 ? "" : t)
                    .Where(t => t.Any())
                    ;
            var key = String.Join(";", addressTokens);
            return Dictionary.ContainsKey(key) ? Dictionary[key] : null;
        }
        public static SignalEnum GetSignalEnum(string addressValue)
        {
            var parsed = GetParsedSignal(addressValue);
            return parsed == null ? SignalEnum.Undefined : parsed.SignalEnum;
        }



        public Tuple<ParsedSignal, bool> GetSignalDetails(IIOSignal s, string addressValue)
        {
            var udioSignal = s as UDIOSignal;
            var plcSignal = s as PlcSignal;
            if (udioSignal != null)
            {
                var signal = GetParsedSignal(addressValue);
                var value = Boolean.Parse(addressValue.Split(';')[2]);
                return Tuple.Create(signal, value);
            }
            else if (plcSignal != null)
            {
                var key = $"{plcSignal.Address};{plcSignal.DeviceId}";
                var signal = GetParsedSignal(key);
                return Tuple.Create(signal, (int)s.Value != 0);
            }
            else
                throw ExceptionWithCode.Create(ErrorCodes.APP_InternalError, "Undefined signal type.");
        }

        public static bool IsUDIOSignal(SignalEnum signal) => signal.ToString().StartsWith("U");
        public static bool IsPLCSignal(SignalEnum signal) => signal.ToString().StartsWith("P");
    }
}
