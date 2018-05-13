using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.Data
{
    public class DbContextFactory
    {
            public static IServiceProvider ServiceProvider;
        public static void Initialize(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SkyMallDBContext>(options =>
               options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<ISkyMallDbContext, SkyMallDBContext>();
            services.AddScoped(typeof(IRespositoryBase<>), typeof(RespositoryBase<>));
        }

        //public static SkyMallDBContext GetDBContext()
        //{
        //    return new SkyMallDBContext(
        //        ServiceProvider.GetRequiredService<DbContextOptions<SkyMallDBContext>>());
        //}


    }
}
