using Domain.Entities;

namespace Domain.Abstractions.Repositories;

public interface ICourseRepository : IRepository<CourseEntity>
{
    Task<IEnumerable<CourseEntity?>> GetAllCoursesAsync();
    Task<IEnumerable<CourseEntity?>> GetAllDeletedCourseAsync();
    Task<IEnumerable<CourseEntity?>> GetCoursesByTitleAsync(int courseId);
    Task<CourseEntity?> GetCourseByIdAsync(int courseId);
    Task<bool> PatchCourseNameAsync(int courseId, string newName);
    Task<bool> PatchCourseDescriptionAsync(int courseId, string newDescription);
    Task<bool> PatchDeleteStatusAsync(int courseId);
    Task<bool> DeleteCourseAsync(int courseId);
}