using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace ResumeManagementAPI.Models.Data
{
    public class ResumeContext:IdentityDbContext<AppUser>
    {
        public ResumeContext(DbContextOptions<ResumeContext> options):base(options) 
        {
            
        }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Candidates> Candidates { get; set; }   
        public DbSet<Company> companies { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Company>()
                .Property(company => company.Size)
                .HasConversion<string>();

            modelBuilder.Entity<Job>()
               .Property(j => j.level)
               .HasConversion<string>();
        }
    }
}
