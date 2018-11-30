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
    public class HelpService : ServiceBase<Help>, IHelpService
    {
        ISysLogRespository _LogRespository;
        IHelpRespository _Respository;
        //IHelpCategoryRespository _HelpCategoryRespository;

        public HelpService(ISysLogRespository sysLogRespository, IHelpRespository respository//,
            //IHelpCategoryRespository HelpCategoryRespository
            )
        {
            _LogRespository = sysLogRespository;
            _Respository = respository;
        }



        public List<Help> GetList(string keyword = "")
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
        public PagedList<Help> GetList(HelpSearchView searchView, int pageIndex = 1, int pageSize = 20)
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


        public PagedList<HelpDetailView> GetHelps(HelpSearchView searchView, int pageIndex, int pageSize)
        {
            var expression = base.GetFilterEnabled();
            if (searchView.HelpCode.HasValue)
            {
                expression = expression.And(t => t.HelpCode == (int)searchView.HelpCode);
            }

            if (!string.IsNullOrEmpty(searchView.Keyword))
            {
                expression = expression.And(t => t.Title.Contains(searchView.Keyword) ||
                                                                        t.Description.Contains(searchView.Keyword) ||
                                                                        t.ShortTitle.Contains(searchView.Keyword));
            }

            if (!searchView.Title.IsEmpty())
            {
                expression = expression.And(t => t.Title.Contains(searchView.Title));
            }
         
            //expression = expression.And(t => t.CategoryId == 2);
            return _Respository.GetPagedList(
                u => new HelpDetailView
                {
                    Id = u.Id,
                    Title = u.Title,
                    CoverUrl = u.CoverUrl,
                    ShortTitle = u.ShortTitle,
                    Description = u.Description,
                    Attachment = u.Attachment,
                    //CategoryId = u.CategoryId,
                    ReadCount = u.ReadCount,
                    ResourceUrl = u.ResourceUrl,
                    CreatorTime = u.CreatorTime,
                    HelpCode = (HelpCode)u.HelpCode
                }
                , expression, pageIndex, pageSize, o => o.OrderBy(t => t.SortCode));
        }



        public Help GetForm(string keyValue)
        {
            return _Respository.Get(keyValue);
        }
        public InvokeResult<bool> DeleteForm(string keyValue)
        {
            var b = _Respository.Delete(keyValue);
            return RequestResult.Result(b);
        }
        public InvokeResult<bool> DelHelp(string memberId, string keyValue)
        {
            var b = _Respository.Delete(keyValue);
            return RequestResult.Result(b, "删除失败");
        }





        public void SubmitForm(Help Help, string[] permissionIds, string keyValue)
        {
            //if (!string.IsNullOrEmpty(keyValue))
            //{
            //    Help.Id = keyValue;
            //}
            //else
            //{
            //    Help.Id = Common.GuId();
            //}
            //var moduledata = _SysModuleService.GetList();
            //var buttondata = _SysModuleButtonService.GetList();
            //List<HelpAuthorize> HelpAuthorizes = new List<HelpAuthorize>();
            //foreach (var itemId in permissionIds)
            //{
            //    HelpAuthorize HelpAuthorize = new HelpAuthorize();
            //    HelpAuthorize.Id = Common.GuId();
            //    HelpAuthorize.ObjectType = 1;
            //    HelpAuthorize.ObjectId = Help.Id;
            //    HelpAuthorize.ItemId = itemId;
            //    if (moduledata.Find(t => t.Id == itemId) != null)
            //    {
            //        HelpAuthorize.ItemType = 1;
            //    }
            //    if (buttondata.Find(t => t.Id == itemId) != null)
            //    {
            //        HelpAuthorize.ItemType = 2;
            //    }
            //    HelpAuthorizes.Add(HelpAuthorize);
            //}
            //_Respository.SubmitForm(Help, HelpAuthorizes, keyValue);
        }


        public InvokeResult<bool> SubmitForm(Help roleEntity)
        {
            var b = _Respository.CreateOrUpdate(roleEntity);
            return RequestResult.Result(b);
        }


        public HelpDetailView GetHelpByCode(HelpCode helpCode)
        {
            var expression = base.GetFilterEnabled();
            expression = expression.And(h => h.HelpCode == (int)helpCode);

            return _Respository.GetFeilds(u => new HelpDetailView { Title = u.Title, Id = u.Id, Description = u.Description, Attachment = u.Attachment, CoverUrl = u.CoverUrl }, expression)
                        .FirstOrDefault();
        }


    }


}
