using Domain.Abstractions.Repositories;
using Domain.Entities;

namespace Persistence.DataAccess.Repositories;

public class ModuleRepository(AppDbContext context) : AbstractRepository<ModuleEntity>(context), 
    IModuleRepository
{
    public async Task<IEnumerable<ModuleEntity?>> GetAllModulesAsync()
    {
        return await GetAllByFilterAsync(e => e.IsDeleted == false);
    }

    public async Task<IEnumerable<ModuleEntity?>> GetAllDeletedModulesAsync()
    {
        return await GetAllByFilterAsync(e => e.IsDeleted == true);
    }

    public async Task<IEnumerable<ModuleEntity?>> GetModulesByCourseIdAsync(int courseId)
    {
        return await GetAllByFilterAsync(e => e.CourseId == courseId);
    }

    public async Task<IEnumerable<ModuleEntity?>> GetModulesTitleAsync(string title)
    {
        return await GetAllByFilterAsync(e => e.Title
            .Contains(title, StringComparison.InvariantCultureIgnoreCase));
    }

    public async Task<ModuleEntity?> GetModuleByIdAsync(int moduleId)
    {
        return await GetByFilterAsync(e => e.Id == moduleId);
    }

    public async Task<bool> PatchTitleAsync(int moduleId, string newTitle)
    {
        return await PatchAsync(moduleId, e => e.Title = newTitle);
    }

    public async Task<bool> PatchDeleteStatusAsync(int moduleId)
    {
        return await PatchAsync(moduleId, e => e.IsDeleted = !e.IsDeleted);
    }

    public async Task<bool> DeleteModuleAsync(int moduleId)
    {
        return await DeleteAsync(e => e.Id == moduleId);
    }
}