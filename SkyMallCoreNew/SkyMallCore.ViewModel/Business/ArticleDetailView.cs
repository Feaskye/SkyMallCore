using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.ViewModel
{
    public class ArticleDetailView : DetailView
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
        /// 版权状况：转载	原创	授权	下架
        /// </summary>
        public string Copyright { get; set; }

        /// <summary>
        /// 积分要求    	0共享分 1现金
        /// </summary>
        public int ScoreRequire { get; set; }

        /// <summary>
        /// 专题编号
        /// </summary>
        public string SpecialTopicId { get; set; }

        //暂不做，默认全是公有
        ///// <summary>
        ///// true 公有（所有人可见）	
        ///// false 私有（仅文件夹关联读者群内读者可见）
        ///// </summary>
        //public bool IsSharePublic { get; set; }

        /// <summary>
        /// 是否在线浏览，true全预览，false指定页数
        /// </summary>
        public bool IsOnline { get; set; }

        /// <summary>
        /// 预览页数设定
        /// </summary>
        public int OnlinePageCount { get; set; }

        public int PageCount { get; set; }

        /// <summary>
        /// 下载数量
        /// </summary>
        public int DownloadCount { get; set; }


        /// <summary>
        /// 是否允许下载
        /// </summary>
        public bool AllowDownload { get; set; }


        /// <summary>
        /// 分享类型：0共享资源  1资源售卖 （1 暂时不支持）
        /// </summary>
        public int ShareType { get; set; }

        /// <summary>
        /// 分享积分/金额
        /// </summary>
        public int RequireAmount { get; set; }


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
        /// 资源大小
        /// </summary>
        public int ResourceSize { get; set; }

        /// <summary>
        /// 分类编号
        /// </summary>
        public string CategoryId { get; set; }


        public string ParentCategoryId { get; set; }

        public string Category { get; set; }

        public string MemberId { get; set; }

        public string Member { get; set; }

        public string ResourceType { get; set; }


        public int BookStatus { get; set; }
        public string AuditMessage { get; set; }
        public object AttachmentImage { get; set; }
    }


    public enum BookStatus
    {
        待审核 = 0,

        审核通过 = 1,

        审核失败 = -1,

        重复资源 = -2,

        已下架 = -3
    }


}
