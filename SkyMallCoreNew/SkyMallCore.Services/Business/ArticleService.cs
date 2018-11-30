
using Microsoft.Extensions.Logging;
using SkyCore.GlobalProvider;
using SkyMallCore.Core;
using SkyMallCore.Data;
using SkyMallCore.Models;
using SkyMallCore.Respository;
using SkyMallCore.ViewModel;
using SkyMallCore.ViewModel.Business;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace SkyMallCore.Services
{
    public class ArticleService : ServiceBase<Article>, IArticleService
    {
        ISysLogRespository _LogRespository;
        IArticleRespository _Respository;
        IArticleCategoryRespository _ArticleCategoryRespository;
        IArticleCategoryService _IArticleCategoryService;
        IMemberRespository _IMemberRespository;
        ILogger _logger;

        public ArticleService(ISysLogRespository sysLogRespository, IArticleRespository respository,
            IArticleCategoryRespository articleCategoryRespository, IMemberRespository memberService,
            IArticleCategoryService articleCategoryService
            )
        {
            _LogRespository = sysLogRespository;
            _Respository = respository;
            _ArticleCategoryRespository = articleCategoryRespository;
            _IMemberRespository = memberService;
            _IArticleCategoryService = articleCategoryService;
            _logger = CoreContextProvider.GetLogger("ArticleService");
        }



        public List<Article> GetList(string keyword = "")
        {
            var expression = GetFilterEnabled();
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
        public PagedList<ArticleDetailView> GetTopicArticleList(ArticleSearchView searchView, int pageIndex = 1, int pageSize = 20)
        {
            var expression = GetFilterEnabled();
            var orderpr = base.Order();
            orderpr = o => o.OrderBy(t => t.CreatorTime);
            if (!searchView.SpecialTopicId.IsEmpty())
            {
                expression = base.GetFilterEnabled();
                expression = expression.And(t => t.SpecialTopicId == searchView.SpecialTopicId);
            }
            if (!string.IsNullOrEmpty(searchView.Keyword))
            {
                expression = expression.And(t => t.Title.Contains(searchView.Keyword));
                expression = expression.Or(t => t.Description.Contains(searchView.Keyword));
            }
            return _Respository.GetPagedList(
              u => new ArticleDetailView
              {
                  Id = u.Id,
                  Title = u.Title,
                  ReadCount = u.ReadCount,
                  ResourceType=u.ResourceType
              }
              , expression, pageIndex, pageSize, orderpr);
        }


        public PagedList<ArticleDetailView> GetBooks(ArticleSearchView searchView, int pageIndex, int pageSize)
        {
            var expression = GetFilterEnabled();
            var includeProperties = "ArticleCategory";
            //if (!searchView.SpecialTopicId.IsEmpty())
            //{
            //    includeProperties = "";
            //    expression = base.GetFilterEnabled();
            //    expression = expression.And(t => t.SpecialTopicId == searchView.SpecialTopicId);
            //}
            var orderpr = base.Order();
            orderpr = o => o.OrderBy(t => t.CreatorTime);

            if (!string.IsNullOrEmpty(searchView.Keyword))
            {
                expression = expression.And(t => t.Title.Contains(searchView.Keyword) ||
                                                                    (!t.Description.IsEmpty() &&   t.Description.Contains(searchView.Keyword)) ||
                                                                    (!t.Keyword.IsEmpty() && t.Keyword.Contains(searchView.Keyword)));
            }

            if (!searchView.Title.IsEmpty())
            {
                expression = expression.And(t => t.Title.Contains(searchView.Title));
            }
          
            if (!string.IsNullOrEmpty(searchView.MemberId))
            {
                expression = expression.And(t => t.MemberId == searchView.MemberId);
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
            if (!searchView.ResourceType.IsEmpty())
            {
                //资源类型多样化查询处理
                if (searchView.ResourceType == "pdf" || searchView.ResourceType == "doc" || searchView.ResourceType == "docx" || 
                    searchView.ResourceType == "ppt" || searchView.ResourceType == "pptx")
                {
                    expression = expression.And(t => t.ResourceType == searchView.ResourceType);
                }
                else if (searchView.ResourceType == "jpg")
                {
                    expression = expression.And(t => ConfigManager.UploadAllowImgExtension.Contains(t.ResourceType));
                }
                else if (searchView.ResourceType == "mp4")
                {
                    expression = expression.And(t => ConfigManager.UploadAllowVideoExtension.Contains(t.ResourceType));
                }
                else if (searchView.ResourceType == "mp3")
                {
                    expression = expression.And(t => ConfigManager.UploadAllowVoiceExtension.Contains(t.ResourceType));
                }
                else if (searchView.ResourceType == "swf")
                {
                    expression = expression.And(t => ConfigManager.UploadAllowFlashExtension.Contains(t.ResourceType));
                }
                
            }
            if (searchView.StartScore.HasValue)
            {
                expression = expression.And(t => t.RequireAmount > searchView.StartScore);
            }
            if (searchView.EndScore.HasValue)
            {
                expression = expression.And(t => t.RequireAmount <= searchView.EndScore);
            }
            if (searchView.StartPage.HasValue)
            {
                expression = expression.And(t => t.PageCount >= searchView.StartPage);
            }
            if (searchView.EndPage.HasValue)
            {
                expression = expression.And(t => t.PageCount <= searchView.EndPage);
            }
            //审核状态
            if (searchView.BookStatus.HasValue)
            {
                expression = expression.And(t => t.BookStatus == searchView.BookStatus);
            }
            if (!searchView.SearchStatus.IsEmpty())
            {
                if (searchView.SearchStatus == "0")
                {
                    orderpr = o => o.OrderByDescending(b => b.CreatorTime);
                }
                if (searchView.SearchStatus == "1")
                {
                    orderpr = o => o.OrderByDescending(b => b.DownloadCount);
                }
                if (searchView.SearchStatus == "2")
                {
                    orderpr = o => o.OrderByDescending(b => b.ReadCount);
                }
            }
            //分类
            if (!searchView.ParentCategoryId.IsEmpty())
            {
                if (searchView.CategoryId.IsEmpty())
                {
                    expression = expression.And(t => t.ArticleCategory.ParentId == searchView.ParentCategoryId);
                }
            }
            if (!searchView.CategoryId.IsEmpty())
            {
                expression = expression.And(t => t.CategoryId == searchView.CategoryId);
            }

            //expression = expression.And(t => t.CategoryId == 2);
            var data = _Respository.GetPagedList(
                u => new ArticleDetailView
                {
                    Id = u.Id,
                    Title = u.Title,
                    CoverUrl = u.CoverUrl,
                    ShortTitle = u.ShortTitle,
                    Description = u.Description,
                    Attachment = u.Attachment,
                    CategoryId = u.CategoryId,
                    Category = (u.ArticleCategory == null ? null : u.ArticleCategory.Title),
                    ReadCount = u.ReadCount,
                    DownloadCount = u.DownloadCount,
                    ResourceUrl = u.ResourceUrl,
                    CreatorTime = u.CreatorTime,
                    ResourceSize = u.ResourceSize,
                    RequireAmount = u.RequireAmount,
                    BookStatus = u.BookStatus,
                    AuditMessage = u.AuditMessage,
                    ResourceType = u.ResourceType,
                    MemberId = u.MemberId
                }
                , expression, pageIndex, pageSize, orderpr, includeProperties);
            
            
            if (data.Any()&&searchView.HasMember.HasValue && searchView.HasMember == true)
            {
                var meberIds = data.Select(u => u.MemberId).Distinct().ToArray();
                if (meberIds.Any())
                {
                    var memList = _IMemberRespository.GetFeilds(u => new { u.Id, u.UserName }, w => w.EnabledMark == true && meberIds.Contains(w.Id)).ToDictionary(k => k.Id, v => v.UserName);
                    data.ForEach(article => {
                        article.Member = "系统管理员";
                        if (!article.MemberId.IsEmpty())
                        {
                            article.Member = memList.TryGetValue(article.MemberId);
                        }
                    });
                }

            }
            return data;
        }



        public Article GetForm(string keyValue)
        {
            return _Respository.Get(keyValue);
        }
        public InvokeResult<bool> DeleteForm(string keyValue)
        {
            //被购买不可删除
            var scoreDao = CoreContextProvider.GetService<IMemberScoreRespository>();
            if (scoreDao.Any(w => w.KeyId == keyValue))
            {
                return RequestResult.Failed<bool>("该文库已被购买，不可删除");
            }

            var entyFile = _Respository.GetFeild(u => u.CoverUrl + "," + u.Attachment + "," + u.ResourceUrl, w => w.Id == keyValue);
            var b = _Respository.Delete(keyValue);
            if (b)
            {
                //删除文件
                CoreContextProvider.DeleteFiles(entyFile);
            }
            return RequestResult.Result(b);
        }


        public InvokeResult<bool> DelArticle(string memberId, string keyValue)
        {
            //被购买不可删除
            if (!_Respository.Any(w => w.MemberId == memberId && w.Id == keyValue))
            {
                return RequestResult.Failed<bool>("该文库资源不属于该账户，删除失败");
            }
            var b = DeleteForm(keyValue);
            if (!b.Success)
            {
                return b;
            }
            return RequestResult.Success(true);
        }


        /// <summary>
        /// 审核状态
        /// </summary>
        /// <param name="aid"></param>
        /// <param name="auditStatus"></param>
        /// <returns></returns>
        public InvokeResult<bool> AuditArticle(string aid, int auditStatus, string auditMessage)
        {
            var entity = GetForm(aid);
            entity.BookStatus = auditStatus;
            entity.AuditMessage = auditMessage;
            using (var trans = _Respository.BeginTransaction())
            {
                try
                {
                    var updateFileds = new List<string>() { "BookStatus", "AuditMessage" };
                    if (!entity.ResourceType.IsEmpty() && auditStatus == (int)BookStatus.审核通过)
                    {
                        if (entity.ResourceType == "pdf" || entity.ResourceType == "doc" || entity.ResourceType == "docx"
                            || entity.ResourceType == "ppt" || entity.ResourceType == "pptx" || entity.ResourceType == "xls"
                            || entity.ResourceType == "xlsx")
                        {
                            entity.HasImages = false;
                        }
                        else
                        {
                            entity.HasImages = true;
                        }
                        //updateFileds.Add("HasImages");
                        if (entity.CoverUrl.IsEmpty() && !entity.Attachment.IsEmpty())
                        {
                            var attFile = entity.Attachment.Split(',')[0];
                            entity.CoverUrl = $"{attFile.Replace(FileHelper.GetExtension(FileHelper.MapFilePath(attFile)), "")}/{FileHelper.GetFileNameNoExtension(attFile)}1.jpg";
                            updateFileds.Add("CoverUrl");
                        }
                    }
                    var b = _Respository.UpdateFields(entity, updateFileds.ToArray());
                    if (!b)
                    {
                        trans.Rollback();
                        return RequestResult.Failed<bool>("审核处理积分失败");
                    }
                    if (auditStatus == (int)BookStatus.审核通过)
                    {
                        //关联专题      数量处理
                        if (!entity.SpecialTopicId.IsEmpty())
                        {
                            var articleTopicRespository = CoreContextProvider.GetService<IArticleTopicRespository>();
                            var topic = articleTopicRespository.Get(entity.SpecialTopicId);
                            if (topic != null)
                            {
                                topic.ResourceCount++;
                                if (!articleTopicRespository.UpdateFields(topic, "ResourceCount"))
                                {
                                    trans.Rollback();
                                    _logger.LogError($"审核时更新{topic.Id}:topic.ResourceCount+1失败！");
                                    return RequestResult.Failed<bool>("审核处理失败，请重试");
                                }
                            }
                        }

                        //积分
                        var memberScoreService = CoreContextProvider.GetService<IMemberScoreService>();
                        var scoreResult = memberScoreService.AddScore(entity.MemberId, ScoreType.addbook, aid);
                        if (!scoreResult.Success)
                        {
                            trans.Rollback();
                            _logger.LogError(scoreResult.Message);
                            return RequestResult.Failed<bool>("审核处理积分失败");
                        }
                    }
                    trans.Commit();
                    //资源文件生成图片操作
                    if (!entity.ResourceType.IsEmpty() && auditStatus == (int)BookStatus.审核通过)
                    {
                        //if (entity.ResourceType == "pdf" || entity.ResourceType == "doc" || entity.ResourceType == "docx"
                        //  || entity.ResourceType == "ppt" || entity.ResourceType == "pptx" || entity.ResourceType == "xls"
                        //  || entity.ResourceType == "xlsx")
                        CoreContextProvider.ConvertFileToImage(entity.Attachment.Split(',').ToList(), entity.PageCount, _logger);
                    }
                    return RequestResult.Success(true);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    _logger.LogError($"[method.AuditArticle({aid},{auditStatus})]:" + ex.Message);
                    return RequestResult.Failed<bool>("审核失败，数据异常");
                }
            }
        }

        public InvokeResult<bool> SubmitForm(Article entity)
        {
            var expression = ExtLinq.True<Article>();
            expression = expression.And(w => w.Title == entity.Title);
            if (!entity.Id.IsEmpty())
            {
                expression = expression.And(w => w.Id != entity.Id);
            }
            if (_Respository.Any(expression))
            {
                return RequestResult.Failed<bool>("该文库标题已存在，请重新输入");
            }
            var b = _Respository.CreateOrUpdate(entity);
            return RequestResult.Result(b);
        }

        /// <summary>
        /// 审核状态统计
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<ListItem> GetAuditStatics(string userId)
        {
            var auditStatics = EnumCommon.GetEnumList<BookStatus>().Select(u => new ListItem { Text = u.Value,  Type = u.Key }).ToList();
            var expression = GetFilterEnabled();
            expression = expression.And(w => w.MemberId == userId);
            var articleCateStatics = _Respository.GetFeilds(u => new
            { u.BookStatus, ArticleId = u.Id },
                expression, o => o.OrderBy(b => b.SortCode), "")
                .GroupBy(g => g.BookStatus).Select(h => new { h.Key, Count = h.Count() }).ToList();

            auditStatics.ForEach(art=> {
                art.Code = articleCateStatics.Where(u => u.Key + "" == art.Type).Select(u => u.Count).FirstOrDefault() + "";
            });

            return auditStatics;
        }

        /// <summary>
        /// 分类下文库统计
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<ListItem> GetBookStatics(string userId)
        {
            //所有分类
            var list = _IArticleCategoryService.GetCateList(null, null, false);
            //用户相关文库
            var expression = GetFilterEnabled();
            expression = expression.And(w => w.MemberId == userId);
            var articleCateStatics = _Respository.GetFeilds(u => new
            { Category = u.ArticleCategory.Title, ArticleId = u.Id, u.ArticleCategory.ParentId },
                expression, o => o.OrderBy(b => b.CreatorTime),
                "ArticleCategory").Select(u => new ListItem { Text = u.Category, ParentId = u.ParentId, Code = u.ArticleId }).ToList();

            var parentIds = articleCateStatics.Where(u => u.ParentId != null).Select(u => u.ParentId).ToArray();
            if (parentIds.Any())//取出父级类别名
            {
                var dics = _IArticleCategoryService.GetCates(parentIds).ToDictionary(k => k.Code, v => v.Text);
                articleCateStatics.ForEach(cate => {
                    if (!cate.ParentId.IsEmpty())
                    {
                        cate.Text = dics.TryGetValue(cate.ParentId);
                    }
                });
            }
            var staticsTictionary = articleCateStatics.Where(w => !w.Text.IsEmpty()).GroupBy(g => g.Text).Select(u => new { u.Key, Count = u.Count() }).ToDictionary(k => k.Key, v => v.Count);


            list.ForEach(data => {
                data.Code = "0";
                if (staticsTictionary.Any(w => w.Key == data.Text))
                {
                    data.Code = staticsTictionary.TryGetValue(data.Text)+"";
                }
            });

            return list;
        }
        

        //todo 重复
        public List<ArticleDetailView> GetTopArticles(ArticleTopEnum? topEnum, int count,string[] parentIds = null)
        {
            var expression = GetFilterEnabled();
            expression = expression.And(w=>w.BookStatus == (int)BookStatus.审核通过);
            if (parentIds != null)
            {
                expression = expression.And(w => parentIds.Contains(w.CategoryId));
            }
            var order = GetTopOrder(topEnum);
            if (topEnum.HasValue && (topEnum == ArticleTopEnum.HotPPT || topEnum == ArticleTopEnum.NewPPT))
            {
                expression = expression.And(w => w.ResourceType == "ppt");
                if (topEnum == ArticleTopEnum.NewPPT)
                {
                    var now = DateTime.Now.Date;
                    expression = expression.And(w => w.CreatorTime > now);
                }
            }

            return _Respository.GetFeilds(
             u => new ArticleDetailView
             {
                 Id = u.Id,
                 Title = u.Title,
                 CategoryId = u.CategoryId,
                 ResourceType = u.ResourceType,
                 ReadCount = u.ReadCount,
                 CoverUrl = u.CoverUrl,
                 SpecialTopicId = u.SpecialTopicId
             }
             , expression, order).Take(count).ToList();
        }




        private Func<IQueryable<Article>, IOrderedQueryable<Article>> GetTopOrder(ArticleTopEnum? topEnum)
        {
            Func<IQueryable<Article>, IOrderedQueryable<Article>> order = or => or.OrderBy(w => w.SortCode);
            if (topEnum.HasValue)
            {
                switch (topEnum)
                {
                    case ArticleTopEnum.HotArticle:
                        order = or => or.OrderByDescending(w => w.ReadCount);
                        break;
                    case ArticleTopEnum.MoreDownload:
                        order = or => or.OrderByDescending(w => w.DownloadCount);
                        break;
                    case ArticleTopEnum.SiteHot:
                        order = or => or.OrderBy(w => w.SortCode);
                        break;
                    case ArticleTopEnum.NewArticle:
                        order = or => or.OrderByDescending(w => w.CreatorTime);
                        break;
                    case ArticleTopEnum.NewHotArticle:
                        order = or => or.OrderByDescending(w =>w.CreatorTime).OrderByDescending(d=>d.DownloadCount);
                        break;
                    case ArticleTopEnum.HotPPT:
                        order = or => or.OrderByDescending(w => w.ReadCount);
                        break;
                    case ArticleTopEnum.NewPPT:
                        order = or => or.OrderByDescending(w => w.CreatorTime);
                        break;
                }
            }
            return order;
        }



        /// <summary>
        /// 阅读数量
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public InvokeResult<bool> ClientRead(string id)
        {
            var entity = GetForm(id);
            entity.ReadCount++;
            return RequestResult.Result(_Respository.UpdateFields(entity, "ReadCount"));
        }

        /// <summary>
        /// 下载数量
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public InvokeResult<Article> ClientDownLoad(string id)
        {
            var entity = GetForm(id);
            if (CoreContextProvider.CurrentMember.UserId == entity.MemberId)
            {
                return RequestResult.Success(entity);
            }
            //下载更新积分
            //此下载直接按购买处理
            var memScoreService = CoreContextProvider.GetService<IMemberScoreService>();
            var scoreResult = memScoreService.MarketBuy(CoreContextProvider.CurrentMember.UserId, ScoreType.buybook, id, entity.RequireAmount, entity.MemberId);
            if (!scoreResult.Success)
            {
                _logger.LogError($"用户{CoreContextProvider.CurrentMember.Account}[{CoreContextProvider.CurrentMember.UserId}]下载文库[{id}]积分更新失败：");
                return RequestResult.Failed<Article>(scoreResult.Message??"下载操作失败请重试！");
            }
            entity.DownloadCount++;
            var b = _Respository.UpdateFields(entity, "DownloadCount");
            return RequestResult.Success(entity);
        }

        /// <summary>
        /// 资源总数量
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public int GetTotalBooks(string categoryId = null)
        {
            var expression = GetFilterEnabled();
            string includeProperties = "";
            if (!categoryId.IsEmpty())
            {
                includeProperties = "ArticleCategory";
                expression = expression.And(w=>w.CategoryId == categoryId || w.ArticleCategory.ParentId == categoryId);
            }
            return _Respository.Count(expression, includeProperties);
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="articles"></param>
        /// <returns></returns>
        public InvokeResult<bool> AddBetchArticle(List<Article> articles)
        {
            _Respository.AddBetch(articles);
            return RequestResult.Success(true);
        }

        /// <summary>
        /// 获取所有标题
        /// </summary>
        /// <returns></returns>
        public List<string> GetTitles()
        {
            var expression = GetFilterEnabled();
            return _Respository.GetFeilds(u=>u.Title, expression).ToList();
        }

        /// <summary>
        /// 获取所有文件
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        public List<ListItem> GetPackageByTopicId(string topicId)
        {
            Expression<Func<Article, bool>> expression = 
                                        w=> w.SpecialTopicId == topicId && !string.IsNullOrWhiteSpace(w.Attachment);
            return _Respository.GetFeilds(u => new ListItem { Code = u.Attachment, Text = u.Title }, expression).ToList();
        }


        public void AddBetchBooks(List<Article> newArticleList, int perCount = 50)
        {
            _Respository.AddBetch(newArticleList, perCount);
        }




        ///// <summary>
        ///// 专题信息 作者、资源量
        ///// </summary>
        ///// <param name="memberId"></param>
        ///// <returns></returns>
        //public MemResourceStaticsView GetMemResourceStatics(string memberId)
        //{
        //    var data = new MemResourceStaticsView() { UserName = "admin"};
        //    Member mem = new Member();
        //    if (!memberId.IsEmpty())
        //    {
        //        var memberRespository = SkyCore.GlobalProvider.CoreContextProvider.GetService<IMemberRespository>();
        //        var members = memberRespository.GetMemberInfos(new string[] { memberId });
        //        if (members != null && members.Any())
        //        {
        //            data.UserName = members.FirstOrDefault().UserName;
        //            data.HeadIcon = members.FirstOrDefault().HeadIcon;
        //            data.UserLevel = members.FirstOrDefault().UserLevel;
        //            data.UpdateTime = (members.FirstOrDefault().LastModifyTime ?? members.FirstOrDefault().CreatorTime)??DateTime.Now.AddDays(-1);
        //        }
        //    }

        //    var expression = base.Enabled();
        //    if (memberId.IsEmpty())
        //    {
        //        expression = expression.And(w => w.MemberId == null);
        //    }
        //    else
        //    {
        //        expression = expression.And(w => w.MemberId == memberId);
        //    }
        //    data.UpdateTime = _Respository.Max(w=> w.LastModifyTime ?? w.CreatorTime, expression)??DateTime.Now;
        //    data.ResourceCount = _Respository.Count(expression);
        //    data.ReadCount = _Respository.Get(expression).Sum(s=>s.ReadCount);
        //}




        public override Expression<Func<Article, bool>> GetFilterEnabled()
        {
            var filter = base.GetFilterEnabled();
            return filter.And(w => !(!w.SpecialTopicId.IsEmpty() && w.CategoryId.IsEmpty()));
        }


    }


}
