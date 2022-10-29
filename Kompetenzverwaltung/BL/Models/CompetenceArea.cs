using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Models
{
    public class CompetenceArea
    {
        [Required]
        public int Id { get; set; }
        public virtual ICollection<Competence> Competences { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
