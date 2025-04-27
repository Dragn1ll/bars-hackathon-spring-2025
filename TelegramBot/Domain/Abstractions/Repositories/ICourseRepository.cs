using Domain.Entities;

namespace Domain.Abstractions.Repositories;

public interface ICourseRepository : IRepository<CourseEntity>
{
    Task<CourseEntity?> GetCourseWithModules(Guid courseId);
}