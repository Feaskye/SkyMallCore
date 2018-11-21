using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SkyNetCore.Common
{
    public class PrinterHostedService : BackgroundService
    {

        private readonly ILogger _logger;
        //private readonly AppSettings _appSetting;

        public PrinterHostedService(ILoggerFactory loggerFactory)//, IOptionsSnapshot<AppSettings> options)
        {
            this._logger = loggerFactory.CreateLogger<PrinterHostedService>();
            //this._settings = options.Value;
        }


        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Printer1 Starting");

            new Timer(async x=> {

                _logger.LogInformation("Started polling");

            },null,1000,1000);


            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Printer1 Stop");

            return base.StopAsync(cancellationToken);
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Printer1 is working");
                await Task.Delay(TimeSpan.FromSeconds(200), stoppingToken);
            }
        }
    }
}
