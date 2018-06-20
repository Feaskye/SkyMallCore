using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SkyMallCore.Models
{
    [Table("ArticleCategory")]
    public class ArticleCategory : ModelEntity
    {
        public string Title { get; set; }

        public string Keyword { get; set; }
        /// <summary>
        /// 短标题
        /// </summary>
        public string ShortTitle { get; set; }

        //public string Content { get; set; }


        public int ParentId { get; set; }

        
    }
}
