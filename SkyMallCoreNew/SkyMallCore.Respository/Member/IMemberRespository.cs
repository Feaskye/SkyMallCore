using SkyMallCore.Data;
using SkyMallCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.Respository
{
    public interface IMemberRespository : IRespositoryBase<Member>
    {
        /// <summary>
        /// 获取用户名集合
        /// </summary>
        /// <param name="memberIds"></param>
        /// <returns></returns>
        Dictionary<string, string> GetMemNames(string[] memberIds);

        /// <summary>
        /// 获取用户名、头像集合
        /// </summary>
        /// <param name="memberIds"></param>
        /// <returns></returns>
        List<Member> GetMemberInfos(string[] memberIds);

        /// <summary>
        /// 获取用户积分
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        int GetMemScore(string userId);
        



    }
}
