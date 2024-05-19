
using CrimeManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrimeManagement.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Suspect> Suspects { get; set; }
        public DbSet<CriminalRecord> CriminalRecords { get; set; }
        /*public DbSet<CaseManager> CaseManagers { get; set; }*/

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Suspect>()
                .HasMany(s => s.CriminalRecords)
                .WithOne(cr => cr.Suspect)
                .HasForeignKey(cr => cr.SuspectNo);

            modelBuilder.Entity<CriminalRecord>()

                .HasOne(cr => cr.CaseManager)
                .WithMany(u => u.AssignedCriminalRecords)
                .HasForeignKey(cr => cr.CaseManagerId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            /*modelBuilder.Entity<CriminalRecord>()
                 .Property(cr => cr.CaseManagerId)
                 .HasColumnName("CaseManagerId")
                 .HasColumnType("nvarchar(450)") 
                 .IsRequired(false);*/




            /*modelBuilder.Entity<CriminalRecord>()
                .HasOne(cr => cr.CaseManager)
                .WithMany(cm => cm.CriminalRecords)
                .HasForeignKey(cr => cr.CaseManagerId);*/

            /*modelBuilder.Entity<CaseManager>().HasData(
                new CaseManager
                {
                    CaseManagerId = 1,
                    FirstName = "Sthe",
                    LastName = "Zwane"
                },
                new CaseManager
                {
                    CaseManagerId = 2,
                    FirstName = "Thapelo",
                    LastName = "Kgasi"
                },
                new CaseManager
                {
                    CaseManagerId = 3,
                    FirstName = "Kat",
                    LastName = "Aphane"
                }
            );*/
        }
    }
}
