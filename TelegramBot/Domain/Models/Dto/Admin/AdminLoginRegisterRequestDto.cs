namespace Domain.Models.Dto.Admin;

public record AdminLoginRegisterRequestDto(
    string Username,
    string PasswordHash);