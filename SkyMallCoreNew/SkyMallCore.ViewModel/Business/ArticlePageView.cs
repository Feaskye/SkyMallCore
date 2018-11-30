using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.ViewModel.Business
{
    public class ArticlePageView
    {
        public ArticleDetailView ArticleDetail { get; set; }

        public List<ArticleDetailView> AboutArticles { get; set; }

        public List<ArticleDetailView> LikeArticles { get; set; }

        public MemberDetailView Member { get; set; }

        


    }
}
