using SkyMallCore.Core;
using SkyMallCore.Models;
using SkyMallCore.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.Services
{
    public interface ISysLogService
    {
        PagedList<SysLog> GetList(LogSearchView search, int pageIndex, int pageSize);

        void RemoveLog(string keepTime);


        void OperatLog(bool result, string resultLog);


        void WriteSysLog(SysLog SysLog);


    }


}
