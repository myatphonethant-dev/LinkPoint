var builder = WebApplication.CreateBuilder(args);

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure YARP
builder.Services.AddReverseProxy()
                .LoadFromConfig(builder.Configuration.GetSection("Yarp"));

var app = builder.Build();

// Enable Swagger in Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "LinkPoint API Gateway");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

// Map reverse proxy
app.MapReverseProxy();

app.Run();