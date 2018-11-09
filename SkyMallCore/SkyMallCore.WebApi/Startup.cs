using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using SkyMallCore.WebApi.Helpers;
using Swashbuckle.AspNetCore.Swagger;
using SkyMallCore.WebApiUtils;
using Polly;
using System.Net.Http;
using SkyMallCore.WebApiData;
using Microsoft.EntityFrameworkCore;

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

        /*
         AddSingleton 整个应用程序生命周期以内只创建一个实例
         AddScoped 在同一个Scope内只初始化一个实例，可以理解为（ 每一个request级别只创建一个实例，同一个http request会在一个 scope内）
         AddTransient 每一次GetService都会创建一个新的实例
         */
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient("default")//可以添加多个，或多个client使用相同策略
                        //AddPolicyHandler添加处理超时的策略，必须是IAsyncPolicy<HttpResponseMessage>，这个中策略在任何请求超过10s都会触发。
                        .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(10)))
                        //瞬时故障时添加基本重试，以及重试的次数
                        .AddTransientHttpErrorPolicy(p=>p.RetryAsync(3));
            //指定某Client的写法
            //services.AddHttpClient<GithubClient>();

            //IdentityServer
            services.AddIdentityServer()
                   .AddDeveloperSigningCredential()
                   .AddInMemoryApiResources(IdentityServerConfig.GetApiResources())
                   .AddInMemoryClients(IdentityServerConfig.GetClients());

            //Api Version
            services.AddApiVersioning(v => {
                v.ReportApiVersions = true;//true, 在Api请求的响应头部，会追加当前Api支持的版本
                v.AssumeDefaultVersionWhenUnspecified = true;//标记没使用版本号时使用默认版本号
                v.DefaultApiVersion = new ApiVersion(1,0);//默认版本号
                //  o.ApiVersionReader = new HeaderApiVersionReader("x-api-version");   在HTTP请求头中添加版本号参数。
            });

            //json
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options => options.SerializerSettings.ContractResolver
                = new Newtonsoft.Json.Serialization.DefaultContractResolver());
            
            services.AddDbContext<MysqlDbContext>(op=> {
                op.UseMySql(Configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddScoped<IMysqlDbContext, MysqlDbContext>();

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

            //官方文档：https://docs.microsoft.com/en-us/aspnet/core/tutorials/web-api-vsc?view=aspnetcore-2.1
            //博客用法：https://www.strathweb.com/2018/02/exploring-the-apicontrollerattribute-and-its-features-for-asp-net-core-mvc-2-1/
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


            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                    var exception = errorFeature.Error;

                    // log the exception etc..
                    // produce some response for the caller
                    //使用过滤器的方式，记录异常log
                    await context.Response.WriteAsync(new Controllers.ApiResult<object>(null, false, message: exception.Message).ToJson());
                });
            });


            app.UseStaticFiles();



            app.UseIdentityServer();

            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SkyMallCoreWebApi");
            });//https://www.cnblogs.com/wms01/p/6667771.html


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




        }
    }
}
