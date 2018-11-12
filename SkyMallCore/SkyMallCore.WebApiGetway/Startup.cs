using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IdentityServer4.Configuration;
using Ocelot.DependencyInjection;
using IdentityServer4.AccessTokenValidation;
using Ocelot.Middleware;

namespace SkyMallCore.WebApiGetway
{
    public class Startup
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


            void ooptions(IdentityServerAuthenticationOptions o)
            {
                o.Authority = "http://localhost:63656";
                o.RequireHttpsMetadata = false;
                o.ApiName = "api1";
            }

            services
               .AddOcelot(new ConfigurationBuilder()
                   .AddJsonFile("configuration.json")
                   .Build())
               .AddAdministration("/administration", ooptions);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                        .AddIdentityServerAuthentication("TestKey", ooptions);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();
            app.UseOcelot();

            app.UseMvc();
        }
    }
}
