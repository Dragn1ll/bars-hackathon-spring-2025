using Domain.Abstractions.Services;
using Domain.Models.Dto.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("/quiz/options")]
public class QuizOptionController(IQuizOptionService quizOptionService): ControllerBase
{
    [Authorize]
    [HttpPost("/create")]
    public async Task<IActionResult> CreateQuestionOption(CreateQuestionOptionDto questionOptionDto)
    {
        var result = await quizOptionService.AddQuizOption(questionOptionDto);
        return ResultRouter.GetActionResult(result);
    }
    
    [Authorize]
    [HttpDelete("/delete/{questionOptionId:guid}")]
    public async Task<IActionResult> DeleteQuestionOption(Guid questionOptionId)
    {
        var result = await quizOptionService.DeleteQuizOption(questionOptionId);
        return ResultRouter.GetActionResult(result);
    }    
}