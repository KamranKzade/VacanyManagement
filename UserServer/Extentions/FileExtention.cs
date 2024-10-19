namespace UserServer.API.Extentions;

public static class FileExtention
{
	public static int GetFileSizeInMB(this IFormFile file)
	{
		return Convert.ToInt32(Math.Round(file.Length / 1024f) / 1024f);
	}

	public static int GetFileSizeInKB(this IFormFile file)
	{
		return Convert.ToInt32(Math.Round(file.Length / 1024f));
	}
}
