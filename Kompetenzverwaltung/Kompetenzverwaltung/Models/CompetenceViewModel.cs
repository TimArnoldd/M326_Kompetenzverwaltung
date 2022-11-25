using BL.Models;

namespace Kompetenzverwaltung.Models
{
    public class CompetenceViewModel
    {
        public string UserId { get; set; }
        public List<Competence> Competences { get; set; }
        public List<Competence> PinnedCompetences { get; set; }
        public List<CompetenceArea> CompatenceAreas { get;set; }
    }
}
