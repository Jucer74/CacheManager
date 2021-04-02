using CacheWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CacheWebApi.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite(@"Data Source=./Database/CacheDatabase.db");

        public DbSet<Country> Countries { get; set; }
    }
}