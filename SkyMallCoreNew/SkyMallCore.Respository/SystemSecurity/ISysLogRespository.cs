using SkyMallCore.Data;
using SkyMallCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.Respository
{
    public interface ISysLogRespository : IRespositoryBase<SysLog>
    {
        void OperatLog(bool result, string resultLog);

        void OperatLog(SysLog log);
    }
}
