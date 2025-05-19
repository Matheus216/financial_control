using financial_control.Infrastructure.Context;
using financial_control_infrastructure.Message;
using Microsoft.AspNetCore.Diagnostics;
using financial_control.Services;
using financial_control_Infrastructure.Repositories;
using financial_control_domain.Interfaces.Services;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.Services.AddDbContext<DbFinancialContext>();

builder.Services.AddScoped<PersonRepository>();
builder.Services.AddScoped<IPersonService, PersonService>();

string connectionStringRabbit = string.Empty, queue = string.Empty;

if (!builder.Environment.IsEnvironment("Testing"))
{
    connectionStringRabbit = builder.Configuration["RABBITMQ:ConnectionString"] ?? throw new ArgumentException("RABBITMQ:ConnectionString");
    queue = builder.Configuration["RABBITMQ:QUEUE"] ?? throw new ArgumentException("RABBITMQ:QUEUE");
}

builder.Services.AddSingleton<IPublisherService, PublisherService>();
builder.Services.AddSingleton<ConfigurationConnection>(new ConfigurationConnection(
    connectionStringRabbit,
    queue
    ));

builder.Services.AddSingleton<IConnectionFactory>(new ConnectionFactory
{
    Uri = new Uri(connectionStringRabbit)
});

builder.Services.AddHostedService<ConsumerService>();

var app = builder.Build();

app.UseExceptionHandler(appBuilder => {
    appBuilder.Run(async context => {
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        var exception = context.Features.Get<IExceptionHandlerFeature>();

        if (exception != null)
        {
            logger.LogError(exception.Error, "An error occurred.");
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("An error occurred.");
        }
    });
});

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.UseHttpsRedirection();

if (!builder.Environment.IsEnvironment("Testing"))
    app.InitializeMigration();

app.Run();

