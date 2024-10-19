namespace SharedLibrary.Models;

public class ApplierForAdmin
{
	public Guid Id { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string Email { get; set; }
	public string PhoneNumber { get; set; }
	public DateTime AppliedDate { get; set; }
	public string? FilePath { get; set; }

    public string ApplierTestResponse { get; set; }

    public ICollection<Result> Results { get; set; }
}
