using SkyMallCore.Core;
using SkyMallCore.Models;
using SkyMallCore.Respository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SkyMallCore.Services
{
    public class SysRoleService : ISysRoleService
    {
        ISysLogRespository _LogRespository;
        ISysRoleRespository _Respository;
        public SysRoleService(ISysLogRespository sysLogRespository, ISysRoleRespository sysRoleRespository)
        {
            _LogRespository = sysLogRespository;
            _Respository = sysRoleRespository;
        }


        public IList<SysRole> GetList()
        {
            return _Respository.GetAll().ToList();
        }


        public List<SysRole> GetListBykeyword(string keyword = "")
        {
            var expression = ExtLinq.True<SysRole>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.FullName.Contains(keyword));
                expression = expression.Or(t => t.EnCode.Contains(keyword));
            }
            expression = expression.And(t => t.Category == 2);
            return _Respository.Get(expression).OrderBy(t => t.SortCode).ToList();
        }



    }


}
