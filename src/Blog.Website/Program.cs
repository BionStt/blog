using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace Blog.Website
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }
        
        private static IWebHostBuilder CreateWebHostBuilder(String[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .UseContentRoot(Directory.GetCurrentDirectory())
                   .ConfigureAppConfiguration((hostingContext, config) =>
                    {
                        var environment = hostingContext.HostingEnvironment;

                        config.AddJsonFile("configs/public/appsettings.json", false, true)
                              .AddJsonFile("configs/public/newAppsettings.json", false, true)
                              .AddJsonFile("configs/private/dbsettings.json", false, true)
                              .AddJsonFile("configs/private/authSettings.json", false, true);

                        if (environment.IsDevelopment())
                        {
                            var appAssembly = Assembly.Load(new AssemblyName(environment.ApplicationName));
                            if (appAssembly != null)
                            {
                                config.AddUserSecrets(appAssembly, true);
                            }
                        }

                        config.AddEnvironmentVariables();

                        if (args != null)
                        {
                            config.AddCommandLine(args);
                        }
                    })
                   .ConfigureLogging((hostingContext,
                                      logging) =>
                    {
                        if(hostingContext.HostingEnvironment.IsDevelopment())
                        {
                            logging.AddDebug();
                        }

                        logging.AddConsole();
                    })
                   .UseSerilog((context, configuration) =>
                    {
                        if(context.HostingEnvironment.IsDevelopment())
                        {
                            configuration.MinimumLevel.Information()
                                         .WriteTo.Console();
                        }
                        else
                        {
                            configuration.MinimumLevel.Information()
                                         .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                                         .WriteTo.Console();
                        }
                    })
                   .UseStartup<Startup>();
    }
}