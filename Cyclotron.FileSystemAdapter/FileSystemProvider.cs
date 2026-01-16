using Microsoft.Extensions.DependencyInjection;

namespace Cyclotron.FileSystemAdapter;

/// <summary>
/// Provides a singleton instance for accessing file system services through dependency injection.
/// </summary>
/// <remarks>
/// This class implements the singleton pattern and provides access to registered file and folder handlers,
/// as well as picker services.
/// </remarks>
public sealed partial class FileSystemProvider
{
    private static IServiceProvider _ServiceProvider = default!;

    /// <summary>
    /// Gets the singleton instance of the FileSystemProvider.
    /// </summary>
    /// <value>The singleton instance of FileSystemProvider.</value>
    public static FileSystemProvider Instance { get { return FileSystemProviderSingleton.Instance; } }

    /// <summary>
    /// Initializes a new instance of the FileSystemProvider class.
    /// </summary>
    /// <remarks>
    /// The constructor is private to enforce the singleton pattern.
    /// </remarks>
    private FileSystemProvider() { }

    /// <summary>
    /// Retrieves a service of the specified type from the service provider.
    /// </summary>
    /// <typeparam name="T">The type of the service to retrieve.</typeparam>
    /// <returns>An instance of the requested service type, or null if the service is not registered.</returns>
    public T GetService<T>()
    {
        return _ServiceProvider.GetService<T>();
    }

    #region FileSystemProvider Singleton class
    /// <summary>
    /// Provides the singleton instance of FileSystemProvider.
    /// </summary>
    private class FileSystemProviderSingleton
    {
        /// <summary>
        /// The singleton instance of FileSystemProvider.
        /// </summary>
        //Marked as internal as it will be accessed from the enclosing class. It doesn't raise any problem, as the class itself is private.
        internal static readonly FileSystemProvider Instance = new FileSystemProvider();

        /// <summary>
        /// Initializes the FileSystemProviderSingleton class.
        /// </summary>
        // Explicit static constructor
        static FileSystemProviderSingleton() { }
    }
    #endregion
}
