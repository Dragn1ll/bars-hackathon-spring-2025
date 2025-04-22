namespace Domain.Entities;

public class ModuleEntity
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public CourseEntity Course { get; set; }
    public string Title { get; set; }
    public ICollection<LessonEntity> Lessons { get; set; }
}