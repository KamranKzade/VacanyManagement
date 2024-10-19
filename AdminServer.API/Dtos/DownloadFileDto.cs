namespace AdminServer.API.Dto;

public class DownloadFileDto
{
	public byte[] File { get; set; }
    public string ContentType { get; set; }
    public string FileName { get; set; }
}
