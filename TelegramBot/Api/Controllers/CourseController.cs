using Domain.Abstractions.Services;
using Domain.Models.Dto.Admin;
using Domain.Models.Dto.General;
using Domain.Models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("/courses")]
public class CourseController(ICourseService courseService): ControllerBase
{
    [HttpPost]
    [Route("/add")]
    public async Task<IActionResult> CreateCourse(CreateCourseDto createCourseDto)
    {
        var result = await courseService.CreateCourse(createCourseDto);
        return ResultRouter.GetActionResult(result);
    }

    [HttpPatch]
    [Route("/update")]
    public async Task<IActionResult> UpdateCourse(CourseDto courseDto)
    {
        var result = await courseService.ChangeCourse(courseDto);
        return ResultRouter.GetActionResult(result);
    }

    [HttpDelete]
    [Route("/delete/{courseId:guid}")]
    public async Task<IActionResult> DeleteCourse(Guid courseId)
    {
        var result = await courseService.DeleteCourse(courseId);
        return ResultRouter.GetActionResult(result);
    }
    
    [HttpGet]
    [Route("/all")]
    public async Task<IActionResult> GetCourses()
    {
        var result = await courseService.GetAllCourses();
        return ResultRouter.GetActionResult(result);
    }

    [HttpGet]
    [Route("/{courseId:guid}")]
    public async Task<IActionResult> GetCourse(Guid courseId)
    {
        var result = await courseService.GetCourseWithModules(courseId);
        return ResultRouter.GetActionResult(result);
    }
}