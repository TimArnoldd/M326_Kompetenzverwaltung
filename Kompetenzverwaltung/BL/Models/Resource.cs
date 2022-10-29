using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Models
{
    public class Resource
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public virtual Competence Competence { get; set; } // CompetenceAreaId in Resources?!?
        public string DisplayText { get; set; } = string.Empty;
        [Required]
        public string Link { get; set; } = string.Empty;
    }
}
