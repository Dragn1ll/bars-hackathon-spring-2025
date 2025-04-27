using Domain.Entities;

namespace Domain.Abstractions.Repositories;

public interface IModuleRepository : IRepository<ModuleEntity>
{
    Task<ModuleEntity?> GetModuleWithLessons(Guid moduleId); 
}