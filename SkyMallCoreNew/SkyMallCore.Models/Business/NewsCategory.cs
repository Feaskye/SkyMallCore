using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SkyMallCore.Models
{
    [Table("NewsCategory")]
    public class NewsCategory : ModelEntity
    {
        public string Title { get; set; }

        public string Keyword { get; set; }
        /// <summary>
        /// 短标题
        /// </summary>
        public string ShortTitle { get; set; }

        //public string Content { get; set; }


        public string ParentId { get; set; }
        public string CoverUrl { get; set; }
        //public int ReadCount { get; set; }

        [ForeignKey("ParentId")]
        public virtual NewsCategory Category { get; set; }


        //public virtual List<News> NewsList { get; set; }


    }
}
