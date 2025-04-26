namespace Domain.Models.Dto;

public record LessonDto(
    string Title,
    string Description,
    List<string>? FileNames,
    TestDto? Test);