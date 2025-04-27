using Domain.Abstractions.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

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

    public async Task<CourseEntity?> GetCourseByIdAsync(Guid courseId)
    {
        return await GetByFilterAsync(e => e.CourseId == courseId);
    }

    public async Task<bool> PatchCourseTitleAsync(Guid courseId, string newTitle)
    {
        return await PatchAsync(courseId, e => e.Title = newTitle);
    }

    public async Task<bool> PatchCourseDescriptionAsync(Guid courseId, string newDescription)
    {
        return await PatchAsync(courseId, e => e.Description = newDescription);
    }

    public async Task<bool> PatchDeleteStatusAsync(Guid courseId)
    {
        return await PatchAsync(courseId, e => e.IsDeleted = !e.IsDeleted);
    }

    public async Task<bool> DeleteCourseAsync(Guid courseId)
    {
        return await DeleteAsync(e => e.CourseId == courseId);
    }

    public async Task<CourseEntity?> GetCourseWithModules(int courseId)
    {
        return context.Set<CourseEntity>()
            .AsNoTracking()
            .Include(course => course.Modules)
            .FirstOrDefault(course => course.CourseId == courseId);
    }
}