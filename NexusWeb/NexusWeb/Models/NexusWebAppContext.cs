using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace NexusWeb.Models;

public partial class NexusWebAppContext : DbContext
{
    public NexusWebAppContext()
    {
    }

    public NexusWebAppContext(DbContextOptions<NexusWebAppContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Connection> Connections { get; set; }

    public virtual DbSet<ConnectionType> ConnectionTypes { get; set; }

    public virtual DbSet<Distributor> Distributors { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductImage> ProductImages { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=ADMIN\\SQLEXPRESS;Initial Catalog=NexusWebApp;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Booking__3213E83FE9F41294");

            entity.ToTable("Booking");

            entity.HasIndex(e => e.Name, "UQ__Booking__737584F6C67520A2").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address).HasMaxLength(256);
            entity.Property(e => e.ConnectionId).HasColumnName("Connection_id");
            entity.Property(e => e.Message).HasMaxLength(256);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(15);
            entity.Property(e => e.UserId).HasColumnName("User_id");

            entity.HasOne(d => d.Connection).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.ConnectionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Booking__Connect__6E01572D");

            entity.HasOne(d => d.User).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Booking__User_id__6D0D32F4");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3213E83F6854469D");

            entity.HasIndex(e => e.Name, "UQ__Categori__737584F695834B27").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Connection>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Connecti__3213E83FC029698F");

            entity.HasIndex(e => e.Name, "UQ__Connecti__737584F648F8B6B4").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ConnectionTypeId).HasColumnName("ConnectionType_id");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Price).HasColumnType("money");

            entity.HasOne(d => d.ConnectionType).WithMany(p => p.Connections)
                .HasForeignKey(d => d.ConnectionTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Connectio__Conne__693CA210");
        });

        modelBuilder.Entity<ConnectionType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Connecti__3213E83FD4FB3681");

            entity.ToTable("ConnectionType");

            entity.HasIndex(e => e.Name, "UQ__Connecti__737584F66E67DB24").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Deposit).HasColumnType("money");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Distributor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Distribu__3213E83F2C8C70C2");

            entity.HasIndex(e => e.Name, "UQ__Distribu__737584F6FF9A586E").IsUnique();

            entity.HasIndex(e => e.Address, "UQ__Distribu__7D0C3F325F0581DC").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(15);
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Invoice__3213E83F171566C5");

            entity.ToTable("Invoice");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateAt).HasColumnName("Create_at");
            entity.Property(e => e.OrderId).HasColumnName("Order_id");
            entity.Property(e => e.TotalInvoice)
                .HasColumnType("money")
                .HasColumnName("Total_Invoice");
            entity.Property(e => e.UserId).HasColumnName("User_id");

            entity.HasOne(d => d.Order).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Invoice__Order_i__71D1E811");

            entity.HasOne(d => d.User).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Invoice__User_id__70DDC3D8");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Orders__3213E83F0F6004C0");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateAt).HasColumnName("Create_at");
            entity.Property(e => e.Total).HasColumnType("money");
            entity.Property(e => e.UserId).HasColumnName("User_id");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Orders__User_id__5AEE82B9");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderIte__3213E83F752C4636");

            entity.ToTable("OrderItem");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.OrderId).HasColumnName("Order_id");
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.ProductId).HasColumnName("Product_id");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderItem__Order__5EBF139D");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderItem__Produ__5DCAEF64");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Products__3213E83F10BD2D19");

            entity.HasIndex(e => e.Name, "UQ__Products__737584F6349AB363").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AvatarImages).HasMaxLength(256);
            entity.Property(e => e.CategoryId).HasColumnName("Category_id");
            entity.Property(e => e.Description).HasMaxLength(256);
            entity.Property(e => e.Detail).HasMaxLength(256);
            entity.Property(e => e.DistributorId).HasColumnName("Distributor_id");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("money");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Products__Catego__5535A963");

            entity.HasOne(d => d.Distributor).WithMany(p => p.Products)
                .HasForeignKey(d => d.DistributorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Products__Distri__5441852A");
        });

        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductI__3213E83FCFCC9420");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.ProductId).HasColumnName("Product_id");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductImages)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductIm__Produ__5812160E");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Reviews__3213E83F576C0474");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Comment).HasMaxLength(256);
            entity.Property(e => e.CreateAt).HasColumnName("Create_at");
            entity.Property(e => e.ProductId).HasColumnName("Product_id");
            entity.Property(e => e.UserId).HasColumnName("User_id");

            entity.HasOne(d => d.Product).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reviews__Product__628FA481");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Reviews__User_id__619B8048");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3213E83FEB849803");

            entity.HasIndex(e => e.UserName, "UQ__Users__C9F28456B7CB8293").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address).HasMaxLength(256);
            entity.Property(e => e.FullName).HasMaxLength(50);
            entity.Property(e => e.Image).HasMaxLength(256);
            entity.Property(e => e.Password).HasMaxLength(256);
            entity.Property(e => e.Phone).HasMaxLength(15);
            entity.Property(e => e.RandomKey)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserName).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
