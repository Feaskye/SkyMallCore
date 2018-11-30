using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SkyMallCore.ViewModel
{
    public class TopicDetailView: DetailView
    {
        public string Title { get; set; }

        ///// <summary>
        ///// 自定义Url      是否需要
        ///// </summary>
        //public string CustomizeUrl { get; set; }

        public string ParentId { get; set; }
        public string ShortTitle { get; set; }
        public string CoverUrl { get; set; }

        public string BigCoverUrl { get; set; }

        public string Attachment { get; set; }

        

        public int ReadCount { get; set; }

        /// <summary>
        /// 分类
        /// </summary>
        public string Category { get; set; }
        



        public List<ArticleDetailView> ArticleDetails { get; set; }

        /// <summary>
        /// 资源数量
        /// </summary>
        public int ResourceCount { get; set; }

        public string MemberName { get; set; }

        public string Description { get; set; }
        

        public string MemberId { get; set; }
        public string Tag { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string MemHeader { get; set; }
        public int UserLevel { get; set; }

        /// <summary>
        /// 打包价
        /// </summary>
        public int PackageAmount { get; set; }

        public int TopicAmount { get; set; }



        public int TopicStatus { get; set; }
    }



    public enum TopicStatus
    {

        /// <summary>
        /// 审核拒绝
        /// </summary>
        [Description("审核拒绝")]
        AuditFail = -1,

        /// <summary>
        /// 未审核
        /// </summary>
        [Description("未审核")]
        UnAudit = 0,

        /// <summary>
        /// 审核成功
        /// </summary>
        [Description("审核成功")]
        Audited = 1
    }


    public class TopicSearchView
    {

        public List<TopicDetailView> HotTopics { get; set; }


        public List<ListItem> TopicCates { get; set; }

        public int TopicCount { get; set; }

        public PagedList<TopicDetailView> TopicList { get; set; }
    }







    public class ArticleTopicSearchView
    {

        public string Keyword { get; set; }
        //public string ParentId { get; set; }


        public string[] ParentId { get; set; }

        /// <summary>
        /// 推荐
        /// </summary>
        public bool? IsRemmand { get; set; }

        public bool? HotTopic { get; set; }
        public string TopicId { get; set; }


        public bool? IgnoreCate { get; set; }


        public TopicStatus? TopicStatus { get; set; }


        public PagedList<TopicDetailView> TopicDetails { get; set; }
        public string MemberId { get; set; }
    }








}
