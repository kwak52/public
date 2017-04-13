using Dsu.Common.Utilities.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsu.Driver.Util.Emergency
{
    public interface ISignal
    {
        string Message { get; set; }
    }

    public interface IIOSignal : ISignal
    {
        string Address { get; set; }
        string Comment { get; set; }
        object Value { get; set; }
    }
    public interface IPlcSignal : IIOSignal { }
    public interface IUDIOSignal : IIOSignal { }

    public abstract class SignalBase
    {
        protected SignalBase(string address, string deviceId, string type, string message, string comment="")
        {
            Type = type;
            DeviceId = deviceId;
            Address = address;
            Message = message;
            Comment = comment;
        }
        public string Address { get; set; }
        public string Message { get; set; }
        public string Comment { get; set; }
        public object Value { get; set; }

        /// IO type : {PLC, DI, DO}
        public string Type { get; protected set; }
        public string DeviceId { get; protected set; }
    }

    public class ParsedSignal : SignalBase
    {
        public SignalEnum SignalEnum { get; private set; }
        public ParsedSignal(SignalEnum signalEnum, string address, string deviceId, string type, string message, string comment="")
            : base(address, deviceId, type, message, comment)
        {
            SignalEnum = signalEnum;
        }
    }

    public class PlcSignal : SignalBase, IPlcSignal
    {
        public PlcSignal(string address, string deviceId, object value)
            : base(address, deviceId, "PLC", "") { Value = value; }
    }


    public class UDIOSignal : SignalBase, IUDIOSignal
    {
        public UDIOSignal(string type, string index, string deviceId, string message="")
            : base(index, deviceId, type, message)
        {
            Debug.Assert(type.IsOneOf("DI", "DO"));
        }

        public UDIOSignal(string address, bool hasValue=true)
            : base(address, "unknown-device-id", "unknown-type", "")
        {
            var tokens = address.Split(';').ToArray();
            Debug.Assert(tokens[0].IsOneOf("DI", "DO"));
            var i = 0;
            Type = tokens[i++];
            Index = Int32.Parse(tokens[i++]);
            if (hasValue)
                State = Boolean.Parse(tokens[i++]);
            DeviceId = tokens[i++];
        }

        public int Index { get; private set; }
        public bool State { get; private set; }
    }

    public class ExtraSignal : ISignal
    {
        public SignalEnum Enum { get; set; }
        public string Message { get; set; }

        public ExtraSignal(SignalEnum sigEnum, string message)
        {
            Enum = sigEnum;
            Message = message;
        }
    }

    public class FilteredSignal
    {
        public SignalEnum Enum {get; set; }
        public bool Value { get; set; }
        public FilteredSignal(SignalEnum sigEnum, bool value)
        {
            Enum = sigEnum;
            Value = value;
        }
    }

    public enum SignalEnum
    {
        Undefined = 0,

        /*
         * UDIO signals
         */
        UEmergency, 
        USpare1,    
        USpare2,
        UStart1,
        UStart2,
        UDoor1,
        UDoor2,
        UDoor3,
        UDoor4,
        UDoorFL,
        UDoorFR,    
        UBreakZ,
        UBreakTilt,
        UOriginSensor,

        UDoorLock1,
        UDoorLock2,
        UDoorLock3,
        UDoorLock4,

        UPower1,
        UPower2,
        UPower3,
        UPower4,
        UPower5,

        UColorPower1,
        UColorPower2,

        UColorBit1,
        UColorBit2,
        UColorBit3,
        UColorBit4,

        ULampRed,
        ULampYellow,
        ULampGreen,
        ULampBuzzer,


        UPart7,
        UPart8,

        /*
         * Gamma CVT, only
         */
        UTiltPusherAdvanced,
        UTiltPusherReturned,

        UGcvtPartSensorTurbine,
        UGcvtPartSensorPrimary,
        UGcvtPartSensorOutput,

        UGcvtWheelSensorBit1,
        UGcvtWheelSensorBit2,
        UGcvtWheelSensorBit3,

        UEmergencyOut,

        UGcvtMotorSlideAdvanced,
        UGcvtMotorSlideReturned,

        UGcvtAreaSensor,

        UGcvtZAxisHomeMoveCompleted,
        UGcvtYAxisHomeMoveCompleted,

        UMotorMC,
        UTiltMC,
        UTiltPusher,
        UMotorSlidePusher,

        UZAxisHomeMovePulse,
        UYAxisHomeMovePulse,



        /*
         * PLC's
         */

        PMessageM1901,
        PMessageM1902,
        PMessageM1903,
        PMessageM1904,
        PMessageM1905,
        PMessageM1906,
        PMessageM1907,
        PMessageM1908,
        PMessageM1909,
        PMessageM1910,
        PMessageM1911,
        PMessageM1912,
        PMessageM1913,
        PMessageM1914,
        PMessageM1915,
        PMessageM1916,
        PMessageM1917,
        PMessageM1918,
        PMessageM1919,
        PMessageM1920,
        PMessageM1921,
        PMessageM1922,
        PMessageM1923,
        PMessageM1924,
        PMessageM1925,
        PMessageM1926,
        PMessageM1927,
        PMessageM1928,
        PMessageM1929,
        PMessageM1930,
        PMessageM1931,
        PMessageM1932,
        PMessageM1933,
        PMessageM1934,
        PMessageM1935,
        PMessageM1936,
        PMessageM1937,
        PMessageM1938,
        PMessageM1939,
        PMessageM1940,
        PMessageM1941,
        PMessageM1942,
        PMessageM1943,
        PMessageM1944,
        PMessageM1945,
        PMessageM1946,
        PMessageM1947,
        PMessageM1948,
        PMessageM1949,
        PMessageM1950,
        PMessageM1951,
        PMessageM1952,
        PMessageM1953,
        PMessageM1954,
        PMessageM1955,
        PMessageM1956,
        PMessageM1957,
        PMessageM1958,
        PMessageM1959,
        PMessageM1960,
        PMessageM1961,
        PMessageM1962,
        PMessageM1963,
        PMessageM1964,
        PMessageM1965,
        PMessageM1966,
        PMessageM1967,
        PMessageM1968,
        PMessageM1969,
        PMessageM1970,
        PMessageM1971,
        PMessageM1972,
        PMessageM1973,
        PMessageM1974,
        PMessageM1975,
        PMessageM1976,
        PMessageM1977,
        PMessageM1978,
        PMessageM1979,
        PMessageM1980,
        PMessageM1981,
        PMessageM1982,
        PMessageM1983,
        PMessageM1984,
        PMessageM1985,
        PMessageM1986,
        PMessageM1987,
        PMessageM1988,
        PMessageM1989,
        PMessageM1990,
        PMessageM1991,
        PMessageM1992,
        PMessageM1993,
        PMessageM1994,
        PMessageM1995,
        PMessageM1996,
        PMessageM1997,
        PMessageM1998,
        PMessageM1999,
        PMessageM2900,
        PMessageM2901,
        PMessageM2902,
        PMessageM2903,
        PMessageM2904,
        PMessageM2905,
        PMessageM2906,
        PMessageM2907,
        PMessageM2908,
        PMessageM2909,

        /*
         * Arbitararily generated enums
         */
        XEnvironmentTemperature,
        XEnvironmentHumidity,
        XNoPartsLoaded,
        XPartMismatched,
        XUDIOConnected,
        XUDIODisconnected,
        XUDIODisconnectedUnrecoverbly,
        XException,
        XExceptionWithCode,
    }
}
