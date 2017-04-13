using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Win32;
using Dsu.Common.Utilities.Exceptions;
using Dsu.Common.Utilities.ExtensionMethods;

namespace Dsu.Common.Utilities
{
    /// <summary> Registry serialize 기능을 사용하려는 class 가 구현해야 할 interface </summary>
    public interface IRegistrySerializable
    {
        
    }


    /// <summary>
    /// Configuration 객체에 대한 registry serialization
    /// </summary>
    public class RegistrySerializer
    {
        public static readonly LogProxy logger = LogProxy.CreateLoggerProxy(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 주어진 객체의 property 의 값을 읽어 온다.
        /// </summary>
        /// http://stackoverflow.com/questions/1196991/get-property-value-from-string-using-reflection-in-c-sharp
        internal class PropertyValueExtractor
        {
            public static object GetPropValue(object obj, string name)
            {
                foreach (String part in name.Split('.'))
                {
                    if (obj == null) { return null; }

                    Type type = obj.GetType();
                    PropertyInfo info = type.GetProperty(part);
                    if (info == null) { return null; }

                    obj = info.GetValue(obj, null);
                }
                return obj;
            }

            public static T GetPropValue<T>(object obj, string name)
            {
                Object retval = GetPropValue(obj, name);
                if (retval == null) { return default(T); }

                // throws InvalidCastException if types are incompatible
                return (T)retval;
            }
        }

        private static IEnumerable<PropertyInfo> GetPropertyInfos(IRegistrySerializable config)
        {
            Type t = config.GetType();
            return config.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && p.CanWrite);
        }

        /// <summary>
        /// 객체가 가진 property 중에서 public 으로 읽고 쓰기 가능한 property 들을 registry 에 저장한다.
        /// </summary>
        /// <param name="config">저장할 객체</param>
        /// <param name="key">저장할 registry key.</param>
        /// <param name="customSaver">저장할 때에 Type 과 property 이름을 받아 custom 으로 객체를 만들어 반환.  null 반환시 default 로직을 따름.</param>
        /// http://stackoverflow.com/questions/824802/c-how-to-get-all-public-both-get-and-set-string-properties-of-a-type
        public static void ToRegistry(IRegistrySerializable config, RegistryKey key, Func<Type, string, object> customSaver=null)
        {
            Type t = config.GetType();
            GetPropertyInfos(config).ForEach(p =>
            {
                object value = null;
                if (customSaver != null)
                    value = customSaver(t.GetProperty(p.Name).PropertyType, p.Name);
                
                if ( value == null )
                    value = PropertyValueExtractor.GetPropValue(config, p.Name);

                if ( value != null )
                    key.SetValue(p.Name, value.ToString());
            });
        }

        /// <summary>
        /// 객체가 가진 property 중에서 public 으로 읽고 쓰기 가능한 property 들의 값을 registry 에서 읽어서 채운다.
        /// </summary>
        /// <param name="config"></param>
        /// <param name="key"></param>
        /// http://stackoverflow.com/questions/619767/set-object-property-using-reflection
        public static void FromRegistry(IRegistrySerializable config, RegistryKey key)
        {
            Type t = config.GetType();
            GetPropertyInfos(config).ForEach(p =>
            {
                string strValue = (string)key.GetValue(p.Name);
                try
                {
                    if (strValue == null)   // registry 에 정보 누락된 경우
                        logger.ErrorFormat("Failed to read property {0} from registry", p.Name);
                    else
                    {
                        object value = ObjectValueParser.Parse(p.PropertyType, strValue);
                        p.SetValue(config, value);
                    }
                }
                catch (Exception ex)
                {
                    logger.ErrorFormat("Failed to read property {0} from registry : {1}", p.Name, ex.Message);
                    ExceptionHider.SwallowException(ex);
                }
            });
        }
    }
}
