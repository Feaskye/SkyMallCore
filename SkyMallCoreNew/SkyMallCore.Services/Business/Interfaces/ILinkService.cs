using SkyMallCore.Models;
using SkyMallCore.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.Services
{
    public interface ILinkService
    {

        List<LinkDetailView> GetList(string keyword = "");


        Link GetForm(string keyValue);


        void DeleteForm(string keyValue);


        void SubmitForm(Link Link, string[] permissionIds, string keyValue);


        void SubmitForm(Link roleEntity);
    }


}
