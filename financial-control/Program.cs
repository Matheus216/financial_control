using financial_control_infrastructure.Connections;
using financial_control.Infrastructure.Context;
using financial_control_infrastructure.Message;
using Microsoft.AspNetCore.Diagnostics;
using financial_control.Services;
using financial_control_Infrastructure.Repositories;
using financial_control_domain.Interfaces.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.Services.AddDbContext<DbFinancialContext>();

builder.Services.AddScoped<PersonRepository>();
builder.Services.AddScoped<IPersonService, PersonService>();

builder.Services.AddSingleton<IPublisherService, PublisherService>();
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

app.InitializeMigration();

app.Run();


