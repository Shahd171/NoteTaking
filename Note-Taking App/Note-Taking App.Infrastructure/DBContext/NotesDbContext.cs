using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Note_Taking_App.Infrastructure;

namespace Note_Taking_App.Infrastructure.DBContext;

public partial class NotesDbContext : DbContext
{
    public NotesDbContext()
    {
    }

    public NotesDbContext(DbContextOptions<NotesDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Note> Notes { get; set; }
    //builder.Entity<Note>()
    //    .HasOne(n => n.User)
    //    .WithMany(u => u.Notes)
    //    .HasForeignKey(n => n.UserId);

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-BOGODD7\\DEVELOPER;Database=Note-TakingApp;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Note>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Notes__3214EC07B6DA36D0");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Title).HasMaxLength(200);
            entity.Property(e => e.UserId).HasMaxLength(450);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
