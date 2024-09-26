using ASPtask.Core;
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

        public DbSet<Product> Product { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<ProductImage> ProductImage { get; set; }
        public DbSet<ProductOwner> ProductOwner { get; set; }
        public DbSet<Wishlistitem> Wishlistitem { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<ProductByStore> ProductByStore { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DbContext"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Defining composite key for ProductByStore
            modelBuilder.Entity<ProductByStore>()
                .HasKey(p => new { p.ProductId, p.StoreId });

            // Define foreign key relationship with Product
            modelBuilder.Entity<ProductByStore>()
                .HasOne(pbs => pbs.Product)
                .WithMany()
                .HasForeignKey(pbs => pbs.ProductId);

            // Define foreign key relationship with Store
            modelBuilder.Entity<ProductByStore>()
                .HasOne(pbs => pbs.Store)
                .WithMany()
                .HasForeignKey(pbs => pbs.StoreId);

            // Existing mappings
            modelBuilder.Entity<Favorite>().ToTable("Favorite");
        }
    }
}
