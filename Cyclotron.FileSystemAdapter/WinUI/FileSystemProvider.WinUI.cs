using Cyclotron.FileSystemAdapter.Abstractions.Pickers;
using Cyclotron.FileSystemAdapter.WinUI.Handlers;
using Cyclotron.FileSystemAdapter.WinUI.Pickers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Cyclotron.FileSystemAdapter;

/// <summary>
/// FileSystemProvider partial class that contains WinUI-specific initialization logic.
/// </summary>
public sealed partial class FileSystemProvider
{
    /// <summary>
    /// Initializes the FileSystemProvider with the required WinUI file system services.
    /// </summary>
    /// <remarks>
    /// This method validates that all required services are registered in the service collection,
    /// including file handlers, folder handlers, and file/folder pickers.
    /// It then builds the service provider used internally for service resolution.
    /// </remarks>
    /// <param name="services">The service collection to initialize. If null, a new ServiceCollection is created.</param>
    /// <exception cref="InvalidOperationException">Thrown if any required service type is not found in the service collection.</exception>
    public static void Initialize(IServiceCollection? services = default)
    {
        var serviceCollection = services ?? new ServiceCollection();
        serviceCollection.TryAddSingleton<IFileHandler,WinUIFileHandler>();
        serviceCollection.TryAddSingleton<IFolderHandler, WinUIFolderHandler>();
        serviceCollection.TryAddTransient<IFileOpenPicker, WinUIFileOpenPicker>();
        serviceCollection.TryAddTransient<IFileSavePicker, WinUIFileSavePicker>();
        serviceCollection.TryAddTransient<IFolderPicker, WinUIFolderPicker>();

        serviceCollection.EnsureIfExists<IFileHandler>();
        serviceCollection.EnsureIfExists<IFolderHandler>();
        serviceCollection.EnsureIfExists<IFileOpenPicker>();
        serviceCollection.EnsureIfExists<IFileSavePicker>();
        serviceCollection.EnsureIfExists<IFolderPicker>();

        _initServiceProvider = serviceCollection.BuildServiceProvider();
    }
}
