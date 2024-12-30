using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using UdemyAuthServer.API;

namespace Mini_Kurs_Satis_Sitesi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
