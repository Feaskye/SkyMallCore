using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SkyMallCore.ViewModel
{
    public class MemScoreDetailView: DetailView
    {

        public int Score { get; set; }


        /// <summary>
        /// 积分操作类型：0获取，1消费
        /// </summary>
        public int OperatType { get; set; }

        /// <summary>
        /// 积分类型
        /// </summary>
        public int ScoreType { get; set; }

        public string ScoreTypeName { get; set; }


        public string MemberId { get; set; }


        public string Description { get; set; }


        public string Member { get; set; }
    }
    

    /// <summary>
    /// 
    /// </summary>
    public enum ScoreType
    {
        [Description("用户注册")]
        reg,
        [Description("添加文库")]
        addbook,
        [Description("添加专题")]
        addtopic,
        [Description("购买文库")]
        buybook,
        [Description("购买专题")]
        buytopic,
        /// <summary>
        /// 文库被购买
        /// </summary>
        [Description("文库被购买")]
        ubuybook,
        /// <summary>
        /// 专题被购买
        /// </summary>
        [Description("专题被购买")]
        ubuytopic,
        [Description("下载资源")]
        downloadbook,
        [Description("下载专题")]
        downloadtopic,
        [Description("资源被下载")]
        udownloadbook,
        [Description("专题被下载")]
        udownloadtopic
    }




}
