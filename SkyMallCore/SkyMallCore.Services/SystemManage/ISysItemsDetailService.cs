using SkyMallCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.Services
{
    public interface ISysItemsDetailService
    {
        IList<SysItemsDetail> GetList(string itemId = "", string keyword = "");

        List<SysItemsDetail> GetItemList(string enCode);

        SysItemsDetail GetForm(string keyValue);

        void DeleteForm(string keyValue);


        void SubmitForm(SysItemsDetail SysItemsDetail, string keyValue);
    }


}
