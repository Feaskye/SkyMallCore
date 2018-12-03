using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SkyMallCore.Core;
using SkyMallCore.Data;
using SkyMallCore.Models;
using SkyMallCore.Respository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SkyMallCore.Services
{
    public class SysUserService : ServiceBase<SysUser>, ISysUserService
    {
        ISysUserLogOnService _SysUserLogOnService;

        ISysUserRespository _SysUserRespository;
        public SysUserService(ISysUserRespository sysUserRespository
            ,ISysUserLogOnService sysUserLogOnService)
        {
            _SysUserRespository = sysUserRespository;
            _SysUserLogOnService = sysUserLogOnService;
        }

        public IList<SysUser> GetUsers()
        {
            return _SysUserRespository.GetSysUsers();
        }

        public List<SysUser> GetList(Pagination pagination, string keyword)
        {
            var expression = base.GetFilterEnabled();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.Account.Contains(keyword));
                expression = expression.Or(t => t.RealName.Contains(keyword));
                expression = expression.Or(t => t.MobilePhone.Contains(keyword));
            }
            return _SysUserRespository.GetPagList(expression, pagination);
        }
        public SysUser GetForm(string id)
        {
            return _SysUserRespository.Get(id);
        }
        public void DeleteForm(string id)
        {
            _SysUserRespository.DeleteForm(id);
        }
        public void SubmitForm(SysUser sysUser, SysUserLogOn userLogOnEntity, string userId)
        {
            _SysUserRespository.SubmitForm(sysUser, userLogOnEntity, userId);
        }


        public void UpdateForm(SysUser sysUser)
        {
            _SysUserRespository.Update(sysUser);
        }


        public SysUser CheckLogin(string userName, string password)
        {
            var sysUser = _SysUserRespository.FirstOrDefault(t => t.Account == userName);
            if (sysUser != null)
            {
                if (sysUser.EnabledMark == true)
                {
                    var userLogOnEntity = _SysUserLogOnService.GetForm(sysUser.Id);
                    string dbPassword = Md5Hash.Md5(DESEncrypt.Encrypt(password.ToLower(), userLogOnEntity.UserSecretkey).ToLower(), 32).ToLower();
                    if (dbPassword == userLogOnEntity.UserPassword)
                    {
                        DateTime lastVisitTime = DateTime.Now;
                        int logOnCount = (userLogOnEntity.LogOnCount).ToInt() + 1;
                        if (userLogOnEntity.LastVisitTime != null)
                        {
                            userLogOnEntity.PreviousVisitTime = userLogOnEntity.LastVisitTime.ToDate();
                        }
                        userLogOnEntity.LastVisitTime = lastVisitTime;
                        userLogOnEntity.LogOnCount = logOnCount;
                        _SysUserLogOnService.UpdateForm(userLogOnEntity);
                        return sysUser;
                    }
                    else
                    {
                        throw new Exception("密码不正确，请重新输入");
                    }
                }
                else
                {
                    throw new Exception("账户被系统锁定,请联系管理员");
                }
            }
            else
            {
                throw new Exception("账户不存在，请重新输入");
            }
        }


        public bool IsAdmin(string id)
        {
            return _SysUserRespository.Any(w=>w.Id == id && w.Account == "admin");
        }


    }

    public static class ServiceFactory
    {
        /// <summary>
        /// 业务逻辑、数据处理层
        /// </summary>
        /// <param name="services"></param>
        public static void AddBusinessService(this IServiceCollection services,IConfiguration configuration)
        {
            //基类
            services.AddScoped<ISkyMallDbContext, SkyMallDBContext>();
            services.AddScoped(typeof(IRespositoryBase<>), typeof(RespositoryBase<>));

            //Respository
            var scopedRespositorys = Reflector.GetScopedList(typeof(AuditedRespository<>).Assembly)
                .Where(w => w.Interface.Name.EndsWith("Respository")).ToList();
            scopedRespositorys.ForEach(item =>
            {
                services.AddScoped(item.Interface, item.Class);
            });
            //Services
            var scopedServices = Reflector.GetScopedList(typeof(ServiceFactory).Assembly).
                Where(w => w.Interface.Name.EndsWith("Service")).ToList();
            scopedServices.ForEach(item =>
                 {
                     services.AddScoped(item.Interface, item.Class);
                 });
        }
    }


}
