using SkyMallCore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using SkyMallCore.ViewModel;

namespace SkyMallCore.Services
{
    public interface IMemberService
    {

        string GetMemName(string userId);

        Member GetMember(string userId);

        /// <summary>
        /// 获取用户名集合
        /// </summary>
        /// <param name="memberIds"></param>
        /// <returns></returns>
        Dictionary<string, string> GetMemNames(string[] memberIds);

        PagedList<MemberDetailView> GetList(MemberSearchView search, int pageIndex, int pageSize = 20);


        Member GetForm(string keyValue,string mobile);


        void DeleteForm(string keyValue);


        //void SubmitForm(Member Member, string[] permissionIds, string keyValue);


        InvokeResult<bool> SubmitForm(Member roleEntity);

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        InvokeResult<bool> Register(RegisterViewModel model);

        InvokeResult<Member> CheckLogin(string userName, string password);
        InvokeResult<bool> ChangeEmail(string userId, string email);
        InvokeResult<bool> ChangePwd(string userId, string password);
        InvokeResult<bool> ChangeImage(string userId, string headIcon);
        InvokeResult<bool> ExistUserName(string name, string memberId = null);


        List<MemTopDetailView> GetTopMembers(MemTopEnum? memTopEnum, int count);


        /// <summary>
        /// 获取用户编号
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        string GetUserIdByName(string userName);
    }


}
