using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.DataAccess.Configurations;

namespace Persistence.DataAccess;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new CourseConfiguration());
        modelBuilder.ApplyConfiguration(new ModuleConfiguration());
        modelBuilder.ApplyConfiguration(new LessonConfiguration());
        modelBuilder.ApplyConfiguration(new LessonContentConfiguration());
        modelBuilder.ApplyConfiguration(new QuizQuestionConfiguration());
        modelBuilder.ApplyConfiguration(new QuizOptionConfiguration());
        modelBuilder.ApplyConfiguration(new UserAnsweredQuestionConfiguration());
    }
}