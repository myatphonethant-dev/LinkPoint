using LinkPoint.Infrastructure.Data;
using LinkPoint.PartnerService.Data;
using LinkPoint.PartnerService.Endpoints;
using LinkPoint.PartnerService.Services;
using LinkPoint.SharedKernel.Settings;

var builder = WebApplication.CreateBuilder(args);

// Load MongoDB settings
MongoDbSettings mongoSettings = builder.Configuration
    .GetSection("MongoDbSettings")
    .Get<MongoDbSettings>()!;

builder.Services.AddSingleton(mongoSettings);
builder.Services.AddSingleton<MongoDbContext>();
builder.Services.AddScoped<IPartnerService, PartnerService>();

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Partner Service",
        Version = "v1",
        Description = "API documentation for Partner Service"
    });
});

var app = builder.Build();

// Enable Swagger in Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Partner Service V1");
        c.DocumentTitle = "Partner Swagger";
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

// Map minimal API endpoints
app.MapPartnerEndpoints();

app.Run();