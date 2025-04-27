namespace Domain.Models.Dto.Admin;

public record CreateQuestionOptionDto(
    string Answer, 
    bool IsCorrect);