using System;
using System.Collections.Generic;
using System.Text;

namespace SkyCoreLib.Utils
{
    public class RegularPattern
    {
        public const string Email = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

        public const string Phone = @"^1\d{10}$";

        public const string Tel = @"^(\d{3}|\d{4})-(\d{7}|\d{8})(-\d+)*$";

        /// <summary>
        /// 邮编
        /// </summary>
        public const string PostCode = @"^\d{6}$";

        /// <summary>
        /// 传真格式
        /// </summary>
        public const string Tax = @"^(\d{3}|\d{4})-(\d{7}|\d{8})(-\d+)*$";

        /// <summary>
        /// 正整数
        /// </summary>
        public const string Num = @"^\d+$";

        /// <summary>
        /// Url
        /// </summary>
        public const string Url = @"^http(s)?:\/\/([\w-]+\.)+[\w-]+(\/[\w- .\/?%&=]*)?$";
    }
}
