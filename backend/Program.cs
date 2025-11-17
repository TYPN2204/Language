using LanguageApp.Api.Models;
using LanguageApp.Api.Options;
using LanguageApp.Api.Seed;
using LanguageApp.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});

builder.Services.AddDbContext<LanguageAppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("LanguageAppDb")
        ?? throw new InvalidOperationException("Connection string 'LanguageAppDb' is not configured.");

    options.UseSqlServer(connectionString);
});

builder.Services.AddHttpClient();
builder.Services.Configure<ChatbotOptions>(builder.Configuration.GetSection("ExternalServices:Chatbot"));
builder.Services.AddScoped<LessonKnowledgeService>();
builder.Services.AddScoped<ChatbotBridge>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.MapControllers();

await DataSeeder.SeedAsync(app.Services);

app.Run();
