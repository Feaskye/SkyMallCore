using SkyMallCore.Models;
using SkyMallCore.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.Services
{
    public interface ISysItemsDetailService
    {
        IList<SysItemsDetail> GetList(string itemId = "", string keyword = "");

        List<SysItemsDetail> GetItemList(string enCode);

        /// <summary>
        /// 获取界面字典集合
        /// </summary>
        /// <param name="enCode"></param>
        /// <returns></returns>
        List<ListItem> GetListItem(string enCode, string[] childCodes = null);

        SysItemsDetail GetItem(string encode, string itemcode);

        SysItemsDetail GetForm(string keyValue);

        void DeleteForm(string keyValue);


        void SubmitForm(SysItemsDetail SysItemsDetail, string keyValue);
    }


}
