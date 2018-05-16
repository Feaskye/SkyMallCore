using SkyMallCore.Core;
using SkyMallCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.Services
{
    public interface ISysUserService
    {
        IList<SysUser> GetUsers();

        List<SysUser> GetList(Pagination pagination, string keyword);
        SysUser GetForm(string id);

        SysUser CheckLogin(string userName, string password);

        void DeleteForm(string id);

        void SubmitForm(SysUser sysUser, SysUserLogOn userLogOnEntity, string userId);

        void UpdateForm(SysUser sysUser);
    }


}
