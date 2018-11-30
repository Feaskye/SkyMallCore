using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.ViewModel
{
    public class ArticleSearchView
    {
        public string Keyword { get; set; }

        public string ParentCategoryId { get; set; }
        
        public string CategoryId { get; set; }

        public string MemberId { get; set; }

        public bool? HasMember { get; set; }

        /// <summary>
        /// 审核状态：0待审核、1审核通过、-1审核拒绝、-2重复资源、-3已下架
        /// </summary>
        public int? BookStatus { get; set; }

        /// <summary>
        /// 版权状况
        /// </summary>
        public int? Copyright { get; set; }

        /// <summary>
        /// 查询上架天数
        /// </summary>
        public int? OnShelfDays { get; set; }

        /// <summary>
        /// 资源类型
        /// </summary>
        public string ResourceType { get; set; }
        
        public string ScoreType { get; set; }

        public int? StartScore { get; set; }

        public int? EndScore { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }


        public int? StartPage { get; set; }

        public int? EndPage { get; set; }


        /// <summary>
        /// 按条件查询
        /// 1">下载最多
        /// 2">浏览最多
        /// </summary>
        public string SearchStatus { get; set; }


        public string SpecialTopicId { get; set; }

        public string Title { get; set; }

        /// <summary>
        /// 文章列表页
        /// </summary>
        public PagedList<ArticleDetailView> Articles { get; set; }

    }

    public enum ArticleTopEnum
    {

        /// <summary>
        /// 热门推荐    根据阅读量取前几条
        /// </summary>
        HotArticle,

        /// <summary>
        /// 下载量多推荐
        /// </summary>
        MoreDownload,

        /// <summary>
        /// 本站推荐
        /// </summary>
        SiteHot,

        //今日推荐
        NewArticle,

        /// <summary>
        /// 最新精品
        /// </summary>
        NewHotArticle,

        /// <summary>
        /// 精品PPT推荐
        /// </summary>
        HotPPT,

        /// <summary>
        /// 今日更新PPT
        /// </summary>
        NewPPT


    }












}
