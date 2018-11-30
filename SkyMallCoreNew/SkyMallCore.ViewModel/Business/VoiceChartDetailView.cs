using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SkyMallCore.ViewModel
{
    public class VoiceChartDetailView
    {
        [DisplayName("本地通道")]
        public string LineNumber { get; set; }

        /// <summary>
        /// 呼出总时长
        /// </summary>
        //[ChartSeries(SeriesType)]
        [DisplayName("呼出总时长")]
        public int OutTime { get; set; }

        /// <summary>
        /// 呼出总时长
        /// </summary>
        [DisplayName("呼入总时长")]
        public int InTime { get; set; }

        /// <summary>
        /// 呼出总次数
        /// </summary>
        [DisplayName("呼出总次数")]
        public int OutLineCount { get; set; }

        /// <summary>
        /// 呼出总次数
        /// </summary>
        [DisplayName("呼入总次数")]
        public int InLineCount { get; set; }

        /// <summary>
        /// 未接电话总次数（次）
        /// </summary>
        [DisplayName("累计时长")]
        public int CutLineCount { get; set; }

        
    }
}
