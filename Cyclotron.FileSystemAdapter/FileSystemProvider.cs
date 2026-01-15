using Microsoft.Extensions.DependencyInjection;

namespace Cyclotron.FileSystemAdapter;

public sealed partial class FileSystemProvider
{
    private static IServiceProvider _ServiceProvider;

    /// <summary>Singleton instance of this class</summary>
    public static FileSystemProvider Instance { get { return FileSystemProviderSingleton.Instance; } }

    private FileSystemProvider() { }

    public T GetService<T>()
    {
        return _ServiceProvider.GetService<T>();
    }

    #region FileSystemProvider Singleton class
    private class FileSystemProviderSingleton
    {
        //Marked as internal as it will be accessed from the enclosing class. It doesn't raise any problem, as the class itself is private.
        internal static readonly FileSystemProvider Instance = new FileSystemProvider();

        // Explicit static constructor
        static FileSystemProviderSingleton() { }
    }
    #endregion
}
