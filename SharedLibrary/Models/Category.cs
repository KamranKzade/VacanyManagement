namespace SharedLibrary.Models;

public class Category : BaseEntity
{
    public string Name { get; set; }
    public List<Vacancy> Vacansies { get; set; }
    public List<Question> Questions { get; set; }
}
