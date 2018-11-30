using SkyMallCore.Core;
using SkyMallCore.Models;
using SkyMallCore.Respository;
using SkyMallCore.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using SkyCore.GlobalProvider;

namespace SkyMallCore.Services
{
    public class SysLogService : ISysLogService
    {
        ISysLogRespository _Respository;
        public SysLogService(ISysLogRespository sysLogRespository)
        {
            _Respository = sysLogRespository;
        }


        public PagedList<SysLog> GetList(LogSearchView search,int pageIndex,int pageSize)
        {
            var expression = ExtLinq.True<SysLog>();
            if (!string.IsNullOrWhiteSpace( search.Keyword))
            {
                string keyword = search.Keyword;
                expression = expression.And(t => t.Account.Contains(keyword) || 
                    t.Description.Contains(keyword)
                    || t.ModuleName.Contains(keyword)
                    || t.NickName.Contains(keyword)
                    || t.Type.Contains(keyword));
            }
            if (!string.IsNullOrWhiteSpace(search.Admin))
            {
                string admin = search.Admin;
                expression = expression.And(t => t.Account.Contains(admin));
            }
            if (search.StartTime.HasValue)
            {
                expression = expression.And(t => t.Date >= search.StartTime && t.Date <=search.EndTime);
            }
            return _Respository.GetPagedList(expression,pageIndex,pageSize,w=>w.OrderByDescending(d=>d.Date));
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
        /// <summary>
        /// 操作日志
        /// </summary>
        /// <param name="result"></param>
        /// <param name="resultLog"></param>
        public void OperatLog(bool result, string resultLog)
        {
            _Respository.OperatLog(result, resultLog);
        }

        /// <summary>
        /// 登陆日志
        /// </summary>
        /// <param name="SysLog"></param>
        public void WriteSysLog(SysLog SysLog)
        {
            SysLog.Id = Common.GuId();
            SysLog.Date = DateTime.Now;
            SysLog.IPAddress = "127.0.0.1";
            SysLog.IPAddressName = NetClient.GetLocation(SysLog.IPAddress);
            _Respository.Insert(SysLog);
        }
    }







    








}
