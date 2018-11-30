using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SkyMallCore.ViewModel
{
    public class HelpDetailView : DetailView
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
        /// 分类编码
        /// </summary>
        public HelpCode HelpCode { get; set; }

        public string Category { get; set; }

    }

    /// <summary>
    /// 分类编码
    /// </summary>
    public enum HelpCode
    {
        /// <summary>
        /// 帮助中心
        /// </summary>
        [Description("帮助中心")]
        HelpCenter = 0,
        ///// <summary>
        ///// 公告
        ///// </summary>
        //[Description("公告")]
        //Announcement = 1,
        /// <summary>
        /// 首页轮播
        /// </summary>
        [Description("首页轮播")]
        HomeCarousel = 2,

        /// <summary>
        /// 用户协议
        /// </summary>
        [Description("用户协议")]
        MemProtocol = 3

        
    }



}
