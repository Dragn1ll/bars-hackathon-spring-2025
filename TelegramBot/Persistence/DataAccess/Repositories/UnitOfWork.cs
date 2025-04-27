using Domain.Abstractions.Repositories;

namespace Persistence.DataAccess.Repositories;

public class UnitOfWork(AppDbContext context,
    IUserRepository userRepository,
    ICourseRepository courseRepository,
    IModuleRepository moduleRepository,
    ILessonRepository lessonRepository,
    ILessonContentRepository lessonContentRepository,
    IQuizQuestionRepository quizQuestionRepository,
    IQuizOptionRepository quizOptionRepository): IUnitOfWork
{
    public IUserRepository Users { get; set; } = userRepository;
    public ICourseRepository Courses { get; set; } = courseRepository;
    public IModuleRepository Modules { get; set; } = moduleRepository;
    public ILessonRepository Lessons { get; set; } = lessonRepository;
    public ILessonContentRepository LessonContents { get; set; } = lessonContentRepository;
    public IQuizQuestionRepository QuizQuestions { get; set; } = quizQuestionRepository;
    public IQuizOptionRepository QuizOptions { get; set; } = quizOptionRepository;
    
    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        await context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await context.Database.CommitTransactionAsync(cancellationToken);
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        await context.Database.RollbackTransactionAsync(cancellationToken);
    }
}