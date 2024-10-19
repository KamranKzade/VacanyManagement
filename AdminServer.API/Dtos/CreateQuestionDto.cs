using System.ComponentModel.DataAnnotations;

namespace AdminServer.API.Dtos;

public class CreateQuestionDto
{
    [Required]
    [MinLength(5)]
    [MaxLength(255)]
    public string Description { get; set; }
    [Required]
    public Guid CategoryId { get; set; }
}
