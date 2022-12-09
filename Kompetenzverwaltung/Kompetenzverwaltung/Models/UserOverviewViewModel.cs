using BL.Models;

namespace Kompetenzverwaltung.Models
{
    public class UserOverviewViewModel
    {
        public List<CompetenceArea> CompetenceAreas { get; set; } = new();
        public List<UserCompetence> UserCompetences { get; set; } = new();
    }
}
