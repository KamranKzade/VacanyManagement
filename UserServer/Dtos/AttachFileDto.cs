namespace UserServer.API.Dto;

public class AttachFileDto
{
    public Guid Id { get; set; }
    public IFormFile File { get; set; }
}
