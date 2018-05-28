using SkyMallCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.Services
{
    public interface ISysItemsService
    {
        IList<SysItems> GetList();

        SysItems GetForm(string keyValue);

        void DeleteForm(string keyValue);

        void SubmitForm(SysItems SysItems, string keyValue);
    }


}
