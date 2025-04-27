namespace Domain.Models.Dto.Bot;

public record LessonDto(
    string Title,
    string Description,
    List<string>? FileNames,
    TestDto? Test);