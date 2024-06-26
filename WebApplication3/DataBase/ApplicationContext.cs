using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebApplication3.DataBase
{
    public class ApplicationContext  : DbContext, IDataProtectionKeyContext
    {
        public DbSet<ChatKey> ChatKeys { get; set; }

        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ChatKey>().HasKey(k => k.Id);
        }
    }
}
