using SkyCoreLib.Utils;
using SkyMallCore.Models;
using SkyMallCore.Respository;
using SkyMallCore.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SkyMallCore.Services
{
    public class ArticleCategoryService : ServiceBase<ArticleCategory>, IArticleCategoryService
    {
        ISysLogRespository _LogRespository;
        IArticleCategoryRespository _Respository;

        public ArticleCategoryService(ISysLogRespository sysLogRespository, IArticleCategoryRespository ArticleCategoryRespository
            )
        {
            _LogRespository = sysLogRespository;
            _Respository = ArticleCategoryRespository;
        }



        public List<ArticleCategory> GetList(ArticleCateSearchView searchView)
        {
            var expression = base.GetFilterEnabled();

            if (!string.IsNullOrEmpty(searchView.Keyword))
            {
                expression = expression.And(t => t.Title.Contains(searchView.Keyword));
                expression = expression.Or(t => t.ShortTitle.Contains(searchView.Keyword));
            }
            return _Respository.Get(expression,o=>o.OrderBy(t=>t.SortCode)).ToList();
        }


        /// <summary>
        /// 分类列表
        /// </summary>
        /// <param name="parentIds"></param>
        /// <returns></returns>
        public List<ListItem> GetCates(string[] parentIds,bool? hasParentId = null)
        {
            var expression = base.GetFilterEnabled();
            if (hasParentId.HasValue && hasParentId.Value)
            {
                expression = expression.And(w => w.ParentId == null);
            }
            if (parentIds != null)
            {
                expression = expression.And(w => parentIds.Contains(w.Id));
            }
            return _Respository.GetFeilds(u => new ListItem { Code = u.Id, Text = u.Title }, expression).ToList();
        }


        public PagedList<ArticleCateDetailView> GetCateList(ArticleCateSearchView searchView, int pageIndex = 1, int pageSize = 20)
        {
            var expression = base.GetFilterEnabled();
            var order = base.Order();
            if (searchView.ParentId != null)
            {
                searchView.ParentId = searchView.ParentId.Where(w => !w.IsEmpty()).ToArray();
                if (searchView.ParentId.Any())
                {
                    expression = expression.And(w => searchView.ParentId.Contains(w.ParentId));
                }
            }

            if (searchView.IsRemmand.HasValue)
            {
                order = o => o.OrderBy(w => w.SortCode);
            }

            var data = _Respository.GetPagedList(u => new ArticleCateDetailView
            {
                Id = u.Id,
                Title = u.Title,
                CreatorTime = u.CreatorTime,
                ParentId = u.ParentId,
                ShortTitle = u.ShortTitle,
                CoverUrl = u.CoverUrl,
                ReadCount = u.ReadCount
            }, expression, pageIndex, pageSize, order);

            if (searchView.IsRemmand.HasValue)
            {
                var parentIds = data.Select(u => u.Id).ToArray();
                var articles = SkyCore.GlobalProvider.CoreContextProvider.GetService<IArticleService>().GetTopArticles(ArticleTopEnum.NewHotArticle, 14, parentIds);
                data.ForEach(cate => {
                    cate.ArticleDetails = articles.Where(w=>w.CategoryId == cate.Id).ToList();
                });
            }
            return data;
        }

        /// <summary>
        /// 分类列表（按需加载）
        /// </summary>
        /// <param name="currentId"></param>
        /// <param name="parentId"></param>
        /// <param name="containsChild"></param>
        /// <param name="isTopic"></param>
        /// <returns></returns>
        public List<ListItem> GetCateList(string currentId, string parentId,bool containsChild=false)
        {
            var expression = ExtLinq.True<ArticleCategory>();
            if (!parentId.IsEmpty())
            {
                expression = expression.And(t => t.ParentId == parentId);
            }
            else if (!containsChild)
            {
                expression = expression.And(t => t.ParentId == null);
            }
          
            //todo 编辑时已选择
            var curparentId = "";
            if (!currentId.IsEmpty())
            {
                curparentId = _Respository.Get(w => w.Id == currentId).Select(u => u.ParentId).FirstOrDefault();
            }

            var data= _Respository.GetFeilds(u => new ListItem { Code = u.Id, Text = u.Title, ParentId=u.ParentId, Selected = (u.Id == currentId || u.Id == curparentId) },
                expression, o => o.OrderBy(t => t.SortCode)).ToList();
            //var ids = new string[]{curparentId,currentId };
            //var any = data.Any(w => ids.Contains(w.Code));
            if (containsChild && data.Any())
            {
                var pclist = data.Where(w=>w.ParentId == null).ToList();
                foreach(var item in pclist)
                {
                    item.Childs = data.Where(w => w.ParentId == item.Code).ToList();
                }
                return pclist;
            }
            return data;
        }


        public ListItem GetCate(string id)
        {
            var expression = ExtLinq.True<ArticleCategory>();
            return _Respository.GetFeilds(u => new ListItem { Code = u.Id, Text = u.Title, ParentId = u.ParentId }, w => w.Id == id).FirstOrDefault();
        }

        public ArticleCategory GetForm(string keyValue)
        {
            return _Respository.Get(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            _Respository.Delete(keyValue);
        }
        public void SubmitForm(ArticleCategory ArticleCategory, string[] permissionIds, string keyValue)
        {
            //if (!string.IsNullOrEmpty(keyValue))
            //{
            //    ArticleCategory.Id = keyValue;
            //}
            //else
            //{
            //    ArticleCategory.Id = Common.GuId();
            //}
            //var moduledata = _SysModuleService.GetList();
            //var buttondata = _SysModuleButtonService.GetList();
            //List<ArticleCategoryAuthorize> ArticleCategoryAuthorizes = new List<ArticleCategoryAuthorize>();
            //foreach (var itemId in permissionIds)
            //{
            //    ArticleCategoryAuthorize ArticleCategoryAuthorize = new ArticleCategoryAuthorize();
            //    ArticleCategoryAuthorize.Id = Common.GuId();
            //    ArticleCategoryAuthorize.ObjectType = 1;
            //    ArticleCategoryAuthorize.ObjectId = ArticleCategory.Id;
            //    ArticleCategoryAuthorize.ItemId = itemId;
            //    if (moduledata.Find(t => t.Id == itemId) != null)
            //    {
            //        ArticleCategoryAuthorize.ItemType = 1;
            //    }
            //    if (buttondata.Find(t => t.Id == itemId) != null)
            //    {
            //        ArticleCategoryAuthorize.ItemType = 2;
            //    }
            //    ArticleCategoryAuthorizes.Add(ArticleCategoryAuthorize);
            //}
            //_Respository.SubmitForm(ArticleCategory, ArticleCategoryAuthorizes, keyValue);
        }


        public InvokeResult<bool> SubmitForm(ArticleCategory entity)
        {
            var expression = ExtLinq.True<ArticleCategory>();
            expression = expression.And(w => w.Title == entity.Title);
            if (!entity.Id.IsEmpty())
            {
                expression = expression.And(w => w.Id != entity.Id);
            }
            if (_Respository.Any(expression))
            {
                return RequestResult.Failed<bool>("该分类标题已存在，请重新输入");
            }
            bool b= _Respository.CreateOrUpdate(entity);
            return RequestResult.Result(b);
        }



        

        public InvokeResult<bool> ClientRead(string cateId)
        {
            var entity = GetForm(cateId);
            entity.ReadCount++;
            return RequestResult.Result(_Respository.UpdateFields(entity, "ReadCount"));
        }





    }



}
