
using Microsoft.Extensions.Logging;
using SkyCoreLib.Utils;
using SkyMallCore.Models;
using SkyMallCore.Respository;
using SkyMallCore.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SkyMallCore.Services
{
    public class MemberService : ServiceBase<Member>, IMemberService
    {
        IMemberRespository _Respository;
        IMemberScoreService _MemberScoreService;
        ILogger _logger;
        public MemberService(ISysLogRespository sysLogRespository, IMemberRespository respository
            , IMemberScoreService memberScoreService)
        {
            _Respository = respository;
            _MemberScoreService = memberScoreService;
            _logger = SkyCore.GlobalProvider.CoreContextProvider.GetLogger("MemberService");
        }



        public PagedList<MemberDetailView> GetList(MemberSearchView search, int pageIndex, int pageSize = 20)
        {
            var expression = base.GetFilterEnabled();
            
            if (!string.IsNullOrEmpty(search.keyword))
            {
                var keyword = search.keyword;
                expression = expression.And(t => t.UserName.Contains(search.keyword)
                                                                    || t.RealName.Contains(search.keyword)
                                                                    || t.Telephone.Contains(keyword)
                                                                    || t.MobilePhone.Contains(keyword));
            }

            if (search.UserLevel.HasValue)
            {
                expression = expression.And(p => p.UserLevel == (int)search.UserLevel);
            }

            return _Respository.GetPagedList(t => new MemberDetailView
            {
                Id = t.Id,
                UserName = t.UserName,
                UserLevel = (UserLevel)t.UserLevel,
                UserScore = t.UserScore,
                RealName = t.RealName,
                Address = t.Address,
                Company = t.Company,
                Position = t.Position,
                Telephone = t.Telephone,
                HomePhone = t.HomePhone,
                Tax = t.Tax,
                Email = t.Email,
                Birthday = t.Birthday,
                MobilePhone = t.MobilePhone,
                ZipCode = t.ZipCode,
                EnabledMark = t.EnabledMark
            }, expression, pageIndex, pageSize, d => d.OrderBy(t => t.SortCode));
        }

        public string GetMemName(string userId)
        {
            var filter = base.GetFilterEnabled();
            filter = filter.And(w => w.Id == userId);
            return _Respository.GetFeild(u => u.UserName, filter);
        }

        public Member GetMember(string userId)
        {

            return _Respository.Get(userId);
        }


        public Dictionary<string, string> GetMemNames(string[] memberIds)
        {
            return _Respository.GetMemNames(memberIds);
        }

        public Member GetForm(string keyValue, string mobile)
        {
            if (!string.IsNullOrWhiteSpace(mobile))
            {
                return _Respository.Get(w => w.MobilePhone.Contains(mobile) || w.Telephone.Contains(mobile)).FirstOrDefault();
            }
            return _Respository.Get(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            _Respository.Delete(keyValue);
        }

        public InvokeResult<bool> ChangeEmail(string userId, string email)
        {
            var entity = GetMember(userId);
            entity.Email = email;
            var result = _Respository.UpdateFields(entity, "Email");
            return RequestResult.Result(result, "邮箱修改失败！");
        }

        public InvokeResult<bool> ChangePwd(string userId, string password)
        {
            var entity = GetMember(userId);
            if (entity == null)
            {
                return RequestResult.Result(false, "未找到该用户密码修改失败！");
            }
            entity.Password = EncodePassword(password);
            var result = _Respository.UpdateFields(entity, "Password");
            return RequestResult.Result(result, "密码修改失败！");
        }

        public InvokeResult<bool> ChangeImage(string userId, string headIcon)
        {
            var entity = GetMember(userId);
            entity.HeadIcon = headIcon;
            var result = _Respository.UpdateFields(entity, "HeadIcon");
            return RequestResult.Result(result, "头像修改失败！");
        }

        public InvokeResult<bool> ExistUserName(string name, string memberId = null)
        {
            var filter = base.GetFilterEnabled();
            filter = w => w.UserName == name;
            if (memberId.IsEmpty())
            {
                filter = filter.And(w => w.Id != memberId);
            }
            var b = _Respository.Any(filter);
            return RequestResult.Result(b);
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public InvokeResult<bool> Register(RegisterViewModel model)
        {
            if (model.UserName.IsEmpty())
            {
                return RequestResult.Result(false, "用户名不能为空注册失败 请重试");
            }
            if (model.Password.IsEmpty())
            {
                return RequestResult.Result(false, "密码不能为空注册失败 请重试");
            }

            if (_Respository.Any(w => w.UserName == model.UserName))
            {
                return RequestResult.Result(false, "该用户名已存在，请更改重试");
            }

            return SubmitForm(new Member()
            {
                UserName = model.UserName,
                Password = model.Password,
                UserLevel = (int)UserLevel.Common,
                //UserScore = 1   //todo 注册积分+1 记录
            });
        }

        /// <summary>
        /// 信息编辑
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public InvokeResult<bool> SubmitForm(Member member)
        {
            if (_Respository.Any(w => w.UserName == member.UserName && (member.Id.IsEmpty()?true:w.Id != member.Id)))
            {
                return RequestResult.Failed<bool>("该用户名已存在");
            }

            if (!string.IsNullOrEmpty(member.Id))
            {
                if (!string.IsNullOrWhiteSpace(member.MobilePhone) &&
                    _Respository.Any(w => w.Id != member.Id
                    && w.MobilePhone == member.MobilePhone))
                {
                    return RequestResult.Failed<bool>("该手机号已存在");
                }
                var b =_Respository.Update(member);
                if (!b)
                {
                    return RequestResult.Failed<bool>("修改失败，请重试！");
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(member.MobilePhone) &&
                    _Respository.Any(w => w.MobilePhone == member.MobilePhone))
                {
                    return RequestResult.Failed<bool>("该手机号已存在");
                }
                if (member.Password.IsEmpty())
                {
                    return RequestResult.Failed<bool>("密码不能为空");
                }
                member.Password = EncodePassword(member.Password);
                member.Id = Guid.NewGuid().ToString();

                using (var tran = _Respository.BeginTransaction())
                {
                    try
                    {

                        var b = _Respository.Insert(member);
                        if (!b)
                        {
                            tran.Rollback();
                            return RequestResult.Failed<bool>("注册失败，请重试！");
                        }

                        var scoreResult = _MemberScoreService.AddScore(member.Id, ScoreType.reg, member.Id);
                        if (!scoreResult.Success)
                        {
                            tran.Rollback();
                            return RequestResult.Failed<bool>("注册积分处理失败，请重试！");
                        }
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        _logger.LogError(ex.ToString());
                        return RequestResult.Failed<bool>("注册处理失败，请重试！");
                    }
                }

            }
            return RequestResult.Success(true);
        }

        public InvokeResult<Member> CheckLogin(string userName, string password)
        {
            var member = _Respository.FirstOrDefault(t => t.UserName == userName);
            if (member == null)
            {
                return RequestResult.Failed<Member>("账户不存在，请重新输入");
            }
            if (member.EnabledMark == false)
            {
                return RequestResult.Failed<Member>("账户被系统锁定,请联系管理员");
            }
            string dbPassword = EncodePassword(password);
            if (dbPassword != member.Password)
            {
                return RequestResult.Failed<Member>("密码不正确，请重新输入");
            }
            return RequestResult.Success(member);
        }



        /// <summary>
        /// 获取前几条
        /// </summary>
        /// <param name="memTopEnum"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<MemTopDetailView> GetTopMembers(MemTopEnum? memTopEnum, int count)
        {
            var filter = base.GetFilterEnabled();
            var order = base.Order();
            if (memTopEnum.HasValue && memTopEnum.Value == MemTopEnum.ResourseCount)
            {
                order = w => w.OrderByDescending(b => b.Articles.Count);
            }

            return _Respository.GetFeilds(t => new MemTopDetailView
            {
                UserId = t.Id,
                RealName = t.RealName,
                ArticleCount = t.Articles.Count(w => w.ArticleCategory != null),
                TopicCount = t.Articles.Count(w => w.ArticleTopic != null),
                FansCount = 0,
                HeadIcon = t.HeadIcon,
                NickName = t.NickName
            }, filter, order, "Articles,ArticleCategory").ToList();

        }



        /// <summary>
        /// 获取用户编号
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public string GetUserIdByName(string userName)
        {
            return _Respository.GetFeild(u => u.Id, w => w.UserName == userName);
        }









        /// <summary>
        /// 密码加密
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        private string EncodePassword(string password)
        {
            return Md5Hash.Md5(DESEncrypt.Encrypt(password.ToLower(), 
                        ConstParameters.MemLoginUserKey).ToLower(), 32).ToLower();
        }








    }


}
