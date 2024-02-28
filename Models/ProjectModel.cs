using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectApp.Models;

public class ProjectModel
{
    // Unik identifierare
    public int Id { get; set; }

    // Egenskap för titel
    [Required] // Krävs
    [Display(Name = "Titel")]
    public string? Title { get; set; }

    // Egenskap för beskrivning
    [Required] // Krävs
    [Display(Name = "Beskrivning")]
    public string? Description { get; set; }

    // Egenskap för URL
    [Required] // Krävs
    [Url] // Validerar att det är en giltig URL
    [Display(Name = "URL")]
    public string? Url { get; set; }

    // Egenskap för bild
    [Display(Name = "Bild")]
    public string? ImageName { get; set; }

    // Egenskap för bildfil som inte sparas i databasen
    [NotMapped]
    [Display(Name = "Bild")]
    public IFormFile? ImageFile { get; set; }

    // Navigationsegenskap som representerar relationen mellan ProjectModel och TechniqueModel
    [Display(Name = "Tekniker")]
    public ICollection<TechniqueModel> Techniques { get; set; } = [];
}