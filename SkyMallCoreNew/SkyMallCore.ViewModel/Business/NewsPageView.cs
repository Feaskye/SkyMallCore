using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.ViewModel.Business
{
    public class NewsPageView
    {
        public NewsDetailView NewsDetail { get; set; }

        public List<NewsDetailView> AboutNews { get; set; }

        public List<NewsDetailView> LikeNewss { get; set; }

        


    }
}
