using SkyMallCore.Core;
using SkyMallCore.Models;
using SkyMallCore.Respository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SkyMallCore.Services
{
    public class SysConfigurationService : ServiceBase<SysConfiguration>, ISysConfigurationService
    {
        ISysLogRespository _LogRespository;
        ISysConfigurationRespository _Respository;

        public SysConfigurationService(ISysLogRespository sysLogRespository, 
            ISysConfigurationRespository LinkRespository
            )
        {
            _LogRespository = sysLogRespository;
            _Respository = LinkRespository;
        }



        public List<SysConfiguration> GetList(string keyword = "")
        {
            var expression = base.GetFilterEnabled();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.ConfigName.Contains(keyword));
            }
            return _Respository.Get(expression).OrderBy(t => t.SortCode).ToList();
        }

        public SysConfiguration GetForm(string keyValue)
        {
            return _Respository.Get(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            _Respository.Delete(keyValue);
        }
        public void SubmitForm(SysConfiguration Link, string[] permissionIds, string keyValue)
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


        public void SubmitForm(SysConfiguration roleEntity)
        {
            _Respository.CreateOrUpdate(roleEntity);
        }


    }


}
