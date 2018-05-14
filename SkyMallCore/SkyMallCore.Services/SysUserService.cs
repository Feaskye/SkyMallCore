using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SkyMallCore.Core;
using SkyMallCore.Models;
using SkyMallCore.Respository;
using System.Linq;

namespace SkyMallCore.Services
{
    public class SysUserService : ISysUserService
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


    }

    public class ServiceFactory
    {
        public static void Initialize(IServiceCollection services, IConfiguration configuration)
        {
            RespositoryFactory.Initialize(services, configuration);

            var scopedServices = Reflector.GetScopedList(typeof(ServiceFactory).Assembly).
                Where(w => w.Interface.Name.EndsWith("Service")).ToList();
            scopedServices.ForEach(item =>
                 {
                     services.AddScoped(item.Interface, item.Class);
                 });
        }
    }


}
