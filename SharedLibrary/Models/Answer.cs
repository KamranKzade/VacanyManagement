using System.ComponentModel.DataAnnotations.Schema;

namespace SharedLibrary.Models;

public class Answer : BaseEntity
{
    public string Descrtiption { get; set; }
    public bool IsTrue { get; set; }
    public Guid QuestionId { get; set; }
    [ForeignKey(nameof(QuestionId))]
    public Question Question { get; set; }
}
