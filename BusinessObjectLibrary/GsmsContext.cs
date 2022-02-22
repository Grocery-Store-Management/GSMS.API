using System;
using GsmsLibrary;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace BusinessObjectLibrary
{
    public partial class GsmsContext : DbContext
    {
        public GsmsContext()
        {
        }

        public GsmsContext(DbContextOptions<GsmsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<ImportOrder> ImportOrders { get; set; }
        public virtual DbSet<ImportOrderDetail> ImportOrderDetails { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductDetail> ProductDetails { get; set; }
        public virtual DbSet<Receipt> Receipts { get; set; }
        public virtual DbSet<ReceiptDetail> ReceiptDetails { get; set; }
        public virtual DbSet<Store> Stores { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(GsmsConfiguration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Brand>(entity =>
            {
                entity.ToTable("Brand");

                entity.Property(e => e.Id).HasMaxLength(40);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.Id).HasMaxLength(40);

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<ImportOrder>(entity =>
            {
                entity.ToTable("ImportOrder");

                entity.Property(e => e.Id).HasMaxLength(40);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.ProductId).HasMaxLength(40);

                entity.Property(e => e.StoreId).HasMaxLength(40);

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.ImportOrders)
                    .HasForeignKey(d => d.StoreId)
                    .HasConstraintName("FK_ImportOrder_Store");
            });

            modelBuilder.Entity<ImportOrderDetail>(entity =>
            {
                entity.ToTable("ImportOrderDetail");

                entity.Property(e => e.Id).HasMaxLength(40);

                entity.Property(e => e.Distributor).HasMaxLength(50);

                entity.Property(e => e.ImportOrderId).HasMaxLength(40);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Price).HasColumnType("money");

                entity.Property(e => e.ProductId).HasMaxLength(40);

                entity.HasOne(d => d.ImportOrder)
                    .WithMany(p => p.ImportOrderDetails)
                    .HasForeignKey(d => d.ImportOrderId)
                    .HasConstraintName("FK_ImportOrderDetails_ImportOrders");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ImportOrderDetails)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_ImportOrderDetail_Product");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.Id).HasMaxLength(40);

                entity.Property(e => e.AtomicPrice).HasColumnType("money");

                entity.Property(e => e.CategoryId).HasMaxLength(40);

                entity.Property(e => e.MasterProductId).HasMaxLength(40);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Products_Categories");

                entity.HasOne(d => d.MasterProduct)
                    .WithMany(p => p.InverseMasterProduct)
                    .HasForeignKey(d => d.MasterProductId)
                    .HasConstraintName("FK_MasterProduct_Product");
            });

            modelBuilder.Entity<ProductDetail>(entity =>
            {
                entity.ToTable("ProductDetail");

                entity.Property(e => e.Id).HasMaxLength(40);

                entity.Property(e => e.ExpiringDate).HasColumnType("datetime");

                entity.Property(e => e.ManufacturingDate).HasColumnType("datetime");

                entity.Property(e => e.Price).HasColumnType("money");

                entity.Property(e => e.ProductId).HasMaxLength(40);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductDetails)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_ProductDetails_Products");
            });

            modelBuilder.Entity<Receipt>(entity =>
            {
                entity.ToTable("Receipt");

                entity.Property(e => e.Id).HasMaxLength(40);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CustomerId).HasMaxLength(40);

                entity.Property(e => e.EmployeeId).HasMaxLength(40);

                entity.Property(e => e.StoreId).HasMaxLength(40);

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Receipts)
                    .HasForeignKey(d => d.StoreId)
                    .HasConstraintName("FK_Receipts_Stores");
            });

            modelBuilder.Entity<ReceiptDetail>(entity =>
            {
                entity.ToTable("ReceiptDetail");

                entity.Property(e => e.Id).HasMaxLength(40);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Price).HasColumnType("money");

                entity.Property(e => e.ProductId).HasMaxLength(40);

                entity.Property(e => e.ReceiptId).HasMaxLength(40);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ReceiptDetails)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_ReceiptDetail_Product");

                entity.HasOne(d => d.Receipt)
                    .WithMany(p => p.ReceiptDetails)
                    .HasForeignKey(d => d.ReceiptId)
                    .HasConstraintName("FK_ReceiptDetails_Receipts");
            });

            modelBuilder.Entity<Store>(entity =>
            {
                entity.ToTable("Store");

                entity.Property(e => e.Id).HasMaxLength(40);

                entity.Property(e => e.BrandId).HasMaxLength(40);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Stores)
                    .HasForeignKey(d => d.BrandId)
                    .HasConstraintName("FK_Stores_Brands");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
