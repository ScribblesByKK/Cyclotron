using Microsoft.Extensions.DependencyInjection;

namespace Cyclotron.FileSystemAdapter;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Ensures that a service of type <typeparamref name="TService"/> exists in the service collection.
    /// Throws an InvalidOperationException if the service is not found.
    /// </summary>
    /// <typeparam name="TService">The type of the service to check for.</typeparam>
    /// <param name="services">The service collection.</param>
    public static void EnsureIfExists<TService>(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        if (!services.Any(descriptor => descriptor.ServiceType == typeof(TService)))
        {
            throw new InvalidOperationException($"Required service of type '{typeof(TService).FullName}' was not found in the service collection.");
        }
    }
}

