using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SkyCoreLib.Utils
{
    public static class Reflector
    {
        /// <summary>  
        /// 获取程序集中的实现类对应的多个接口
        /// </summary>  
        /// <param name="assemblyName">程序集</param>
        public static List<ScopedModel> GetScopedList(Assembly assembly)
        {
            if (assembly!=null)
            {
                var result = assembly.GetTypes().ToList()
                    .Where(w => !w.IsInterface && w.GetInterfaces().FirstOrDefault(d => !d.IsGenericType) != null);

                return result.Select(u => new ScopedModel
                {
                    Interface = u.GetInterfaces().FirstOrDefault(d => !d.IsGenericType),
                    Class = u
                }).ToList();
            }
            return null;
        }


        /// <summary>
        /// 说明：获得一个对象的所有属性
        /// </summary>
        /// <returns></returns>
        public static PropertyInfo[] GetProperties<T>()
        {
            try
            {
                Type type = typeof(T);
                object obj = Activator.CreateInstance(type);
                return type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(i => i.PropertyType.IsValueType || i.PropertyType == typeof(string)).ToArray();
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        /// <summary>
        /// 说明：获得一个对象的所有属性
        /// </summary>
        /// <returns></returns>
        public static string[] GetPropertyNames<T>()
        {
            try
            {
                Type type = typeof(T);
                object obj = Activator.CreateInstance(type);
                var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(i => i.PropertyType.IsValueType || i.PropertyType == typeof(string));

                string[] array = props.Select(t => t.Name).ToArray();
                return array;
            }
            catch (Exception ex)
            {
                return null;
            }

        }


        public static string GetModelValue(string FieldName, object obj)
        {
            try
            {
                Type Ts = obj.GetType();
                object o = Ts.GetProperty(FieldName).GetValue(obj, null);
                string Value = Convert.ToString(o);
                if (string.IsNullOrEmpty(Value)) return null;
                return Value;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 设置类中的属性值
        /// </summary>
        /// <param name="FieldName"></param>
        /// <param name="Value"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool SetModelValue(string FieldName, string Value, object obj)
        {
            try
            {
                Type Ts = obj.GetType();
                object v = Convert.ChangeType(Value, Ts.GetProperty(FieldName).PropertyType);
                Ts.GetProperty(FieldName).SetValue(obj, v, null);
                return true;
            }
            catch
            {
                return false;
            }
        }




    }



    public class ScopedModel
    {
        public Type Interface { get; set; }
        public Type Class { get; set; }
    }


}
