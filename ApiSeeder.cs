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
            var Name1= new String[]{ "Artred", "Hobniir", "Toraard", "Ockam", "Nelen", "Talon", "Thomanar", "Vusera", "Tasuur", "Huvil" };
            var Name2 = new String[] { "Bronzetask", "Ironstriker", "Cravensword", "Rabidchampion", "Quickfist", "Warheart", " Lowfire", "Warslice", "Deepflame", "Warpmaw" };
            var Name = new List<String> { };
            foreach (var name1 in Name1)
            {
                foreach (var name2 in Name2)
                {
                    var name = name1 + " " +name2;
                    Name.Add(name);
                }
            };
            var rnd = new Random();
            var random_name = Name.OrderBy(item => rnd.Next());

            var Race = new String[] { "Draenei", "Dwarf", "Gnome", "Human", "Night Elf", "Worgen", "Blood Elf", "Goblin", "Orc", "Tauren", "Troll" };
            var Class = new String[] { "Warrior", "Hunter", "Paladin", "Rogue", "Priest", "Shaman", "Mage", "Warlock", "Druid", "Mage", "Death Knight"};

            var Location1 = new String[] { "Landow", "Cewmann", "Arkney", "Burrafirth", "Rochdale", "Satbury", "Yellowseed", "Ballingsmallard", "Penrith", "Dornwich" };
            var Location2 = new String[] { "City", "Ruins", "Desert", "Lake", "Forest", "Island", "Cave", "Mountains", "Hills", "Arena", "Dungeons", "Market"};
            var Location = new List<String> { };
            foreach (var loc1 in Location1)
            {
                foreach (var loc2 in Location2)
                {
                    var loc = loc1 + " " + loc2;
                    Location.Add(loc);
                }
            };
            var characters = new List<Character>();
            foreach (var name in random_name)
            {
                string race = Race[rnd.Next(Race.Count())];
                string _class = Class[rnd.Next(Class.Count())];
                string location = Location[rnd.Next(Location.Count())];
                var character = new Character()
                {
                    Name = name,
                    Class = _class,
                    Race = race,
                    Level = rnd.Next(70, 80),
                    Location = location,
                    Money = rnd.Next(1000, 99999)
                };
                Console.WriteLine(character.Name + " " + character.Class + " " + character.Race);
                Console.WriteLine(character.Location + " " + character.Money + " " + character.Level);
                characters.Add(character);
            }
            return characters;
        }
    }
}
