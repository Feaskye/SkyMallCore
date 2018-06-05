using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SkyMallCore.Core
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
    }



    public class ScopedModel
    {
        public Type Interface { get; set; }
        public Type Class { get; set; }
    }


}
