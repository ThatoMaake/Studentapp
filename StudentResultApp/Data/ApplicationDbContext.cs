using Microsoft.EntityFrameworkCore;
using StudentResultApp.Models;

namespace StudentResultApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Module> Modules { get; set; }
        public DbSet<StudentResult> StudentResults { get; set; }
    }
}