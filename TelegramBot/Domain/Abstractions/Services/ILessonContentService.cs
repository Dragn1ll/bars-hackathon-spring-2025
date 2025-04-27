using Domain.Models.Dto.Admin;
using Domain.Utils;

namespace Domain.Abstractions.Services;

public interface ILessonContentService
{
    Task<Result<AdminLessonContentResponseDto>> AddLessonContent(CreateLessonContentDto lessonContent);
    Task<Result> RemoveLessonContent(int lessonContentId);
    Task<Result<List<AdminLessonContentResponseDto>>> GetAllLessonContents(int lessonId);
}