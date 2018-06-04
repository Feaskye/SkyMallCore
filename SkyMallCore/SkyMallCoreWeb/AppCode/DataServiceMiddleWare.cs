using Microsoft.Extensions.DependencyInjection;
using SkyMallCore.Core;
using SkyMallCore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkyMallCoreWeb.AppCode
{
    //public static class DataServiceMiddleWare
    //{
    //    /// <summary>
    //    /// 业务逻辑、数据处理层
    //    /// </summary>
    //    /// <param name="services"></param>
    //    public static void AddDataService(this IServiceCollection services)
    //    {
    //        services.AddScoped(typeof(IRespositoryBase<>), typeof(RespositoryBase<>));
    //        //services.AddScoped<ISysUserRespository, SysUserRespository>();
    //        var scopedServices = Reflector.GetScopedList(typeof(RespositoryFactory).Assembly)
    //            .Where(w => w.Interface.Name.EndsWith("Respository")).ToList();
    //        scopedServices.ForEach(item =>
    //        {
    //            services.AddScoped(item.Interface, item.Class);
    //        });


    //        //service
    //        var scopedServices = Reflector.GetScopedList(typeof(ISysLogService).Assembly).
    //            Where(w => w.Interface.Name.EndsWith("Service")).ToList();
    //        scopedServices.ForEach(item =>
    //        {
    //            services.AddScoped(item.Interface, item.Class);
    //        });
    //    }
    //}
}
