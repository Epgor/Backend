using System.ComponentModel.DataAnnotations;
namespace Back.Models
{
    public class UpdateCharacterDto
    {
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        public string Class { get; set; }
        public int Level { get; set; }
        public string Location { get; set; } 
        public int Money { get; set; }
        public string? Guild { get; set; }
    }
}
