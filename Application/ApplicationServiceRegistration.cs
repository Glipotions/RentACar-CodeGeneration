using Core.Application.Rules;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddSubClassesOfType(Assembly.GetExecutingAssembly(), typeof(BaseBusinessRules));


        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        return services;
    }

    /// <summary>
    /// Assemblydeki belirtilen classtan türüyen tüm sınıfları scope olarak lifecycle a ekler.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="assembly"></param>
    /// <param name="type"></param>
    /// <param name="addWithLifeCycle"></param>
    /// <returns></returns>
    public static IServiceCollection AddSubClassesOfType(
   this IServiceCollection services,
   Assembly assembly,
   Type type,
   Func<IServiceCollection, Type, IServiceCollection>? addWithLifeCycle = null
)
    {
        var types = assembly.GetTypes().Where(t => t.IsSubclassOf(type) && type != t).ToList();
        foreach (var item in types)
            if (addWithLifeCycle == null)
                services.AddScoped(item);

            else
                addWithLifeCycle(services, type);
        return services;
    }
}
