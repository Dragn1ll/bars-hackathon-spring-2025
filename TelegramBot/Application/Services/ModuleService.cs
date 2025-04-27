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
            
            return await unitOfWork.Modules.AddAsync(mapper.Map<CreateModuleDto, ModuleEntity>(module))
                ? Result<ModuleDto>.Success(mapper.Map<CreateModuleDto, ModuleDto>(module)) 
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
            
            return await unitOfWork.Modules.PatchAsync(module.ModuleId, m => m.Title = module.Title)
                ? Result<ModuleDto>.Success(module) 
                : Result<ModuleDto>.Failure(new Error(ErrorType.ServerError, "Can't update module")); 
        }
        catch (Exception exception)
        {
            return Result<ModuleDto>.Failure(new Error(ErrorType.ServerError, exception.Message));
        }
    }

    public async Task<Result> DeleteModule(int moduleId)
    {
        try
        {
            if (!await ThereIsAModule(m => m.ModuleId == moduleId))
                return Result<ModuleDto>.Failure(
                    new Error(ErrorType.NotFound, "Module already not exists"));
            
            return await unitOfWork.Modules.DeleteAsync(m => m.ModuleId == moduleId)
                ? Result.Success()
                : Result.Failure(new Error(ErrorType.ServerError, "Can't delete module"));
        }
        catch (Exception exception)
        {
            return Result<ModuleDto>.Failure(new Error(ErrorType.ServerError, exception.Message));
        }
    }

    public async Task<Result<List<ModuleDto>>> GetModules(int courseId)
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

    private async Task<bool> ThereIsAModule(Expression<Func<ModuleEntity, bool>> predicate)
    {
        return await unitOfWork.Modules.GetByFilterAsync(predicate) != null;
    }
}