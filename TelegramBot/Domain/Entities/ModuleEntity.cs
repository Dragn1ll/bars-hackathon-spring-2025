namespace Domain.Entities;

public class ModuleEntity
{
    public Guid ModuleId { get; set; }
    public Guid CourseId { get; set; }
    public CourseEntity Course { get; set; }
    public string Title { get; set; }
    public bool IsDeleted { get; set; }
    public ICollection<LessonEntity> Lessons { get; set; }
}