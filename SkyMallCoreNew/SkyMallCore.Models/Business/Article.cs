using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SkyMallCore.Models
{
    [Table("Article")]
    public class Article : ModelEntity
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
        /// 版权状况：1转载	2原创	3授权	0下架
        /// </summary>
        public int? Copyright { get; set; }

        /// <summary>
        /// 积分要求    	0共享分 1现金
        /// </summary>
        public int ScoreRequire { get; set; }

        /// <summary>
        /// 专题编号
        /// </summary>
        public string SpecialTopicId { get; set; }


        [ForeignKey("SpecialTopicId")]
        public virtual ArticleTopic ArticleTopic { get; set; }


        //暂不做，默认全是公有
        ///// <summary>
        ///// true 公有（所有人可见）	
        ///// false 私有（仅文件夹关联读者群内读者可见）
        ///// </summary>
        //public bool IsSharePublic { get; set; }

        /// <summary>
        /// 是否在线浏览
        /// </summary>
        public bool IsOnline { get; set; }

        /// <summary>
        /// 预览页数设定
        /// </summary>
        public int OnlinePageCount { get; set; }

        /// <summary>
        /// 文库页数 默认1
        /// </summary>
        public int PageCount { get; set; }


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
        /// 原文链接/云下载地址
        /// </summary>
        public string ResourceUrl { get; set; }
        /// <summary>
        /// 资源格式
        /// </summary>
        public string ResourceType { get; set; }
        
        /// <summary>
        /// 分类编号
        /// </summary>
        [Required(ErrorMessage = "所属分类必选")]
        public string CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual ArticleCategory ArticleCategory { get; set; }

        [Required(ErrorMessage = "所属人无效")]
        public string MemberId { get; set; }

        [ForeignKey("MemberId")]
        public virtual Member Member { get; set; }

        /// <summary>
        /// 审核状态：0待审核、1审核通过、-1审核拒绝、-2重复资源、-3已下架
        /// </summary>
        public int BookStatus { get; set; }

        /// <summary>
        /// 资源类型
        /// </summary>
        public int DownloadCount { get; set; }

        /// <summary>
        /// 资源大小
        /// </summary>
        public int ResourceSize { get; set; }


        public string AuditMessage { get; set; }

        /// <summary>
        /// 是否已生成图片
        /// </summary>
        public bool HasImages { get; set; }
    }
}
