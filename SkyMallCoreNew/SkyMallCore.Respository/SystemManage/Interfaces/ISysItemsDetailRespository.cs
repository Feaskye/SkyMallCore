﻿using SkyMallCore.Data;
using SkyMallCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.Respository
{
    public interface ISysItemsDetailRespository : IRespositoryBase<SysItemsDetail>
    {
        List<SysItemsDetail> GetItemList(string enCode);
    }
}
