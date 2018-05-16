
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyMallCore.Core;
using SkyMallCore.Services;
using SkyMallCore.Models;

namespace SkyMallCoreWeb.Areas.SystemManage.Controllers
{
    /// <summary>
    /// 加载后台菜单等数据
    /// </summary>
    public class ClientsDataController : BaseSysController
    {
        ISysItemsService _SysItemsService;
        ISysItemsDetailService _SysItemsDetailService;
        ISysOrganizeService _SysOrganizeService;
        ISysRoleService _SysRoleService;
        ISysRoleAuthorizeService _SysRoleAuthorizeService;
        ISysModuleService _SysModuleService;
        ISysModuleButtonService _SysModuleButtonService;
        public ClientsDataController(ISysItemsService sysItemsService, ISysItemsDetailService sysItemsDetailService
            , ISysOrganizeService sysOrganizeService, ISysRoleService sysRoleService,
        ISysRoleAuthorizeService sysRoleAuthorizeService,
        ISysModuleService sysModuleService,
        ISysModuleButtonService sysModuleButtonService)
        {
            _SysItemsService = sysItemsService;
            _SysItemsDetailService = sysItemsDetailService;
            _SysOrganizeService = sysOrganizeService;
            _SysRoleService = sysRoleService;
            _SysRoleAuthorizeService = sysRoleAuthorizeService;
            _SysModuleService = sysModuleService;
            _SysModuleButtonService = sysModuleButtonService;
        }




        [HttpGet]
        public ActionResult GetClientsDataJson()
        {
            var data = new
            {
                dataItems = this.GetDataItemList(),
                organize = this.GetOrganizeList(),
                role = this.GetRoleList(),
                duty = this.GetDutyList(),
                user = "",
                authorizeMenu = this.GetMenuList(),
                authorizeButton = this.GetMenuButtonList(),
            };
            return Content(data.ToJson());
        }
        private object GetDataItemList()
        {
            var itemdata = _SysItemsDetailService.GetList();
            Dictionary<string, object> dictionaryItem = new Dictionary<string, object>();
            foreach (var item in _SysItemsService.GetList())
            {
                var dataItemList = itemdata.Where(t => t.ItemId.Equals(item.Id));
                Dictionary<string, string> dictionaryItemList = new Dictionary<string, string>();
                foreach (var itemList in dataItemList)
                {
                    dictionaryItemList.Add(itemList.ItemCode, itemList.ItemName);
                }
                dictionaryItem.Add(item.EnCode, dictionaryItemList);
            }
            return dictionaryItem;
        }
        private object GetOrganizeList()
        {
            var data = _SysOrganizeService.GetList();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            foreach (var item in data)
            {
                var fieldItem = new
                {
                    encode = item.EnCode,
                    fullname = item.FullName
                };
                dictionary.Add(item.Id, fieldItem);
            }
            return dictionary;
        }
        private object GetRoleList()
        {
            var data = _SysRoleService.GetList();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            foreach (var item in data)
            {
                var fieldItem = new
                {
                    encode = item.EnCode,
                    fullname = item.FullName
                };
                dictionary.Add(item.Id, fieldItem);
            }
            return dictionary;
        }
        private object GetDutyList()
        {
            var data = _SysRoleService.GetListBykeyword();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            foreach (var item in data)
            {
                var fieldItem = new
                {
                    encode = item.EnCode,
                    fullname = item.FullName
                };
                dictionary.Add(item.Id, fieldItem);
            }
            return dictionary;
        }
        private object GetMenuList()
        {
            var roleId = CoreProviderContext.Provider.CurrentSysUser.RoleId;
            return ToMenuJson(_SysRoleAuthorizeService.GetMenuList(roleId), "0");
        }
        private string ToMenuJson(List<SysModule> data, string parentId)
        {
            StringBuilder sbJson = new StringBuilder();
            sbJson.Append("[");
            var entitys = data.FindAll(t => t.ParentId == parentId);
            if (entitys.Count > 0)
            {
                foreach (var item in entitys)
                {
                    string strJson = item.ToJson();
                    strJson = strJson.Insert(strJson.Length - 1, ",\"ChildNodes\":" + ToMenuJson(data, item.Id) + "");
                    sbJson.Append(strJson + ",");
                }
                sbJson = sbJson.Remove(sbJson.Length - 1, 1);
            }
            sbJson.Append("]");
            return sbJson.ToString();
        }


        private object GetMenuButtonList()
        {
            var roleId = CoreProviderContext.Provider.CurrentSysUser.RoleId;
            var data = _SysRoleAuthorizeService.GetButtonList(roleId);
            var dataModuleId = data.Distinct(new ExtList<SysModuleButton>("ModuleId"));
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            foreach (SysModuleButton item in dataModuleId)
            {
                var buttonList = data.Where(t => t.ModuleId.Equals(item.ModuleId));
                dictionary.Add(item.ModuleId, buttonList);
            }
            return dictionary;
        }


    }
}
