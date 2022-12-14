using BL;
using BL.Enums;
using BL.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.ComponentModel.DataAnnotations;

namespace Kompetenzverwaltung.Models
{
    public class CompetenceViewModel
    {
        public int CompetenceId { get; set; }
        public int CompetenceAreaId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public CompetenceLevel Level { get; set; }
        public List<SelectListItem> Levels { get; set; }

        public CompetenceViewModel()
        {
            Levels = Enum.GetNames(typeof(CompetenceLevel))
                .Select(x => new SelectListItem { Text = x, Value = x })
                .ToList();
        }
    }
}
