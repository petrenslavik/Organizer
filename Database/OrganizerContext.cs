using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    public class OrganizerContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        
        public OrganizerContext(DbContextOptions<OrganizerContext> options):base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<User>().HasData(
            //    new User[]
            //    {
            //    new User { Id = 1, Username = "Tom", PasswordHash = "123" },
            //        new User { Id = 2, Username = "Alice", PasswordHash = "26" },
            //        new User { Id = 3, Username = "Sam", PasswordHash = "28" }
            //     });
        }
    }
}
