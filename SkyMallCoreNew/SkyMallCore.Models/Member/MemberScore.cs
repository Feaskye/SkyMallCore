using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SkyMallCore.Models
{
    [DisplayName("会员积分")]
    [Table("MemberScore")]
    public class MemberScore : ModelEntity
    {
        public int Score { get; set; }
        
        /// <summary>
        /// 积分操作类型：0获取，1消费
        /// </summary>
        public int OperatType { get; set; }

        /// <summary>
        /// 积分类型
        /// </summary>
        public int ScoreType { get; set; }


        public string MemberId { get; set; }

        public virtual Member Member { get; set; }

        /// <summary>
        /// 购买物品相关主键编号
        /// </summary>
        public string KeyId { get; set; }
    }
}
