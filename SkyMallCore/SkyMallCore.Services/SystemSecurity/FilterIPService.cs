using SkyMallCore.Core;
using SkyMallCore.Models;
using SkyMallCore.Respository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyMallCore.Services
{
    public class FilterIPService : IFilterIPService
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
        public void SubmitForm(FilterIP FilterIP, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                FilterIP.Id = keyValue;
                _Respository.Update(FilterIP);
            }
            else
            {
                _Respository.Insert(FilterIP);
            }
        }
    }


}
