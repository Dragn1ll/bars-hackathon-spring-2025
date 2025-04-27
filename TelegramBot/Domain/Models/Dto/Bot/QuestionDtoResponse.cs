namespace Domain.Models.Dto.Bot;

public record QuestionDtoResponse(
    int QuestionId,
    string QuestionText,
    List<AnswerDtoResponse> Answers
    );