using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.ViewModel
{
    public class LogSearchView
    {
        public string Keyword { get; set; }
        public string Admin { get; set; }
        public string TimeType { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }
    }
}
