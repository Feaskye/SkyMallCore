using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SkyMallCore.Data;
using SkyMallCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkyMallCore.Respository
{
    public class MemberRespository : AuditedRespository<Member>, IMemberRespository
    {
        public MemberRespository(ISkyMallDbContext skyMallDbContext) : base(skyMallDbContext)
        {
        }

        public Dictionary<string, string> GetMemNames(string[] memberIds)
        {
            return this.GetFeilds(u =>new { u.Id, u.UserName }, w => memberIds.Contains(w.Id)).ToDictionary(k=>k.Id,v=>v.UserName);
        }


        /// <summary>
        /// 获取用户名、头像集合
        /// </summary>
        /// <param name="memberIds"></param>
        /// <returns></returns>
        public List<Member> GetMemberInfos(string[] memberIds)
        {
            return this.GetFeilds(u => new { u.Id, u.UserName, u.HeadIcon,u.UserLevel,u.LastModifyTime,u.CreatorTime }, w => memberIds.Contains(w.Id)).ToList()
                            .Select(u=>new Member { Id=u.Id,UserName = u.UserName,HeadIcon =u.HeadIcon, UserLevel= u.UserLevel}).ToList();
        }

        /// <summary>
        /// 获取用户积分
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetMemScore(string userId)
        {
            return this.GetFeild(u=>u.UserScore,w => w.Id == userId);
        }
        //public void SubmitForm(SysRole sysRole, List<SysRoleAuthorize> sysRoleAuthorizes, string keyValue)
        //{
        //    using (var db =this.BeginTransaction())
        //    {
        //        if (!string.IsNullOrEmpty(keyValue))
        //        {
        //            this.Update(sysRole);
        //        }
        //        else
        //        {
        //            sysRole.Category = 1;
        //            this.Insert(sysRole);
        //        }
        //        SysRoleAuthorizeRespository.Delete(t => t.ObjectId == sysRole.Id);
        //        SysRoleAuthorizeRespository.Insert(sysRoleAuthorizes);
        //        db.Commit();
        //    }
        //}




    }

}
