//using CardCostApi.Infrastructure.Entities;
//using Microsoft.EntityFrameworkCore;

//namespace CardCostApi.Infrastructure.Store
//{
//    public class CardCostContext : DbContext
//    {
//        public CardCostContext(DbContextOptions<CardCostContext> options)
//            : base(options)
//        {
//        }

//        //protected override void OnConfiguring(DbContextOptionsBuilder options)
//        //{
//        //    // connect to postgres with connection string from app settings
//        //    options.UseNpgsql("");
//        //}

//        public DbSet<CardCostEntity> CardCosts { get; set; }

//        //protected override void OnModelCreating(ModelBuilder builder)
//        //{
//        //    base.OnModelCreating(builder);
//        //}

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            modelBuilder.Entity<CardCostEntity>()
//                .HasKey(b => b.Id);

//            modelBuilder.Entity<CardCostEntity>().Property(b => b.Country).HasMaxLength(2);

//            modelBuilder.Entity<CardCostEntity>()
//                .Property(b => b.Cost).IsRequired();
//        }
//    }

//    public delegate TContext DbContextFactory<out TContext>() where TContext : DbContext;
//}