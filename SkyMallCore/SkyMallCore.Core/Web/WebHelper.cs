using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.Core
{
    public class WebHelper
    {
        #region Cookie操作
        //https://www.c-sharpcorner.com/article/asp-net-core-working-with-cookie/
        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        public static void WriteCookie(string strName, string strValue)
        {
            CoreProviderContext.HttpContext.Response.Cookies.Append(strName, strValue);
        }
        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="key">名称</param>
        /// <param name="value">值</param>
        /// <param name="expires">过期时间(分钟)</param>
        public static void WriteCookie(string key, string value, int expires)
        {
            var cookie = CoreProviderContext.HttpContext.Request.Cookies[key];
            //if (cookie == null)
            //{
            //    cookie = new HttpCookie(strName);
            //}
            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddMinutes(expires);
            CoreProviderContext.HttpContext.Response.Cookies.Append(key, value, options);
        }
        /// <summary>
        /// 读cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <returns>cookie值</returns>
        public static string GetCookie(string strName)
        {
            if (CoreProviderContext.HttpContext.Request.Cookies != null && 
                CoreProviderContext.HttpContext.Request.Cookies[strName] != null)
            {
                return CoreProviderContext.HttpContext.Request.Cookies[strName].ToString();
            }
            return "";
        }
        /// <summary>
        /// 删除Cookie对象
        /// </summary>
        /// <param name="key">Cookie对象名称</param>
        public static void RemoveCookie(string key)
        {
            CoreProviderContext.HttpContext.Response.Cookies.Delete(key);
        }
        #endregion


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
            CoreProviderContext.HttpContext.Session.SetString(key, value.ToString());
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
            return CoreProviderContext.HttpContext.Session.GetString(key);
        }
        /// <summary>
        /// 删除指定Session
        /// </summary>
        /// <param name="key">Session的键名</param>
        public static void RemoveSession(string key)
        {
            if (key.IsEmpty())
                return;
           CoreProviderContext.HttpContext.Session.Remove(key);
        }

        #endregion
        

    }
}
