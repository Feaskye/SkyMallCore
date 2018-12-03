using SkyCoreLib.Utils;
using SkyMallCore.Models;
using SkyMallCore.Respository;
using SkyMallCore.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SkyMallCore.Services
{
    public class LinkService : ServiceBase<Link>, ILinkService
    {
        ISysLogRespository _LogRespository;
        ILinkRespository _Respository;
        ISysModuleService _SysModuleService;
        ISysModuleButtonService _SysModuleButtonService;

        public LinkService(ISysLogRespository sysLogRespository, ILinkRespository LinkRespository,
            ISysModuleService sysModuleService,
        ISysModuleButtonService sysModuleButtonService
            )
        {
            _LogRespository = sysLogRespository;
            _Respository = LinkRespository;
            _SysModuleService = sysModuleService;
            _SysModuleButtonService = sysModuleButtonService;
        }



        public List<LinkDetailView> GetList(string keyword = "")
        {
            var expression = base.GetFilterEnabled();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.LinkName.Contains(keyword));
            }
            return _Respository.GetFeilds(u=>new LinkDetailView {
                LinkName = u.LinkName,
                LinkUrl =u.LinkUrl,
                Description=u.Description,
                Id=u.Id,
                CreatorTime=u.CreatorTime
            }, expression, o=>o.OrderBy(t => t.SortCode)).ToList();
        }

        public Link GetForm(string keyValue)
        {
            return _Respository.Get(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            _Respository.Delete(keyValue);
        }
        public void SubmitForm(Link Link, string[] permissionIds, string keyValue)
        {
            //if (!string.IsNullOrEmpty(keyValue))
            //{
            //    Link.Id = keyValue;
            //}
            //else
            //{
            //    Link.Id = Common.GuId();
            //}
            //var moduledata = _SysModuleService.GetList();
            //var buttondata = _SysModuleButtonService.GetList();
            //List<LinkAuthorize> LinkAuthorizes = new List<LinkAuthorize>();
            //foreach (var itemId in permissionIds)
            //{
            //    LinkAuthorize LinkAuthorize = new LinkAuthorize();
            //    LinkAuthorize.Id = Common.GuId();
            //    LinkAuthorize.ObjectType = 1;
            //    LinkAuthorize.ObjectId = Link.Id;
            //    LinkAuthorize.ItemId = itemId;
            //    if (moduledata.Find(t => t.Id == itemId) != null)
            //    {
            //        LinkAuthorize.ItemType = 1;
            //    }
            //    if (buttondata.Find(t => t.Id == itemId) != null)
            //    {
            //        LinkAuthorize.ItemType = 2;
            //    }
            //    LinkAuthorizes.Add(LinkAuthorize);
            //}
            //_Respository.SubmitForm(Link, LinkAuthorizes, keyValue);
        }


        public void SubmitForm(Link roleEntity)
        {
            _Respository.CreateOrUpdate(roleEntity);
        }


    }


}
