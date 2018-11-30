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

        PagedList<ArticleDetailView> GetTopicArticleList(ArticleSearchView searchView, int pageIndex = 1, int pageSize = 20);

        List<Article> GetList(string keyword = "");


        Article GetForm(string keyValue);


        InvokeResult<bool> DeleteForm(string keyValue);

        InvokeResult<bool> DelArticle(string memberId,string keyValue);


        /// <summary>
        /// 审核状态
        /// </summary>
        /// <param name="aid"></param>
        /// <param name="auditStatus"></param>
        /// <returns></returns>
        InvokeResult<bool> AuditArticle(string aid, int auditStatus,string auditMessage);
        


        InvokeResult<bool> SubmitForm(Article entity);
        
        PagedList<ArticleDetailView> GetBooks(ArticleSearchView searchView, int pageIndex, int pageSize);

        /// <summary>
        /// 审核状态的文库统计
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<ListItem> GetAuditStatics(string userId);

        /// <summary>
        /// 会员上传文库统计
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<ListItem> GetBookStatics(string userId);


        List<ArticleDetailView> GetTopArticles(ArticleTopEnum? topEnum, int count, string[] parentIds = null);


        /// <summary>
        /// 更新阅读数量
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        InvokeResult<bool> ClientRead(string id);


        /// <summary>
        /// 客户端下载
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        InvokeResult<Article> ClientDownLoad(string id);


        /// <summary>
        /// 资源总数量
        /// </summary>
        /// <returns></returns>
        int GetTotalBooks(string categoryId = null);


        InvokeResult<bool> AddBetchArticle(List<Article> articles);

        /// <summary>
        /// 获取所有标题
        /// </summary>
        /// <returns></returns>
        List<string> GetTitles();
        void AddBetchBooks(List<Article> newArticleList, int perCount = 50);

        /// <summary>
        /// 获取压缩包含文件
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        List<ListItem> GetPackageByTopicId(string topicId);


        ///// <summary>
        ///// 专题信息 作者、资源量
        ///// </summary>
        ///// <param name="memberId"></param>
        ///// <returns></returns>
        //MemResourceStaticsView GetMemResourceStatics(string memberId);
    }


}
