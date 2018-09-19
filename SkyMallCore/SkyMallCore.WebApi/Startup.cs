using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
            services.AddMvc();
            //    .AddJsonOptions(options=> options.SerializerSettings.ContractResolver
            //    =new Newtonsoft.Json.Serialization.DefaultContractResolver());

            //swagger
            services.AddSwaggerGen(options=> {
                options.SwaggerDoc("v1",new Info {
                    Version ="v1",
                    Title = "SkyMallCore.WebApi"
                });
                var xmlPath = Path.Combine(AppContext.BaseDirectory, "SkyMallCore.WebApi.xml");
                options.IncludeXmlComments(xmlPath);
            });


            services.AddSkyApiProvider();


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
