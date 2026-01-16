namespace Cyclotron.FileSystemAdapter.Abstractions.Models;

/// <summary>
/// Defines the contract for file operations in the file system.
/// </summary>
/// <remarks>
/// Extends <see cref="IStorageItem"/> to provide file-specific functionality.
/// </remarks>
public interface IFile : IStorageItem
{
    /// <summary>
    /// Asynchronously retrieves the size of the file in bytes.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The result contains the file size in bytes.</returns>
    Task<ulong> GetSizeAsync();
}
