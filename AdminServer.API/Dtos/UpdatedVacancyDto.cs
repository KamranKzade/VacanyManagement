using SharedLibrary.Models;

namespace AdminServer.API.Dtos;

public class UpdatedVacancyDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid CategoryId { get; set; }
    public bool IsActive { get; set; }
    public int QuestionCount { get; set; }
}
