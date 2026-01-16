namespace Cyclotron.FileSystemAdapter.Abstractions.Models;

/// <summary>
/// Defines the base contract for storage items (files and folders) in the file system.
/// </summary>
public interface IStorageItem
{
    /// <summary>
    /// Gets the name of the storage item.
    /// </summary>
    /// <value>The name of the file or folder.</value>
    string Name { get; }

    /// <summary>
    /// Gets the full path of the storage item.
    /// </summary>
    /// <value>The complete file system path.</value>
    string Path { get; }

    /// <summary>
    /// Gets the folder relative ID of the storage item.
    /// </summary>
    /// <remarks>
    /// This is a unique identifier relative to the containing folder, used for identification
    /// and comparison purposes across different API calls.
    /// </remarks>
    /// <value>A unique identifier relative to the containing folder.</value>
    string FolderRelativeId { get; }
}
