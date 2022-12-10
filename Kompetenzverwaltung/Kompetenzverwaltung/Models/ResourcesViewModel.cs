namespace Kompetenzverwaltung.Models
{
    public class ResourcesViewModel
    {
        public int CompetenceId { get; set; }
        public string CompetenceName { get; set; } = string.Empty;
        public List<ResourceViewModel> Resources { get; set; } = new();
    }
}
