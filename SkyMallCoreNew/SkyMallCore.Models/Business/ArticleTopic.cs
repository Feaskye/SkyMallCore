using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SkyMallCore.Models
{
    [Table("ArticleTopic")]
    public class ArticleTopic : ModelEntity
    {
        public string Title { get; set; }

        public string Keyword { get; set; }
        /// <summary>
        /// 短标题
        /// </summary>
        public string ShortTitle { get; set; }

        //public string Content { get; set; }


        public string ParentId { get; set; }

        [ForeignKey("ParentId")]
        public virtual ArticleTopic ParentTopic { get; set; }

        /// <summary>
        /// 封面图/缩略图
        /// </summary>
        public string CoverUrl { get; set; }

        /// <summary>
        /// 大图
        /// </summary>
        public string BigCoverUrl { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public string Tag { get; set; }


        public int ReadCount { get; set; }

        /// <summary>
        /// 原始积分（忽略）
        /// </summary>
        public int TopicAmount { get; set; }

        /// <summary>
        /// 打包积分（主要用于计算、显示）
        /// </summary>
        public int PackageAmount { get; set; }

        //[ForeignKey("ParentId")]
        //public ArticleCategory Category { get; set; }

        public int TopicStatus { get; set; }

        /// <summary>
        /// 资源数量
        /// </summary>
        public int ResourceCount { get; set; }

        /// <summary>
        /// 资源包文件
        /// </summary>
        public string Attachment { get; set; }


        public string AuditMessage { get; set; }


    }
    


}
