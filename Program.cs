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
                string UserCommand = System.Console.ReadLine() ?? "exit";
                string[] Command = UserCommand.Split(' ');
                System.Console.CursorTop--;
                Console.Log(Console.LogType.Warning, "User issued server command: " + UserCommand);
                switch (Command[0].ToLower())
                {
                    case "exit": case "quit": case "stop":
                    {
                        Games.SyncAll();
                        Shutdown = true;
                        break;
                    }
                    case "help": case "cmd":
                    {
                        Console.Log(Console.LogType.Info, "Available commands: cmd exit help info quit stop");
                        break;
                    }
                    case "info":
                    {
                        if(Command.Length < 2) { Command = new[] { Command[0], " " }; }
                        
                        switch(Command[1].ToLower())
                        {
                            case "users":
                            {
                                Console.Log(Console.LogType.Info, "Battleship - Registered Users:");
                                string[][] UsersNames = Database.Get("Battleship_Users", "Username");
                                
                                string AllUsers = "";
                                for (int i = 0; i < UsersNames.Length; i++)
                                {
                                    if(AllUsers.Length > 50)
                                    {
                                        Console.Log(Console.LogType.Info, AllUsers);
                                        AllUsers = "";
                                    }
                                    AllUsers += UsersNames[i][0] + " ";
                                }
                                Console.Log(Console.LogType.Info, AllUsers);
                                break;
                            }
                            case "games":
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
                            default:
                            {
                                Console.Log(Console.LogType.Error, "Incomplete command! Usage: info [page]");
                                Console.Log(Console.LogType.Warning, "  info users  -  display all users");
                                Console.Log(Console.LogType.Warning, "  info games  -  display all games");
                                break;
                            }
                        }
                        break;
                    }
                    default:
                    {
                        Console.Log(Console.LogType.Error, "Unknown command! Use 'help' for help.");
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