AspNetCore WebApi教程总结说明

一：说明
在.Net Core中Api 直接是MVC的控制器，无需安装WebApi包，当配置要求生成XML后，
编译你的项目时会产生一系列的警告信息，因为你暂时还未完成代码的文档注释
项目->属性->生成->输出->xml文档文件  打钩√

二：路由指定
在控制器中加入路由指定即可：
[Route("api/[controller]")]
public class HomeController
并在对应Api函数头部加入相应说明描述

三：Api文档生成
生成Api文档：<a href='https://docs.microsoft.com/zh-cn/aspnet/core/tutorials/web-api-help-pages-using-swagger' target='_blank' >微软官方文档</a>

1：安装Swashbuckle.AspNetCore包
2：Startup加入相应配置
		public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            //swagger
            services.AddSwaggerGen(options=> {
                options.SwaggerDoc("v1",new Info {
                    Version ="v1",
                    Title = "SkyMallCore.WebApi"
                });
                var xmlPath = Path.Combine(AppContext.BaseDirectory, "SkyMallCore.WebApi.xml");
                options.IncludeXmlComments(xmlPath);
            });
        }


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

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "api/{controller=Home}/{action=Index}/{id?}");
            });

			//=== 加入配置 可参考https://www.cnblogs.com/wms01/p/6667771.html
            app.UseSwagger();
            app.UseSwaggerUI(c=> {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SkyMallCoreWebApi");
            });


        }
3：项目->属性->生成->输出->xml文档文件  打钩√
4：浏览文档成功！

四：应用
1：文档提交数据，默认参数如：Token、Headers等
TODO待完善