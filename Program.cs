using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Battleship
{
    public static class Program
    {
        public static void Main(string[] Args)
        {
            Console.WriteLine("Starting application, please wait...");
            IConfigurationRoot? Config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            WebApplicationBuilder AppBuilder = WebApplication.CreateBuilder(Args);
            AppBuilder.Services.AddControllersWithViews();
            AppBuilder.Logging.ClearProviders();
            WebApplication App = AppBuilder.Build();
            App.MapControllerRoute("default", "api/{controller}/{action=Index}/{id?}");
            App.MapFallbackToFile("index.html");
            Task TaskStart = App.RunAsync();
            Console.WriteLine("Connecting to database...");
            Database.Connect(Config["Database:Host"], Config["Database:User"], Config["Database:Password"], Config["Database:Database"]);
            
            Console.WriteLine("Done! Type 'help' for help.");
            
            bool Shutdown = false;
            while(!Shutdown)
            {
                string[] Command = (Console.ReadLine() ?? "exit").Split(' ');
                switch (Command[0].ToLower())
                {
                    case "exit": case "quit":
                    {
                        Shutdown = true;
                        break;
                    }
                    case "help":
                    {
                        Console.WriteLine("Available commands: exit help info quit");
                        break;
                    }
                    case "info":
                    {
                        Console.WriteLine("Battleship v. 1.0");
                        break;
                    }
                }
            }
            
            Console.WriteLine("Disconnecting from database...");
            Database.Disconnect();
            Console.WriteLine("Stopping application...");
            Task TaskStop = App.StopAsync();
            
            while(!TaskStart.IsCompleted || !TaskStop.IsCompleted)
            { Thread.Sleep(1); }
        }
    }
}