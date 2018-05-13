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
    public class SysUserRespository: RespositoryBase<SysUser>,ISysUserRespository
    {
        public SysUserRespository(ISkyMallDbContext skyMallDbContext) : base(skyMallDbContext)
        { }


        public IList<SysUser> GetSysUsers()
        {
            return this.GetAll().ToList();
        }
    }

    public class RespositoryFactory
    {
        public static void Initialize(IServiceCollection services, IConfiguration configuration)
        {
            DbContextFactory.Initialize(services, configuration);
            services.AddScoped<ISysUserRespository, SysUserRespository>();
        }
    }

}
