using SkyCoreLib.Utils;
using SkyMallCore.Models;
using SkyMallCore.Respository;
using SkyMallCore.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SkyMallCore.Services
{
    public class FilterIPService : ServiceBase<FilterIP>, IFilterIPService
    {
        ISysLogRespository _SysLogRespository;
        IFilterIPRespository _Respository;

        public FilterIPService(ISysLogRespository sysLogRespository, IFilterIPRespository respository)
        {
            _SysLogRespository = sysLogRespository;
            _Respository = respository;
        }


        public List<FilterIP> GetList(string keyword)
        {
            var expression = ExtLinq.True<FilterIP>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.StartIP.Contains(keyword));
            }
            return _Respository.Get(expression).OrderByDescending(t => t.DeleteTime).ToList();
        }
        public FilterIP GetForm(string keyValue)
        {
            return _Respository.Get(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            _Respository.Delete(t => t.Id == keyValue);
        }
        public InvokeResult<bool> SubmitForm(FilterIP FilterIP, string keyValue)
        {
            bool b = false;
            if (!string.IsNullOrEmpty(keyValue))
            {
                FilterIP.Id = keyValue;
                var filter = _Respository.Get(keyValue);
                filter.StartIP = FilterIP.StartIP;
                filter.EndIP = FilterIP.EndIP;
                filter.Description = FilterIP.Description;
                filter.Type = FilterIP.Type;
                filter.EnabledMark = FilterIP.EnabledMark;
              b=  _Respository.Update(filter);
            }
            else
            {
                 b =  _Respository.Insert(FilterIP);
            }
            if (!b)
            {
                return RequestResult.Failed<bool>("失败");
            }
           return RequestResult.Success(b);
        }
    }


}
