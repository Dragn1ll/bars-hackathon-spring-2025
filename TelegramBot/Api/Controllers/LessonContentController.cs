using Domain.Abstractions.Services;
using Domain.Models.Dto.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("lessons/content")]
public class LessonContentController(ILessonContentService lessonContentService): ControllerBase
{
    [Authorize]
    [HttpPost("/create")]
    public async Task<IActionResult> CreateLessonContent(CreateLessonContentDto lessonContentDto, IFormFile file)
    {
        await using var stream = file.OpenReadStream();
        var result = await lessonContentService.AddLessonContent(lessonContentDto, file.FileName, stream, file.ContentType);
        return ResultRouter.GetActionResult(result);
    }
    
    [Authorize]
    [HttpDelete("/delete/{lessonContentId:guid}")]
    public async Task<IActionResult> DeleteLessonContent(Guid lessonContentId)
    {
        var result = await lessonContentService.RemoveLessonContent(lessonContentId);
        return ResultRouter.GetActionResult(result);
    }

    [HttpGet("/all/{lessonId:guid}")]
    public async Task<IActionResult> GetAllLessonContent(Guid lessonId)
    {
        var result = await lessonContentService.GetAllLessonContents(lessonId);
        return ResultRouter.GetActionResult(result);
    }
}