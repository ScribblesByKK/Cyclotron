using Cyclotron.FileSystemAdapter;
using Microsoft.Extensions.DependencyInjection;

namespace Cyclotron.Extensions.DependencyInjection;

/// <summary>
/// WinUI-specific extension methods for <see cref="IServiceCollection"/> used in dependency injection setup.
/// </summary>
public static partial class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the Cyclotron FileSystemAdapter WinUI-backed file and folder handlers, and pickers, to the DI container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddCyclotronFileSystemAdapter(this IServiceCollection services)
    {
        FileSystemProvider.Initialize(services);
        return services;
    }
}
