using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Battleship
{
    public static class Program
    {
        public static void Main(string[] Args)
        {
            WebApplicationBuilder AppBuilder = WebApplication.CreateBuilder(Args);
            AppBuilder.Services.AddControllersWithViews();
            WebApplication App = AppBuilder.Build();
            App.MapControllerRoute("default", "api/{controller}/{action=Index}/{id?}");
            App.MapFallbackToFile("index.html");
            App.Run();
        }
    }
}