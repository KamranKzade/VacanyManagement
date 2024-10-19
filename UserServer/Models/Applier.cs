using SharedLibrary.Models;

namespace UserServer.Models;


public class Applier : BaseEntity
{
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string Email { get; set; }
	public string PhoneNumber { get; set; }
	public DateTime AppliedDate { get; set; }
	public string? FilePath { get; set; }

    public string? SendAnswerResponse { get; set; }
}
