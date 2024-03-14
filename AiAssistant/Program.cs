using AiAssistant.Shared.Clients.OpenAIClient;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
        $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
});

builder.Services.AddHttpClient();
builder.Services.AddTransient<OpenAIClient>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=CodeStyle}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=UnitTests}/{action=Index}/{id?}");

app.Run();
