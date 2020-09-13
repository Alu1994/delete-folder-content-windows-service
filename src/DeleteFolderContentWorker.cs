using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DeleteFolderContentService;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DeleteFolderService
{
    public class DeleteFolderContentWorker : BackgroundService
    {
        private readonly ILogger<DeleteFolderContentWorker> _logger;
        private readonly FolderSettings _folderSettings;

        public DeleteFolderContentWorker(ILogger<DeleteFolderContentWorker> logger, IOptions<FolderSettings> options)
        {
            _logger = logger;
            _folderSettings = options.Value;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Delete Folder Content Worker running");
            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"Delete Folder Content Worker checking folder: {_folderSettings.Path}");

                if (Directory.Exists(_folderSettings.Path) &&
                    (Directory.GetFiles(_folderSettings.Path).Length > 0 ||
                    Directory.GetDirectories(_folderSettings.Path).Length > 0))
                {
                    Directory.GetFiles(_folderSettings.Path).ToList().ForEach(file =>
                    {
                        _logger.LogInformation($"Deleting File: {file}");
                        File.Delete(file);
                    });

                    Directory.GetDirectories(_folderSettings.Path).ToList().ForEach(directory =>
                    {
                        _logger.LogInformation($"Deleting Directory: {directory}");
                        Directory.Delete(directory, true);
                    });
                }

                await Task.Delay(TimeSpan.FromMinutes(_folderSettings.DeleteTimespanInMinutes), stoppingToken);
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Delete Folder Content Worker stoped");
            return base.StopAsync(cancellationToken);
        }
    }
}
