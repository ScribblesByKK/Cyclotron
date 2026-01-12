using Cyclotron.FileSystemAdapter.WinUI.Handlers;
using Cyclotron.FileSystemAdapter.WinUI.Models;
using Windows.Storage;

namespace Cyclotron.FileSystemAdapter.WinUI;

public static class FolderExtensions
{
    private static readonly WinUIFolderHandler _folderHandler = new();

    public static Task CreateFileAsync(this IFolder folder, string desiredName)
    {
        return _folderHandler.CreateFileAsync(folder, desiredName);
    }

    public static Task CreateFileAsync(this IFolder folder, string desiredName, CreationCollisionOption options)
    {
        return _folderHandler.CreateFileAsync(folder, desiredName, options);
    }

    public static Task CreateFolderAsync(this IFolder folder, string desiredName)
    {
        return _folderHandler.CreateFolderAsync(folder, desiredName);
    }

    public static Task CreateFolderAsync(this IFolder folder, string desiredName, CreationCollisionOption options)
    {
        return _folderHandler.CreateFolderAsync(folder, desiredName, options);
    }

    public static Task DeleteAsync(this IFolder folder)
    {
        return _folderHandler.DeleteAsync(folder);
    }

    public static Task DeleteAsync(this IFolder folder, StorageDeletionOption option)
    {
        return _folderHandler.DeleteAsync(folder, option);
    }

    public static Task<IFile> GetFileAsync(this IFolder folder, string name)
    {
        return _folderHandler.GetFileAsync(folder, name);
    }

    public static Task<IReadOnlyList<IFile>> GetFilesAsync(this IFolder folder)
    {
        return _folderHandler.GetFileAsync(folder);
    }

    public static Task<IFolder> GetFolderAsync(this IFolder folder, string name)
    {
        return _folderHandler.GetFolderAsync(folder, name);
    }

    public static Task<IReadOnlyList<IFolder>> GetFoldersAsync(this IFolder folder)
    {
        return _folderHandler.GetFoldersAsync(folder);
    }

    public static Task<Abstractions.Models.IStorageItem> GetItemAsync(this IFolder folder, string name)
    {
        return _folderHandler.GetItemAsync(folder, name);
    }

    public static Task<IReadOnlyList<Abstractions.Models.IStorageItem>> GetItemsAsync(this IFolder folder)
    {
        return _folderHandler.GetItemsAsync(folder);
    }

    public static Task<IFolder> GetParentAsync(this IFolder folder)
    {
        return _folderHandler.GetParentAsync(folder);
    }

    public static Task RenameAsync(this IFolder folder, string desiredName)
    {
        return _folderHandler.RenameAsync(folder, desiredName);
    }

    public static Task RenameAsync(this IFolder folder, string desiredName, NameCollisionOption option)
    {
        return _folderHandler.RenameAsync(folder, desiredName, option);
    }

    public static Task<Abstractions.Models.IStorageItem> TryGetItemAsync(this IFolder folder, string name)
    {
        return _folderHandler.TryGetItemAsync(folder, name);
    }

    public static IFolder AsIFolder(this StorageFolder folder)
    {
        return new WinUIFolder(folder);
    }
}
