using System.ComponentModel.DataAnnotations;

namespace AdminServer.API.Dtos;

public class UpdatedQuestionDto
{
    public string Id { get; set; }
    [Required]
    [MinLength(5)]
    [MaxLength(255)]
    public string Description { get; set; }
    [Required]
    public Guid CategoryId { get; set; }
}
