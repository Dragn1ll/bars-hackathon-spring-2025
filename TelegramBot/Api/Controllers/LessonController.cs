using Domain.Abstractions.Services;
using Domain.Models.Dto.Admin;
using Domain.Models.Dto.General;
using Domain.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("/lessons")]
public class LessonController(ILessonService lessonService): ControllerBase
{
    [HttpPost]
    [Route("/create")]
    public async Task<IActionResult> CreateLesson(CreateLessonDto createLessonDto)
    {
        var result = await lessonService.CreateLesson(createLessonDto);
        return ResultRouter.GetActionResult(result);
    }

    [HttpPatch]
    [Route("/update")]
    public async Task<IActionResult> UpdateLesson(LessonDto updateLessonDto)
    {
        var result = await lessonService.UpdateLesson(updateLessonDto);
        return ResultRouter.GetActionResult(result);
    }

    [HttpDelete]
    [Route("/delete")]
    public async Task<IActionResult> DeleteLesson(int lessonId)
    {
        var result = await lessonService.DeleteLesson(lessonId);
        return ResultRouter.GetActionResult(result);
    }

    [HttpGet]
    [Route("/all/{moduleId:int}")]
    public async Task<IActionResult> GetAllLessons(int moduleId)
    {
        var result = await lessonService.GetAllLessons(moduleId);
        return ResultRouter.GetActionResult(result);
    }

    [HttpGet]
    [Route("/all/files/{lessonId:int}")]
    public async Task<IActionResult> GetAllLessonFiles(int lessonId)
    {
        var result = await lessonService.GetAllLessonFiles(lessonId);
        return ResultRouter.GetActionResult(result);
    }
}