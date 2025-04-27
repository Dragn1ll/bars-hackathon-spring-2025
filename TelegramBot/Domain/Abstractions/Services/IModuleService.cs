using Domain.Models.Dto.Admin;
using Domain.Models.Dto.General;
using Domain.Utils;

namespace Domain.Abstractions.Services;

public interface IModuleService
{
    Task<Result<ModuleDto>> CreateModule(CreateModuleDto module);
    Task<Result<ModuleDto>> UpdateModule(ModuleDto module);
    Task<Result> DeleteModule(int moduleId);
    Task<Result<List<ModuleDto>>> GetModules(int courseId);
}