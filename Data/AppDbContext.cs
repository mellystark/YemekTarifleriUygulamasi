using Microsoft.EntityFrameworkCore;
using YemekTarifleriUygulamasi.Models;

namespace YemekTarifleriUygulamasi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Recipe> Recipes { get; internal set; }
    }
}
