using System.ComponentModel.DataAnnotations.Schema;

namespace SharedLibrary.Models;

public class Vacancy : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid CategoryId { get; set; }
    [ForeignKey(nameof(CategoryId))]
    public Category Category { get; set; }
    public bool IsActive { get; set; }
    public int QuestionCount { get; set; }

	public ICollection<Result> Results { get; set; }

}