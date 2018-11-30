using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.ViewModel
{
    public class MemScoreSearchView
    {
        public int? SearchType { get; set; } = 0;

        public int? ScoreType { get; set; }


        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }


        public string MemberId { get; set; }


        public PagedList<MemScoreDetailView> ScoreList { get; set; }


        public int TotalScore { get; set; }
        /// <summary>
        /// 剩余积分
        /// </summary>
        public int MemScore { get; set; }
    }


}
