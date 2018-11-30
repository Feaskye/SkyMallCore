using SkyMallCore.Models;
using SkyMallCore.ViewModel;
using SkyMallCore.ViewModel.Member;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.Services
{
    public interface IMemberScoreService
    {

        MemScoreSearchView GetList(MemScoreSearchView searchView, int pageIndex, int pageSize);

        List<MemScoreDetailView> GetList(bool isTop, int count);



        MemberScore GetForm(string keyValue);


        InvokeResult<bool> DeleteForm(string keyValue);


        void SubmitForm(MemberScore MemberScore, string[] permissionIds, string keyValue);


        InvokeResult<bool> SubmitForm(MemberScore score);

        InvokeResult<int> AddScore(string userId, ScoreType scoreType, string keyId);


        /// <summary>
        /// 购买
        /// </summary>
        /// <param name="buyUserId"></param>
        /// <param name="buyType"></param>
        /// <param name="keyId"></param>
        /// <param name="buyAmount"></param>
        /// <param name="creatorUserId"></param>
        /// <returns></returns>
        InvokeResult<bool> MarketBuy(string buyUserId, ScoreType scoreType, string keyId, int buyAmount,string creatorUserId);

        

    }


}
