using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Dsu.Common.Utilities.ExtensionMethods;
using log4net;
using log4net.Appender;

namespace Dsu.Common.Utilities
{
    public static class Log4NetWrapper
    {
        #region Extension methods
        [Conditional("DEBUG")]
        public static void DEBUG(this ILog logger, object message, Exception exception)
        {
            if (logger.IsDebugEnabled) logger.Debug(message, exception);
        }

        [Conditional("DEBUG")]
        public static void DEBUG(this ILog logger, object message)
        {
            if (logger.IsDebugEnabled) logger.Debug(message);
        }

        [Conditional("DEBUG")]
        public static void DEBUGFORMAT(this ILog logger, string format, params object[] args)
        {
            if (logger.IsDebugEnabled) logger.DebugFormat(format, args);
        }

        [Conditional("DEBUG")]
        public static void DEBUGFORMAT(this ILog logger, string format, object arg0)
        {
            if (logger.IsDebugEnabled) logger.DebugFormat(format, arg0);
        }

        [Conditional("DEBUG")]
        public static void DEBUGFORMAT(this ILog logger, string format, object arg0, object arg1)
        {
            if (logger.IsDebugEnabled) logger.DebugFormat(format, arg0, arg1);
        }

        [Conditional("DEBUG")]
        public static void DEBUGFORMAT(this ILog logger, string format, object arg0, object arg1, object arg2)
        {
            if (logger.IsDebugEnabled) logger.DebugFormat(format, arg0, arg1, arg2);
        }

        [Conditional("DEBUG")]
        public static void DEBUGFORMAT(this ILog logger, IFormatProvider provider, string format, params object[] args)
        {
            if (logger.IsDebugEnabled) logger.DebugFormat(provider, format, args);
        }
        #endregion



		/// <summary>
		/// 
		/// </summary>
		/// <param name="mainForm"></param>
		/// <param name="configFile">xml log configuration file</param>
		/// <param name="logFileName">real log file name to be generated.</param>
        public static void Install(IAppender mainForm, string configFile, string logFileName)
        {
            // http://stackoverflow.com/questions/2815940/where-will-log4net-create-this-log-file
            // see log4netXXXX.xml configuration file
            var appName = Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location);
            log4net.GlobalContext.Properties["LogFileName"] = logFileName.IsNullOrEmpty() ? Path.Combine(CommonApplication.GetProfilePath(), appName) : logFileName;

            if ( File.Exists(configFile) )
            {
                log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(configFile));
                ((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository()).Root.AddAppender(mainForm);
            }
            else
                MessageBox.Show(String.Format("Failed to load configuration file {0}.\r\nLog message will not be available.", configFile), appName);
        }

        /// <summary>
        /// 분석 대상 assembly 의 type 을 검사하여, "logger" 라는 이름의 static 멤버를 찾고, 
        /// 사전에 해당 객체를 생성해서 등록해 둔다.
        /// </summary>
        /// <param name="mainForm"></param>
        /// <param name="configFile">e.g "log4net.xml"</param>
        /// <param name="assemblies">Logger 를 포함하는 분석 대상 assemblies</param>
        /// <param name="logFileName">logFileName</param>
        /// <param name="staticLoggerMemberName">e.g "logger"</param>
        public static void Install(IAppender mainForm, string configFile, IEnumerable<Assembly> assemblies, string logFileName=null, string staticLoggerMemberName = "logger")
        {
            Install(mainForm, configFile, logFileName);

            List<Type> types = new List<Type>();
            foreach (var assembly in assemblies)
            {
                foreach (Type t_ in assembly.GetTypes())
                {
                    Type t = t_.DeclaringType ?? t_;
                    Utilities.DEBUG.WriteLine("Inspecting " + t.FullName);
                    MemberInfo[] mis = t.GetMember(staticLoggerMemberName, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
                    foreach (var mi in mis)
                    {
                        Type candidateType = mi.DeclaringType.DeclaringType ?? mi.DeclaringType;
                        if (!types.Contains(candidateType))
                        {
                            Utilities.DEBUG.WriteLine("===>" + candidateType);
                            types.Add(candidateType);
                        }
                    }
                }
            }

            foreach (var type in types)
                LogProxy.CreateLoggerProxy(type);            
        }
    }
}
