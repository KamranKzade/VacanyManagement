namespace AdminServer.API.Dtos;

public class AnswerForResultDto
{
    public string AppierId { get; set; }

    // Key --> QuestionId
    // Value --> AnswerId
    public string Answer { get; set; }
}
