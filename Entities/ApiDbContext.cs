using Microsoft.EntityFrameworkCore;
namespace Back.Entities
{
    public class ApiDbContext : DbContext
    {
        private string _connectionString =
            "Server=(LocalDB)\\MSSQLLocalDB;Database=ProjApiDb;Trusted_Connection=True;";
        public DbSet<Character> Characters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Character>()
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(30);

            modelBuilder.Entity<Character>()
                .Property(x => x.Class)
                .IsRequired();

            modelBuilder.Entity<Character>()
                .Property(x => x.Race)
                .IsRequired();

            modelBuilder.Entity<Character>()
                .Property(x => x.Location)
                .IsRequired();

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

    }
}
