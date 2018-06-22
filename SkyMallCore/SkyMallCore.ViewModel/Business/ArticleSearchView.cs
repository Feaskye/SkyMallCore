using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.ViewModel.Business
{
    public class ArticleSearchView
    {
        public string Keyword { get; set; }

        

        public string CategoryId { get; set; }

        /// <summary>
        /// 文章列表页
        /// </summary>
        public PagedList<ArticleDetailView> Articles { get; set; }
        
    }


    public class ArticleDetailView
    {
        public string Title { get; set; }

        public string Keyword { get; set; }
        /// <summary>
        /// 短标题
        /// </summary>
        public string ShortTitle { get; set; }

        public string Content { get; set; }

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


        //public ArticleCategory ArticleCategory { get; set; }
    }







}
