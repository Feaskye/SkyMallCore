
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyMallCoreWeb.Areas.SystemManage.Controllers
{
    ///// <summary>
    ///// 加载后台菜单等数据
    ///// </summary>
    //[HandlerLogin]
    //public class ClientsDataController : Controller
    //{
    //    [HttpGet]
    //    [HandlerAjaxOnly]
    //    public ActionResult GetClientsDataJson()
    //    {
    //        var data = new
    //        {
    //            dataItems = this.GetDataItemList(),
    //            organize = this.GetOrganizeList(),
    //            role = this.GetRoleList(),
    //            duty = this.GetDutyList(),
    //            user = "",
    //            authorizeMenu = this.GetMenuList(),
    //            authorizeButton = this.GetMenuButtonList(),
    //        };
    //        return Content(data.ToJson());
    //    }
    //    private object GetDataItemList()
    //    {
    //        var itemdata = new ItemsDetailApp().GetList();
    //        Dictionary<string, object> dictionaryItem = new Dictionary<string, object>();
    //        foreach (var item in new ItemsApp().GetList())
    //        {
    //            var dataItemList = itemdata.FindAll(t => t.ItemId.Equals(item.Id));
    //            Dictionary<string, string> dictionaryItemList = new Dictionary<string, string>();
    //            foreach (var itemList in dataItemList)
    //            {
    //                dictionaryItemList.Add(itemList.ItemCode, itemList.ItemName);
    //            }
    //            dictionaryItem.Add(item.EnCode, dictionaryItemList);
    //        }
    //        return dictionaryItem;
    //    }
    //    private object GetOrganizeList()
    //    {
    //        OrganizeApp organizeApp = new OrganizeApp();
    //        var data = organizeApp.GetList();
    //        Dictionary<string, object> dictionary = new Dictionary<string, object>();
    //        foreach (OrganizeEntity item in data)
    //        {
    //            var fieldItem = new
    //            {
    //                encode = item.EnCode,
    //                fullname = item.FullName
    //            };
    //            dictionary.Add(item.Id, fieldItem);
    //        }
    //        return dictionary;
    //    }
    //    private object GetRoleList()
    //    {
    //        RoleApp roleApp = new RoleApp();
    //        var data = roleApp.GetList();
    //        Dictionary<string, object> dictionary = new Dictionary<string, object>();
    //        foreach (RoleEntity item in data)
    //        {
    //            var fieldItem = new
    //            {
    //                encode = item.EnCode,
    //                fullname = item.FullName
    //            };
    //            dictionary.Add(item.Id, fieldItem);
    //        }
    //        return dictionary;
    //    }
    //    private object GetDutyList()
    //    {
    //        DutyApp dutyApp = new DutyApp();
    //        var data = dutyApp.GetList();
    //        Dictionary<string, object> dictionary = new Dictionary<string, object>();
    //        foreach (RoleEntity item in data)
    //        {
    //            var fieldItem = new
    //            {
    //                encode = item.EnCode,
    //                fullname = item.FullName
    //            };
    //            dictionary.Add(item.Id, fieldItem);
    //        }
    //        return dictionary;
    //    }
    //    private object GetMenuList()
    //    {
    //        var roleId = OperatorProvider.Provider.GetCurrent().RoleId;
    //        return ToMenuJson(new RoleAuthorizeApp().GetMenuList(roleId), "0");
    //    }
    //    private string ToMenuJson(List<ModuleEntity> data, string parentId)
    //    {
    //        StringBuilder sbJson = new StringBuilder();
    //        sbJson.Append("[");
    //        List<ModuleEntity> entitys = data.FindAll(t => t.ParentId == parentId);
    //        if (entitys.Count > 0)
    //        {
    //            foreach (var item in entitys)
    //            {
    //                string strJson = item.ToJson();
    //                strJson = strJson.Insert(strJson.Length - 1, ",\"ChildNodes\":" + ToMenuJson(data, item.Id) + "");
    //                sbJson.Append(strJson + ",");
    //            }
    //            sbJson = sbJson.Remove(sbJson.Length - 1, 1);
    //        }
    //        sbJson.Append("]");
    //        return sbJson.ToString();
    //    }
    //    private object GetMenuButtonList()
    //    {
    //        var roleId = OperatorProvider.Provider.GetCurrent().RoleId;
    //        var data = new RoleAuthorizeApp().GetButtonList(roleId);
    //        var dataModuleId = data.Distinct(new ExtList<ModuleButtonEntity>("ModuleId"));
    //        Dictionary<string, object> dictionary = new Dictionary<string, object>();
    //        foreach (ModuleButtonEntity item in dataModuleId)
    //        {
    //            var buttonList = data.Where(t => t.ModuleId.Equals(item.ModuleId));
    //            dictionary.Add(item.ModuleId, buttonList);
    //        }
    //        return dictionary;
    //    }
    //}
}
