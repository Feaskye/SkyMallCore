
using Microsoft.Extensions.Hosting;
using SkyCore.GlobalProvider;
using SkyMallCore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SkyMallCore.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace SkyMallCoreWeb.AppCode
{
    public class SysConfigService : BackgroundService
    {
        private ISysConfigurationService _sysConfigurationService;
        private ILogger _logger;
        private Timer _timer;
        private SkyMallCore.Data.ISkyMallDbContext _skyMallDbContext;

        public SysConfigService(IServiceScopeFactory serviceScope, ILogger<SysConfigService> logger)
        {
            _skyMallDbContext = serviceScope.CreateScope().ServiceProvider.GetService<SkyMallCore.Data.ISkyMallDbContext>();
            //_sysConfigurationService = serviceScope.CreateScope().ServiceProvider.GetService<ISysConfigurationService>();
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("---------------Start-------");
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("---------------Stop-------");
            _timer?.Change(Timeout.Infinite, 0);
            return base.StopAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("---------------Execute-------");

            _timer = new Timer(LoadConfiguration, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            return Task.CompletedTask;
        }


        private void LoadConfiguration(object state)
        {
            Console.WriteLine("------------------------state:" + state);
            //加载配置文件
            if (ConfigManager.SysConfiguration != null)
            {
                return;
            }
            try
            {
                var configs = _skyMallDbContext.Set<SkyMallCore.Models.SysConfiguration>().Where(w=>w.EnabledMark == true)
                                    .Select(u => new { u.ConfigCode, u.ConfigValue }).ToDictionary(k => k.ConfigCode, v => v.ConfigValue);
                var _sysConfig = new SysConfigurationModel()
                {
                    SiteName = configs.TryGetValue("SiteName"),
                    UploadFolder = configs.TryGetValue("UploadFolderCode"),
                    SiteKeywords = configs.TryGetValue("SiteKeywords"),
                    SiteDescription = configs.TryGetValue("SiteDescription"),
                    SiteLogo = configs.TryGetValue("SiteLogo"),
                    SiteQQ = configs.TryGetValue("SiteQQ"),
                    SysMailPassword = configs.TryGetValue("SysMailPassword"),
                    SysMailUser = configs.TryGetValue("SysMailUser"),
                    //SiteQQ = configs.TryGetValue("SiteQQ"),
                    //VoiceRootFolder = configlist.Where(w => w.ConfigCode == ConstParameters.VoiceRootFolderCode).Select(u => u.ConfigValue).FirstOrDefault() //,
                    //ExecuteHour = Convert.ToInt32(CoreContextProvider.Configuration.GetSection("SysConfiguration")["JobExecuteHour"]),
                    //ExecuteMunite = Convert.ToInt32(CoreContextProvider.Configuration.GetSection("SysConfiguration")["JobExecuteMunite"])
                };
                ConfigManager.SysConfiguration = _sysConfig;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }



    }
}
