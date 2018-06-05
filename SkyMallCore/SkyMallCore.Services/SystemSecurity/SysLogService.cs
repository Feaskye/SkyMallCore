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


        public List<SysLog> GetList(Pagination pagination, string queryJson)
        {
            var expression = ExtLinq.True<SysLog>();
            var queryParam = queryJson.ToJObject();
            if (!queryParam["keyword"].IsEmpty())
            {
                string keyword = queryParam["keyword"].ToString();
                expression = expression.And(t => t.Account.Contains(keyword));
            }
            if (!queryParam["timeType"].IsEmpty())
            {
                string timeType = queryParam["timeType"].ToString();
                DateTime startTime = DateTime.Now.ToString("yyyy-MM-dd").ToDate();
                DateTime endTime = DateTime.Now.ToString("yyyy-MM-dd").ToDate().AddDays(1);
                switch (timeType)
                {
                    case "1":
                        break;
                    case "2":
                        startTime = DateTime.Now.AddDays(-7);
                        break;
                    case "3":
                        startTime = DateTime.Now.AddMonths(-1);
                        break;
                    case "4":
                        startTime = DateTime.Now.AddMonths(-3);
                        break;
                    default:
                        break;
                }
                expression = expression.And(t => t.Date >= startTime && t.Date <= endTime);
            }
            return _Respository.GetPagList(expression, pagination);
        }
        public void RemoveLog(string keepTime)
        {
            DateTime operateTime = DateTime.Now;
            if (keepTime == "7")            //保留近一周
            {
                operateTime = DateTime.Now.AddDays(-7);
            }
            else if (keepTime == "1")       //保留近一个月
            {
                operateTime = DateTime.Now.AddMonths(-1);
            }
            else if (keepTime == "3")       //保留近三个月
            {
                operateTime = DateTime.Now.AddMonths(-3);
            }
            var expression = ExtLinq.True<SysLog>();
            expression = expression.And(t => t.Date <= operateTime);
            _Respository.Delete(expression);
        }
        public void WriteDbLog(bool result, string resultLog)
        {
            SysLog SysLog = new SysLog();
            SysLog.Id = Common.GuId();
            SysLog.Date = DateTime.Now;
            SysLog.Account = CoreContextProvider.CurrentSysUser.UserCode;
            SysLog.NickName = CoreContextProvider.CurrentSysUser.RealName;
            SysLog.IPAddress = Net.Ip;
            SysLog.IPAddressName = Net.GetLocation(SysLog.IPAddress);
            SysLog.Result = result;
            SysLog.Description = resultLog;
            _Respository.Insert(SysLog);
        }

        public void WriteSysLog(SysLog SysLog)
        {
            SysLog.Id = Common.GuId();
            SysLog.Date = DateTime.Now;
            SysLog.IPAddress = "127.0.0.1";
            SysLog.IPAddressName = Net.GetLocation(SysLog.IPAddress);
            _Respository.Insert(SysLog);
        }
    }


}
