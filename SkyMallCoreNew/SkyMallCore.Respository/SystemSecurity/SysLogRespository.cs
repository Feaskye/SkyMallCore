using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SkyCore.GlobalProvider;
using SkyMallCore.Core;
using SkyMallCore.Data;
using SkyMallCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkyMallCore.Respository
{
    public class SysLogRespository : RespositoryBase<SysLog>, ISysLogRespository
    {
        public SysLogRespository(ISkyMallDbContext skyMallDbContext) : base(skyMallDbContext)
        { }

        public void OperatLog(bool result, string resultLog)
        {
            var SysLog = new SysLog();
            SysLog.Id = Common.GuId();
            SysLog.Date = DateTime.Now;
            SysLog.Account = CoreContextProvider.CurrentSysUser == null?"":CoreContextProvider.CurrentSysUser.UserCode;
            SysLog.NickName = CoreContextProvider.CurrentSysUser == null ? "" : CoreContextProvider.CurrentSysUser.RealName;
            SysLog.IPAddress = CoreContextProvider.HttpContext.GetIP();
            SysLog.IPAddressName = NetClient.GetLocation(SysLog.IPAddress);
            SysLog.Result = result;
            SysLog.Description = resultLog;
            this.Insert(SysLog);
        }

        public void OperatLog(SysLog log)
        {
            this.Insert(log);
        }

    }

}
