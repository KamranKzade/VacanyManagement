namespace UserServer.API.Dto;

public class QuestionForUserDto
{
	public Guid Id { get; set; }
	public string Description { get; set; }
	public List<AnswerForUserDto> Answers { get; set; }
}
