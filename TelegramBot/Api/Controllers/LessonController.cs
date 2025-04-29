using Api.Filters;
using Domain.Abstractions.Services;
using Domain.Models.Dto.Admin;
using Domain.Models.Dto.General;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("lessons")]
public class LessonController(ILessonService lessonService): ControllerBase
{
    [HttpPost]
    [Authorize]
    [Route("create")]
    public async Task<IActionResult> CreateLesson(CreateLessonDto createLessonDto)
    {
        var result = await lessonService.CreateLesson(createLessonDto);
        return ResultRouter.GetActionResult(result);
    }

    [HttpPatch]
    [Authorize]
    [Route("update")]
    public async Task<IActionResult> UpdateLesson(LessonDto updateLessonDto)
    {
        var result = await lessonService.UpdateLesson(updateLessonDto);
        return ResultRouter.GetActionResult(result);
    }

    [HttpDelete]
    [Authorize]
    [Route("delete/{lessonId:guid}")]
    public async Task<IActionResult> DeleteLesson(Guid lessonId)
    {
        var result = await lessonService.DeleteLesson(lessonId);
        return ResultRouter.GetActionResult(result);
    }
    
    [ServiceFilter(typeof(TelegramUserAuthFilter))]
    [HttpGet]
    [Route("all/{moduleId:guid}")]
    public async Task<IActionResult> GetAllLessons(Guid moduleId)
    {
        var result = await lessonService.GetAllLessons(moduleId);
        return ResultRouter.GetActionResult(result);
    }
    
    [ServiceFilter(typeof(TelegramUserAuthFilter))]
    [HttpGet]
    [Route("files/{lessonId:guid}")]
    public async Task<IActionResult> GetAllLessonFiles(Guid lessonId)
    {
        var files = await lessonService.GetAllLessonFiles(lessonId);
        return ResultRouter.GetActionResult(files);
    }
    
    [ServiceFilter(typeof(TelegramUserAuthFilter))]
    [HttpGet]
    [Route("files/urls/{lessonId:guid}")]
    public async Task<IActionResult> GetAllLessonFileUrls(Guid lessonId)
    {
        var filesUrls = await lessonService.GetLessonFilesUrls(lessonId);
        return ResultRouter.GetActionResult(filesUrls);
    }
    
    [ServiceFilter(typeof(TelegramUserAuthFilter))]
    [HttpGet]
    [Route("{lessonId:guid}")]
    public async Task<IActionResult> GetLesson(Guid lessonId)
    {
        var lesson = await lessonService.GetLesson(lessonId);
        return ResultRouter.GetActionResult(lesson);
    }
}