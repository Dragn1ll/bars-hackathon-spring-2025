using System.Linq.Expressions;
using Domain.Abstractions.Repositories;
using Domain.Abstractions.Services;
using Domain.Entities;
using Domain.Models.Dto.Admin;
using Domain.Models.Dto.General;
using Domain.Models.Enums;
using Domain.Utils;

namespace Application.Services;

public class CourseService(IUnitOfWork unitOfWork, Mapper mapper) : ICourseService
{
    public async Task<Result<CourseDto>> CreateCourse(CreateCourseDto createCourse)
    {
        try
        {
            if (await ThereIsACourse(c => createCourse.Title == c.Title))
                return Result<CourseDto>.Failure(new 
                    Error(ErrorType.BadRequest, "Course already exists"));
            
            var courseEntity = mapper.Map<CreateCourseDto, CourseEntity>(createCourse);
            courseEntity.CourseId = Guid.NewGuid();
            
            var result = await unitOfWork.Courses
                .AddAsync(courseEntity);

            await unitOfWork.SaveChangesAsync();
            
            return result 
                ? Result<CourseDto>.Success(mapper.Map<CourseEntity, CourseDto>(courseEntity)) 
                : Result<CourseDto>.Failure(
                    new Error(ErrorType.ServerError, "Can't register course"));
        }
        catch (Exception exception)
        {
            return Result<CourseDto>.Failure(new Error(ErrorType.ServerError, exception.Message));
        }
    }

    public async Task<Result<CourseDto>> ChangeCourse(CourseDto course)
    {
        try
        {
            if (!await ThereIsACourse(c => c.CourseId == course.CourseId))
                return Result<CourseDto>.Failure(
                    new Error(ErrorType.BadRequest, "Course does not exist"));

            var result = await unitOfWork.Courses.PatchAsync(course.CourseId, c =>
            {
                c.Title = course.Title;
                c.Description = course.Description;
            });
            
            await unitOfWork.SaveChangesAsync();
            
            return result
                ? Result<CourseDto>.Success(mapper.Map<CourseDto, CourseDto>(course))
                : Result<CourseDto>.Failure(new Error(ErrorType.ServerError, "Can't change course"));
        }
        catch (Exception exception)
        {
            return Result<CourseDto>.Failure(new Error(ErrorType.ServerError, exception.Message));
        }
    }

    public async Task<Result> DeleteCourse(Guid courseId)
    {
        try
        {
            if (!await ThereIsACourse(c => c.CourseId == courseId))
                return Result.Failure(new Error(ErrorType.BadRequest, "Course does not exist"));

            var result = await unitOfWork.Courses.DeleteAsync(c => c.CourseId == courseId);
            
            await unitOfWork.SaveChangesAsync();
            
            return result
                ? Result.Success()
                : Result.Failure(new Error(ErrorType.ServerError, "Can't delete course"));
        }
        catch (Exception exception)
        {
            return Result<CourseDto>.Failure(new Error(ErrorType.ServerError, exception.Message));
        }
    }

    public async Task<Result<List<CourseDto>>> GetAllCourses()
    {
        try
        {
            return Result<List<CourseDto>>.Success((await unitOfWork.Courses
                    .GetAllByFilterAsync(c => true))
                .Select(c => mapper.Map<CourseEntity, CourseDto>(c)).ToList());
        }
        catch (Exception exception)
        {
            return Result<List<CourseDto>>.Failure(new Error(ErrorType.ServerError, exception.Message));
        }
    }

    public async Task<Result<CourseDto>> GetCourseWithModules(Guid courseId)
    {
        try
        {
            if (!await ThereIsACourse(c => c.CourseId == courseId))
                return Result<CourseDto>.Failure(new Error(ErrorType.BadRequest, "Course does not exist"));
            
            return Result<CourseDto>.Success(
                mapper.Map<CourseEntity, CourseDto>(await unitOfWork.Courses
                    .GetCourseWithModules(courseId) ?? new CourseEntity()));
        }
        catch (Exception exception)
        {
            return Result<CourseDto>.Failure(new Error(ErrorType.ServerError, exception.Message));
        }
    }

    private async Task<bool> ThereIsACourse(Expression<Func<CourseEntity, bool>> predicate)
    {
        return await unitOfWork.Courses.GetByFilterAsync(predicate) != null;
    }
}