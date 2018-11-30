using SkyMallCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.Services
{
    public interface ISysRoleService
    {

        List<SysRole> GetList(string keyword = "", bool isDuty = false);


        SysRole GetForm(string keyValue);


        void DeleteForm(string keyValue);


        void SubmitForm(SysRole SysRole, string[] permissionIds, string keyValue);


        void SubmitForm(SysRole roleEntity);
    }


}
