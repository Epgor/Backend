using System.ComponentModel.DataAnnotations;
namespace Back.Models
{
    public class CreateCharacterDto
    {
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required]
        public string Class { get; set; }
        [Required]
        public string Race { get; set; }
    }
}
