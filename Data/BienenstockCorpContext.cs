using System;
using System.Collections.Generic;
using BienenstockCorpAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BienenstockCorpAPI.Data;

public partial class BienenstockCorpContext : DbContext
{
    public BienenstockCorpContext()
    {
    }

    public BienenstockCorpContext(DbContextOptions<BienenstockCorpContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Dispatch> Dispatch { get; set; }

    public virtual DbSet<Inform> Inform { get; set; }

    public virtual DbSet<Note> Note { get; set; }

    public virtual DbSet<Product> Product { get; set; }

    public virtual DbSet<ProductPurchase> ProductPurchase { get; set; }

    public virtual DbSet<ProductSale> ProductSale { get; set; }
    
    public virtual DbSet<Purchase> Purchase { get; set; }
    
    public virtual DbSet<Sale> Sale { get; set; }

    public virtual DbSet<User> User { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Dispatch>(entity =>
        {
            entity.HasKey(e => e.DispatchId).HasName("Dispatch_PK");

            entity.ToTable("Dispatch");

            entity.Property(e => e.DispatchDate).HasColumnType("datetime");

            entity.HasOne(d => d.Product).WithMany(p => p.Dispatches)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Dispatch_Product_FK");
        });

        modelBuilder.Entity<Inform>(entity =>
        {
            entity.HasKey(e => e.InformId).HasName("Inform_PK");

            entity.ToTable("Inform");

            entity.Property(e => e.Description)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.IssueDate).HasColumnType("datetime");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");

            entity.HasOne(d => d.User).WithMany(p => p.Informs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Inform_User_FK");
        });

        modelBuilder.Entity<Note>(entity =>
        {
            entity.HasKey(e => e.NoteId).HasName("Note_PK");

            entity.ToTable("Note");

            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");

            entity.HasOne(d => d.User).WithMany(p => p.Notes)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("Note_User_FK");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("Product_PK");

            entity.ToTable("Product");

            entity.Property(e => e.EnterDate).HasColumnType("datetime");
            entity.Property(e => e.ExpirationDate).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ProductCode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<ProductPurchase>(entity =>
        {
            entity.HasKey(e => new { e.ProductId, e.PurchaseId }).HasName("ProductPurchase_PK");

            entity.ToTable("ProductPurchase");

            entity.Property(e => e.UnitPrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductPurchases)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ProductPurchase_Product_FK");

            entity.HasOne(d => d.Purchase).WithMany(p => p.ProductPurchases)
                .HasForeignKey(d => d.PurchaseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ProductPurchase_Purchase_FK");
        });

        modelBuilder.Entity<ProductSale>(entity =>
        {
            entity.HasKey(e => new { e.ProductId, e.SaleId }).HasName("ProductSale_PK");

            entity.ToTable("ProductSale");

            entity.Property(e => e.UnitPrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductSales)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ProductSale_Product_FK");

            entity.HasOne(d => d.Sale).WithMany(p => p.ProductSales)
                .HasForeignKey(d => d.SaleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ProductSale_Sale_FK");
        });

        modelBuilder.Entity<Purchase>(entity =>
        {
            entity.HasKey(e => e.PurchaseId).HasName("Purchase_PK");

            entity.ToTable("Purchase");

            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.User).WithMany(p => p.Purchases)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Purchase_User_FK");
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.SaleId).HasName("Sale_PK");

            entity.ToTable("Sale");

            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.User).WithMany(p => p.Sales)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Sale_User_FK");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CC4C65C9E7FF");

            entity.ToTable("User");

            entity.Property(e => e.Avatar).IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.PassHash)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.UserType)
                .HasMaxLength(20)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
