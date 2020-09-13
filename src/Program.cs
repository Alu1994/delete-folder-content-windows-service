using DeleteFolderContentService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;
using System.IO;
using System.Reflection;

namespace DeleteFolderService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            var currentPath = Path.GetDirectoryName(path);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.File($"{currentPath}\\log_{DateTime.Now.Date:dd_MM_yyyy}.txt")
                .CreateLogger();

            try
            {                
                CreateHostBuilder(args).Build().Run();
                return;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "There was a problem starting the service");
                return;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {
                    var folderSettings = hostContext.Configuration.GetSection("FolderSettings");
                    services.Configure<FolderSettings>(folderSettings);

                    services.AddHostedService<DeleteFolderContentWorker>();
                })
                .UseSerilog();
    }
}
