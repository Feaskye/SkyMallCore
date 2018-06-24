using SkyMallCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.Services
{
    public interface ILinkService
    {

        List<Link> GetList(string keyword = "");


        Link GetForm(string keyValue);


        void DeleteForm(string keyValue);


        void SubmitForm(Link Link, string[] permissionIds, string keyValue);


        void SubmitForm(Link roleEntity, string keyValue);
    }


}
