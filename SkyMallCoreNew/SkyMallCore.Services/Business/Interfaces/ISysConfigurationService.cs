using SkyMallCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.Services
{
    public interface ISysConfigurationService
    {

        List<SysConfiguration> GetList(string keyword = "");


        SysConfiguration GetForm(string keyValue);


        void DeleteForm(string keyValue);


        void SubmitForm(SysConfiguration Link, string[] permissionIds, string keyValue);


        void SubmitForm(SysConfiguration roleEntity);
    }


}
