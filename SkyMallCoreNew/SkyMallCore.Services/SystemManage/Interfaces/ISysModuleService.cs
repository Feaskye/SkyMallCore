using SkyMallCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.Services
{
    public interface ISysModuleService
    {
        List<SysModule> GetList();

        SysModule GetForm(string keyValue);

        void DeleteForm(string keyValue);

        void SubmitForm(SysModule SysModule, string keyValue);
    }


}
