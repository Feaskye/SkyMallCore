using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.ViewModel
{
    public class NewsDetailView : DetailView
    {
        public string Title { get; set; }

        public string Keyword { get; set; }
        /// <summary>
        /// 短标题
        /// </summary>
        public string ShortTitle { get; set; }

        public string Description { get; set; }


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
        public string CategoryId { get; set; }

        public string Category { get; set; }
    }






    public enum ReadType
    {
        /// <summary>
        /// 文库
        /// </summary>
        Article,
        /// <summary>
        /// 专题
        /// </summary>
        Topic,
        /// <summary>
        /// 新闻
        /// </summary>
        News
    }


}
