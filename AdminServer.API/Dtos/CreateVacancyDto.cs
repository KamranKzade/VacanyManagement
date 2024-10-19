using System.ComponentModel.DataAnnotations;

namespace AdminServer.API.Dtos;

public class CreateVacancyDto
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    public DateTime EndDate { get; set; }
    [Required]
    public Guid CategoryId { get; set; }
    [Required]
    public bool IsActive { get; set; }
    [Required]
    [Range(10,30)]
    public int QuestionCount { get; set; }
}
