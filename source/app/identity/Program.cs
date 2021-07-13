using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using identity.Data;
using identity.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace identity
{
    public class Program
    {
        // public static void Main(string[] args)
        // {
        //     CreateHostBuilder(args).Build().Run();
        // }
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<DataContext>();
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
                await Seed.InitSeed(context, userManager, roleManager);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception: {exception}");
            }

            await host.RunAsync(); 
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
