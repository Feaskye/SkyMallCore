using SkyCore.GlobalProvider;
using SkyCoreLib.Utils;
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
    public class NewsService : ServiceBase<News>, INewsService
    {
        ISysLogRespository _LogRespository;
        INewsRespository _Respository;
        INewsCategoryRespository _NewsCategoryRespository;

        public NewsService(ISysLogRespository sysLogRespository, INewsRespository respository,
            INewsCategoryRespository NewsCategoryRespository
            )
        {
            _LogRespository = sysLogRespository;
            _Respository = respository;
            _NewsCategoryRespository = NewsCategoryRespository;
        }



        public List<News> GetList(string keyword = "")
        {
            var expression = base.GetFilterEnabled();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.Title.Contains(keyword));
                expression = expression.Or(t => t.Description.Contains(keyword));
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
        public PagedList<News> GetList(NewsSearchView searchView, int pageIndex = 1, int pageSize = 20)
        {
            var expression = base.GetFilterEnabled();
            if (!string.IsNullOrEmpty(searchView.Keyword))
            {
                expression = expression.And(t => t.Title.Contains(searchView.Keyword));
                expression = expression.Or(t => t.Description.Contains(searchView.Keyword));
            }
            //expression = expression.And(t => t.CategoryId == 2);
            return _Respository.GetPagedList(expression, pageIndex, pageSize);
        }


        public PagedList<NewsDetailView> GetNewsList(NewsSearchView searchView, int pageIndex, int pageSize)
        {
            var expression = base.GetFilterEnabled();
            if (!searchView.Keyword.IsEmpty())
            {
                expression = expression.And(t => t.Title.Contains(searchView.Keyword) ||
                                                                        t.Description.Contains(searchView.Keyword) ||
                                                                        t.ShortTitle.Contains(searchView.Keyword));
            }

            if (!searchView.Title.IsEmpty())
            {
                expression = expression.And(t => t.Title.Contains(searchView.Title));
            }

            if (!searchView.CategoryId.IsEmpty())
            {
                expression = expression.And(t => t.CategoryId == searchView.CategoryId);
            }

            if (searchView.StartDate.HasValue)
            {
                expression = expression.And(t => t.CreatorTime >= searchView.StartDate);
            }
            if (searchView.EndDate.HasValue)
            {
                expression = expression.And(t => t.CreatorTime <= searchView.EndDate);
            }
         
            //expression = expression.And(t => t.CategoryId == 2);
            return _Respository.GetPagedList(
                u => new NewsDetailView
                {
                    Id = u.Id,
                    Title = u.Title,
                    CoverUrl = u.CoverUrl,
                    ShortTitle = u.ShortTitle,
                    Description = u.Description,
                    Attachment = u.Attachment,
                    CategoryId = u.CategoryId,
                    Category = u.NewsCategory.Title,
                    ReadCount = u.ReadCount,
                    ResourceUrl = u.ResourceUrl,
                    CreatorTime = u.CreatorTime
                }
                , expression, pageIndex, pageSize, o => o.OrderBy(t => t.CreatorTime), "NewsCategory");
        }



        public News GetForm(string keyValue)
        {
            return _Respository.Get(keyValue);
        }


        /// <summary>
        /// 删除新闻
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public InvokeResult<bool> DeleteForm(string keyValue)
        {
            var entyFile = _Respository.GetFeild(u => u.CoverUrl + "," + u.Attachment + "," + u.ResourceUrl, w => w.Id == keyValue);
            var b = _Respository.Delete(keyValue);
            if (b)
            {
                //删除文件
                CoreContextProvider.DeleteFiles(entyFile);
            }
            return RequestResult.Result(b);
        }



        public InvokeResult<bool> DelNews(string memberId, string keyValue)
        {
            var b = _Respository.Delete(keyValue);
            return RequestResult.Result(b, "删除失败");
        }





        ///// <summary>
        ///// 获取分类加新闻列表
        ///// </summary>
        ///// <returns></returns>
        //public List<NewsCateDetailView> GetCateNews(int cateCount,int newsCount)
        //{
        //    var expression = base.Enabled();
        //    var cateList = _NewsCategoryRespository.GetFeilds(u => new NewsCateDetailView
        //    {
        //        Title = u.Title,
        //        Id = u.Id,
        //        CoverUrl = u.CoverUrl,
        //        ShortTitle = u.ShortTitle,
        //        NewsDetails = u.NewsList.Where(p=>p.EnabledMark == true).Select(k=>new NewsDetailView { Title = k.Title, Id = k.Id, CoverUrl = k.CoverUrl, ShortTitle = k.ShortTitle}).Take(newsCount).ToList()
        //    }, w => w.EnabledMark == true
        //                                            , o => o.OrderBy(b => b.SortCode), "NewsList")
        //                                            .Take(cateCount).ToList();
        //    return cateList;
        //}


        public InvokeResult<bool> SubmitForm(News roleEntity)
        {
            var b = _Respository.CreateOrUpdate(roleEntity);
            return RequestResult.Result(b);
        }


        public InvokeResult<bool> ClientRead(string id)
        {
            var entity = GetForm(id);
            entity.ReadCount++;
            return RequestResult.Result(_Respository.UpdateFields(entity, "ReadCount"));
        }


        public List<NewsDetailView> GetTopNewss(NewsTopEnum? topEnum, int count,string[] parentIds = null)
        {
            var expression = base.GetFilterEnabled();
            if (parentIds != null)
            {
                expression = expression.And(w=>parentIds.Contains(w.CategoryId));
            }
            var order = GetTopOrder(topEnum);
            var includeProperties = "";
            if (topEnum.HasValue && topEnum.Value == NewsTopEnum.Announcement)
            {
                expression = expression.And(w => w.NewsCategory != null && w.NewsCategory.Title.Contains("公告"));
                includeProperties = "NewsCategory";
            }


            return _Respository.GetFeilds(
             u => new NewsDetailView
             {
                 Id = u.Id,
                 Title = u.Title,
                 CategoryId= u.CategoryId,
                 ReadCount = u.ReadCount,
                 CoverUrl=u.CoverUrl
             }
             , expression, order,includeProperties).Take(count).ToList();
        }




        private Func<IQueryable<News>, IOrderedQueryable<News>> GetTopOrder(NewsTopEnum? topEnum)
        {
            Func<IQueryable<News>, IOrderedQueryable<News>> order = or => or.OrderBy(w => w.SortCode);
            if (topEnum.HasValue)
            {
                switch (topEnum)
                {
                    case NewsTopEnum.HotNews:
                        order = or => or.OrderByDescending(w => w.ReadCount);
                        break;
                    case NewsTopEnum.NewHotNews:
                        order = or => or.OrderByDescending(w => w.CreatorTime).OrderByDescending(d => d.ReadCount);
                        break;
                    case NewsTopEnum.Announcement:
                        order = or => or.OrderByDescending(w => w.NewsCategory.Title.Contains("公告")).OrderByDescending(d => d.CreatorTime);
                        break;
                }
            }
            return order;
        }



    }


}
