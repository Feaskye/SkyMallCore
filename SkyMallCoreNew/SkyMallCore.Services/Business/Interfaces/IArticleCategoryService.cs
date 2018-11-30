using SkyMallCore.Models;
using SkyMallCore.ViewModel;
using SkyMallCore.ViewModel.Business;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.Services
{
    public interface IArticleCategoryService
    {

        List<ArticleCategory> GetList(ArticleCateSearchView searchView);

        /// <summary>
        /// 列表；包含IsRemmand处理的相关文章列表
        /// </summary>
        /// <param name="searchView"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedList<ArticleCateDetailView> GetCateList(ArticleCateSearchView searchView,int pageIndex = 1,int pageSize = 20);


        List<ListItem> GetCateList(string currentId, string parentId, bool containsChild = false);


        ListItem GetCate(string id);

        ArticleCategory GetForm(string keyValue);


        void DeleteForm(string keyValue);


        void SubmitForm(ArticleCategory ArticleCategory, string[] permissionIds, string keyValue);


        InvokeResult<bool> SubmitForm(ArticleCategory entity);
        
        /// <summary>
        /// 更新阅读数量
        /// </summary>
        /// <param name="cateId"></param>
        /// <returns></returns>
        InvokeResult<bool> ClientRead(string cateId);
        /// <summary>
        /// 分类列表
        /// </summary>
        /// <param name="parentIds"></param>
        /// <returns></returns>
        List<ListItem> GetCates(string[] parentIds, bool? hasParentId = null);
    }


}
