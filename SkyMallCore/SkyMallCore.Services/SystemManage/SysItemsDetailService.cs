using SkyMallCore.Core;
using SkyMallCore.Models;
using SkyMallCore.Respository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyMallCore.Services
{
    public class SysItemsDetailService : ISysItemsDetailService
    {
        ISysLogRespository _LogRespository;
        ISysItemsDetailRespository _Respository;
        public SysItemsDetailService(ISysLogRespository sysLogRespository, ISysItemsDetailRespository sysItemsDetailRespository
            )
        {
            _LogRespository = sysLogRespository;
            _Respository = sysItemsDetailRespository;
        }


        public IList<SysItemsDetail> GetList()
        {
            return _Respository.GetAll().ToList();
        }



    }


}
