using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SkyMallCore.ViewModel
{
    public class VoiceStatisDetailView
    {
        [DisplayName("本地通道")]
        public string LineNumber { get; set; }

        /// <summary>
        /// 方向：拨入I拨出O手动M方向
        /// </summary>
        [DisplayName("方向")]
        public string Direction { get; set; }

        /// <summary>
        /// 拨入拨出累计次数
        /// </summary>
        [DisplayName("累计次数")]
        public int LineCount { get; set; }

        /// <summary>
        /// 通话时长（秒） 录音总共有多少秒
        /// </summary>
        [DisplayName("累计时长")]
        public int LineTime { get; set; }


        [DisplayName("累计时长")]
        public int AverageTime { get; set; }
        
    }


    public class SeriesDetail
    {
        public string name { get; set; }
        public string type { get; set; }
        public List<int> data { get; set; }
    }



}
