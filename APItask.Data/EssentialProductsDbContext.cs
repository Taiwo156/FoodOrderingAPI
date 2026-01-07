using APItask.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace APItask.Data
{
    public class EssentialProductsDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public EssentialProductsDbContext(DbContextOptions<EssentialProductsDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        // Existing DbSets - NO CHANGES HERE
        public DbSet<Product> Product { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<ProductImage> ProductImage { get; set; }
        public DbSet<ProductOwner> ProductOwner { get; set; }
        public DbSet<Wishlistitem> Wishlistitem { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<ProductByStore> ProductByStore { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<PaymentAttempt> PaymentAttempts { get; set; }

        public DbSet<Payment> Payments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DbContext"));
            }
        }

 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<ProductByStore>()
                .HasKey(p => new { p.ProductId, p.StoreId, });

            modelBuilder.Entity<ProductByStore>()
                .HasOne(pbs => pbs.Product)
                .WithMany()
                .HasForeignKey(pbs => pbs.ProductId);

            modelBuilder.Entity<ProductByStore>()
                .HasOne(pbs => pbs.Store)
                .WithMany()
                .HasForeignKey(pbs => pbs.StoreId);
           
            modelBuilder.Entity<Favorite>().ToTable("Favorite");

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderId);
                entity.Property(e => e.TaxAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.SubTotal).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Total).HasColumnType("decimal(18,2)");

                // Relationships
                entity.HasMany(e => e.OrderItems)
                      .WithOne(e => e.Order)
                      .HasForeignKey(e => e.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure OrderItem entity
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(e => e.OrderItemId);

                entity.Property(e => e.OrderItemId)
                      .ValueGeneratedOnAdd() 
                      .UseIdentityColumn();

                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18,2)");

                entity.HasOne(e => e.Order)
                      .WithMany(e => e.OrderItems)
                      .HasForeignKey(e => e.OrderId);

                entity.HasOne(e => e.Product)
                      .WithMany()
                      .HasForeignKey(e => e.ProductId);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Price).HasPrecision(18, 2);
            });

            modelBuilder.Entity<Product>(p =>
            {
                p.Property(x => x.Price).HasPrecision(18, 2);
            });

            modelBuilder.Entity<Payment>(entity => {
                entity.HasKey(e => e.PaymentId);
                entity.HasIndex(e => e.Reference).IsUnique();
                entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.HasIndex(e => e.Email);
                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => e.Provider);
                entity.HasIndex(e => e.CreatedAt);
            });
            modelBuilder.Entity<Payment>() 
                .HasOne<Order>()
                .WithMany()
                .HasForeignKey(p => p.OrderId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<PaymentAttempt>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Reference).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Currency).HasMaxLength(3).HasDefaultValue("NGN");
                entity.Property(e => e.Method).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Email).HasMaxLength(255);
                entity.Property(e => e.Phone).HasMaxLength(20);
                entity.Property(e => e.Status).IsRequired().HasMaxLength(20).HasDefaultValue("pending");
                entity.Property(e => e.CreatedAt).IsRequired().HasDefaultValueSql("GETUTCDATE()");
                entity.HasIndex(e => e.Reference).IsUnique();
                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => e.CreatedAt);
            });

            modelBuilder.Entity<BankTransferDetails>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.PaymentAttemptId).IsRequired();
                entity.Property(e => e.BankName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.AccountNumber).IsRequired().HasMaxLength(20);
                entity.Property(e => e.AccountName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Currency).HasMaxLength(3).HasDefaultValue("NGN");
                entity.Property(e => e.CreatedAt).IsRequired().HasDefaultValueSql("GETUTCDATE()");

                // Foreign key relationship
                entity.HasOne(b => b.PaymentAttempt)
                    .WithMany()
                    .HasForeignKey(b => b.PaymentAttemptId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => e.PaymentAttemptId);
            });



        }
    }
}