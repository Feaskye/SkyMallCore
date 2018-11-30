using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SkyMallCore.ViewModel
{
    public class VoiceDetailView
    {
        [DisplayName("本地通道")]
        public string LineNumber { get; set; }

        [DisplayName("对方号码")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 方向：拨入I拨出O手动M方向
        /// </summary>
        [DisplayName("方向")]
        public string Direction { get; set; }
        /// <summary>
        /// 挂断
        /// </summary>
        [DisplayName("挂断")]
        public string CutLine { get; set; }

        /// <summary>
        /// 通话时长（秒） 录音总共有多少秒
        /// </summary>
        [DisplayName("通话时长")]
        public int LineTime { get; set; }

        /// <summary>
        /// 文件
        /// </summary>
        [DisplayName("文件")]
        public string VoiceFile { get; set; }

        [DisplayName("类型")]
        public string Type { get; set; }

        /// <summary>
        /// 线路号
        /// </summary>
        [DisplayName("备注")]
        public string Description { get; set; }



        /// <summary>
        ///文件日期
        /// </summary>
        [DisplayName("时间")]
        public DateTime FileDate { get; set; }
    }
}
