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

    public virtual DbSet<Bill> Bill { get; set; }

    public virtual DbSet<Log> Log { get; set; }

    public virtual DbSet<Message> Message { get; set; }

    public virtual DbSet<Product> Product { get; set; }

    public virtual DbSet<ProductPurchase> ProductPurchase { get; set; }

    public virtual DbSet<ProductSale> ProductSale { get; set; }

    public virtual DbSet<Purchase> Purchase { get; set; }

    public virtual DbSet<Sale> Sale { get; set; }
    
    public virtual DbSet<Stock> Stock { get; set; }

    public virtual DbSet<User> User { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bill>(entity =>
        {
            entity.HasKey(e => e.BillId).HasName("Bill_PK");

            entity.ToTable("Bill");

            entity.Property(e => e.Address)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BillType)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.IssueDate).HasColumnType("datetime");
            entity.Property(e => e.Reason)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");

            entity.HasOne(d => d.Sale).WithMany(p => p.Bills)
                .HasForeignKey(d => d.SaleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Bill_Sale_FK");

            entity.HasOne(d => d.User).WithMany(p => p.Bills)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Bill_User_FK");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("Log_PK");

            entity.ToTable("Log");

            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Description).IsUnicode(false);

            entity.HasOne(d => d.User).WithMany(p => p.Logs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Log_User_FK");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.MessageId).HasName("Message_PK");

            entity.ToTable("Message");

            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Desciption)
                .HasMaxLength(240)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithMany(p => p.Messages)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Message_User_FK");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("Product_PK");

            entity.ToTable("Product");

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
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
            entity.Property(e => e.EnterDate).HasColumnType("datetime");
            entity.Property(e => e.Supplier)
                .HasMaxLength(100)
                .IsUnicode(false);
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
            entity.Property(e => e.DispatchDate).HasColumnType("datetime");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.User).WithMany(p => p.Sales)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Sale_User_FK");
        });

        modelBuilder.Entity<Stock>(entity =>
        {
            entity.HasKey(e => e.StockId).HasName("Stock_PK");

            entity.ToTable("Stock");

            entity.Property(e => e.ExpirationDate).HasColumnType("date");

            entity.HasOne(d => d.Product).WithMany(p => p.Stocks)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Stock_Product_FK");
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
