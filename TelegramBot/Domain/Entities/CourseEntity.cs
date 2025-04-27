namespace Domain.Entities;

public class CourseEntity
{
    public Guid CourseId { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public bool IsDeleted { get; set; }
    public ICollection<ModuleEntity> Modules { get; set; }
}