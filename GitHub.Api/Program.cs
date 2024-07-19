using GitHub.Api.Extensions;
using GitHub.Api.Middlewares;
using GitHub.Api.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.
    SwaggerDoc("v1", new OpenApiInfo { Title = "GitHubApi", Version = "v1" });
});

builder.Services.AddHttpConfig(); 

builder.Services.AddScoped<IGitHubService, GitHubService>();

builder.Services.AddTransient<HttpResilience>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "GitHub API v1");
    });
}

app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("swagger/index.html");
        return;
    }

    await next();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
