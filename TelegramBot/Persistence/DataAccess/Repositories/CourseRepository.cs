using Domain.Abstractions.Repositories;
using Domain.Entities;

namespace Persistence.DataAccess.Repositories;

public class CourseRepository(AppDbContext context) : 
    AbstractRepository<CourseEntity>(context), 
    ICourseRepository
{
    public async Task<IEnumerable<CourseEntity?>> GetAllCoursesAsync()
    {
        return await GetAllByFilterAsync(e => e.IsDeleted == false);
    }

    public async Task<IEnumerable<CourseEntity?>> GetAllDeletedCourseAsync()
    {
        return await GetAllByFilterAsync(e => e.IsDeleted == true);
    }

    public async Task<IEnumerable<CourseEntity?>> GetCoursesByTitleAsync(string title)
    {
        return await GetAllByFilterAsync(e => e.Title
            .Contains(title, StringComparison.CurrentCultureIgnoreCase));
    }

    public async Task<CourseEntity?> GetCourseByIdAsync(int courseId)
    {
        return await GetByFilterAsync(e => e.Id == courseId);
    }

    public async Task<bool> PatchCourseTitleAsync(int courseId, string newTitle)
    {
        return await PatchAsync(courseId, e => e.Title = newTitle);
    }

    public async Task<bool> PatchCourseDescriptionAsync(int courseId, string newDescription)
    {
        return await PatchAsync(courseId, e => e.Description = newDescription);
    }

    public async Task<bool> PatchDeleteStatusAsync(int courseId)
    {
        return await PatchAsync(courseId, e => e.IsDeleted = !e.IsDeleted);
    }

    public async Task<bool> DeleteCourseAsync(int courseId)
    {
        return await DeleteAsync(e => e.Id == courseId);
    }
}