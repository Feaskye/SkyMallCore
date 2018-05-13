using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.Core
{
    public class WebHelper
    {
        public static IHttpContextAccessor HttpContextAccessor { get; set; }

        #region Session操作
        /// <summary>
        /// 写Session
        /// </summary>
        /// <typeparam name="T">Session键值的类型</typeparam>
        /// <param name="key">Session的键名</param>
        /// <param name="value">Session的键值</param>
        public static void WriteSession<T>(string key, T value)
        {
            if (key.IsEmpty())
                return;
            GetHttpContext().Session.SetString(key, value.ToString());
        }

        /// <summary>
        /// 写Session
        /// </summary>
        /// <param name="key">Session的键名</param>
        /// <param name="value">Session的键值</param>
        public static void WriteSession(string key, string value)
        {
            WriteSession<string>(key, value);
        }

        /// <summary>
        /// 读取Session的值
        /// </summary>
        /// <param name="key">Session的键名</param>        
        public static string GetSession(string key)
        {
            if (key.IsEmpty())
                return string.Empty;
            return GetHttpContext().Session.GetString(key);
        }
        /// <summary>
        /// 删除指定Session
        /// </summary>
        /// <param name="key">Session的键名</param>
        public static void RemoveSession(string key)
        {
            if (key.IsEmpty())
                return;
           GetHttpContext().Session.Remove(key);
        }

        #endregion

        public static HttpContext GetHttpContext()
        {
            object factory = GetService(typeof(Microsoft.AspNetCore.Http.IHttpContextAccessor));
            Microsoft.AspNetCore.Http.HttpContext context = ((IHttpContextAccessor)factory).HttpContext;
            return context;
        }


        public static object GetService(Type type)
        {
            return HttpContextAccessor.HttpContext.RequestServices.GetService(type);
        }

    }
}
