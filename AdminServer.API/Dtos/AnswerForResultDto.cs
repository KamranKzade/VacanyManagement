namespace AdminServer.API.Dtos;

public class AnswerForResultDto
{
    public string AppierId { get; set; }
    public string VacancyId { get; set; }

    // Key --> QuestionId
    // Value --> AnswerId
    public Dictionary<string, string> Answer { get; set; }
}
