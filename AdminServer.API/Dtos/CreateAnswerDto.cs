namespace AdminServer.API.Dtos;

public class CreateAnswerDto
{
    public string QuestionId { get; set; }
    public string Descrtiption { get; set; }
    public bool IsTrue { get; set; }
}
