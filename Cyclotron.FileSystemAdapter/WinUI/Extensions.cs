using Cyclotron.FileSystemAdapter.WinUI.Models;

namespace Cyclotron.FileSystemAdapter.WinUI;

/// <summary>
/// Provides extension methods for <see cref="Windows.Storage.StorageFile"/> operations in WinUI.
/// </summary>
public static class FileExtensions
{
    /// <summary>
    /// Converts a <see cref="Windows.Storage.StorageFile"/> to an <see cref="IFile"/> instance.
    /// </summary>
    /// <param name="storageFile">The <see cref="Windows.Storage.StorageFile"/> to convert.</param>
    /// <returns>An <see cref="IFile"/> instance wrapping the storage file.</returns>
    public static IFile AsIFile(this Windows.Storage.StorageFile storageFile)
    {
        return new WinUIFile(storageFile);
    }
}

/// <summary>
/// Provides extension methods for <see cref="Windows.Storage.StorageFolder"/> operations.
/// </summary>
public static class FolderExtensions
{
    /// <summary>
    /// Converts a <see cref="Windows.Storage.StorageFolder"/> to an <see cref="IFolder"/> instance.
    /// </summary>
    /// <param name="storageFolder">The <see cref="Windows.Storage.StorageFolder"/> to convert.</param>
    /// <returns>An <see cref="IFolder"/> instance wrapping the storage folder.</returns>
    public static IFolder AsIFolder(this Windows.Storage.StorageFolder storageFolder)
    {
        return new WinUIFolder(storageFolder);
    }
}