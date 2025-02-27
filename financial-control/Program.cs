using financial_control.Infraestructure.Context;
using financial_control.Infraestructure.Repository;
using financial_control.Services;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.Services.AddDbContext<DbFinancialContext>();

builder.Services.AddScoped<PersonRepository>();
builder.Services.AddScoped<PersonService>();

var app = builder.Build();

// apply automaticlly logger information in case of exception
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


