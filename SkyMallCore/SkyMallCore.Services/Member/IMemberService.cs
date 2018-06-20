using SkyMallCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.Services
{
    public interface IMemberService
    {

        List<Member> GetList(string keyword = "");


        Member GetForm(string keyValue);


        void DeleteForm(string keyValue);


        void SubmitForm(Member Member, string[] permissionIds, string keyValue);


        void SubmitForm(Member roleEntity, string keyValue);
    }


}
