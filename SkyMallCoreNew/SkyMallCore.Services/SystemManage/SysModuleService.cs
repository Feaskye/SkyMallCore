using SkyCoreLib.Utils;
using SkyMallCore.Models;
using SkyMallCore.Respository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SkyMallCore.Services
{
    public class SysModuleService : ServiceBase<SysModule>, ISysModuleService
    {
        ISysLogRespository _LogRespository;
        ISysModuleRespository _Respository;
        public SysModuleService(ISysLogRespository sysLogRespository,
            ISysModuleRespository sysModuleRespository)
        {
            _LogRespository = sysLogRespository;
            _Respository = sysModuleRespository;
        }


        public List<SysModule> GetList()
        {
            return _Respository.GetAll().ToList();
        }

        public SysModule GetForm(string keyValue)
        {
            return _Respository.Get(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            if (_Respository.Count(t => t.ParentId.Equals(keyValue)) > 0)
            {
                throw new Exception("删除失败！操作的对象包含了下级数据。");
            }
            else
            {
                _Respository.Delete(t => t.Id == keyValue);
            }
        }
        public void SubmitForm(SysModule SysModule, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                SysModule.Id = keyValue;
                _Respository.Update(SysModule);
            }
            else
            {
                _Respository.Insert(SysModule);
            }
        }

    }


}
