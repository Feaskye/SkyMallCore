using SkyMallCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.Services
{
    public interface ISysUserLogOnService
    {
        SysUserLogOn GetForm(string key);


        void UpdateForm(SysUserLogOn userLogOnEntity);
        void RevisePassword(string userPassword, string keyValue);
    }


}
