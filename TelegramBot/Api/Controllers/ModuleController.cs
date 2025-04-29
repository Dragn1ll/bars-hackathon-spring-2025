using Api.Filters;
using Domain.Abstractions.Services;
using Domain.Models.Dto.Admin;
using Domain.Models.Dto.General;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("modules")]
public class ModuleController(IModuleService moduleService): ControllerBase
{
    [HttpPost]
    [Authorize]
    [Route("add")]
    public async Task<IActionResult> CreateModule(CreateModuleDto moduleDto)
    {
        var module = await moduleService.CreateModule(moduleDto);
        return ResultRouter.GetActionResult(module);
    }

    [HttpPatch]
    [Authorize]
    [Route("update")]
    public async Task<IActionResult> UpdateModule(ModuleDto moduleDto)
    {
        var module = await moduleService.UpdateModule(moduleDto);
        return ResultRouter.GetActionResult(module);
    }

    [HttpDelete]
    [Authorize]
    [Route("delete/{moduleId:guid}")]
    public async Task<IActionResult> DeleteModule(Guid moduleId)
    {
        var module = await moduleService.DeleteModule(moduleId);
        return ResultRouter.GetActionResult(module);
    }
    
    [ServiceFilter(typeof(TelegramUserAuthFilter))]
    [HttpGet]
    [Route("all/{courseId:guid}")]
    public async Task<IActionResult> GetModules(Guid courseId)
    {
        var modules = await moduleService.GetModules(courseId);
        return ResultRouter.GetActionResult(modules);
    }
    
    [ServiceFilter(typeof(TelegramUserAuthFilter))]
    [HttpGet]
    [Route("{moduleId:guid}")]
    public async Task<IActionResult> GetModule(Guid moduleId)
    {
        var modules = await moduleService.GetModuleWithLessons(moduleId);
        return ResultRouter.GetActionResult(modules);
    }
}