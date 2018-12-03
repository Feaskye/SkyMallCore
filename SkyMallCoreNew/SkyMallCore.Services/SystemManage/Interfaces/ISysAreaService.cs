using SkyCoreLib.Utils;
using SkyMallCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.Services
{
    public interface ISysAreaService
    {

        List<SysArea> GetList();
        
        SysArea GetForm(string keyValue);

        void DeleteForm(string keyValue);


        void SubmitForm(SysArea SysArea, string keyValue);



        List<TreeSelectModel> GetAreaList(string parentId = null);

    }


}
