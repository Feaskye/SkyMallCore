using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.ViewModel
{
    public class NewsCateDetailView: DetailView
    {
        public string Title { get; set; }

        ///// <summary>
        ///// 自定义Url      是否需要
        ///// </summary>
        //public string CustomizeUrl { get; set; }

        public string ParentId { get; set; }

        public string Category { get; set; }


        public string ShortTitle { get; set; }
        public string CoverUrl { get; set; }
        public int ReadCount { get; set; }


        public List<NewsDetailView> NewsDetails { get; set; }


    }

    







    public class NewsCateSearchView
    {

        public string Keyword { get; set; }
        //public string ParentId { get; set; }

        public string[] ParentId { get; set; }

        /// <summary>
        /// 推荐
        /// </summary>
        public bool? IsRemmand { get; set; }

    }








}
