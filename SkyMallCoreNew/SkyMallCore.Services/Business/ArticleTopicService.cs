using Microsoft.Extensions.Logging;
using SkyCore.GlobalProvider;
using SkyMallCore.Core;
using SkyMallCore.Models;
using SkyMallCore.Respository;
using SkyMallCore.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace SkyMallCore.Services
{
    public class ArticleTopicService :ServiceBase<ArticleTopic>, IArticleTopicService
    {
        ISysLogRespository _LogRespository;
        IArticleTopicRespository _Respository;
        ILogger _logger;
        IArticleRespository _ArticleRespository;
        public ArticleTopicService(ISysLogRespository sysLogRespository, IArticleTopicRespository ArticleTopicRespository
            , IArticleRespository articleRespository)
        {
            _LogRespository = sysLogRespository;
            _Respository = ArticleTopicRespository;
            _ArticleRespository = articleRespository;
            _logger = CoreContextProvider.GetLogger("IArticleTopicService");
        }



        public List<ArticleTopic> GetList(ArticleTopicSearchView searchView)
        {
            var expression = ExtLinq.True<ArticleTopic>();

            if (!string.IsNullOrEmpty(searchView.Keyword))
            {
                expression = expression.And(t => t.Title.Contains(searchView.Keyword));
                expression = expression.Or(t => t.ShortTitle.Contains(searchView.Keyword));
            }
            return _Respository.Get(expression,o=>o.OrderBy(t=>t.SortCode)).ToList();
        }


        /// <summary>
        /// 包含详细信息
        /// </summary>
        /// <param name="searchView"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public PagedList<TopicDetailView> GetTopicInfoList(ArticleTopicSearchView searchView, int pageIndex, int pageSize)
        {
            var expression = base.GetFilterEnabled();
            var order = base.Order();

            if (searchView.IgnoreCate.HasValue && searchView.IgnoreCate == true)
            {
                expression = expression.And(w => w.ParentId != null);
                if (!searchView.ParentId.IsEmpty())
                {
                    expression = expression.And(w => searchView.ParentId.Contains(w.ParentId));
                }
            }
            else
            {
                if (searchView.ParentId == null)
                {
                    expression = expression.And(w => w.ParentId == null);
                }
                else
                {
                    expression = expression.And(w => searchView.ParentId.Contains(w.ParentId));
                }
            }

            if (!searchView.TopicId.IsEmpty())
            {
                expression = expression.And(w => w.Id == searchView.TopicId);
            }


            if (searchView.HotTopic.HasValue && searchView.HotTopic.Value)
            {
                order = o => o.OrderByDescending(w => w.ReadCount);
            }

            var data = _Respository.GetPagedList(u => new TopicDetailView
            {
                Id = u.Id,
                Title = u.Title,
                CreatorTime = u.CreatorTime,
                ParentId = u.ParentId,
                ShortTitle = u.ShortTitle,
                CoverUrl = u.CoverUrl,
                BigCoverUrl = u.BigCoverUrl,
                ReadCount = u.ReadCount,
                MemberId = u.CreatorUserId,
                PackageAmount = u.PackageAmount,
                TopicStatus = u.TopicStatus,
                MemberName = "admin",
                ResourceCount=u.ResourceCount
            }, expression, pageIndex, pageSize, o => o.OrderBy(b => b.SortCode));

            var parentIds = data.Where(w => w.ParentId != null).Select(u => u.Id).Distinct().ToList();
            Dictionary<string, int> valuePairs = new Dictionary<string, int>();
            if (parentIds.Any())
            {
                //资源数量统计
                if (parentIds.Any())
                {
                    var articleService = CoreContextProvider.GetService<IArticleRespository>();
                    valuePairs = articleService.GetStaticsCount(parentIds, true);
                }
            }

            //默认全是管理员
            List<Member> memberDics = new List<Member>();
            var memberIds = data.Where(w => w.MemberId != null).Select(u => u.MemberId).Distinct().ToArray();
            if (memberIds.Any())
            {
                var memberRespository = CoreContextProvider.GetService<IMemberRespository>();
                memberDics = memberRespository.GetMemberInfos(memberIds);
            }

            //相关用户统计
            data.ForEach(cate =>
            {
                var statics = valuePairs.TryGetValue(cate.Id);
                if (cate.ResourceCount == 0 && parentIds.Any())
                {
                    cate.ResourceCount = statics;
                }
                if (memberDics.Any())
                {
                    var mem = memberDics.Where(w => w.Id == cate.MemberId).FirstOrDefault();
                    if (mem != null)
                    {
                        cate.MemberName = mem.UserName;
                        cate.MemHeader = mem.HeadIcon;
                        cate.UserLevel = mem.UserLevel;
                    }
                }
            });


            return data;
        }


        /// <summary>
        /// 加载列表
        /// </summary>
        /// <param name="searchView"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public PagedList<TopicDetailView> GetTopicsList(ArticleTopicSearchView searchView, int pageIndex = 1, int pageSize = 20)
        {
            var expression = base.GetFilterEnabled();
            var order = base.Order();

            if (!searchView.Keyword.IsEmpty())
            {
                expression = expression.And(w => w.Title.Contains(searchView.Keyword));
            }

            if (searchView.IgnoreCate.HasValue && searchView.IgnoreCate == true)
            {
                expression = expression.And(w => w.ParentId != null);
                if (!searchView.ParentId.IsEmpty())
                {
                    expression = expression.And(w => searchView.ParentId.Contains(w.ParentId));
                }
            }
            else
            {
                //if (searchView.ParentId == null)
                //{
                //    expression = expression.And(w => w.ParentId == null);
                //}
                //else
                //{
                //    expression = expression.And(w => searchView.ParentId.Contains(w.ParentId));
                //}
                if (searchView.ParentId != null)
                {
                    searchView.ParentId = searchView.ParentId.Where(w => !w.IsEmpty()).ToArray();
                    if (searchView.ParentId.Any())
                    {
                        expression = expression.And(w => searchView.ParentId.Contains(w.ParentId));
                    }
                }
            }

            if (searchView.TopicStatus.HasValue)
            {
                expression = expression.And(w => w.TopicStatus == (int)searchView.TopicStatus);
            }


            if (!searchView.MemberId.IsEmpty())
            {
                expression = expression.And(w => w.CreatorUserId == searchView.MemberId);
            }


            if (searchView.IsRemmand.HasValue)
            {
                order = o => o.OrderBy(w => w.SortCode);
            }

            var data = _Respository.GetPagedList(u => new TopicDetailView
            {
                Id = u.Id,
                Title = u.Title,
                CreatorTime = u.CreatorTime,
                ParentId = u.ParentId,
                ShortTitle = u.ShortTitle,
                CoverUrl = u.CoverUrl,
                BigCoverUrl =u.BigCoverUrl,
                ReadCount = u.ReadCount,
                MemberId = u.CreatorUserId,
                TopicStatus= u.TopicStatus,
                EnabledMark = u.EnabledMark,
                Description = u.Description,
                 SortCode=u.SortCode,
                  PackageAmount=u.PackageAmount
            }, expression, pageIndex, pageSize, order);

            if (searchView.IsRemmand.HasValue)
            {
                var parentIds = data.Select(u => u.Id).ToArray();
                var articles = CoreContextProvider.GetService<IArticleService>().GetTopArticles(ArticleTopEnum.NewHotArticle, 14, parentIds);
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
        /// <returns></returns>
        public List<ListItem> GetTopicCateList(string currentId, string parentId,bool containsChild=false)
        {
            var expression = ExtLinq.True<ArticleTopic>();
            if (!parentId.IsEmpty())
            {
                expression = expression.And(t => t.ParentId == parentId);
            }
            else if (!containsChild)
            {
                expression = expression.And(t => t.ParentId == null);
            }
            else
            {
                expression = expression.And(t => t.TopicStatus == (int)TopicStatus.Audited);
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

        /// <summary>
        /// 分类列表
        /// </summary>
        /// <param name="parentIds"></param>
        /// <returns></returns>
        public List<ListItem> GetCates(string[] parentIds)
        {
            var expression = base.GetFilterEnabled();
            expression = expression.And(w=>w.ParentId == null);
            if (parentIds != null)
            {
                expression = expression.And(w => parentIds.Contains(w.Id));
            }
            return _Respository.GetFeilds(u => new ListItem { Code = u.Id, Text = u.Title }, expression).ToList();
        }


        public ListItem GetCate(string id)
        {
            return _Respository.GetFeilds(u => new ListItem { Code = u.Id, Text = u.Title, ParentId = u.ParentId }, w => w.Id == id).FirstOrDefault();
        }

        public ArticleTopic GetForm(string keyValue)
        {
            return _Respository.Get(keyValue);
        }
        public InvokeResult<bool> DeleteForm(string keyValue)
        {
            //被购买不可删除
            var scoreDao = CoreContextProvider.GetService<IMemberScoreRespository>();
            if (scoreDao.Any(w => w.KeyId == keyValue))
            {
                return RequestResult.Failed<bool>("该专题已被购买，不可删除");
            }

            var entyFile = _Respository.GetFeild(u => u.CoverUrl + "," + u.BigCoverUrl+","+ u.Attachment, w => w.Id == keyValue);
            var b = _Respository.Delete(keyValue);
            if (b)
            {
                //删除文件
                CoreContextProvider.DeleteFiles(entyFile);
            }

            return RequestResult.Result(b);
        }
        public void SubmitForm(ArticleTopic ArticleTopic, string[] permissionIds, string keyValue)
        {
            //if (!string.IsNullOrEmpty(keyValue))
            //{
            //    ArticleTopic.Id = keyValue;
            //}
            //else
            //{
            //    ArticleTopic.Id = Common.GuId();
            //}
            //var moduledata = _SysModuleService.GetList();
            //var buttondata = _SysModuleButtonService.GetList();
            //List<ArticleTopicAuthorize> ArticleTopicAuthorizes = new List<ArticleTopicAuthorize>();
            //foreach (var itemId in permissionIds)
            //{
            //    ArticleTopicAuthorize ArticleTopicAuthorize = new ArticleTopicAuthorize();
            //    ArticleTopicAuthorize.Id = Common.GuId();
            //    ArticleTopicAuthorize.ObjectType = 1;
            //    ArticleTopicAuthorize.ObjectId = ArticleTopic.Id;
            //    ArticleTopicAuthorize.ItemId = itemId;
            //    if (moduledata.Find(t => t.Id == itemId) != null)
            //    {
            //        ArticleTopicAuthorize.ItemType = 1;
            //    }
            //    if (buttondata.Find(t => t.Id == itemId) != null)
            //    {
            //        ArticleTopicAuthorize.ItemType = 2;
            //    }
            //    ArticleTopicAuthorizes.Add(ArticleTopicAuthorize);
            //}
            //_Respository.SubmitForm(ArticleTopic, ArticleTopicAuthorizes, keyValue);
        }


        public InvokeResult<bool> SubmitForm(ArticleTopic entity)
        {
            var expression = ExtLinq.True<ArticleTopic>();
            expression = expression.And(w => w.Title == entity.Title);
            if (!entity.Id.IsEmpty())
            {
                expression = expression.And(w => w.Id != entity.Id);
            }
            if (_Respository.Any(expression))
            {
                return RequestResult.Failed<bool>("该专题标题已存在，请重新输入");
            }
            bool b= _Respository.CreateOrUpdate(entity);
            return RequestResult.Result(b);
        }




        public List<ListItem> GetHotTopics(int count)
        {
            var expression = base.GetFilterEnabled();
            expression = w => w.ParentId != null && w.TopicStatus == (int)TopicStatus.Audited;

            return _Respository.GetFeilds(u => new ListItem { Text = u.Title, Code = u.Id, PicUrl = u.CoverUrl },
                expression,
                o => o.OrderByDescending(b => b.ReadCount)
            ).Take(count).ToList();
        }


        public InvokeResult<bool> ClientRead(string cateId)
        {
            var entity = GetForm(cateId);
            entity.ReadCount++;
            return RequestResult.Result(_Respository.UpdateFields(entity, "ReadCount"));
        }


        /// <summary>
        /// 删除某用户的专题
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public InvokeResult<bool> DelTopic(string memberId, string keyValue)
        {
            if (!_Respository.Any(w => w.CreatorUserId == memberId && w.Id == keyValue))
            {
                return RequestResult.Failed<bool>("该专题资源不属于该账户，删除失败");
            }
            var b = DeleteForm(keyValue);
            if (!b.Success)
            {
                return b;
            }
            return RequestResult.Success(true);
        }



        public string GetAttachment(string tid, string field)
        {
            return _Respository.GetFeild(u => u.Attachment,w=>w.Id == tid);
        }


        public InvokeResult<bool> ExistTopicName(string tid, string name)
        {
            var expression = ExtLinq.True<ArticleTopic>();
            expression = expression.And(w => w.Title == name);
            if (!tid.IsEmpty())
            {
                expression = expression.And(w=>w.Id != tid);
            }
            return RequestResult.Result(_Respository.Any(expression));
        }

        /// <summary>
        /// 审核专题
        /// </summary>
        /// <param name="id"></param>
        /// <param name="auditStatus"></param>
        /// <returns></returns>
        public InvokeResult<bool> AuditTopic(string id, int auditStatus,string auditMessage)
        {
            var entity = GetForm(id);
            if (entity.TopicStatus == (int)TopicStatus.Audited)
            {
                return RequestResult.Failed<bool>("该专题已审核成功，不能重复审核！");
            }
            entity.TopicStatus = auditStatus;
            entity.EnabledMark = true;
            entity.AuditMessage = auditMessage;
            
            //审核其他文件操作
            using (var tran = _Respository.BeginTransaction())
            {
                try
                {
                    var b = false;
                    var articles = new List<Article>();
                    //先处理文件
                    string[] unZipFiles=new string[] { };
                    if (auditStatus == (int)TopicStatus.Audited && !entity.Attachment.IsEmpty())
                    {
                        //后台审核通过时写入
                        //压缩包文件 解压，把数据写入文库
                        if (File.Exists(FileHelper.MapFilePath(entity.Attachment)))
                        {
                            var zipfileInfo = new FileInfo(FileHelper.MapFilePath(entity.Attachment));//存在则覆盖
                            var unzipDir = FileDownHelper.UnZip(zipfileInfo.FullName, zipfileInfo.FullName.Replace(zipfileInfo.Extension, "topic"));
                            unZipFiles = Directory.GetFiles(unzipDir);
                            if (!unZipFiles.Any())
                            {
                                tran.Rollback();
                                return RequestResult.Failed<bool>("资源解压后无文件，审核失败");
                            }

                            System.Threading.Thread.Sleep(800);
                            
                            foreach (var file in unZipFiles)
                            {
                                var extension = FileHelper.GetExtension(file);
                                var dir = $"/{ConfigManager.SysConfiguration.UploadFolder}/{DateTime.Now.ToString("yyyyMMdd")}";
                                if (!Directory.Exists(FileHelper.MapFilePath(dir)))
                                {
                                    Directory.CreateDirectory(FileHelper.MapFilePath(dir));
                                }
                                var newFileName = $"{dir}/{Common.GuId().Replace("-", "")}{extension}";
                                FileHelper.CopyFile(file, FileHelper.MapFilePath(newFileName));
                                var title = FileHelper.GetFileName(file, false);
                                var article = new Article()
                                {
                                    Title = title,
                                    ShortTitle = title,
                                    Keyword = title,
                                    Description = title,
                                    Attachment = newFileName,
                                    AllowDownload = false,
                                    SpecialTopicId = id,
                                    IsOnline = true,
                                    MemberId = entity.CreatorUserId,
                                    OnlinePageCount = 5,
                                    RequireAmount = 0,
                                    PageCount = 5,
                                    ResourceSize = (int)FileHelper.GetFileSize(file),
                                    ResourceType = extension.Replace(".", ""),
                                    BookStatus = (int)BookStatus.审核通过,
                                    HasImages = true,
                                };
                                articles.Add(article);

                                b = _ArticleRespository.CreateOrUpdate(article);
                                if (!b)
                                {
                                    tran.Rollback();
                                    return RequestResult.Failed<bool>("解压数据处理失败");
                                }

                                CoreContextProvider.ConvertFileToImage(article.Attachment.Split(',').ToList(), article.PageCount, _logger);
                            }
                        }
                    }

                    //后处理专题
                    if (!articles.Any())
                    {
                        tran.Rollback();
                        return RequestResult.Failed<bool>("审核解压后文档数为0，审核失败！");
                    }
                    entity.ResourceCount = articles.Count;
                    b = _Respository.UpdateFields(entity, "TopicStatus", "AuditMessage", "ResourceCount", "EnabledMark");
                    if (!b)
                    {
                        tran.Rollback();
                        return RequestResult.Failed<bool>("审核失败");
                    }

                    if (!entity.CreatorUserId.IsEmpty())
                    {
                        var memberScoreService = CoreContextProvider.GetService<IMemberScoreService>();
                        b = memberScoreService.AddScore(entity.CreatorUserId, ScoreType.addtopic, id).Success;
                        if (!b)
                        {
                            tran.Rollback();
                            return RequestResult.Failed<bool>("审核处理积分失败");
                        }
                    }
                    
                    if (unZipFiles!=null && unZipFiles.Any())
                    {
                        

                        //[method.AuditTopic(cea705b9-ae49-40f6-ad4c-bb06fef1ff7d,1)]:Cannot access destination table 'Article'.
                        //insert into articles
                        //articleService.AddBetch(articles);
                    }

                    tran.Commit();
                    return RequestResult.Success(true);
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    _logger.LogError($"[method.AuditTopic({id},{auditStatus})]:" + ex.ToString());
                    return RequestResult.Failed<bool>("审核失败，数据异常");
                }
            }
        }

        









    }



}
