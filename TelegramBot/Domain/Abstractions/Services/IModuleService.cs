using Domain.Models.Dto.Admin;
using Domain.Models.Dto.General;
using Domain.Utils;

namespace Domain.Abstractions.Services;

public interface IModuleService
{
    Task<Result<ModuleDto>> CreateModule(CreateModuleDto module);
    Task<Result<ModuleDto>> UpdateModule(ModuleDto module);
    Task<Result> DeleteModule(Guid moduleId);
    Task<Result<List<ModuleDto>>> GetModules(Guid courseId);
    Task<Result<ModuleDto>> GetModuleWithLessons(Guid moduleId);
}