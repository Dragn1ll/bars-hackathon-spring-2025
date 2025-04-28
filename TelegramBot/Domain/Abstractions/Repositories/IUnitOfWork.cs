namespace Domain.Abstractions.Repositories;

public interface IUnitOfWork
{
    public IUserRepository Users { get; set; }
    public ICourseRepository Courses { get; set; }
    public IModuleRepository Modules { get; set; }
    public ILessonRepository Lessons { get; set; }
    public ILessonContentRepository LessonContents { get; set; }
    public IQuizQuestionRepository QuizQuestions { get; set; }
    public IQuizOptionRepository QuizOptions { get; set; }
    public IUserAnsweredQuestionRepository AnsweredQuestionsRepository { get; set; }
    public IAdminRepository Admins { get; set; }
    public Task SaveChangesAsync();
    public Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    public Task CommitAsync(CancellationToken cancellationToken = default);
    public Task RollbackAsync(CancellationToken cancellationToken = default);
}