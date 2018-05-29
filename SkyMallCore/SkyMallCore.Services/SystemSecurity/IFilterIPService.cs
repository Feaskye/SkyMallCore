using SkyMallCore.Core;
using SkyMallCore.Models;
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

        void SubmitForm(FilterIP FilterIP, string keyValue);


    }


}
