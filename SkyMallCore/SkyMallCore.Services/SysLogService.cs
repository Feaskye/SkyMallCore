using SkyMallCore.Core;
using SkyMallCore.Models;
using SkyMallCore.Respository;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.Services
{
    public class SysLogService : ISysLogService
    {
        ISysLogRespository _Respository;
        public SysLogService(ISysLogRespository sysLogRespository)
        {
            _Respository = sysLogRespository;
        }
        
        public void WriteSysLog(SysLog logEntity)
        {
            logEntity.Id = Common.GuId();
            logEntity.Date = DateTime.Now;
            logEntity.IPAddress = "127.0.0.1";
            logEntity.IPAddressName = Net.GetLocation(logEntity.IPAddress);
            //logEntity.Create();
            _Respository.Insert(logEntity);
        }
    }


}
