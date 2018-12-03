using SkyCoreLib.Utils;
using SkyMallCore.Models;
using SkyMallCore.Respository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SkyMallCore.Services
{
    public class SysModuleButtonService : ServiceBase<SysModuleButton>, ISysModuleButtonService
    {
        ISysLogRespository _LogRespository;
        ISysModuleButtonRespository _Respository;
        public SysModuleButtonService(ISysLogRespository sysLogRespository
            , ISysModuleButtonRespository sysModuleButtonRespository)
        {
            _LogRespository= sysLogRespository;
            _Respository = sysModuleButtonRespository;
        }

        public List<SysModuleButton> GetList(string moduleId = "")
        {
            var expression = base.GetFilterEnabled();
            if (!string.IsNullOrEmpty(moduleId))
            {
                expression = expression.And(t => t.ModuleId == moduleId);
            }
            return _Respository.Get(expression).OrderBy(t => t.SortCode).ToList();
        }
        public SysModuleButton GetForm(string keyValue)
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
        public void SubmitForm(SysModuleButton sysModuleButton, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                sysModuleButton.Id = keyValue;
                _Respository.Update(sysModuleButton);
            }
            else
            {
                _Respository.Insert(sysModuleButton);
            }
        }
        public void SubmitCloneButton(string moduleId, string Ids)
        {
            string[] ArrayId = Ids.Split(',');
            var data = this.GetList();
            List<SysModuleButton> entitys = new List<SysModuleButton>();
            foreach (string item in ArrayId)
            {
                SysModuleButton SysModuleButton = data.Find(t => t.Id == item);
                SysModuleButton.Id = Common.GuId();
                SysModuleButton.ModuleId = moduleId;
                entitys.Add(SysModuleButton);
            }
            _Respository.SubmitCloneButton(entitys);
        }


    }


}
