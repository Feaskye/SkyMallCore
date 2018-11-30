using SkyMallCore.Models;
using SkyMallCore.ViewModel;
using SkyMallCore.ViewModel.Business;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.Services
{
    public interface IArticleTopicService
    {

        List<ArticleTopic> GetList(ArticleTopicSearchView searchView);

        /// <summary>
        /// 列表；包含IsRemmand处理的相关文章列表
        /// </summary>
        /// <param name="searchView"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedList<TopicDetailView> GetTopicsList(ArticleTopicSearchView searchView,int pageIndex = 1,int pageSize = 20);

        /// <summary>
        /// 以分类角度加载
        /// </summary>
        /// <param name="currentId"></param>
        /// <param name="parentId"></param>
        /// <param name="containsChild"></param>
        /// <returns></returns>
        List<ListItem> GetTopicCateList(string currentId, string parentId, bool containsChild = false);


        ListItem GetCate(string id);

        /// <summary>
        /// 分类列表
        /// </summary>
        /// <param name="parentIds"></param>
        /// <returns></returns>
        List<ListItem> GetCates(string[] parentIds);

        ArticleTopic GetForm(string keyValue);


        InvokeResult<bool> DeleteForm(string keyValue);


        void SubmitForm(ArticleTopic ArticleTopic, string[] permissionIds, string keyValue);


        InvokeResult<bool> SubmitForm(ArticleTopic entity);

        List<ListItem> GetHotTopics(int count);

        /// <summary>
        /// 更新阅读数量
        /// </summary>
        /// <param name="cateId"></param>
        /// <returns></returns>
        InvokeResult<bool> ClientRead(string cateId);

        /// <summary>
        /// 专题 包含统计信息：创建人、专题资源数量
        /// </summary>
        /// <param name="searchView"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedList<TopicDetailView> GetTopicInfoList(ArticleTopicSearchView searchView, int pageIndex,int pageSize);


        /// <summary>
        /// 删除某用户的专题
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        InvokeResult<bool> DelTopic(string memberId, string keyValue);


        string GetAttachment(string tid, string field);


        /// <summary>
        /// 专题名称是否存在
        /// </summary>
        /// <param name="tid"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        InvokeResult<bool> ExistTopicName(string tid, string name);

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="id"></param>
        /// <param name="auditStatus"></param>
        /// <returns></returns>
        InvokeResult<bool> AuditTopic(string id, int auditStatus, string auditMessage);
    }


}
