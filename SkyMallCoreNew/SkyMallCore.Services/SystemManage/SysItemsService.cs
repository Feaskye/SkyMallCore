using SkyMallCore.Core;
using SkyMallCore.Models;
using SkyMallCore.Respository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyMallCore.Services
{
    public class SysItemsService : ServiceBase<SysItems>, ISysItemsService
    {
        ISysLogRespository _LogRespository;
        ISysItemsRespository _Respository;
        public SysItemsService(ISysLogRespository sysLogRespository
            , ISysItemsRespository sysItemsRespository)
        {
            _LogRespository = sysLogRespository;
            _Respository = sysItemsRespository;
        }


        public IList<SysItems> GetList()
        {
            return _Respository.GetAll().ToList();
        }


        public SysItems GetForm(string keyValue)
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
        public void SubmitForm(SysItems SysItems, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                SysItems.Id = keyValue;
                _Respository.Update(SysItems);
            }
            else
            {
                _Respository.Insert(SysItems);
            }
        }


    }


}
