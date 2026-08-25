using API.Middleware;
using Application.Behaviors;
using Application.CommandHandlers.Users;
using Application.Queries.Users;
using Domain.AggregatesModel.ClinicalCaseAggregate.Repository;
using Domain.AggregatesModel.ExamAggregate.Repository;
using Domain.AggregatesModel.UserAggregate.Repository;
using FluentValidation;
using Infraestructure.Data;
using Infraestructure.Queries;
using Infraestructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(CreateUserCommandHandler).Assembly);
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(typeof(CreateUserCommandHandler).Assembly);

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IClinicalCaseRepository, ClinicalCaseRepository>();
builder.Services.AddScoped<IExamRepository, ExamRepository>();

builder.Services.AddScoped<IUserQueryService, UserQueryService>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDataContext>();
    dbContext.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
