using Microsoft.EntityFrameworkCore;
using web.Entities;
using web.Model;
using web.Models;

namespace web
{
    public class StoreDbContext : DbContext
    {
        public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<subGenre> subGenres { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<UserAccount> UserAccounts { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Order> Orders { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Specify the tables
            modelBuilder.Entity<Product>().ToTable("product_update");
            modelBuilder.Entity<Genre>().ToTable("Genre");
            modelBuilder.Entity<subGenre>().ToTable("subGenre");
            modelBuilder.Entity<Cart>().ToTable("Cart");
            modelBuilder.Entity<CartItem>().ToTable("CartItem");
            modelBuilder.Entity<Address>().ToTable("Addresses");
            modelBuilder.Entity<Order>().ToTable("Order");


            // Configure relationships
            modelBuilder.Entity<subGenre>()
                .HasKey(sg => new { sg.genreID, sg.subGenreID });

            modelBuilder.Entity<Product>()
                .HasOne(p => p.GenreNavigation)
                .WithMany(g => g.Products)
                .HasForeignKey(p => p.Genre);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.subGenreNavigation)
                .WithMany(sg => sg.Products)
                .HasForeignKey(p => new { p.Genre, p.subGenre });

            modelBuilder.Entity<Cart>()
                .HasMany(c => c.CartItems)
                .WithOne(ci => ci.Cart)
                .HasForeignKey(ci => ci.CartID);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithMany()
                .HasForeignKey(ci => ci.ProductID);



        }
    }
}
