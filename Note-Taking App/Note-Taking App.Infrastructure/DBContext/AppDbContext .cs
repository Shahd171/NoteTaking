using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Note_Taking_App.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Note_Taking_App.Infrastructure.DBContext
{
    public class AppDbContext:IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public AppDbContext() { }
        //public DbSet<Note> Notes { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-BOGODD7\\DEVELOPER;Initial Catalog=Note-TakingApp;Trusted_Connection=True;TrustServerCertificate=True");
            }
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //builder.Entity<Note>()
            //    .HasOne(n => n.User)
            //    .WithMany(u => u.Notes)
            //    .HasForeignKey(n => n.UserId);
        }


    }
}
