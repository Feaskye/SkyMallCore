using SkyMallCore.Models;
using SkyMallCore.ViewModel;
using SkyMallCore.ViewModel.Business;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.Services
{
    public interface INewsCategoryService
    {

        List<NewsCategory> GetList(NewsCateSearchView searchView);

        PagedList<NewsCateDetailView> GetCateList(NewsCateSearchView searchView,int pageIndex = 1,int pageSize = 20);


        List<ListItem> GetCateList(string currentId, string parentId, bool containsChild = false);


        ListItem GetCate(string id);

        NewsCategory GetForm(string keyValue);


        void DeleteForm(string keyValue);


        void SubmitForm(NewsCategory NewsCategory, string[] permissionIds, string keyValue);


        InvokeResult<bool> SubmitForm(NewsCategory roleEntity);

        List<ListItem> GetHotTopics(int count);
    }


}
