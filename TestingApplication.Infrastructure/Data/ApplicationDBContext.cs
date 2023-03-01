using Microsoft.EntityFrameworkCore;
using TestingApplication.Infrastructure.Domains;

namespace TestingApplication.Infrastructure.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext()
        {
            Database.EnsureCreated();
            Database.Migrate();
        }

        public DbSet<Word> Words { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer(@"Server=localhost;Database=TestApplication;Integrated Security=True;TrustServerCertificate=True");
        }
    }
}
