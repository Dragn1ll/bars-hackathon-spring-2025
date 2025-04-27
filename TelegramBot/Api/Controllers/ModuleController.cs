using Domain.Abstractions.Services;
using Domain.Models.Dto.Admin;
using Domain.Models.Dto.General;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("/modules")]
public class ModuleController(IModuleService moduleService): ControllerBase
{
    [HttpPost]
    [Route("/add")]
    public async Task<IActionResult> CreateModule(CreateModuleDto moduleDto)
    {
        var module = await moduleService.CreateModule(moduleDto);
        return ResultRouter.GetActionResult(module);
    }

    [HttpPatch]
    [Route("/update")]
    public async Task<IActionResult> UpdateModule(ModuleDto moduleDto)
    {
        var module = await moduleService.UpdateModule(moduleDto);
        return ResultRouter.GetActionResult(module);
    }

    [HttpDelete]
    [Route("/delete")]
    public async Task<IActionResult> DeleteModule(int moduleId)
    {
        var module = await moduleService.DeleteModule(moduleId);
        return ResultRouter.GetActionResult(module);
    }
    
    [HttpGet]
    [Route("/all/{courseId:int}")]
    public async Task<IActionResult> GetModules(int courseId)
    {
        var modules = await moduleService.GetModules(courseId);
        return ResultRouter.GetActionResult(modules);
    }
}