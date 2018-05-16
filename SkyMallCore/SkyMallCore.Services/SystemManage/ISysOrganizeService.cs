using SkyMallCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.Services
{
    public interface ISysOrganizeService
    {
        IList<SysOrganize> GetList();
        SysOrganize GetForm(string id);
        void SubmitForm(SysOrganize sysOrganize, string id);
        void DeleteForm(string id);
    }


}
