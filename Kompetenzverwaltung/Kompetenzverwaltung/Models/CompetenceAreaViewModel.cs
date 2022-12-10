using System.ComponentModel.DataAnnotations;

namespace Kompetenzverwaltung.Models
{
    public class CompetenceAreaViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
