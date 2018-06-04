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
               options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<ISkyMallDbContext, SkyMallDBContext>();
            services.AddScoped(typeof(IRespositoryBase<>), typeof(RespositoryBase<>));
        }


    }
}
