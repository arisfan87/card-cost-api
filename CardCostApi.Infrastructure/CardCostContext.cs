using CardCostApi.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace CardCostApi.Infrastructure
{
    public class CardCostContext : DbContext
    {
        public CardCostContext(DbContextOptions<CardCostContext> options)
            : base(options)
        {
        }

        public DbSet<CardCostEntity> CardCosts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CardCostEntity>()
                .HasKey(b => b.Country);

            modelBuilder.Entity<CardCostEntity>().Property(b => b.Cost).HasMaxLength(2);

            modelBuilder.Entity<CardCostEntity>()
                .Property(b => b.Cost).IsRequired();
        }
    }

    public delegate TContext DbContextFactory<out TContext>() where TContext : DbContext;
}