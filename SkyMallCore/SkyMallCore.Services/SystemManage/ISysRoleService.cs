using SkyMallCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.Services
{
    public interface ISysRoleService
    {
        IList<SysRole> GetList();

        List<SysRole> GetListBykeyword(string keyword = "");
    }


}
