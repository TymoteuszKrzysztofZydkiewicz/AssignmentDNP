using FileRepositories;
using RepositoryContracts;
using EfcRepository;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using AppContext = EfcRepository.AppContext;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppContext>();


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IPostRepository, EfcPostRepository>();
builder.Services.AddScoped<IUserRepository, EfcUserRepository>();
builder.Services.AddScoped<ICommentRepository, EfcCommentRepository>();


var app = builder.Build();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();