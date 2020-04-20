using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using SFF.Models;

namespace SFF
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //using (var db = new SFFContext())
            //{
            //    var movie = new Movie { IsDigital = true, MaximumRentals = 2, Title = "Låt den rätte komma in" };
            //    db.Add<Movie>(movie);
            //    db.SaveChanges();
            //    Console.WriteLine(movie.Title + " in med max rental:  " + movie.MaximumRentals + "har nu lagts till");
            //}


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
