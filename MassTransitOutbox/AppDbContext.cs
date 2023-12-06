using System.Reflection;
using MassTransit;
using MassTransitOutbox.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MassTransitOutbox
{
    public class AppDbContext : DbContext
    {
        public DbSet<BankAccount>  Accounts { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            builder.AddInboxStateEntity();
            builder.AddOutboxMessageEntity();
            builder.AddOutboxStateEntity();
        }
    }

    public class AccountEntityTypeConfiguration : IEntityTypeConfiguration<BankAccount>
    {
        public void Configure(EntityTypeBuilder<BankAccount> builder)
        {
            builder.ToTable("Accounts");
            builder.HasIndex(x => x.Name).IsUnique();
        }
    }

    public class TransactionEntityTypeConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("Transactions");

            builder
                .HasOne(x => x.Account)
                .WithMany()
                .HasForeignKey(x => x.AccountId);
        }
    }
}
