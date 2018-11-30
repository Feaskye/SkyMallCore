using SkyMallCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.Services
{
    public interface ISysModuleButtonService
    {
        List<SysModuleButton> GetList(string moduleId = "");

        void SubmitForm(SysModuleButton sysModuleButton, string keyValue);

        void DeleteForm(string keyValue);

        SysModuleButton GetForm(string keyValue);

        void SubmitCloneButton(string moduleId, string ids);
    }


}
