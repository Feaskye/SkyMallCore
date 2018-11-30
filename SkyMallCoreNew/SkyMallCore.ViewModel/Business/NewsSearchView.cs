using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.ViewModel
{
    public class NewsSearchView
    {
        public string Keyword { get; set; }

        public string ParentCategoryId { get; set; }
        
        public string CategoryId { get; set; }

    
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
        
        public string Title { get; set; }

        /// <summary>
        /// 文章列表页
        /// </summary>
        public PagedList<ArticleDetailView> Articles { get; set; }

    }

    public enum NewsTopEnum
    {
        /// <summary>
        /// 最新热门新闻
        /// </summary>
        NewHotNews,

        /// <summary>
        /// 热门新闻
        /// </summary>
        HotNews,

        /// <summary>
        /// 网站公告
        /// </summary>
        Announcement
    }












}
