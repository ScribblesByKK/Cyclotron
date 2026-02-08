using Microsoft.Extensions.DependencyInjection;

namespace Cyclotron.Extensions.DepepndencyInjection;

/// <summary>
/// Extension methods for <see cref="IServiceCollection"/> used in dependency injection setup.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Ensures that a service of type <typeparamref name="TService"/> exists in the service collection.
    /// Throws an InvalidOperationException if the service is not found.
    /// </summary>
    /// <typeparam name="TService">The type of the service to check for.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <exception cref="InvalidOperationException">Thrown if the required service type is not found in the collection.</exception>
    public static void EnsureIfExists<TService>(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        if (!services.Any(descriptor => descriptor.ServiceType == typeof(TService)))
        {
            throw new InvalidOperationException($"Required service of type '{typeof(TService).FullName}' was not found in the service collection.");
        }
    }
}

