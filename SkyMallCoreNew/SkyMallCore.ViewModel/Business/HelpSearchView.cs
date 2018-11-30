using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.ViewModel
{
    public class HelpSearchView
    {
        public string Keyword { get; set; }

        public string ParentCategoryId { get; set; }

        public string CategoryId { get; set; }

        public string MemberId { get; set; }


        public string Title { get; set; }


        public HelpCode? HelpCode { get; set; }

    }
}
