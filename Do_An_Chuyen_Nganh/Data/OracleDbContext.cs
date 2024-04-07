//using Do_An_Chuyen_Nganh.Models;
//using Microsoft.EntityFrameworkCore;

//namespace Do_An_Chuyen_Nganh.Data
//{
//    public class OracleDbContext : DbContext
//    {
//        public OracleDbContext(DbContextOptions<OracleDbContext> options) : base(options)
//        {
//        }

//        public DbSet<Category> Categories { get; set; }
//        public DbSet<Product> Products { get; set; }
//        public DbSet<Color> Colors { get; set; }
//        public DbSet<Condition> Conditions { get; set; }
//        public DbSet<Provenience> Proveniences { get; set; }
//        public DbSet<Warranty> Warranties { get; set; }
//        public DbSet<OrderDetail> OrderDetails { get; set; }
//        public DbSet<Order> Orders { get; set; }
//        public DbSet<WishList> WishList { get; set; }
//        public DbSet<Role> Roles { get; set; }
//        public DbSet<ProductImage> ProductImages { get; set; }
//        public DbSet<Message> Messages { get; set; }



//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            base.OnModelCreating(modelBuilder);

//            modelBuilder.HasDefaultSchema("C##MBTG"); // Set the default schema

//            modelBuilder.Entity<Category>().ToTable("Category");
//            modelBuilder.Entity<Product>().ToTable("Product");
//            modelBuilder.Entity<Color>().ToTable("Color");
//            modelBuilder.Entity<Condition>().ToTable("Condition");
//            modelBuilder.Entity<Provenience>().ToTable("Provenience");
//            modelBuilder.Entity<Warranty>().ToTable("Warranty");
//            modelBuilder.Entity<OrderDetail>().ToTable("OrderDetail");
//            modelBuilder.Entity<Order>().ToTable("Order");
//            modelBuilder.Entity<Role>().ToTable("Role");
//            modelBuilder.Entity<ProductImage>().ToTable("ProductImage");
//            modelBuilder.Entity<Message>().ToTable("Message");
//        }
//    }
//}
