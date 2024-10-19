using System.ComponentModel.DataAnnotations.Schema;

namespace SharedLibrary.Models;

public class Question : BaseEntity
{
    public string Description { get; set; }
    public Guid CategoryId { get; set; }
    [ForeignKey(nameof(CategoryId))]
    public Category Category { get; set; }
    public List<Answer> Answers { get; set; }
}
