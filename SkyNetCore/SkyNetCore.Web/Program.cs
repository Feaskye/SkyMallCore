using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SkyNetCore.Web.Services;

namespace SkyNetCore.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();

            //CreateHost_Main(args).RunSynchronously();
            //await GenericHostTask(args);
        }


        //以下2中主机处理方式
        //1：web host
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();




        public static async Task CreateHost_Main(string[] args)
        {
            await GenericHostTask(args);
        }


        //2：Generic Host 通用主机，即web 和 其他应用都可以用
        public static async Task GenericHostTask(string[] args)
        {
                var host = new HostBuilder()
                      .ConfigureHostConfiguration(configHost =>
                      {
                          configHost.SetBasePath(Directory.GetCurrentDirectory());
                          configHost.AddJsonFile("hostsettings.json", optional: true);
                          configHost.AddEnvironmentVariables(prefix: "PREFIX_");
                          configHost.AddCommandLine(args);
                      })
                      .ConfigureAppConfiguration((hostContext, configApp) =>
                      {
                          configApp.AddJsonFile("appsettings.json", optional: true);
                          configApp.AddJsonFile(
                              $"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json",
                              optional: true);
                          configApp.AddEnvironmentVariables(prefix: "PREFIX_");
                          configApp.AddCommandLine(args);
                      })
                      .ConfigureServices((hostContext, services) =>
                      {
                          services.AddLogging();
                          services.AddHostedService<LifetimeEventsHostedService>();
                          services.AddHostedService<TimedHostedService>();

                          services.AddMvc();

                      })
                      .ConfigureLogging((hostContext, configLogging) =>
                      {
                          configLogging.AddConsole();
                          configLogging.AddDebug();
                      })
                      .UseConsoleLifetime()
                      .Build();
            await host.RunAsync();    //在控制台中运行通用主机
        }






    }






















}
