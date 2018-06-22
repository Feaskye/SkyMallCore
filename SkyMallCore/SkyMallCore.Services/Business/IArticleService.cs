using SkyMallCore.Models;
using SkyMallCore.ViewModel;
using SkyMallCore.ViewModel.Business;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.Services
{
    public interface IArticleService
    {

        PagedList<Article> GetList(ArticleSearchView searchView, int pageIndex = 1,int pageSize = 20);

        List<Article> GetList(string keyword = "");


        Article GetForm(string keyValue);


        void DeleteForm(string keyValue);


        void SubmitForm(Article Article, string[] permissionIds, string keyValue);


        void SubmitForm(Article roleEntity, string keyValue);
    }


}
