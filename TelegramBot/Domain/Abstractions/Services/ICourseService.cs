using Domain.Entities;
using Domain.Utils;

namespace Domain.Abstractions.Services;

public interface ICourseService
{
    Task<Result<CourseEntity>> CreateCourse(CourseEntity course);
    Task<Result<CourseEntity>> ChangeCourse(CourseEntity course);
    Task<Result> DeleteCourse(int courseId);
    Task<Result<List<CourseEntity>>> GetAllCourses();
}