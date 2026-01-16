namespace Cyclotron.FileSystemAdapter.Abstractions.Handlers;

/// <summary>
/// Defines the contract for folder operations such as creating files, creating folders, and managing folder contents.
/// </summary>
public interface IFolderHandler
{
    /// <summary>
    /// Asynchronously creates a file in the specified folder with a desired name.
    /// </summary>
    /// <param name="folder">The folder in which to create the file.</param>
    /// <param name="desiredName">The desired name for the new file.</param>
    Task CreateFileAsync(IFolder folder, string desiredName);

    /// <summary>
    /// Asynchronously creates a file in the specified folder with a desired name and collision option.
    /// </summary>
    /// <param name="folder">The folder in which to create the file.</param>
    /// <param name="desiredName">The desired name for the new file.</param>
    /// <param name="options">The collision option to apply if a file with the same name already exists.</param>
    Task CreateFileAsync(IFolder folder, string desiredName, CreationCollisionOption options);

    /// <summary>
    /// Asynchronously creates a subfolder in the specified folder with a desired name.
    /// </summary>
    /// <param name="folder">The parent folder in which to create the subfolder.</param>
    /// <param name="desiredName">The desired name for the new subfolder.</param>
    Task CreateFolderAsync(IFolder folder, string desiredName);

    /// <summary>
    /// Asynchronously creates a subfolder in the specified folder with a desired name and collision option.
    /// </summary>
    /// <param name="folder">The parent folder in which to create the subfolder.</param>
    /// <param name="desiredName">The desired name for the new subfolder.</param>
    /// <param name="options">The collision option to apply if a folder with the same name already exists.</param>
    Task CreateFolderAsync(IFolder folder, string desiredName, CreationCollisionOption options);

    /// <summary>
    /// Asynchronously deletes a folder.
    /// </summary>
    /// <param name="folder">The folder to delete.</param>
    Task DeleteAsync(IFolder folder);

    /// <summary>
    /// Asynchronously deletes a folder with a deletion option.
    /// </summary>
    /// <param name="folder">The folder to delete.</param>
    /// <param name="option">The deletion option (move to recycle bin or permanent delete).</param>
    Task DeleteAsync(IFolder folder, StorageDeletionOption option);

    /// <summary>
    /// Asynchronously retrieves a file from the specified folder by name.
    /// </summary>
    /// <param name="folder">The folder to search.</param>
    /// <param name="name">The name of the file to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The result contains the file.</returns>
    Task<IFile> GetFileAsync(IFolder folder, string name);

    /// <summary>
    /// Asynchronously retrieves all files from the specified folder.
    /// </summary>
    /// <param name="folder">The folder to search.</param>
    /// <returns>A task that represents the asynchronous operation. The result contains a read-only list of files.</returns>
    Task<IReadOnlyList<IFile>> GetFileAsync(IFolder folder);

    /// <summary>
    /// Asynchronously retrieves a subfolder from the specified folder by name.
    /// </summary>
    /// <param name="folder">The parent folder to search.</param>
    /// <param name="name">The name of the subfolder to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The result contains the subfolder.</returns>
    Task<IFolder> GetFolderAsync(IFolder folder, string name);

    /// <summary>
    /// Asynchronously retrieves a folder from the specified path.
    /// </summary>
    /// <param name="path">The folder path.</param>
    /// <returns>A task that represents the asynchronous operation. The result contains the folder.</returns>
    Task<IFolder> GetFolderFromPathAsync(string path);

    /// <summary>
    /// Asynchronously retrieves all subfolders from the specified folder.
    /// </summary>
    /// <param name="folder">The parent folder to search.</param>
    /// <returns>A task that represents the asynchronous operation. The result contains a read-only list of subfolders.</returns>
    Task<IReadOnlyList<IFolder>> GetFoldersAsync(IFolder folder);

    /// <summary>
    /// Asynchronously retrieves a storage item (file or folder) from the specified folder by name.
    /// </summary>
    /// <param name="folder">The folder to search.</param>
    /// <param name="name">The name of the item to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The result contains the storage item.</returns>
    Task<IStorageItem> GetItemAsync(IFolder folder, string name);

    /// <summary>
    /// Asynchronously retrieves all storage items (files and folders) from the specified folder.
    /// </summary>
    /// <param name="folder">The folder to search.</param>
    /// <returns>A task that represents the asynchronous operation. The result contains a read-only list of storage items.</returns>
    Task<IReadOnlyList<IStorageItem>> GetItemsAsync(IFolder folder);

    /// <summary>
    /// Asynchronously retrieves the parent folder of a folder.
    /// </summary>
    /// <param name="folder">The folder whose parent folder is to be retrieved.</param>
    /// <returns>A task that represents the asynchronous operation. The result contains the parent folder.</returns>
    Task<IFolder> GetParentAsync(IFolder folder);

    /// <summary>
    /// Asynchronously renames a folder with a new name.
    /// </summary>
    /// <param name="folder">The folder to rename.</param>
    /// <param name="desiredName">The new name for the folder.</param>
    Task RenameAsync(IFolder folder, string desiredName);

    /// <summary>
    /// Asynchronously renames a folder with a new name and a collision option.
    /// </summary>
    /// <param name="folder">The folder to rename.</param>
    /// <param name="desiredName">The new name for the folder.</param>
    /// <param name="option">The collision option to apply if a folder with the same name already exists.</param>
    Task RenameAsync(IFolder folder, string desiredName, NameCollisionOption option);

    /// <summary>
    /// Asynchronously attempts to retrieve a storage item from the specified folder by name.
    /// </summary>
    /// <remarks>
    /// This method differs from <see cref="GetItemAsync(IFolder, string)"/> in that it returns null
    /// if the item is not found instead of throwing an exception.
    /// </remarks>
    /// <param name="folder">The folder to search.</param>
    /// <param name="name">The name of the item to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The result contains the storage item if found, or null if not found.</returns>
    Task<IStorageItem> TryGetItemAsync(IFolder folder, string name);
}
