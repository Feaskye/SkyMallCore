using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SkyMallCore.ViewModel
{
    public class MemberDetailView: DetailView
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public string RealName { get; set; }

        /// <summary>
        /// 积分
        /// </summary>
        public int UserScore { get; set; }

        public string HeadIcon { get; set; }



        public string NickName { get; set; }
        /// <summary>
        /// 会员级别
        /// </summary>
        public UserLevel UserLevel { get; set; }
        
        public DateTime? Birthday { get; set; }
        public string MobilePhone { get; set; }
        public string Description { get; set; }

        public string Telephone { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string Company { get; set; }
        /// <summary>
        /// 传真
        /// </summary>
        public string Tax { get; set; }
        public string Address { get; set; }
        /// <summary>
        /// 宅电
        /// </summary>
        public string HomePhone { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string ZipCode { get; set; }
        /// <summary>
        /// 职务
        /// </summary>
        public string Position { get; set; }

        public string Email { get; set; }

        public string GroupId { get; set; }
        public string GroupName { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public bool Gender { get; set; }
        public string Province { get; set; }

        public string City { get; set; }

        public string QQ { get; set; }

    }

    public enum UserLevel
    {
        [Description("普通会员")]
        Common = 1,
        [Description("VIP会员")]
        Vip = 2
    }




    public enum MemTopEnum
    {
        /// <summary>
        /// 按资源数量排序取
        /// </summary>
        ResourseCount

    }



}
