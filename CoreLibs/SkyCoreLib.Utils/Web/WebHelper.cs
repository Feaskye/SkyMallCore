using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyCoreLib.Utils
{
    public static class WebHelper
    {
        #region Cookie操作
        //https://www.c-sharpcorner.com/article/asp-net-core-working-with-cookie/
        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        public static void WriteCookie(this HttpContext httpContext,string strName, string strValue)
        {
            httpContext.Response.Cookies.Append(strName, strValue);
        }
        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="key">名称</param>
        /// <param name="value">值</param>
        /// <param name="expires">过期时间(分钟)</param>
        public static void WriteCookie(this HttpContext httpContext,string key, string value, int expires)
        {
            var cookie = httpContext.Request.Cookies[key];
            //if (cookie == null)
            //{
            //    cookie = new HttpCookie(this HttpContext httpContextstrName);
            //}
            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddMinutes(expires);
            httpContext.Response.Cookies.Append(key, value, options);
        }
        /// <summary>
        /// 读cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <returns>cookie值</returns>
        public static string GetCookie(this HttpContext httpContext,string strName)
        {
            if (httpContext.Request.Cookies != null && 
                httpContext.Request.Cookies[strName] != null)
            {
                return httpContext.Request.Cookies[strName].ToString();
            }
            return "";
        }
        /// <summary>
        /// 删除Cookie对象
        /// </summary>
        /// <param name="key">Cookie对象名称</param>
        public static void RemoveCookie(this HttpContext httpContext,string key)
        {
            httpContext.Response.Cookies.Delete(key);
        }
        #endregion


        #region Session操作
        /// <summary>
        /// 写Session
        /// </summary>
        /// <typeparam name="T">Session键值的类型</typeparam>
        /// <param name="key">Session的键名</param>
        /// <param name="value">Session的键值</param>
        public static void WriteSession<T>(this HttpContext httpContext,string key, T value)
        {
            if (key.IsEmpty())
                return;
            httpContext.Session.SetString(key, value.ToString());
        }

        /// <summary>
        /// 写Session
        /// </summary>
        /// <param name="key">Session的键名</param>
        /// <param name="value">Session的键值</param>
        public static void WriteSession(this HttpContext httpContext,string key, string value)
        {
            httpContext.WriteSession<string>(key, value);
        }

        /// <summary>
        /// 读取Session的值
        /// </summary>
        /// <param name="key">Session的键名</param>        
        public static string GetSession(this HttpContext httpContext,string key)
        {
            if (key.IsEmpty())
                return string.Empty;
            return httpContext.Session.GetString(key);
        }
        /// <summary>
        /// 删除指定Session
        /// </summary>
        /// <param name="key">Session的键名</param>
        public static void RemoveSession(this HttpContext httpContext, string key)
        {
            if (key.IsEmpty())
                return;
           httpContext.Session.Remove(key);
        }

        #endregion
        

    }
}
