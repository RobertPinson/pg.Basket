using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using pg.Basket.Dal.Entity;

namespace pg.Basket.Dal
{
    public partial class BasketDbContext : DbContext, IBasketDbContext
    {
        public BasketDbContext(DbContextOptions<BasketDbContext> options) : base(options)
        {

        }

        public const string DEFAULT_SCHEMA = "basket";

        public DbSet<Entity.Basket> Baskets { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }
        public DbSet<BaseVoucher> Vouchers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<GiftVoucher>();
            modelBuilder.Entity<OfferVoucher>();

            modelBuilder.ApplyConfiguration(new BasketEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new BasketItemEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new VoucherEntityTypeConfiguration());
        }
    }

    public class VoucherEntityTypeConfiguration : IEntityTypeConfiguration<BaseVoucher>
    {
        public void Configure(EntityTypeBuilder<BaseVoucher> voucherConfiguration)
        {
            voucherConfiguration.ToTable("vouchers", BasketDbContext.DEFAULT_SCHEMA);

            voucherConfiguration.HasKey(v => v.Id);
        }
    }

    public class BasketItemEntityTypeConfiguration : IEntityTypeConfiguration<BasketItem>
    {
        public void Configure(EntityTypeBuilder<BasketItem> basketItemConfiguration)
        {
            basketItemConfiguration.ToTable("basketItems", BasketDbContext.DEFAULT_SCHEMA);

            basketItemConfiguration.HasKey(b => b.Id);

            basketItemConfiguration.Property(o => o.Id)
                .ForSqlServerUseSequenceHiLo("orderitemseq");

            basketItemConfiguration.Property<int>("BasketId")
                .IsRequired();

            basketItemConfiguration.Property<int>("ProductId")
                .IsRequired();

            basketItemConfiguration.Property<string>("Description")
                .IsRequired();

            basketItemConfiguration.Property<string>("CategoryId")
                .IsRequired();

            basketItemConfiguration.Property<decimal>("UnitPrice")
                .IsRequired();

            basketItemConfiguration.Property<int>("Quantity")
                .IsRequired();
        }
    }

    public class BasketEntityTypeConfiguration : IEntityTypeConfiguration<Entity.Basket>
    {
        public void Configure(EntityTypeBuilder<Entity.Basket> basketConfiguration)
        {
            basketConfiguration.ToTable("baskets", BasketDbContext.DEFAULT_SCHEMA);

            basketConfiguration.HasKey(b => b.Id);

            basketConfiguration.Ignore(b => b.VoucherNotAppliedMessage);

            basketConfiguration.Property(o => o.Id)
                .ForSqlServerUseSequenceHiLo("orderseq", BasketDbContext.DEFAULT_SCHEMA);

            var navigation = basketConfiguration.Metadata.FindNavigation(nameof(Entity.Basket.BasketItems));
            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            var voucherNavigation = basketConfiguration.Metadata.FindNavigation(nameof(Entity.Basket.Vouchers));
            voucherNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
            
            basketConfiguration.HasMany(b => b.Vouchers)
                .WithOne("Basket")
                .HasForeignKey("BasketId");
        }
    }
}
