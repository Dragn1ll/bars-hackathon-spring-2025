using Domain.Entities;

namespace Domain.Abstractions.Services;

public interface ICourseService
{
    Task<CourseEntity> CreateCourse(CourseEntity course);
    Task<CourseEntity> ChangeCourse(CourseEntity course);
    Task<bool> DeleteCourse(int courseId);
    Task<List<CourseEntity>> GetAllCourses();
}