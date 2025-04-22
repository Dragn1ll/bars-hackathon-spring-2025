namespace Domain.Entities;

public class CourseEntity
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public ICollection<ModuleEntity> Modules { get; set; }
}