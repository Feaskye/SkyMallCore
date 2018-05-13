using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SkyMallCore.Models;
using SkyMallCore.Respository;

namespace SkyMallCore.Services
{
    public class SysUserService : ISysUserService
    {
        ISysUserRespository _SysUserRespository;
        public SysUserService(ISysUserRespository sysUserRespository)
        {
            _SysUserRespository = sysUserRespository;
        }

        public IList<SysUser> GetUsers()
        {
            return _SysUserRespository.GetSysUsers();
        }
    }

    public class ServiceFactory
    {
        public static void Initialize(IServiceCollection services, IConfiguration configuration)
        {
            RespositoryFactory.Initialize(services,configuration);
            services.AddScoped<ISysUserService, SysUserService>();
        }
    }


}
