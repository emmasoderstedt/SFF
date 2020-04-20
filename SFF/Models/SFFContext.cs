using System;
using Microsoft.EntityFrameworkCore;
using SFF.Models;

namespace SFF.Models
{
    public class SFFContext : DbContext
    {
        public SFFContext(DbContextOptions<SFFContext> options) : base(options)
        { }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Filmclub> Filmclubs { get; set; }
        public DbSet<Rental> Rentals { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=minDatabas.db");
        }

        //tvingar att vi man måste sätta ett grade för att lägga till en review
        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
           
        }

    }

}