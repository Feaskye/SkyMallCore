using SkyCoreLib.Utils;
using SkyMallCore.Models;
using SkyMallCore.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.Services
{
    public interface IFilterIPService
    {
        List<FilterIP> GetList(string keyword);


        FilterIP GetForm(string keyValue);

        void DeleteForm(string keyValue);

        InvokeResult<bool> SubmitForm(FilterIP FilterIP, string keyValue);


    }


}
