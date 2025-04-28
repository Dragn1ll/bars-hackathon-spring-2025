namespace Domain.Models.Dto.Admin;

public record AdminLessonContentResponseDto(
    Guid LessonContentId,
    string FileName
    );