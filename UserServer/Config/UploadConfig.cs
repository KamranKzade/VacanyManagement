namespace UserServer.API.Config;

public class UploadConfig
{
	public int SizeLimitInMegabytes { get; set; }
	public List<string> ValidFileTypes { get; set; }
}
