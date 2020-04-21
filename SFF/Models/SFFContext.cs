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
        public DbSet<Review> Reviews { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=minDatabas.db");
        }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
           
        }

    }

}