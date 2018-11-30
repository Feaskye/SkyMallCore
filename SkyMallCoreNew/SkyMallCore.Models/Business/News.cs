using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SkyMallCore.Models
{
    [Table("News")]
    public class News : ModelEntity
    {
        [DisplayName("标题")]
        [Required(ErrorMessage ="标题不能为空")]
        public string Title { get; set; }

        public string Keyword { get; set; }
        /// <summary>
        /// 短标题
        /// </summary>
        public string ShortTitle { get; set; }

        /// <summary>
        /// 封面Url
        /// </summary>
        public string CoverUrl { get; set; }
        
        /// <summary>
        /// 阅读量
        /// </summary>
        public int ReadCount { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public string Attachment { get; set; }

        /// <summary>
        /// 原文链接
        /// </summary>
        public string ResourceUrl { get; set; }

        /// <summary>
        /// 分类编号
        /// </summary>
        [Required(ErrorMessage = "所属分类必选")]
        public string CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual NewsCategory NewsCategory { get; set; }


    }
}
