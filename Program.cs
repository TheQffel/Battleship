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
            System.Console.BackgroundColor = ConsoleColor.Black;
            Console.Log(Console.LogType.Info, "Starting application, please wait...");
            IConfigurationRoot? Config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            WebApplicationBuilder AppBuilder = WebApplication.CreateBuilder(Args);
            AppBuilder.Services.AddControllersWithViews();
            AppBuilder.Logging.ClearProviders();
            WebApplication App = AppBuilder.Build();
            App.MapControllerRoute("default", "api/{controller}/{action=Index}/{id?}");
            App.MapFallbackToFile("index.html");
            Task TaskStart = App.RunAsync();
            Console.Log(Console.LogType.Info, "Connecting to database...");
            Database.Connect(Config["Database:Host"], Config["Database:User"], Config["Database:Password"], Config["Database:Database"]);
            Console.Log(Console.LogType.Ok, "Done! Type 'help' for help.");
            
            bool Shutdown = false;
            while(!Shutdown)
            {
                string[] Command = (System.Console.ReadLine() ?? "exit").Split(' ');
                System.Console.CursorTop--;
                Console.Log(Console.LogType.Warning, "User issued server command: " + Command[0]);
                switch (Command[0].ToLower())
                {
                    case "exit": case "quit":
                    {
                        Shutdown = true;
                        break;
                    }
                    case "help":
                    {
                        Console.Log(Console.LogType.Info, "Available commands: exit help info quit");
                        break;
                    }
                    case "info":
                    {
                        Console.Log(Console.LogType.Info, "Battleship - Current Games:");
                        string[][] GamesIds = Database.Get("Battleship_Games", "GameId");
                        
                        if(GamesIds[0][0].Length > 0)
                        {
                            for (int i = 0; i < GamesIds.Length; i++)
                            {
                                Console.Log(Console.LogType.Info, Games.ById(long.Parse(GamesIds[i][0])).ToString().Replace("{ ", "").Replace(" }", "").Replace("\"", ""));
                            }
                        }
                        break;
                    }
                }
            }
            
            Console.Log(Console.LogType.Info, "Disconnecting from database...");
            Database.Disconnect();
            Console.Log(Console.LogType.Info, "Stopping application...");
            Task TaskStop = App.StopAsync();
            while(!TaskStart.IsCompleted || !TaskStop.IsCompleted)
            { Thread.Sleep(1); }
        }
    }
}