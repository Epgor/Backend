namespace Back.Entities
{
    public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public string Race { get; set; }
        public int Level { get; set; } = 0;
        public string Location { get; set; } = "Starting Location";
        public int Money { get; set; } = 100;

    }
}
