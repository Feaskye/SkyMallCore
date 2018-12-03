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
    public class NewsCategoryService : ServiceBase<NewsCategory>, INewsCategoryService
    {
        ISysLogRespository _LogRespository;
        INewsCategoryRespository _Respository;
        INewsService _INewsService;

        public NewsCategoryService(ISysLogRespository sysLogRespository, INewsCategoryRespository NewsCategoryRespository,
            INewsService NewsService
            )
        {
            _LogRespository = sysLogRespository;
            _Respository = NewsCategoryRespository;
            _INewsService = NewsService;
        }



        public List<NewsCategory> GetList(NewsCateSearchView searchView)
        {
            var expression = ExtLinq.True<NewsCategory>();

            if (!string.IsNullOrEmpty(searchView.Keyword))
            {
                expression = expression.And(t => t.Title.Contains(searchView.Keyword));
                expression = expression.Or(t => t.ShortTitle.Contains(searchView.Keyword));
            }
            return _Respository.Get(expression,o=>o.OrderBy(t=>t.SortCode)).ToList();
        }

        public PagedList<NewsCateDetailView> GetCateList(NewsCateSearchView searchView, int pageIndex = 1, int pageSize = 20)
        {
            var expression = base.GetFilterEnabled();
            var order = base.Order();
            if (searchView.ParentId != null)
            {
                expression = expression.And(w => searchView.ParentId.Contains(w.ParentId));
            }

            if (searchView.IsRemmand.HasValue)
            {
                order = o => o.OrderBy(w => w.SortCode);
            }

            var data = _Respository.GetPagedList(u => new NewsCateDetailView
            {
                Id = u.Id,
                Title = u.Title,
                CreatorTime = u.CreatorTime,
                ParentId = u.ParentId,
                Category = u.Category.Title,
                ShortTitle = u.ShortTitle,
                CoverUrl = u.CoverUrl,
                //ReadCount = u.ReadCount, //todo 会员?
            }, expression, pageIndex, pageSize, order, "Category");

            if (searchView.IsRemmand.HasValue)
            {
                var parentIds = data.Select(u => u.Id).ToArray();
                var Newss = _INewsService.GetTopNewss(NewsTopEnum.NewHotNews, 14, parentIds);
                data.ForEach(cate => {
                    cate.NewsDetails = Newss.Where(w=>w.CategoryId == cate.Id).ToList();
                });
            }

            return data;
        }

        /// <summary>
        /// 分类列表（按需加载）
        /// </summary>
        /// <param name="currentId"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public List<ListItem> GetCateList(string currentId, string parentId,bool containsChild=false)
        {
            var expression = ExtLinq.True<NewsCategory>();
            if (!parentId.IsEmpty())
            {
                expression = expression.And(t => t.ParentId == parentId);
            }
            if (!containsChild)
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
            return _Respository.GetFeilds(u => new ListItem { Code = u.Id, Text = u.Title, ParentId = u.ParentId }, w => w.Id == id).FirstOrDefault();
        }

        public NewsCategory GetForm(string keyValue)
        {
            return _Respository.Get(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            _Respository.Delete(keyValue);
        }
        public void SubmitForm(NewsCategory NewsCategory, string[] permissionIds, string keyValue)
        {
            //if (!string.IsNullOrEmpty(keyValue))
            //{
            //    NewsCategory.Id = keyValue;
            //}
            //else
            //{
            //    NewsCategory.Id = Common.GuId();
            //}
            //var moduledata = _SysModuleService.GetList();
            //var buttondata = _SysModuleButtonService.GetList();
            //List<NewsCategoryAuthorize> NewsCategoryAuthorizes = new List<NewsCategoryAuthorize>();
            //foreach (var itemId in permissionIds)
            //{
            //    NewsCategoryAuthorize NewsCategoryAuthorize = new NewsCategoryAuthorize();
            //    NewsCategoryAuthorize.Id = Common.GuId();
            //    NewsCategoryAuthorize.ObjectType = 1;
            //    NewsCategoryAuthorize.ObjectId = NewsCategory.Id;
            //    NewsCategoryAuthorize.ItemId = itemId;
            //    if (moduledata.Find(t => t.Id == itemId) != null)
            //    {
            //        NewsCategoryAuthorize.ItemType = 1;
            //    }
            //    if (buttondata.Find(t => t.Id == itemId) != null)
            //    {
            //        NewsCategoryAuthorize.ItemType = 2;
            //    }
            //    NewsCategoryAuthorizes.Add(NewsCategoryAuthorize);
            //}
            //_Respository.SubmitForm(NewsCategory, NewsCategoryAuthorizes, keyValue);
        }


        public InvokeResult<bool> SubmitForm(NewsCategory roleEntity)
        {
            bool b= _Respository.CreateOrUpdate(roleEntity);
            return RequestResult.Result(b);
        }




        public List<ListItem> GetHotTopics(int count)
        {
            var expression = base.GetFilterEnabled();
            expression = w => w.ParentId != null;

            return _Respository.GetFeilds(u => new ListItem { Text = u.Title, Code = u.Id, PicUrl = u.CoverUrl },
                expression,
                o => o.OrderByDescending(b => b.CreatorTime)
            ).Take(count).ToList();
        }



    }



}
