using Domain.Abstractions.Services;
using Domain.Models.Dto.Admin;
using Domain.Models.Dto.Bot;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("quiz/questions")]
public class QuizQuestionController(IQuizQuestionService quizQuestionService): ControllerBase
{
    [Authorize]
    [HttpPost("create")]
    public async Task<IActionResult> CreateQuizQuestion(CreateQuestionDto questionDto)
    {
        var result = await quizQuestionService.CreateQuizQuestion(questionDto);
        return ResultRouter.GetActionResult(result);
    }
    
    [Authorize]
    [HttpDelete("delete/{questionId:guid}")]
    public async Task<IActionResult> DeleteQuizQuestion(Guid questionId)
    {
        var result = await quizQuestionService.DeleteQuizQuestion(questionId);
        return ResultRouter.GetActionResult(result);
    }

    [HttpPost("next")]
    public async Task<IActionResult> GetNextQuizQuestions(UserAnswerDtoRequest userAnswerDtoRequest)
    {
        var result = await quizQuestionService.GetNextQuestionForUser(userAnswerDtoRequest);
        return ResultRouter.GetActionResult(result);
    }
    
    [HttpGet("{lessonId:guid}/{userId:long}")]
    public async Task<IActionResult> GetQuizQuestions(Guid lessonId, long userId)
    {
        var result = await quizQuestionService.GetQuizQuestionForUser(lessonId, userId);
        return ResultRouter.GetActionResult(result);
    }
}