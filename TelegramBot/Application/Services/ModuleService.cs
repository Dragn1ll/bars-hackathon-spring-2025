using System.Linq.Expressions;
using Domain.Abstractions.Repositories;
using Domain.Abstractions.Services;
using Domain.Entities;
using Domain.Models.Dto.Admin;
using Domain.Models.Dto.General;
using Domain.Models.Enums;
using Domain.Utils;

namespace Application.Services;

public class ModuleService(IUnitOfWork unitOfWork, Mapper mapper) : IModuleService
{
    public async Task<Result<ModuleDto>> CreateModule(CreateModuleDto module)
    {
        try
        {
            if (await ThereIsAModule(m => m.Title == module.Title && m.CourseId == module.CourseId))
                return Result<ModuleDto>.Failure(
                    new Error(ErrorType.BadRequest, "Module already exists"));

            var entity = mapper.Map<CreateModuleDto, ModuleEntity>(module);
            entity.ModuleId = Guid.NewGuid();
            
            var result = await unitOfWork.Modules
                .AddAsync(entity);
            await unitOfWork.SaveChangesAsync();
            
            return result 
                ? Result<ModuleDto>.Success(mapper.Map<ModuleEntity, ModuleDto>(entity)) 
                : Result<ModuleDto>.Failure(new Error(ErrorType.ServerError, "Can't create module")); 
        }
        catch (Exception exception)
        {
            return Result<ModuleDto>.Failure(new Error(ErrorType.ServerError, exception.Message));
        }
    }

    public async Task<Result<ModuleDto>> UpdateModule(ModuleDto module)
    {
        try
        {
            if (!await ThereIsAModule(m => m.ModuleId == module.ModuleId))
                return Result<ModuleDto>.Failure(
                    new Error(ErrorType.NotFound, "Module already not exists"));

            var result = await unitOfWork.Modules
                .PatchAsync(module.ModuleId, m => m.Title = module.Title);
            await unitOfWork.SaveChangesAsync();
            
            return result
                ? Result<ModuleDto>.Success(module) 
                : Result<ModuleDto>.Failure(new Error(ErrorType.ServerError, "Can't update module")); 
        }
        catch (Exception exception)
        {
            return Result<ModuleDto>.Failure(new Error(ErrorType.ServerError, exception.Message));
        }
    }

    public async Task<Result> DeleteModule(Guid moduleId)
    {
        try
        {
            if (!await ThereIsAModule(m => m.ModuleId == moduleId))
                return Result<ModuleDto>.Failure(
                    new Error(ErrorType.NotFound, "Module already not exists"));

            var result = await unitOfWork.Modules.DeleteAsync(m => m.ModuleId == moduleId);
            await unitOfWork.SaveChangesAsync();
            
            return result
                ? Result.Success()
                : Result.Failure(new Error(ErrorType.ServerError, "Can't delete module"));
        }
        catch (Exception exception)
        {
            return Result<ModuleDto>.Failure(new Error(ErrorType.ServerError, exception.Message));
        }
    }

    public async Task<Result<List<ModuleDto>>> GetModules(Guid courseId)
    {
        try
        {
            return Result<List<ModuleDto>>.Success((await unitOfWork.Modules
                    .GetAllByFilterAsync(m => m.CourseId == courseId))
                .Select(m => mapper.Map<ModuleEntity, ModuleDto>(m!))
                .ToList());
        }
        catch (Exception exception)
        {
            return Result<List<ModuleDto>>.Failure(new Error(ErrorType.ServerError, exception.Message));
        }
    }

    public async Task<Result<ModuleDto>> GetModuleWithLessons(Guid moduleId)
    {
        try
        {
            if (!await ThereIsAModule(m => m.ModuleId == moduleId))
                return Result<ModuleDto>.Failure(
                    new Error(ErrorType.NotFound, "Module already not exists"));

            return Result<ModuleDto>.Success(
                mapper.Map<ModuleEntity, ModuleDto>(await unitOfWork.Modules
                    .GetModuleWithLessons(moduleId) ?? new ModuleEntity()));
        }
        catch
        {
            return Result<ModuleDto>.Failure(new Error(ErrorType.ServerError, "Can't get module"));
        }
    }

    private async Task<bool> ThereIsAModule(Expression<Func<ModuleEntity, bool>> predicate)
    {
        return await unitOfWork.Modules.GetByFilterAsync(predicate) != null;
    }
}