using SkyMallCore.Core;
using SkyMallCore.Models;
using SkyMallCore.Respository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SkyMallCore.Services
{
    public class SysOrganizeService : ISysOrganizeService
    {
        ISysLogRespository _LogRespository;
        ISysOrganizeRespository _Respository;
        public SysOrganizeService(ISysLogRespository sysLogRespository, ISysOrganizeRespository sysOrganizeRespository)
        {
            _LogRespository = sysLogRespository;
            _Respository = sysOrganizeRespository;

        }


        public IList<SysOrganize> GetList()
        {
            return _Respository.GetAll().ToList();
        }

        public SysOrganize GetForm(string id)
        {
            return _Respository.Get(id);
        }


        public void SubmitForm(SysOrganize sysOrganize, string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                _Respository.Update(sysOrganize);
            }
            else
            {
                _Respository.Insert(sysOrganize);
            }
        }


        public void DeleteForm(string id)
        {
            if (_Respository.Count(t => t.ParentId.Equals(id)) > 0)
            {
                throw new Exception("删除失败！操作的对象包含了下级数据。");
            }
            else
            {
                _Respository.Delete(t => t.Id == id);
            }
        }


    }


}
