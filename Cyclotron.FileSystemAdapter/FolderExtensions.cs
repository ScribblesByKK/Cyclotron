using Microsoft.VisualBasic;

namespace Cyclotron.FileSystemAdapter;

/// <summary>
/// Provides extension methods for <see cref="IFolder"/> operations.
/// </summary>
public static class FolderExtensions
{
    /// <summary>
    /// A static instance of the <see cref="IFolderHandler"/> used for all folder operations.
    /// </summary>
    private static readonly IFolderHandler _folderHandler = FileSystemProvider.Instance.GetService<IFolderHandler>();

    /// <summary>
    /// Asynchronously creates a file in the folder.
    /// </summary>
    /// <param name="folder">The folder in which to create the file.</param>
    /// <param name="desiredName">The desired name for the new file.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static Task CreateFileAsync(this IFolder folder, string desiredName)
    {
        return _folderHandler.CreateFileAsync(folder, desiredName);
    }

    /// <summary>
    /// Asynchronously creates a file in the folder with a collision option.
    /// </summary>
    /// <param name="folder">The folder in which to create the file.</param>
    /// <param name="desiredName">The desired name for the new file.</param>
    /// <param name="options">The collision option if a file with the same name already exists.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static Task CreateFileAsync(this IFolder folder, string desiredName, CreationCollisionOption options)
    {
        return _folderHandler.CreateFileAsync(folder, desiredName, options);
    }

    /// <summary>
    /// Asynchronously creates a subfolder in the folder.
    /// </summary>
    /// <param name="folder">The parent folder in which to create the subfolder.</param>
    /// <param name="desiredName">The desired name for the new subfolder.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static Task CreateFolderAsync(this IFolder folder, string desiredName)
    {
        return _folderHandler.CreateFolderAsync(folder, desiredName);
    }

    /// <summary>
    /// Asynchronously creates a subfolder in the folder with a collision option.
    /// </summary>
    /// <param name="folder">The parent folder in which to create the subfolder.</param>
    /// <param name="desiredName">The desired name for the new subfolder.</param>
    /// <param name="options">The collision option if a folder with the same name already exists.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static Task CreateFolderAsync(this IFolder folder, string desiredName, CreationCollisionOption options)
    {
        return _folderHandler.CreateFolderAsync(folder, desiredName, options);
    }

    /// <summary>
    /// Asynchronously deletes the folder.
    /// </summary>
    /// <param name="folder">The folder to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static Task DeleteAsync(this IFolder folder)
    {
        return _folderHandler.DeleteAsync(folder);
    }

    /// <summary>
    /// Asynchronously deletes the folder with a deletion option.
    /// </summary>
    /// <param name="folder">The folder to delete.</param>
    /// <param name="option">The deletion option (move to recycle bin or permanent delete).</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static Task DeleteAsync(this IFolder folder, StorageDeletionOption option)
    {
        return _folderHandler.DeleteAsync(folder, option);
    }

    /// <summary>
    /// Asynchronously retrieves a file from the folder by name.
    /// </summary>
    /// <param name="folder">The folder to search.</param>
    /// <param name="name">The name of the file to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The result contains the file.</returns>
    public static Task<IFile> GetFileAsync(this IFolder folder, string name)
    {
        return _folderHandler.GetFileAsync(folder, name);
    }

    /// <summary>
    /// Asynchronously retrieves all files from the folder.
    /// </summary>
    /// <param name="folder">The folder to search.</param>
    /// <returns>A task that represents the asynchronous operation. The result contains a read-only list of files.</returns>
    public static Task<IReadOnlyList<IFile>> GetFilesAsync(this IFolder folder)
    {
        return _folderHandler.GetFileAsync(folder);
    }

    /// <summary>
    /// Asynchronously retrieves a subfolder from the folder by name.
    /// </summary>
    /// <param name="folder">The parent folder to search.</param>
    /// <param name="name">The name of the subfolder to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The result contains the subfolder.</returns>
    public static Task<IFolder> GetFolderAsync(this IFolder folder, string name)
    {
        return _folderHandler.GetFolderAsync(folder, name);
    }

    /// <summary>
    /// Asynchronously retrieves all subfolders from the folder.
    /// </summary>
    /// <param name="folder">The parent folder to search.</param>
    /// <returns>A task that represents the asynchronous operation. The result contains a read-only list of subfolders.</returns>
    public static Task<IReadOnlyList<IFolder>> GetFoldersAsync(this IFolder folder)
    {
        return _folderHandler.GetFoldersAsync(folder);
    }

    /// <summary>
    /// Asynchronously retrieves a storage item (file or folder) from the folder by name.
    /// </summary>
    /// <param name="folder">The folder to search.</param>
    /// <param name="name">The name of the item to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The result contains the storage item.</returns>
    public static Task<Abstractions.Models.IStorageItem> GetItemAsync(this IFolder folder, string name)
    {
        return _folderHandler.GetItemAsync(folder, name);
    }

    /// <summary>
    /// Asynchronously retrieves all storage items (files and folders) from the folder.
    /// </summary>
    /// <param name="folder">The folder to search.</param>
    /// <returns>A task that represents the asynchronous operation. The result contains a read-only list of storage items.</returns>
    public static Task<IReadOnlyList<Abstractions.Models.IStorageItem>> GetItemsAsync(this IFolder folder)
    {
        return _folderHandler.GetItemsAsync(folder);
    }

    /// <summary>
    /// Asynchronously retrieves the parent folder.
    /// </summary>
    /// <param name="folder">The folder whose parent folder is to be retrieved.</param>
    /// <returns>A task that represents the asynchronous operation. The result contains the parent folder.</returns>
    public static Task<IFolder> GetParentAsync(this IFolder folder)
    {
        return _folderHandler.GetParentAsync(folder);
    }

    /// <summary>
    /// Asynchronously renames the folder.
    /// </summary>
    /// <param name="folder">The folder to rename.</param>
    /// <param name="desiredName">The new name for the folder.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static Task RenameAsync(this IFolder folder, string desiredName)
    {
        return _folderHandler.RenameAsync(folder, desiredName);
    }

    /// <summary>
    /// Asynchronously renames the folder with a collision option.
    /// </summary>
    /// <param name="folder">The folder to rename.</param>
    /// <param name="desiredName">The new name for the folder.</param>
    /// <param name="option">The collision option if a folder with the same name already exists.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static Task RenameAsync(this IFolder folder, string desiredName, NameCollisionOption option)
    {
        return _folderHandler.RenameAsync(folder, desiredName, option);
    }

    /// <summary>
    /// Asynchronously attempts to retrieve a storage item from the folder by name.
    /// </summary>
    /// <remarks>
    /// This method differs from <see cref="GetItemAsync(IFolder, string)"/> in that it returns null
    /// if the item is not found instead of throwing an exception.
    /// </remarks>
    /// <param name="folder">The folder to search.</param>
    /// <param name="name">The name of the item to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The result contains the storage item if found, or null if not found.</returns>
    public static Task<Abstractions.Models.IStorageItem> TryGetItemAsync(this IFolder folder, string name)
    {
        return _folderHandler.TryGetItemAsync(folder, name);
    }
}
