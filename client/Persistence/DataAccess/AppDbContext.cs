using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.DataAccess.Configurations;

namespace Persistence.DataAccess;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<CourseEntity> Courses { get; set; }
    public DbSet<ModuleEntity> Modules { get; set; }
    public DbSet<LessonEntity> Lessons { get; set; }
    public DbSet<LessonContentEntity> LessonContents { get; set; }
    public DbSet<QuizQuestionEntity> QuizQuestions { get; set; }
    public DbSet<QuizOptionEntity> QuizOptions { get; set; }
    
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
    }
}