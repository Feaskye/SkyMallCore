using SkyMallCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.Services
{
    public interface ISysRoleAuthorizeService
    {
        List<SysModule> GetMenuList(string roleId);

        List<SysModuleButton> GetButtonList(string roleId);


        List<SysRoleAuthorize> GetList(string objectId);
    }


}
