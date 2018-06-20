using SkyMallCore.Core;
using SkyMallCore.Models;
using SkyMallCore.Respository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SkyMallCore.Services
{
    public class MemberService: IMemberService
    {
        ISysLogRespository _LogRespository;
        IMemberRespository _Respository;

        public MemberService(ISysLogRespository sysLogRespository, IMemberRespository respository
            )
        {
            _LogRespository = sysLogRespository;
            _Respository = respository;
        }



        public List<Member> GetList(string keyword = "")
        {
            var expression = ExtLinq.True<Member>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.UserName.Contains(keyword));
                expression = expression.Or(t => t.RealName.Contains(keyword));
                expression = expression.Or(t => t.NickName.Contains(keyword));
            }
            return _Respository.Get(expression).OrderBy(t => t.SortCode).ToList();
        }

        public Member GetForm(string keyValue)
        {
            return _Respository.Get(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            _Respository.Delete(keyValue);
        }
        public void SubmitForm(Member Member, string[] permissionIds, string keyValue)
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


        public void SubmitForm(Member roleEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                roleEntity.Id = keyValue;
                _Respository.Update(roleEntity);
            }
            else
            {
                _Respository.Insert(roleEntity);
            }
        }


    }


}
