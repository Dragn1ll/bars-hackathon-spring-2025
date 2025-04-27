using Domain.Entities;
using Domain.Utils;

namespace Domain.Abstractions.Services;

public interface IModuleService
{
    Task<Result<ModuleEntity>> CreateModule(ModuleEntity moduleEntity);
    Task<Result<ModuleEntity>> UpdateModule(ModuleEntity moduleEntity);
    Task<Result> DeleteModule(int moduleId);
    Task<Result<List<ModuleEntity>>> GetModules(int courseId);
}