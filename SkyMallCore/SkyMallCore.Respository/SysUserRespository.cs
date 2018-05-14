using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SkyMallCore.Core;
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

            services.AddScoped(typeof(IRespositoryBase<>), typeof(RespositoryBase<>));
            //services.AddScoped<ISysUserRespository, SysUserRespository>();
            var scopedServices = Reflector.GetScopedList(typeof(RespositoryFactory).Assembly)
                .Where(w => w.Interface.Name.EndsWith("Respository")).ToList();
               scopedServices .ForEach(item =>
                {
                    services.AddScoped(item.Interface, item.Class);
                });
        }
    }

}
