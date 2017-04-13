using ControlLogixNET;
using ControlLogixNET.LogixType;
using Dsu.Common.Utilities;

namespace Dsu.PLC.AB
{
    internal static class ExtensionAB
    {
        public static object Value(this LogixTag tag)
        {
            switch(tag.LogixType)
            {
                case LogixTypes.Bool:
                    return ((LogixBOOL)tag).Value;
                case LogixTypes.DInt:
                    return ((LogixDINT)tag).Value;
                case LogixTypes.Int:
                    return ((LogixINT)tag).Value;
                case LogixTypes.LInt:
                    return ((LogixLINT)tag).Value;
                case LogixTypes.Real:
                    return ((LogixREAL)tag).Value;
                case LogixTypes.SInt:
                    return ((LogixSINT)tag).Value;
                case LogixTypes.String:
                    return (LogixSTRING)tag;
                //case LogixTypes.Timer:
                //    //Timers again are like the CONTROL and COUNTER types
                //    Console.WriteLine("Timer.ACC value is: " + ((LogixTIMER)tag).ACC.ToString());
                //    break;
                default:
                    throw new UnexpectedCaseOccurredException("Unknown logix tag type: " + tag.LogixType);
            }
        }
    }
}
