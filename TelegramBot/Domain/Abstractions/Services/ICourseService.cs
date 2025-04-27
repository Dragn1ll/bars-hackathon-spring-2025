using Domain.Entities;
using Domain.Models.Dto.Admin;
using Domain.Models.Dto.Bot;
using Domain.Models.Dto.General;
using Domain.Utils;

namespace Domain.Abstractions.Services;

public interface ICourseService
{
    Task<Result<CourseDto>> CreateCourse(CreateCourseDto createCourse);
    Task<Result<CourseDto>> ChangeCourse(CourseDto course);
    Task<Result> DeleteCourse(Guid courseId);
    Task<Result<List<CourseDto>>> GetAllCourses();
    Task<Result<CourseDto>> GetCourseWithModules(Guid courseId);
}