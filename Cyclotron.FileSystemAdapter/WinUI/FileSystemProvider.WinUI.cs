using Microsoft.Extensions.DependencyInjection;

namespace Cyclotron.FileSystemAdapter;

public sealed partial class FileSystemProvider
{
    public static void Initialize(IServiceCollection services)
    {
        var serviceCollection = services ?? new ServiceCollection();

        serviceCollection.EnsureIfExists<IFileHandler>();
        serviceCollection.EnsureIfExists<IFolderHandler>();
        serviceCollection.EnsureIfExists<IFileOpenPicker>();
        serviceCollection.EnsureIfExists<IFileSavePicker>();
        serviceCollection.EnsureIfExists<IFolderPicker>();

        _ServiceProvider = serviceCollection.BuildServiceProvider();
    }
}
