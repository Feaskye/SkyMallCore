using SkyMallCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.Services
{
    public interface ISysLogService
    {
        //SysUserLogOn GetForm(string key);


        //void UpdateForm(SysUserLogOn userLogOnEntity);

        void WriteSysLog(SysLog logEntity);


    }


}
