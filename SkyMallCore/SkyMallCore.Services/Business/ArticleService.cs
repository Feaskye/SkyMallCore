using SkyMallCore.Core;
using SkyMallCore.Data;
using SkyMallCore.Models;
using SkyMallCore.Respository;
using SkyMallCore.ViewModel;
using SkyMallCore.ViewModel.Business;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SkyMallCore.Services
{
    public class ArticleService: IArticleService
    {
        ISysLogRespository _LogRespository;
        IArticleRespository _Respository;
        IArticleCategoryRespository _ArticleCategoryRespository;

        public ArticleService(ISysLogRespository sysLogRespository, IArticleRespository respository,
            IArticleCategoryRespository articleCategoryRespository
            )
        {
            _LogRespository = sysLogRespository;
            _Respository = respository;
            _ArticleCategoryRespository = articleCategoryRespository;
        }



        public List<Article> GetList(string keyword = "")
        {
            var expression = ExtLinq.True<Article>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.Title.Contains(keyword));
                expression = expression.Or(t => t.Content.Contains(keyword));
            }
            //expression = expression.And(t => t.CategoryId == 2);
            return _Respository.Get(expression).OrderBy(t => t.SortCode).ToList();
        }


        /// <summary>
        /// 文章分页
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public PagedList<Article> GetList(ArticleSearchView searchView, int pageIndex = 1, int pageSize = 20)
        {
            var expression = ExtLinq.True<Article>();
            if (!string.IsNullOrEmpty(searchView.Keyword))
            {
                expression = expression.And(t => t.Title.Contains(searchView.Keyword));
                expression = expression.Or(t => t.Content.Contains(searchView.Keyword));
            }
            //expression = expression.And(t => t.CategoryId == 2);
            return _Respository.GetPagedList<Article>(expression, pageIndex, pageSize);
        }

        public Article GetForm(string keyValue)
        {
            return _Respository.Get(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            _Respository.Delete(keyValue);
        }
        public void SubmitForm(Article Article, string[] permissionIds, string keyValue)
        {
            //if (!string.IsNullOrEmpty(keyValue))
            //{
            //    Article.Id = keyValue;
            //}
            //else
            //{
            //    Article.Id = Common.GuId();
            //}
            //var moduledata = _SysModuleService.GetList();
            //var buttondata = _SysModuleButtonService.GetList();
            //List<ArticleAuthorize> ArticleAuthorizes = new List<ArticleAuthorize>();
            //foreach (var itemId in permissionIds)
            //{
            //    ArticleAuthorize ArticleAuthorize = new ArticleAuthorize();
            //    ArticleAuthorize.Id = Common.GuId();
            //    ArticleAuthorize.ObjectType = 1;
            //    ArticleAuthorize.ObjectId = Article.Id;
            //    ArticleAuthorize.ItemId = itemId;
            //    if (moduledata.Find(t => t.Id == itemId) != null)
            //    {
            //        ArticleAuthorize.ItemType = 1;
            //    }
            //    if (buttondata.Find(t => t.Id == itemId) != null)
            //    {
            //        ArticleAuthorize.ItemType = 2;
            //    }
            //    ArticleAuthorizes.Add(ArticleAuthorize);
            //}
            //_Respository.SubmitForm(Article, ArticleAuthorizes, keyValue);
        }


        public void SubmitForm(Article roleEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                roleEntity.Id = keyValue;
                _Respository.Update(roleEntity);
            }
            else
            {
                //roleEntity.CategoryId = 2;
                _Respository.Insert(roleEntity);
            }
        }


    }


}
