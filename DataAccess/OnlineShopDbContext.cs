using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace OnlineShopDataUploader.Models;

public partial class OnlineShopDbContext : DbContext
{
    public OnlineShopDbContext()
    {
    }

    public OnlineShopDbContext(DbContextOptions<OnlineShopDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Purchase> Purchases { get; set; }

    public virtual DbSet<PurchaseProduct> PurchaseProducts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=OnlineShop_DB;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK_Customer_ID");

            entity.ToTable("Customer");

            entity.HasIndex(e => e.Email, "UQ_Customer_Email").IsUnique();

            entity.Property(e => e.CustomerId).HasColumnName("Customer_ID");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Fullname).HasMaxLength(80);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK_Product_ID");

            entity.ToTable("Product");

            entity.Property(e => e.ProductId).HasColumnName("Product_ID");
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<Purchase>(entity =>
        {
            entity.HasKey(e => e.PurchaseId).HasName("PK_Purchase_ID");

            entity.ToTable("Purchase");

            entity.Property(e => e.PurchaseId)
                .ValueGeneratedNever()
                .HasColumnName("Purchase_ID");
            entity.Property(e => e.CustomerId).HasColumnName("Customer_ID");
            entity.Property(e => e.RegistrationDate).HasColumnName("Registration_Date");
            entity.Property(e => e.TotalSum)
                .HasColumnType("money")
                .HasColumnName("Total_Sum");

            entity.HasOne(d => d.Customer).WithMany(p => p.Purchases)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_Customer_ID");
        });

        modelBuilder.Entity<PurchaseProduct>(entity =>
        {
            entity.HasKey(e => new { e.PurchaseId, e.ProductId }).HasName("PK_Purchase_Product_ID");

            entity.ToTable("Purchase_Product");

            entity.Property(e => e.PurchaseId).HasColumnName("Purchase_ID");
            entity.Property(e => e.ProductId).HasColumnName("Product_ID");

            entity.HasOne(d => d.Product).WithMany(p => p.PurchaseProducts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_ID");

            entity.HasOne(d => d.Purchase).WithMany(p => p.PurchaseProducts)
                .HasForeignKey(d => d.PurchaseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Purchase_ID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
