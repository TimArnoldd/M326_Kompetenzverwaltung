using BL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Models
{
    public class UserCompetence
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public virtual Competence Competence { get; set; }
        public int CompetenceId { get; set; }
        [Required]
        public virtual ApplicationUser User { get; set; }
        [Required]
        public CompetenceState State { get; set; }
        [Required]
        public bool Pinned { get; set; }
    }
}
