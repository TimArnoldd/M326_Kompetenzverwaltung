using BL.Models;

namespace Kompetenzverwaltung.Models
{
    public class CompetencesViewModel
    {
        public List<Competence> Competences { get; set; } = new();
        public string CompetenceAreaName { get; set; } = string.Empty;
        public int CompetenceAreaId { get; set; }
    }
}
