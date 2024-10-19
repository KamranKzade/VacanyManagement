namespace UserServer.API.Dtos;

public class SendAnswerDto
{
	public string AppierId { get; set; }
	
	// Key --> QuestionId
	// Value --> AnswerId
	public Dictionary<string, string> Answer;
}
