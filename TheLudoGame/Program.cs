using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using TheLudoGame.Context;

namespace TheLudoGame
{
    class Program
    {
        static void Main(string[] args) => CreateHostBuilder(args).Build().Run();

        public static IHostBuilder CreateHostBuilder(string[] args)
            => Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostContext, configApp) =>
            {
                configApp.SetBasePath(Directory.GetCurrentDirectory());
                configApp.AddJsonFile("appsettings.json");
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddDbContext<LudoContext>(options => options.UseSqlServer(hostContext.Configuration.GetConnectionString("DefaultConnection")));
                services.AddHostedService<PresentationService>();
            });
    }
}
