using Domain.Entities;

namespace Domain.Abstractions.Services;

public interface IModuleService
{
    Task<ModuleEntity> CreateModule(ModuleEntity moduleEntity);
    Task<ModuleEntity> UpdateModule(ModuleEntity moduleEntity);
    Task<bool> DeleteModule(int moduleId);
    Task<List<ModuleEntity>> GetModules(int courseId);
}