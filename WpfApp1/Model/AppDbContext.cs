using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WpfApp1.Model
{
    public class AppDbContext : DbContext
    {
        public DbSet<Car> Persons { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Строка подключения
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=cars;Username=postgres;Password=0104");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Car>().ToTable("cars");
        }
    }
}
