using BL.Enums;
using BL.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Kompetenzverwaltung.Models
{
    public class CompetenceViewModel
    {
        public List<CompetenceArea> CompetenceAreas { get; set; } = new();
        public List<UserCompetence> UserCompetences { get; set; } = new();
        public UserCompetence UserCompetence { get; set; } = new();
        public CompetenceState? CompetenceState { get; set; }
    }
}
