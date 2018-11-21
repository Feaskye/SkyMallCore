using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SkyNetCore.Common;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SkyNetCore.GenericHostExample
{
    class Program
    {
        static void Main(string[] args)
        {
            GenericHostRun(args).Wait();
            Console.WriteLine("Hello World!");
        }



        public static async Task GenericHostRun(string[] args)
        {
            var host = new HostBuilder()
                  .ConfigureHostConfiguration(configHost =>
                  {
                      if (args != null)
                      {
                          configHost.AddCommandLine(args);
                      }
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
                      //services.AddLogging();
                      services.AddHostedService<PrinterHostedService>();
                      //services.AddHostedService<LifetimeEventsHostedService>();
                      //services.AddHostedService<TimedHostedService>();


                      //services.AddMvc();

                  })
                  //.ConfigureLogging((hostContext, configLogging) =>
                  //{
                  //    configLogging.AddConsole();
                  //    configLogging.AddDebug();
                  //})
                  .UseConsoleLifetime()
                  .Build();
            await host.RunAsync();    //在控制台中运行通用主机
        }



    }
}
