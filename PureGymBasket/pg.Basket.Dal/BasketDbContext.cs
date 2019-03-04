using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using pg.Basket.Model;

namespace pg.Basket.Dal
{
    public class BasketDbContext : DbContext, IBasketDbContext
    {
        public BasketDbContext(DbContextOptions<BasketDbContext> options) : base(options)
        {

        }

        public const string DefaultSchema = "basket";

        public DbSet<Model.Basket> Baskets { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }
        public DbSet<BaseVoucher> Vouchers { get; set; }
        public DbSet<GiftVoucher> GiftVouchers { get; set; }
        public DbSet<OfferVoucher> OfferVouchers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new BasketEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new BasketItemEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new VoucherEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new GiftVoucherEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OfferVoucherEntityTypeConfiguration());
        }
    }

    public class GiftVoucherEntityTypeConfiguration : IEntityTypeConfiguration<GiftVoucher>
    {
        public void Configure(EntityTypeBuilder<GiftVoucher> giftVoucherConfiguration)
        {
            giftVoucherConfiguration.ToTable("giftvouchers", BasketDbContext.DefaultSchema);
        }
    }

    public class OfferVoucherEntityTypeConfiguration : IEntityTypeConfiguration<OfferVoucher>
    {
        public void Configure(EntityTypeBuilder<OfferVoucher> offerVoucherConfiguration)
        {
            offerVoucherConfiguration.ToTable("offervouchers", BasketDbContext.DefaultSchema);
        }
    }

    public class VoucherEntityTypeConfiguration : IEntityTypeConfiguration<BaseVoucher>
    {
        public void Configure(EntityTypeBuilder<BaseVoucher> voucherConfiguration)
        {
            voucherConfiguration.ToTable("vouchers", BasketDbContext.DefaultSchema);

            voucherConfiguration.HasKey(v => v.Id);
        }
    }

    public class BasketItemEntityTypeConfiguration : IEntityTypeConfiguration<BasketItem>
    {
        public void Configure(EntityTypeBuilder<BasketItem> basketItemConfiguration)
        {
            basketItemConfiguration.ToTable("basketItems", BasketDbContext.DefaultSchema);

            basketItemConfiguration.HasKey(b => b.Id);

            basketItemConfiguration.Property(o => o.Id)
                .ForSqlServerUseSequenceHiLo("basketitemseq");

            basketItemConfiguration.Property<int>("BasketId")
                .IsRequired();

            basketItemConfiguration.Property<int>("ProductId")
                .IsRequired();

            basketItemConfiguration.Property<string>("Description")
                .HasField("_description")
                .IsRequired();

            basketItemConfiguration.Property<int>("CategoryId")
                .HasField("_categoryId")
                .IsRequired();

            basketItemConfiguration.Property<decimal>("UnitPrice")
                .HasField("_unitPrice")
                .IsRequired();

            basketItemConfiguration.Property<int>("Quantity")
                .HasField("_quantity")
                .IsRequired();
        }
    }

    public class BasketEntityTypeConfiguration : IEntityTypeConfiguration<Model.Basket>
    {
        public void Configure(EntityTypeBuilder<Model.Basket> basketConfiguration)
        {
            basketConfiguration.ToTable("baskets", BasketDbContext.DefaultSchema);

            basketConfiguration.HasKey(b => b.Id);

            basketConfiguration.Ignore(b => b.VoucherNotAppliedMessage);

            basketConfiguration.Property(o => o.Id)
                .ForSqlServerUseSequenceHiLo("orderseq", BasketDbContext.DefaultSchema);

            var navigation = basketConfiguration.Metadata.FindNavigation(nameof(Model.Basket.BasketItems));
            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            var voucherNavigation = basketConfiguration.Metadata.FindNavigation(nameof(Model.Basket.Vouchers));
            voucherNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
            
            basketConfiguration.HasMany(b => b.Vouchers)
                .WithOne("Basket")
                .HasForeignKey("BasketId");
        }
    }
}
