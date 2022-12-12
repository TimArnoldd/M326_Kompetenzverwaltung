using System.ComponentModel.DataAnnotations;

namespace Kompetenzverwaltung.Models
{
    public class ResourceViewModel
    {
        public int ResourceId { get; set; }
        public int CompetenceId { get; set; }
        [Required]
        public string DisplayText { get; set; } = string.Empty;
        [Required]
        [RegularExpression(
            @"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)",
            ErrorMessage = "The field Link must contain a valid Link. Example: \"https://mandelbulber.ch\"")]
        public string Link { get; set; } = string.Empty;
    }
}
