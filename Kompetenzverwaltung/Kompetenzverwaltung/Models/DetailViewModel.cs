using BL.Enums;
using BL.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Kompetenzverwaltung.Models
{
    public class DetailViewModel
    {
        public List<CompetenceArea> CompetenceAreas { get; set; } = new();
        public List<UserCompetence> UserCompetences { get; set; } = new();
        public UserCompetence UserCompetence { get; set; } = new();
        [Display(Name = "State")]
        public CompetenceState? CompetenceState { get; set; }
        public List<Resource> Resources { get; set; } = new();
    }
}
