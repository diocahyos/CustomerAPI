using CustomerAPI.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerAPI.DataContext.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            // Table Name
            builder.ToTable("Customers");

            // Set PrimaryKey
            builder.HasKey(x => x.Id);

            // Set Index
            builder.HasIndex(x => x.Email).IsUnique();
        }
    }
}
