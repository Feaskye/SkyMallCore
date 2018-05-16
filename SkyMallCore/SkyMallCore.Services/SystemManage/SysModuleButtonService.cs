using SkyMallCore.Core;
using SkyMallCore.Models;
using SkyMallCore.Respository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SkyMallCore.Services
{
    public class SysModuleButtonService : ISysModuleButtonService
    {
        ISysLogRespository _LogRespository;
        ISysModuleButtonRespository _Respository;
        public SysModuleButtonService(ISysLogRespository sysLogRespository
            , ISysModuleButtonRespository sysModuleButtonRespository)
        {
            _LogRespository= sysLogRespository;
            _Respository = sysModuleButtonRespository;
        }

        public List<SysModuleButton> GetList()
        {
            return _Respository.GetAll().ToList();
        }


    }


}
