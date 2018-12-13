using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SkyCore.GlobalProvider;
using SkyCoreLib.Utils;
using SkyMallCore.Data;
using SkyMallCore.Services;
using SkyMallCore.ViewModel;
using SkyMallCoreWeb.AppCode;
using System;
using System.Diagnostics;
using UEditorNetCore;

namespace SkyMallCoreWeb
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json",optional:true,reloadOnChange:true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                builder.AddApplicationInsightsSettings(developerMode: true);
            }
            
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            //https://github.com/durow/UEditorNetCore
            //第一个参数为配置文件路径，默认为项目目录下config.json
            //第二个参数为是否缓存配置文件，默认false
            services.AddUEditorService("wwwroot/js/ueditor/net/config.json");

            services.AddLogging(logging=> {
                logging.AddConfiguration(Configuration.GetSection("Logging"));
                logging.AddDebug();
            });


            services.AddDbContext<SkyMallDBContext>(options => options
                .UseMySql(Configuration.GetConnectionString("DefaultConnection"))
            );
            //注册业务服务
            services.AddBusinessService(Configuration);

            
            //用户认证注册
            services.UserAuthenConfig();

            services.AddMvc(options=> {
                //add filters
                options.Filters.Add(typeof(ErrorAttribute));
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            
            services.AddMapConfig();

            services.AddMemoryCache();
            services.AddSession();

            services.AddHostedService<SysConfigService>();

            services.AddCoreProvider();

            IOCContainer.Register(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime appLifetime)
        {
            app.UseCoreContextProvider(); 

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            //使用授权
            app.UseAuthentication();
            // web socket
            app.Map("/ws", SocketHandler.Map);

            app.UseStaticFiles(new StaticFileOptions {
                //不能识别的文件，作为图片处理
                ServeUnknownFileTypes = true,
                DefaultContentType = "image/png"
            });

            //demo
            //app.UseMiddleware<VoiceSchedulerJob>();

            //SEssion
            app.UseSession();
            //mvc路由
            app.UseMvc(routes =>
            {
                //针对Area
               
                routes.MapAreaRoute(
                  name: "SystemManage", areaName: "SystemManage",
                     template: "SystemManage/{controller=Home}/{action=Index}"
                );

                routes.MapAreaRoute(
                name: "SystemSecurity", areaName: "SystemSecurity",
                   template: "SystemSecurity/{controller=Home}/{action=Index}"
              );

                routes.MapAreaRoute(
                name: "SysComponent", areaName: "SysComponent",
                   template: "SysComponent/{controller=Controll}/{action=Index}"
                   , defaults: new string[] { "SkyMallCoreWeb.Areas.SysComponent.Controllers" }
              );


                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}"
                    , defaults: new string[] { "SkyMallCoreWeb.Controllers" }
                    );

            });


            //程序停止调用函数
            appLifetime.ApplicationStopped.Register(() => { IOCContainer.Dispose(); });

        }

        
    }


    


}
