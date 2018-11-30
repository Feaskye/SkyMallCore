using SkyMallCore.Core;
using SkyMallCore.Models;
using SkyMallCore.Respository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SkyMallCore.Services
{
    public class SysRoleService : ServiceBase<SysRole>, ISysRoleService
    {
        ISysLogRespository _LogRespository;
        ISysRoleRespository _Respository;
        ISysModuleService _SysModuleService;
        ISysModuleButtonService _SysModuleButtonService;

        public SysRoleService(ISysLogRespository sysLogRespository, ISysRoleRespository sysRoleRespository,
            ISysModuleService sysModuleService,
        ISysModuleButtonService sysModuleButtonService
            )
        {
            _LogRespository = sysLogRespository;
            _Respository = sysRoleRespository;
            _SysModuleService = sysModuleService;
            _SysModuleButtonService = sysModuleButtonService;
        }



        public List<SysRole> GetList(string keyword = "", bool isDuty = false)
        {
            var expression = base.GetFilterEnabled();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.FullName.Contains(keyword));
                expression = expression.Or(t => t.EnCode.Contains(keyword));
            }
            if (isDuty)
            {
                expression = expression.Or(t => t.Category == 2);
            }
            return _Respository.Get(expression).OrderBy(t => t.SortCode).ToList();
        }

        public SysRole GetForm(string keyValue)
        {
            return _Respository.Get(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            _Respository.Delete(keyValue);
        }
        public void SubmitForm(SysRole SysRole, string[] permissionIds, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                SysRole.Id = keyValue;
            }
            else
            {
                SysRole.Id = Common.GuId();
            }
            var moduledata = _SysModuleService.GetList();
            var buttondata = _SysModuleButtonService.GetList();
            List<SysRoleAuthorize> SysRoleAuthorizes = new List<SysRoleAuthorize>();
            foreach (var itemId in permissionIds)
            {
                SysRoleAuthorize SysRoleAuthorize = new SysRoleAuthorize();
                SysRoleAuthorize.Id = Common.GuId();
                SysRoleAuthorize.ObjectType = 1;
                SysRoleAuthorize.ObjectId = SysRole.Id;
                SysRoleAuthorize.ItemId = itemId;
                if (moduledata.Find(t => t.Id == itemId) != null)
                {
                    SysRoleAuthorize.ItemType = 1;
                }
                if (buttondata.Find(t => t.Id == itemId) != null)
                {
                    SysRoleAuthorize.ItemType = 2;
                }
                SysRoleAuthorizes.Add(SysRoleAuthorize);
            }
            _Respository.SubmitForm(SysRole, SysRoleAuthorizes, keyValue);
        }


        public void SubmitForm(SysRole roleEntity)
        {
            _Respository.CreateOrUpdate(roleEntity);
        }


    }


}
