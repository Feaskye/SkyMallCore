using SkyMallCore.Core;
using SkyMallCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.Services
{
    public interface ISysLogService
    {
        List<SysLog> GetList(Pagination pagination, string queryJson);

        void RemoveLog(string keepTime);


        void WriteDbLog(bool result, string resultLog);


        void WriteSysLog(SysLog SysLog);


    }


}
