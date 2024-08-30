using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace FoodOrderApi.Models;

public partial class YwnacrjeAfoodContext : DbContext
{
    public YwnacrjeAfoodContext()
    {
    }

    public YwnacrjeAfoodContext(DbContextOptions<YwnacrjeAfoodContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Cartitem> Cartitems { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Food> Foods { get; set; }

    public virtual DbSet<Foodorder> Foodorders { get; set; }

    public virtual DbSet<Imageinfo> Imageinfos { get; set; }

    public virtual DbSet<Orderdetail> Orderdetails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("latin1_swedish_ci")
            .HasCharSet("latin1");

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PRIMARY");

            entity.ToTable("cart");

            entity.HasIndex(e => e.CustomerId, "customerID");

            entity.Property(e => e.CartId)
                .HasColumnType("int(11)")
                .HasColumnName("cartID");
            entity.Property(e => e.CustomerId)
                .HasColumnType("int(11)")
                .HasColumnName("customerID");

            entity.HasOne(d => d.Customer).WithMany(p => p.Carts)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("cart_ibfk_1");
        });

        modelBuilder.Entity<Cartitem>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PRIMARY");

            entity.ToTable("cartitem");

            entity.HasIndex(e => e.CartId, "cartID");

            entity.HasIndex(e => e.FoodId, "foodID");

            entity.Property(e => e.ItemId)
                .HasColumnType("int(11)")
                .HasColumnName("itemID");
            entity.Property(e => e.CartId)
                .HasColumnType("int(11)")
                .HasColumnName("cartID");
            entity.Property(e => e.FoodId)
                .HasColumnType("int(11)")
                .HasColumnName("foodID");
            entity.Property(e => e.Quantity)
                .HasColumnType("int(11)")
                .HasColumnName("quantity");

            entity.HasOne(d => d.Cart).WithMany(p => p.Cartitems)
                .HasForeignKey(d => d.CartId)
                .HasConstraintName("cartitem_ibfk_1");

            entity.HasOne(d => d.Food).WithMany(p => p.Cartitems)
                .HasForeignKey(d => d.FoodId)
                .HasConstraintName("cartitem_ibfk_2");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PRIMARY");

            entity
                .ToTable("category")
                .HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_unicode_ci");

            entity.Property(e => e.CategoryId)
                .HasColumnType("int(11)")
                .HasColumnName("categoryID");
            entity.Property(e => e.CategoryImgUrl)
                .HasMaxLength(200)
                .HasColumnName("categoryImgURL");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(30)
                .HasColumnName("categoryName");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PRIMARY");

            entity
                .ToTable("customer")
                .HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.Username, "username").IsUnique();

            entity.Property(e => e.CustomerId)
                .HasColumnType("int(11)")
                .HasColumnName("customerID");
            entity.Property(e => e.Address)
                .HasMaxLength(200)
                .HasColumnName("address");
            entity.Property(e => e.BirthDate).HasColumnName("birthDate");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(50)
                .HasColumnName("fullName");
            entity.Property(e => e.Gender)
                .HasColumnType("bit(1)")
                .HasColumnName("gender");
            entity.Property(e => e.Password)
                .HasMaxLength(500)
                .HasColumnName("password");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .HasColumnName("phoneNumber");
            entity.Property(e => e.Role)
                .HasMaxLength(10)
                .HasColumnName("role");
            entity.Property(e => e.Username)
                .HasMaxLength(30)
                .HasColumnName("username");
        });

        modelBuilder.Entity<Food>(entity =>
        {
            entity.HasKey(e => e.FoodId).HasName("PRIMARY");

            entity
                .ToTable("food")
                .HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.CategoryId, "categoryID");

            entity.Property(e => e.FoodId)
                .HasColumnType("int(11)")
                .HasColumnName("foodID");
            entity.Property(e => e.CategoryId)
                .HasColumnType("int(11)")
                .HasColumnName("categoryID");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .HasColumnName("description");
            entity.Property(e => e.FoodName)
                .HasMaxLength(50)
                .HasColumnName("foodName");
            entity.Property(e => e.ImgUrl)
                .HasMaxLength(200)
                .HasColumnName("imgURL");
            entity.Property(e => e.Price).HasColumnName("price");

            entity.HasOne(d => d.Category).WithMany(p => p.Foods)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("food_ibfk_1");
        });

        modelBuilder.Entity<Foodorder>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PRIMARY");

            entity
                .ToTable("foodorder")
                .HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.CustomerId, "customerID");

            entity.Property(e => e.OrderId)
                .HasColumnType("int(11)")
                .HasColumnName("orderID");
            entity.Property(e => e.CustomerId)
                .HasColumnType("int(11)")
                .HasColumnName("customerID");
            entity.Property(e => e.OrderAddress)
                .HasMaxLength(200)
                .HasColumnName("orderAddress");
            entity.Property(e => e.OrderEmail)
                .HasMaxLength(100)
                .HasColumnName("orderEmail");
            entity.Property(e => e.OrderName)
                .HasMaxLength(50)
                .HasColumnName("orderName");
            entity.Property(e => e.OrderPhoneNumber)
                .HasMaxLength(20)
                .HasColumnName("orderPhoneNumber");
            entity.Property(e => e.OrderTime)
                .HasColumnType("datetime")
                .HasColumnName("orderTime");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.TotalPrice).HasColumnName("totalPrice");

            entity.HasOne(d => d.Customer).WithMany(p => p.Foodorders)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("foodorder_ibfk_1");
        });

        modelBuilder.Entity<Imageinfo>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PRIMARY");

            entity.ToTable("imageinfo");

            entity.Property(e => e.ImageId)
                .HasColumnType("int(11)")
                .HasColumnName("imageID");
            entity.Property(e => e.UploadTime)
                .HasColumnType("datetime")
                .HasColumnName("uploadTime");
            entity.Property(e => e.Url)
                .HasMaxLength(200)
                .HasColumnName("url");
        });

        modelBuilder.Entity<Orderdetail>(entity =>
        {
            entity.HasKey(e => e.DetailId).HasName("PRIMARY");

            entity
                .ToTable("orderdetail")
                .HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.FoodId, "foodID");

            entity.HasIndex(e => e.OrderId, "orderID");

            entity.Property(e => e.DetailId)
                .HasColumnType("int(11)")
                .HasColumnName("detailID");
            entity.Property(e => e.FoodId)
                .HasColumnType("int(11)")
                .HasColumnName("foodID");
            entity.Property(e => e.OrderId)
                .HasColumnType("int(11)")
                .HasColumnName("orderID");
            entity.Property(e => e.Quantity)
                .HasColumnType("int(11)")
                .HasColumnName("quantity");

            entity.HasOne(d => d.Food).WithMany(p => p.Orderdetails)
                .HasForeignKey(d => d.FoodId)
                .HasConstraintName("orderdetail_ibfk_2");

            entity.HasOne(d => d.Order).WithMany(p => p.Orderdetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("orderdetail_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
