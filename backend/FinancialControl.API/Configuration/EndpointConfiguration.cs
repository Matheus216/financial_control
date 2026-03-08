using Microsoft.Extensions.DependencyInjection.Extensions;
using FinancialControl.API.Endpoints;

namespace FinancialControl.API.Configuration;

public static class EndpointConfiguration
{
    public static IServiceCollection ConfigureEndpoint(this IServiceCollection services)
    {
        var assembly = typeof(IEndpointBase).Assembly;

        var servicesDescriptors = assembly.DefinedTypes
            .Where(x => x.IsAssignableTo(typeof(IEndpointBase)) && x is { IsAbstract: false, IsInterface: false })
            .Select(x => ServiceDescriptor.Transient(typeof(IEndpointBase), x))
            .ToArray(); 

        services.TryAddEnumerable(servicesDescriptors);

        return services; 
    }

    public static WebApplication MapVersion1Api(this WebApplication app)
    {
        var endpoints = app.Services.GetServices<IEndpointBase>();

        RouteGroupBuilder versionedGroup = app
            .MapGroup("api/v1");

        foreach (var service in endpoints)
        {
            service.Map(versionedGroup); 
        }

        return app; 
    }
}
