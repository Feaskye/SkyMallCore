using Microsoft.Extensions.Logging;
using SkyCore.GlobalProvider;
using SkyCoreLib.Utils;
using SkyMallCore.Models;
using SkyMallCore.Respository;
using SkyMallCore.ViewModel;
using SkyMallCore.ViewModel.Member;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SkyMallCore.Services
{
    public class MemberScoreService :ServiceBase<MemberScore>, IMemberScoreService
    {
        ISysLogRespository _LogRespository;
        IMemberScoreRespository _Respository;
        IMemberRespository _MemRespository;
        ISysItemsDetailService _SysItemsDetailService;
        ILogger _Logger;
        public MemberScoreService(ISysLogRespository sysLogRespository, IMemberScoreRespository respository
            , IMemberRespository memRespository, ISysItemsDetailService sysItemsDetailService)
        {
            _LogRespository = sysLogRespository;
            _MemRespository=memRespository;
            _Respository = respository;
            _SysItemsDetailService = sysItemsDetailService;
            _Logger = CoreContextProvider.GetLogger("IMemberScoreService");
        }


        /// <summary>
        /// 获取积分列表
        /// </summary>
        /// <param name="searchView"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public MemScoreSearchView GetList(MemScoreSearchView searchView, int pageIndex, int pageSize)
        {
            var expression = base.GetFilterEnabled();

            if (searchView.SearchType.HasValue)
            {
                expression = expression.And(t => t.OperatType == searchView.SearchType.Value);
            }

            if (!searchView.ScoreType.IsEmpty())
            {
                expression = expression.And(t => t.ScoreType == searchView.ScoreType);
            }
            if (searchView.StartDate.HasValue)
            {
                expression = expression.And(t => t.CreatorTime >= searchView.StartDate);
            }

            if (searchView.EndDate.HasValue)
            {
                expression = expression.And(t => t.CreatorTime <= searchView.EndDate);
            }
            //会员信息
            if (!searchView.MemberId.IsEmpty())
            {
                expression = expression.And(t => t.MemberId == searchView.MemberId);
            }

            searchView.MemScore = _MemRespository.GetMemScore(searchView.MemberId);
            searchView.TotalScore = _Respository.Get(expression).Sum(w=>w.Score);
            searchView.ScoreList = _Respository.GetPagedList(u => new MemScoreDetailView
            {
                Id = u.Id,
                Score = u.Score,
                MemberId = u.MemberId,
                ScoreType = u.ScoreType,
                CreatorTime = u.CreatorTime,
                Description = u.Description,
                OperatType = u.OperatType
            }, expression, pageIndex, pageSize, o => o.OrderBy(b => b.SortCode));

            return searchView;
        }

        public List<MemScoreDetailView> GetList(bool isTop, int count)
        {
            var expression = base.GetFilterEnabled();
            Func<IQueryable<MemberScore>, IOrderedQueryable<MemberScore>> order = o => o.OrderBy(b => b.SortCode);
            if (isTop)
            {
                order = o => o.OrderByDescending(w => w.CreatorTime);
            }
            var data = _Respository.GetFeilds(u => new MemScoreDetailView
            {
                Score = u.Score,
                Member = u.Member.UserName,
                MemberId = u.MemberId,
                ScoreType = u.ScoreType,
                OperatType = u.OperatType,
            }, expression, order, "Member").Take(count).ToList();

            var scoreTypes = data.Select(w => w.ScoreType + "").Distinct().ToArray();
            var itemDataService = CoreContextProvider.GetService<ISysItemsDetailService>();
            var itemDatas=  itemDataService.GetListItem("ScoreType",scoreTypes);
            data.ForEach(d =>
            {
                var t = d.OperatType == 0 ? "ScoreType" : "ScoreType1";
                d.ScoreTypeName = itemDatas.Where(w => w.ParentId == t && w.Code == d.ScoreType + "").Select(w => w.Text).FirstOrDefault();
            });
            return data;
        }

        public MemberScore GetForm(string keyValue)
        {
            return _Respository.Get(keyValue);
        }
        public InvokeResult<bool> DeleteForm(string keyValue)
        {
            using (var db = _Respository.BeginTransaction())
            {
                var b = true;
                try
                {
                    if (_MemRespository.Any(w => w.GroupId == keyValue))
                    {//级联归组：未分组
                        var mems = _MemRespository.Get(w => w.GroupId == keyValue).ToList();
                        mems.ForEach(mem =>
                        {
                            mem.GroupId = null;
                        });
                        b = _MemRespository.UpdateFields(mems, "GroupId");
                    }
                    if (!b)
                    {
                        db.Rollback();
                        return RequestResult.Failed<bool>("删除关联的客户信息失败");
                    }
                    _Respository.Delete(keyValue);
                    db.Commit();
                    return RequestResult.Success(true);
                }
                catch (Exception ex)
                {
                    db.Rollback();
                    _Logger.LogError(ex, "修改备注失败-文件重命名失败:" + ex.Message);
                    return RequestResult.Failed<bool>("删除关联的客户信息失败");
                }
            }
        }
        public void SubmitForm(MemberScore Member, string[] permissionIds, string keyValue)
        {
            //if (!string.IsNullOrEmpty(keyValue))
            //{
            //    Member.Id = keyValue;
            //}
            //else
            //{
            //    Member.Id = Common.GuId();
            //}
            //var moduledata = _SysModuleService.GetList();
            //var buttondata = _SysModuleButtonService.GetList();
            //List<MemberAuthorize> MemberAuthorizes = new List<MemberAuthorize>();
            //foreach (var itemId in permissionIds)
            //{
            //    MemberAuthorize MemberAuthorize = new MemberAuthorize();
            //    MemberAuthorize.Id = Common.GuId();
            //    MemberAuthorize.ObjectType = 1;
            //    MemberAuthorize.ObjectId = Member.Id;
            //    MemberAuthorize.ItemId = itemId;
            //    if (moduledata.Find(t => t.Id == itemId) != null)
            //    {
            //        MemberAuthorize.ItemType = 1;
            //    }
            //    if (buttondata.Find(t => t.Id == itemId) != null)
            //    {
            //        MemberAuthorize.ItemType = 2;
            //    }
            //    MemberAuthorizes.Add(MemberAuthorize);
            //}
            //_Respository.SubmitForm(Member, MemberAuthorizes, keyValue);
        }


        public InvokeResult<bool> SubmitForm(MemberScore score)
        {
            var b = false;
            if (!string.IsNullOrEmpty(score.Id))
            {
                b = _Respository.Update(score);
            }
            else
            {
                score.Id = Guid.NewGuid().ToString();
                b = _Respository.Insert(score);
            }
            return RequestResult.Result(b, "更新积分失败");
        }


        /// <summary>
        /// 购买，积分等于0已处理
        /// </summary>
        /// <param name="buyUserId"></param>
        /// <param name="buyType"></param>
        /// <param name="keyId"></param>
        /// <param name="buyAmount"></param>
        /// <param name="creatorUserId"></param>
        /// <returns></returns>
        public InvokeResult<bool> MarketBuy(string buyUserId, ScoreType scoreType, string keyId, int buyAmount, string creatorUserId)
        {
            if (creatorUserId.IsEmpty())
            {
                return RequestResult.Failed<bool>("原会员编号不存在，操作失败！");
            }
            //已购买则返回，直接买
            if (_Respository.Any(w => w.MemberId == buyUserId && w.OperatType == 1 && w.ScoreType == (int)scoreType && w.KeyId == keyId))
            {
                return RequestResult.Success(true);
            }
            var memScore = _MemRespository.GetMemScore(buyUserId);
            if (memScore < buyAmount)
            {
                return RequestResult.Failed<bool>("积分不足，操作失败！");
            }

            var desc = EnumCommon.GetDescription(scoreType);

            var buyScore = new MemberScore
            {
                MemberId = buyUserId,
                Score = buyAmount,
                ScoreType = (int)scoreType,
                OperatType = 1,
                KeyId = keyId,
                Description = $"用户{desc}扣除{buyAmount}积分"
            };


            using (var tran = _Respository.BeginTransaction())
            {
                try
                {
                    var b = false;
                    if (buyAmount > 0)
                    {
                        //扣积分
                        b = _MemRespository.UpdateFields(new Member { Id = buyUserId, UserScore = memScore - buyAmount }, "UserScore");
                        if (!b)
                        {
                            tran.Rollback();
                            return RequestResult.Failed<bool>("扣除积分失败，请重试");
                        }
                    }
                    //扣积分记录
                    b = _Respository.CreateOrUpdate(buyScore);
                    if (!b)
                    {
                        tran.Rollback();
                        return RequestResult.Failed<bool>("扣除积分失败，请重试");
                    }
                    //奖积分
                    if (buyAmount > 0)
                    {
                        memScore = _MemRespository.GetMemScore(creatorUserId);
                        b = _MemRespository.UpdateFields(new Member { Id = creatorUserId, UserScore = memScore + buyAmount }, "UserScore");
                        if (!b)
                        {
                            tran.Rollback();
                            return RequestResult.Failed<bool>("奖励积分失败，请重试");
                        }


                        if (scoreType == ScoreType.buytopic)
                        {
                            scoreType = ScoreType.ubuytopic;
                        }
                        else if (scoreType == ScoreType.buybook)
                        {
                            scoreType = ScoreType.ubuybook;
                        }
                        else if (scoreType == ScoreType.downloadbook)
                        {
                            scoreType = ScoreType.udownloadbook;
                        }
                        else if (scoreType == ScoreType.downloadtopic)
                        {
                            scoreType = ScoreType.udownloadtopic;
                        }

                        desc = EnumCommon.GetDescription(scoreType);

                        var getScore = new MemberScore();
                        getScore.MemberId = creatorUserId;
                        getScore.ScoreType = (int)scoreType;
                        getScore.OperatType = 0;
                        getScore.Description = $"用户购买{desc}奖励{buyAmount}积分";
                        b = _Respository.CreateOrUpdate(getScore);
                        if (!b)
                        {
                            tran.Rollback();
                            return RequestResult.Failed<bool>("奖励积分失败，请重试");
                        }
                    }

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    _Logger.LogError("[MarketBuy]" + ex.Message);
                    return RequestResult.Failed<bool>("更新积分失败，请重试");
                }
            }

            return RequestResult.Success(true);
        }

        /// <summary>
        /// 添加积分
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="scoreType"></param>
        /// <param name="keyId"></param>
        /// <returns></returns>
        public InvokeResult<int> AddScore(string userId, ScoreType scoreType, string keyId)
        {
            //默认赠送积分多少
            var sysitem = _SysItemsDetailService.GetItem("ScoreType", ((int)scoreType).ToString());
            if (sysitem == null)
            {
                _Logger.LogError($"获取积分失败：memberId:{userId};scoreType:{(int)scoreType}");
                return RequestResult.Failed<int>("获取积分失败");
            }
            var amount = sysitem.Description.ToInt();

            if (!_Respository.Any(w => w.MemberId == userId && w.OperatType == 0 && w.ScoreType == (int)scoreType && w.KeyId == keyId))
            {
                var desc = EnumCommon.GetDescription(scoreType);

                if (userId.IsEmpty())
                {
                    return RequestResult.Failed<int>("原会员编号不存在，操作失败！");
                }
                var memScore = _MemRespository.GetMemScore(userId);
                var buyScore = new MemberScore
                {
                    MemberId = userId,
                    Score = amount,
                    ScoreType = (int)scoreType,
                    OperatType = 0,
                    KeyId = keyId,
                    Description = $"用户{desc}赠送{amount}积分"
                };

                var b = false;
                //奖积分
                b = _MemRespository.UpdateFields(new Member { Id = userId, UserScore = memScore + amount }, "UserScore");
                if (!b)
                {
                    return RequestResult.Failed<int>("奖励积分失败，请重试");
                }
                //奖积分记录
                b = _Respository.CreateOrUpdate(buyScore);
                if (!b)
                {
                    return RequestResult.Failed<int>("奖励积分失败，请重试");
                }

            }
            return RequestResult.Success(amount);
        }





    }


}
