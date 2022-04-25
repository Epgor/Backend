using Microsoft.EntityFrameworkCore;
using Back.Entities;
namespace Back
{
    public class ApiSeeder
    {
        private readonly ApiDbContext dbContext;

        public ApiSeeder(ApiDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void seed()
        {
            if (dbContext.Database.CanConnect())
            {
                var pendingMigrations = dbContext.Database.GetPendingMigrations();
                if (pendingMigrations != null && pendingMigrations.Any())
                {
                    dbContext.Database.Migrate();
                }
                if (!dbContext.Characters.Any())
                {
                    var characters = GetCharacters();
                    dbContext.Characters.AddRange(characters);
                    dbContext.SaveChanges();
                }
            }
        }

        private IEnumerable<Character> GetCharacters()
        {
            var characters = new List<Character>();
            for (int i = 0; i < 2; i++)
            {
                var character = new Character()
                {
                    Name = "tEST",
                    Class = "test",
                    Level = 1,
                    Location = "Starting Location",
                    Money = 100,
                    Guild = "test"
                };
                Console.WriteLine("xd");
            }
            return characters;
        }
    }
}
