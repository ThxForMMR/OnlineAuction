using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;


namespace OnlineAuction
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                // Logs Information, Warning, Error and Fatal to "info-logs.txt" file
                .WriteTo.File(path: "info-logs.txt", restrictedToMinimumLevel: LogEventLevel.Information)
                // Logs Error and Fatal to "error-logs.txt" file
                .WriteTo.File(path: "error-logs.txt", restrictedToMinimumLevel: LogEventLevel.Error)
                .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Error)
                .CreateLogger();
            try
            {
                Log.Information("Starting up");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)  
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
