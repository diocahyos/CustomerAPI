using CustomerAPI.DataContext.Configurations;
using CustomerAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace CustomerAPI.DataContext
{
    public class AppDataContext : DbContext
    {
        protected readonly IConfiguration _configuration;
        public AppDataContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbSet<Customer> Customers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseNpgsql(_configuration.GetConnectionString("postgresdb"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CustomerConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
