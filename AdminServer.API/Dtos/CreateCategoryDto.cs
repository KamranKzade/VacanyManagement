using System.ComponentModel.DataAnnotations;

namespace AdminServer.API.Dtos;

public class CreateCategoryDto
{
    [Required]
    [MaxLength(30)]
    [MinLength(2)]
    public string Name { get; set; }
}
