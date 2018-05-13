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
            var md5 = MD5.Create(str);//todo md5出错
            var result = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(str)))
                                .Replace("-", "");
            string strEncrypt;
            if (code == 16)
            {
                result = result.Substring(8, 16);
            }
            return result;
        }
    }
}
