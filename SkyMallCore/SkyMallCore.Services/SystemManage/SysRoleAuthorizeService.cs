using SkyMallCore.Core;
using SkyMallCore.Models;
using SkyMallCore.Respository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

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


        public List<SysModule> GetMenuList(string roleId)
        {
            var data = _SysModuleService.GetList();
            if (!CoreProviderContext.Provider.CurrentSysUser.IsSystem)
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
            if (!CoreProviderContext.Provider.CurrentSysUser.IsSystem)
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


    }


}
