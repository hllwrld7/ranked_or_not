using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RankedOrNot.Core;

namespace RankedOrNot.Window
{
    internal class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static async Task Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            

            var host = CreateHostBuilder().Build();
            var services = host.Services;
            Application.Run(services.GetRequiredService<Form1>());
        }

        private static IHostBuilder CreateHostBuilder()
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();
            var apiSettings = new APISettings();
            apiSettings.ServiceApiKey = config["APISettings:ServiceApiKey"];

            return Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddSingleton<Form1>();
                    services.AddScoped<IRiotAPICommunication>(x => new RiotAPICommunication(apiSettings.ServiceApiKey, apiSettings.LeagueName, apiSettings.Tagline));
                });
        }
    }
}