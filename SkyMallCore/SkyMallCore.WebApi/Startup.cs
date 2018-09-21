using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using SkyMallCore.WebApi.Helpers;
using Swashbuckle.AspNetCore.Swagger;

namespace SkyMallCore.WebApi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                 .SetBasePath(env.ContentRootPath)
                 .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                 .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }
            
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddHttpClient();
            //指定某Client的写法
            //services.AddHttpClient<GithubClient>();

            //json
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options => options.SerializerSettings.ContractResolver
                = new Newtonsoft.Json.Serialization.DefaultContractResolver());

            //swagger
            services.AddSwaggerGen(options=> {
                options.SwaggerDoc("v1",new Info {
                    Version ="v1",
                    Title = "SkyMallCore.WebApi"
                });
                var xmlPath = Path.Combine(AppContext.BaseDirectory, "SkyMallCore.WebApi.xml");
                options.IncludeXmlComments(xmlPath);
            });

            //请求日志、数据过滤、加密等操作
            services.AddSkyApiProvider();

            //https://www.strathweb.com/2018/02/exploring-the-apicontrollerattribute-and-its-features-for-asp-net-core-mvc-2-1/
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressConsumesConstraintForFormFileParameters = true;//多部分/表单数据请求推断 true禁用默认行为
                options.SuppressInferBindingSourcesForParameters = true;//绑定源参数推断 true禁用默认推理规则


                //模型验证
                //options.SuppressModelStateInvalidFilter = true;//属性设置为true时，将禁用自动HTTP 400，即禁用以下方法
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .Select(e =>
                            $"{e.Key}{(string.IsNullOrWhiteSpace(e.Value.Errors.First().ErrorMessage) ? e.Value.Errors.First().Exception?.Message : e.Value.Errors.First()?.ErrorMessage)}"
                        ).ToArray();

                    return new BadRequestObjectResult(new { Success = false, Data = false, Message = string.Join("；", errors) });
                };
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseSkyApiProvider();
            //或者
            //app.UseMiddleware<SkyApiProviderMiddleWare>();

            app.UseStaticFiles();
            
            app.UseMvc(routes =>
            {
               
                //api
                routes.MapRoute(
                    name: "api",
                    template: "api/{controller=Home}/{action=Index}/{id?}");


                //默认起始页
                routes.MapRoute(
                   name: "default",
                   template: "{controller=Home}/{action=Index}/{id?}");


            });

            app.UseSwagger();
            app.UseSwaggerUI(c=> {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SkyMallCoreWebApi");
            });//https://www.cnblogs.com/wms01/p/6667771.html


        }
    }
}
