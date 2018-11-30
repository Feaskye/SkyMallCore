using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace SkyMallCore.Core
{
    /// <summary>
    /// MD5加密
    /// </summary>
    public class Md5Hash
    {
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str">加密字符</param>
        /// <param name="code">加密位数16/32</param>
        /// <returns></returns>
        public static string Md5(string str, int code)
        {
            byte[] bytes;
            using (var md5 = MD5.Create())
            {
                bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            }

            var result = new StringBuilder();
            foreach (byte t in bytes)
            {
                result.Append(t.ToString("X2"));
            }
            if (code == 16)
            {
                return result.ToString().Substring(8, 16);
            }
            return result.ToString();
        }
    }
}
