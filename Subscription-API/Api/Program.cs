using Infrastructure.Messaging.Interfaces;
using Infrastructure.Messaging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer();

builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();

// Add Swagger configurations
SwaggerOptionsSetup.AddSwaggerConfiguration(builder.Services);

// Add Dependency Injection configurations
DependencyInjectionOptionsSetup.AddDependencyInjectionConfiguration(builder.Services);

// Add Database configuration
DatabaseOptionsSetup.AddDatabaseConfiguration(builder.Services, builder.Configuration);

var rabbitMQConfig = builder.Configuration.GetSection("RabbitMQ").Get<RabbitMQOptions>();

builder.Services.AddSingleton<IConnectionProvider>(new ConnectionProvider(rabbitMQConfig));

// Add ApplicationRegistration
builder.Services.AddApplication();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

// Configure CORS
app.UseCors(c =>
{
    c.AllowAnyHeader();
    c.AllowAnyMethod();
    c.AllowAnyOrigin();
});

app.UseAuthentication();

app.UseAuthorization();

app.UseSwaggerSetup();

app.MapControllers();

app.Run();
