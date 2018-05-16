using SkyMallCore.Core;
using SkyMallCore.Models;
using SkyMallCore.Respository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyMallCore.Services
{
    public class SysItemsService : ISysItemsService
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


    }


}
