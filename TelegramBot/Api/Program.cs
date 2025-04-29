using Api.Extensions;
using Api.Filters;
using Application.Services;
using Domain.Abstractions.Repositories;
using Domain.Abstractions.Services;
using Infrastructure.Auth;
using Infrastructure.Storage;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using Persistence.DataAccess;
using Persistence.DataAccess.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddEnvironmentVariables();

var services = builder.Services;

services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
services.Configure<MinioOptions>(builder.Configuration.GetSection("Minio"));
services.AddApiAuthentication(builder.Configuration);
services.AddDbContext<AppDbContext>(options => 
{
    options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(AppDbContext)));
});

services.AddControllers();
services.AddLogging();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddScoped<IUserService, UserService>();
services.AddScoped<ICourseService, CourseService>();
services.AddScoped<IModuleService, ModuleService>();
services.AddScoped<ILessonService, LessonService>();
services.AddScoped<IFileStorageService, MinioService>();
services.AddScoped<ILessonContentService, LessonContentService>();
services.AddScoped<IQuizQuestionService, QuizQuestionService>();
services.AddScoped<IQuizOptionService, QuizOptionService>();
services.AddScoped<IAdminService, AdminService>();

services.AddTransient<Mapper>();

services.AddScoped<IUnitOfWork, UnitOfWork>();
services.AddScoped<ICourseRepository, CourseRepository>();
services.AddScoped<ILessonContentRepository, LessonContentRepository>();
services.AddScoped<ILessonRepository, LessonRepository>();
services.AddScoped<IModuleRepository, ModuleRepository>();
services.AddScoped<IQuizOptionRepository, QuizOptionRepository>();
services.AddScoped<IQuizQuestionRepository, QuizQuestionRepository>();
services.AddScoped<IUserAnsweredQuestionRepository, UserAnsweredQuestionRepository>();
services.AddScoped<IUserCompletedLessonRepository, UserCompletedLessonRepository>();
services.AddScoped<IUserRepository, UserRepository>();
services.AddScoped<IAdminRepository, AdminRepository>();

services.AddScoped<IJwtWorker, JwtWorker>();

services.AddScoped<TelegramUserAuthFilter>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    Secure = CookieSecurePolicy.Always,
    HttpOnly = HttpOnlyPolicy.Always
});

using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider;
    var logger = service.GetRequiredService<ILogger<Program>>();
    try
    {
        var context = service.GetRequiredService<AppDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        logger.LogCritical(ex, "Failed to apply migrations!");
        throw;
    }
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();