using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace App.Models
{
    public partial class DataContext : DbContext
    {
        public virtual DbSet<Brand> Brand { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<OrderDetail> OrderDetail { get; set; }
        public virtual DbSet<OrderHeader> OrderHeader { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<UserAccount> UserAccount { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }

        public DataContext()
        {
        }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Brand>(entity =>
            {
                entity.ToTable("Brand");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(50).IsUnicode(false);
            });
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(50).IsUnicode(false);
            });
            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.ToTable("OrderDetail");
                entity.HasKey(e => new { e.OrderId, e.No } );
                entity.Property(e => e.OrderId).HasColumnName("order_id");
                entity.Property(e => e.No).HasColumnName("no");
                entity.Property(e => e.ProductId).HasColumnName("product_id");
                entity.Property(e => e.Qty).HasColumnName("qty");
            });
            modelBuilder.Entity<OrderHeader>(entity =>
            {
                entity.ToTable("OrderHeader");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.CustomerId).HasColumnName("customer_id");
                entity.Property(e => e.OrderDate).HasColumnName("order_date").HasColumnType("date");
            });
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.Price).HasColumnName("price").HasColumnType("decimal(10,2)");
                entity.Property(e => e.BrandId).HasColumnName("brand_id");
                entity.Property(e => e.Image).HasColumnName("image").HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.CreateUser).HasColumnName("create_user");
                entity.Property(e => e.CreateDate).HasColumnName("create_date").HasColumnType("datetime");
            });
            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(50).IsUnicode(false);
            });
            modelBuilder.Entity<UserAccount>(entity =>
            {
                entity.ToTable("UserAccount");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.Email).HasColumnName("email").HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.Password).HasColumnName("password").HasMaxLength(100).IsUnicode(false);
                entity.Property(e => e.PasswordResetToken).HasColumnName("password_reset_token").HasMaxLength(100).IsUnicode(false);
                entity.Property(e => e.Active).HasColumnName("active").HasConversion(new BoolToZeroOneConverter<Int16>());
            });
            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("UserRole");
                entity.HasKey(e => new { e.UserId, e.RoleId } );
                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.RoleId).HasColumnName("role_id");
            });
        }
    }
}