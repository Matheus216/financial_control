using financial_control.Infrastructure.Context;
using financial_control.Services;
using financial_control_application.Services;
using financial_control_domain.Interfaces.Repositories;
using financial_control_domain.Interfaces.Services;
using financial_control_infrastructure.Message;
using financial_control_infrastructure.Repositories;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace financial_control.Configuration;

public interface IApiConfiguration { }

public static class ApiConfiguration
{
    public static IServiceCollection ConfigureInitializer(
        this IServiceCollection services
    )
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddControllers();
        return services;
    }

    public static IServiceCollection ConfigureDI(
        this IServiceCollection services
    )
    {
        services.AddScoped<IPersonRepository, PersonRepository>();
        services.AddScoped<IPersonService, PersonService>();
        services.AddSingleton<IPublisherService, PublisherService>();
        return services;
    }

    public static WebApplicationBuilder ConfigureHostedService(
        this WebApplicationBuilder builder
    )
    {
        if (!builder.Environment.IsEnvironment("Testing"))
            builder.Services.AddHostedService<ConsumerService>();
        return builder;
    }

    public static WebApplication ConfigureMiddleware(
        this WebApplication app
    )
    {
        app.UseExceptionHandler(appBuilder =>
        {
            appBuilder.Run(async context =>
            {
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

        return app;
    }

    public static WebApplication ConfigureSwagger(
        this WebApplication app
    )
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        return app;
    }

    public static WebApplication ConfigureHttp(
        this WebApplication app
    )
    {
        app.MapControllers();
        app.UseHttpsRedirection();
        return app;
    }

    public static void ConfigureMigration(
        this IApplicationBuilder app
    )
    {
        using var scope = app.ApplicationServices.CreateScope();
        var service = scope.ServiceProvider.GetService<DbFinancialContext>();
        service?.Database.Migrate();
    }
}