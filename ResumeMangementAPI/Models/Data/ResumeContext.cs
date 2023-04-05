using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace ResumeManagementAPI.Models.Data
{
    public class ResumeContext:DbContext
    {
        public ResumeContext(DbContextOptions<ResumeContext> options):base(options) 
        {
            
        }
        public DbSet<Jobs> Jobs { get; set; }
        public DbSet<Candidates> Candidates { get; set; }   
        public DbSet<Company> companies { get; set; }
    }
}
