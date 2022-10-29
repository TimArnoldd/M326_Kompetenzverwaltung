using BL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Models
{
    public class Competence
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public virtual CompetenceArea CompetenceArea { get; set; }
        public virtual ICollection<UserCompetence> UserCompetences { get; set; }
        public virtual ICollection<Resource> Resources { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public CompetenceLevel Level { get; set; }
    }
}
