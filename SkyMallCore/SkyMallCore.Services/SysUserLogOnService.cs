using SkyMallCore.Core;
using SkyMallCore.Models;
using SkyMallCore.Respository;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.Services
{
    public class SysUserLogOnService: ISysUserLogOnService
    {
        ISysUserLogOnRespository _SysUserLogOnRespository;
        public SysUserLogOnService(ISysUserLogOnRespository sysUserLogOnRespository)
        {
            _SysUserLogOnRespository = sysUserLogOnRespository;
        }

        public SysUserLogOn GetForm(string key)
        {
            return _SysUserLogOnRespository.Get(key);
        }


        public void UpdateForm(SysUserLogOn userLogOnEntity)
        {
            _SysUserLogOnRespository.Update(userLogOnEntity);
        }

    }


}
