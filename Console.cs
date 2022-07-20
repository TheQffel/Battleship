using System;

namespace Battleship
{
    public static class Console
    {
        public enum LogType { Info, Ok, Warning, Error }
        
        public static void Log(LogType Type, string Message)
        {
            if(Type == LogType.Info) { System.Console.ForegroundColor = ConsoleColor.DarkCyan; }
            if(Type == LogType.Ok) { System.Console.ForegroundColor = ConsoleColor.DarkGreen; }
            if(Type == LogType.Warning) { System.Console.ForegroundColor = ConsoleColor.DarkYellow; }
            if(Type == LogType.Error) { System.Console.ForegroundColor = ConsoleColor.DarkRed; }
            System.Console.WriteLine(DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss]") + ": " + Message);
            System.Console.ForegroundColor = ConsoleColor.White;
        }
    }
}