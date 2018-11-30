using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.ViewModel.Business
{
   public class VoiceSearchView
    {
        public DateTime? VoiceDateStart { get; set; } = DateTime.Now.AddDays(-1).Date;

        public DateTime? VoiceDateEnd { get; set; } = DateTime.Now.Date.AddSeconds(-1);

        public string LineNumber{ get; set; }

        public string Description{ get; set; }

        public int LineTimeStart { get; set; }

        public int LineTimeEnd { get; set; }

        public string Direction{ get; set; }

        public string PhoneNumber{ get; set; }



        public string SortColumn { get; set; }

        public string SortOrder { get; set; }


        public bool ShowChart { get; set; }

        public string MemPhone { get; set; }


    }
}
