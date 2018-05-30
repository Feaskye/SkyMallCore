using SkyMallCore.Core;
using SkyMallCore.Models;
using SkyMallCore.Respository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SkyMallCore.Services
{
    public class SysAreaService : ISysAreaService
    {
        ISysAreaRespository _Respository;
        public SysAreaService(ISysAreaRespository sysAreaRespository)
        {
            _Respository = sysAreaRespository;
        }

        public List<SysArea> GetList()
        {
            return _Respository.GetAll().ToList();
        }
        public SysArea GetForm(string keyValue)
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
        public void SubmitForm(SysArea SysArea, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                SysArea.Id = keyValue;
                _Respository.Update(SysArea);
            }
            else
            {
                _Respository.Insert(SysArea);
            }
        }


    }


}
