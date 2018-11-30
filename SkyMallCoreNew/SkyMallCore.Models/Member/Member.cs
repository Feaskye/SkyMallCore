using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SkyMallCore.Models
{
    [DisplayName("会员")]
    [Table("Member")]
    public class Member : ModelEntity
    {

        public string UserName { get; set; }
        public string Password { get; set; }

        public string RealName { get; set; }
        public string NickName { get; set; }
        public string HeadIcon { get; set; }
        public bool? Gender { get; set; }

        /// <summary>
        /// 积分
        /// </summary>
        public int UserScore { get; set; }
        /// <summary>
        /// 会员级别
        /// </summary>
        public int UserLevel { get; set; }

        public string LineNumber { get; set; }

        public DateTime? Birthday { get; set; }
        public string MobilePhone { get; set; }
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
        public string WeChat { get; set; }
        public int? SecurityLevel { get; set; }

        public string GroupId { get; set; }
        public string QQ { get; set; }

        public string Province { get; set; }

        public string City { get; set; }




        public virtual List<Article> Articles { get; set; }


    }
}
