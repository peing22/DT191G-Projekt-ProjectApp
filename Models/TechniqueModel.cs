using System.ComponentModel.DataAnnotations;

namespace ProjectApp.Models;

public class TechniqueModel
{
    // Unik identifierare
    public int Id { get; set; }

    // Egenskap för namn
    [Required] // Krävs
    [Display(Name = "Namn")]
    public string? Name { get; set; }

    // Navigationsegenskap som representerar relationen mellan TechniqueModel och ProjectModel
    public ICollection<ProjectModel> Projects { get; set; } = [];
}