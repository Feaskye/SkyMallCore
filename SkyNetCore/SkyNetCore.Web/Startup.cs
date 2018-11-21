using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using SkyNetCore.Common;
using SkyNetCore.Web.Models;
using SkyNetCore.Web.Services;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace SkyNetCore.Web
{
    public partial class Startup
    {
        public Startup(IConfiguration configuration)
        {

            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //services.AddSignalR();


            services.AddHostedService<PrinterHostedService>();

            //添加授权认证来产生Token
            #region ==版本问题待详细分析==
            //https://github.com/aspnet/Security/issues/1310
            var audienceConfig = Configuration.GetSection("Audience");
            var symmetricKeyAsBase64 = audienceConfig["Secret"];
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(keyByteArray);

            var tokenValidationParameters = new TokenValidationParameters
            {
                // The signing key must match!
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                // Validate the JWT Issuer (iss) claim
                ValidateIssuer = true,
                ValidIssuer = "http://catcher1994.cnblogs.com/",

                // Validate the JWT Audience (aud) claim
                ValidateAudience = true,
                ValidAudience = "Catcher Wong",

                // Validate the token expiry
                ValidateLifetime = true,

                ClockSkew = TimeSpan.Zero
            };
            services.AddAuthentication().AddJwtBearer(options => {
                options.Audience = Configuration.GetSection("TokenProviderOptions:Audience").Value;
                options.ClaimsIssuer = Configuration.GetSection("TokenProviderOptions:Issuer").Value;
                options.TokenValidationParameters = tokenValidationParameters;
                options.SaveToken = true;
            });
            #endregion

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            //app.UseSignalR(route => {
            //    route.MapHub<MessageHub>("/msghub");
            //});

            //WebSocket的写法
            app.Map("/ws", Services.SocketHandler.Map);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();


            ConfigureJwtAuth(app);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
