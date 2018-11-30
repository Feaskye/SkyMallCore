using SkyMallCore.Models;
using SkyMallCore.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.Services
{
    public interface ISysUserLogOnService
    {
        SysUserLogOn GetForm(string key);


        void UpdateForm(SysUserLogOn userLogOnEntity);
        InvokeResult<bool> RevisePassword(string oldPassword, string userPassword, string keyValue);
    }


}
