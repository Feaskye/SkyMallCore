using SkyMallCore.Models;
using SkyMallCore.ViewModel;
using SkyMallCore.ViewModel.Business;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.Services
{
    public interface INewsService
    {
        /// <summary>
        /// 后台加载使用
        /// </summary>
        /// <param name="searchView"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedList<News> GetList(NewsSearchView searchView, int pageIndex = 1,int pageSize = 20);

        List<News> GetList(string keyword = "");


        News GetForm(string keyValue);


        InvokeResult<bool> DeleteForm(string keyValue);

        InvokeResult<bool> DelNews(string memberId,string keyValue);

        ///// <summary>
        ///// 获取分类加新闻列表
        ///// </summary>
        ///// <param name="cateCount"></param>
        ///// <param name="newsCount"></param>
        ///// <returns></returns>
        //List<NewsCateDetailView> GetCateNews(int cateCount, int newsCount);


        InvokeResult<bool> SubmitForm(News roleEntity);

        InvokeResult<bool> ClientRead(string id);


        PagedList<NewsDetailView> GetNewsList(NewsSearchView searchView, int pageIndex, int pageSize);

        List<NewsDetailView> GetTopNewss(NewsTopEnum? topEnum, int count, string[] parentIds = null);






    }


}
