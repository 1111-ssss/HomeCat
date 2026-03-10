using Application.Interfaces.Common;

namespace API.Extensions.Bootstrapper;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var handlerInterfaceType = typeof(IHandler<,>);
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        
        var handlerTypes = assemblies
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsClass && !type.IsAbstract)
            .SelectMany(type => type.GetInterfaces()
                .Where(i => i.IsGenericType
                    && i.GetGenericTypeDefinition() == handlerInterfaceType)
                .Select(i => new { Implementation = type, Interface = i }))
            .ToList();
        
        foreach (var handler in handlerTypes)
        {
            // services.AddScoped(handler.Interface, handler.Implementation);

            services.AddScoped(handler.Implementation);
        }

        return services;
    }
}