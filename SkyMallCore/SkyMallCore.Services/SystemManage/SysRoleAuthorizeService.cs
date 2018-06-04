using SkyMallCore.Core;
using SkyMallCore.Models;
using SkyMallCore.Respository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SkyMallCore.ViewModel.SystemManage;

namespace SkyMallCore.Services
{
    public class SysRoleAuthorizeService : ISysRoleAuthorizeService
    {
        ISysLogRespository _LogRespository;
        ISysModuleService _SysModuleService;
        ISysModuleButtonService _SysModuleButtonService;
        ISysRoleAuthorizeRespository _Respository;
        public SysRoleAuthorizeService(ISysLogRespository sysLogRespository
            , ISysModuleService sysModuleService, ISysRoleAuthorizeRespository sysRoleAuthorizeRespository
            , ISysModuleButtonService sysModuleButtonService)
        {
            _LogRespository = sysLogRespository;
            _SysModuleService = sysModuleService;
            _Respository = sysRoleAuthorizeRespository;
            _SysModuleButtonService = sysModuleButtonService;
        }


        public List<SysRoleAuthorize> GetList(string ObjectId)
        {
            return _Respository.Get(t => t.ObjectId == ObjectId).ToList();
        }

        public List<SysModule> GetMenuList(string roleId)
        {
            var data = _SysModuleService.GetList().Where(w=>w.EnabledMark!=null && w.EnabledMark.Value).ToList();
            if (!CoreContextProvider.CurrentSysUser.IsSystem)
            {
                var authorizedata = _Respository.Get(t => t.ObjectId == roleId && t.ItemType == 1).ToList();
                foreach (var item in authorizedata)
                {
                    SysModule SysModule = data.Find(t => t.Id == item.ItemId);
                    if (SysModule != null)
                    {
                        data.Add(SysModule);
                    }
                }
            }
            return data.OrderBy(t => t.SortCode).ToList();
        }
        public List<SysModuleButton> GetButtonList(string roleId)
        {
            var data = _SysModuleButtonService.GetList();
            if (!CoreContextProvider.CurrentSysUser.IsSystem)
            {
              
                var buttondata = _SysModuleButtonService.GetList();
                var authorizedata = _Respository.Get(t => t.ObjectId == roleId && t.ItemType == 2).ToList();
                foreach (var item in authorizedata)
                {
                    var sysModuleButton = buttondata.Find(t => t.Id == item.ItemId);
                    if (sysModuleButton != null)
                    {
                        data.Add(sysModuleButton);
                    }
                }
            }
            return data.OrderBy(t => t.SortCode).ToList();
        }


        public bool ActionValidate(string roleId, string moduleId, string action)
        {
            var authorizeurldata = new List<AuthorizeActionModel>();
            var cachedata = CoreContextProvider.MemCache.GetCache<List<AuthorizeActionModel>>("authorizeurldata_" + roleId);
            if (cachedata == null)
            {
                var moduledata = _SysModuleService.GetList();
                var buttondata = _SysModuleButtonService.GetList();
                var authorizedata = _Respository.Get(t => t.ObjectId == roleId).ToList();
                foreach (var item in authorizedata)
                {
                    if (item.ItemType == 1)
                    {
                        var moduleEntity = moduledata.Find(t => t.Id == item.ItemId);
                        authorizeurldata.Add(new AuthorizeActionModel { Id = moduleEntity.Id, UrlAddress = moduleEntity.UrlAddress });
                    }
                    else if (item.ItemType == 2)
                    {
                        var moduleButtonEntity = buttondata.Find(t => t.Id == item.ItemId);
                        authorizeurldata.Add(new AuthorizeActionModel { Id = moduleButtonEntity.ModuleId, UrlAddress = moduleButtonEntity.UrlAddress });
                    }
                }
                CoreContextProvider.MemCache.SetCache(authorizeurldata, "authorizeurldata_" + roleId, DateTime.Now.AddMinutes(5));
            }
            else
            {
                authorizeurldata = cachedata;
            }
            authorizeurldata = authorizeurldata.FindAll(t => t.Id.Equals(moduleId));
            foreach (var item in authorizeurldata)
            {
                if (!string.IsNullOrEmpty(item.UrlAddress))
                {
                    string[] url = item.UrlAddress.Split('?');
                    if (item.Id == moduleId && url[0] == action)
                    {
                        return true;
                    }
                }
            }
            return false;
        }


    }


}
