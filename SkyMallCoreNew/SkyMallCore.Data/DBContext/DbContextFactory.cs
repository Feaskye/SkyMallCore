using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.Data
{
    public static class DbContextFactory
    {
        public static void InitializeDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SkyMallDBContext>(options =>
               options.UseLazyLoadingProxies()//在您访问导航属性时，会从数据源自动加载相关实体。   
               //大型项目考虑弃用UseLazyLoadingProxies，只使用Include按需加载即可
               .UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<ISkyMallDbContext, SkyMallDBContext>();
            services.AddScoped(typeof(IRespositoryBase<>), typeof(RespositoryBase<>));
        }


    }
}
