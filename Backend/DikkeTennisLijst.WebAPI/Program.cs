using DikkeTennisLijst.Infrastructure.Data;
using NLog;
using NLog.Web;

namespace DikkeTennisLijst.WebAPI
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var logger = LogManager
                .Setup()
                .LoadConfigurationFromAppSettings()
                .GetCurrentClassLogger();

            try
            {
                (await CreateHostBuilder(args)
                    .Build()
                    .SeedDataAsync())
                    .Run();
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Application will not run. Exception in main with args = {args}", args);
                throw;
            }
            finally
            {
                LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
                .ConfigureLogging(logging => logging.ClearProviders())
                .UseNLog();
    }
}