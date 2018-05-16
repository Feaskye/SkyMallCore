using SkyMallCore.Core;
using SkyMallCore.Models;
using SkyMallCore.Respository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SkyMallCore.Services
{
    public class SysModuleService : ISysModuleService
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



    }


}
