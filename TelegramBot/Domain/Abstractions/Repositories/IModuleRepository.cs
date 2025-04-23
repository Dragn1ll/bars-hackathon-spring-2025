using Domain.Entities;

namespace Domain.Abstractions.Repositories;

public interface IModuleRepository : IRepository<ModuleEntity>
{
    Task<IEnumerable<ModuleEntity?>> GetAllModulesAsync();
    Task<IEnumerable<ModuleEntity?>> GetAllDeletedModulesAsync();
    Task<IEnumerable<ModuleEntity?>> GetModulesByCourseIdAsync(int courseId);
    Task<IEnumerable<ModuleEntity?>> GetModulesTitleAsync(string title);
    Task<ModuleEntity?> GetModuleByIdAsync(int moduleId);
    Task<bool> PatchTitleAsync(int moduleId, string newTitle);
    Task<bool> PatchDeleteStatusAsync(int moduleId);
    Task<bool> DeleteModuleAsync(int moduleId);
}