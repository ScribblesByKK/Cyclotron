namespace Cyclotron.FileSystemAdapter.Abstractions.Handlers;

public interface IFolderHandler
{
    Task CreateFileAsync(IFolder folder, string desiredName);

    Task CreateFileAsync(IFolder folder, string desiredName, CreationCollisionOption options);

    Task CreateFolderAsync(IFolder folder, string desiredName);

    Task CreateFolderAsync(IFolder folder, string desiredName, CreationCollisionOption options);

    Task DeleteAsync(IFolder folder);

    Task DeleteAsync(IFolder folder, StorageDeletionOption option);

    Task<IFile> GetFileAsync(IFolder folder, string name);

    Task<IReadOnlyList<IFile>> GetFileAsync(IFolder folder);

    Task<IFolder> GetFolderAsync(IFolder folder, string name);

    Task<IFolder> GetFolderFromPathAsync(string path);

    Task<IReadOnlyList<IFolder>> GetFoldersAsync(IFolder folder);

    Task<IStorageItem> GetItemAsync(IFolder folder, string name);

    Task<IReadOnlyList<IStorageItem>> GetItemsAsync(IFolder folder);

    Task<IFolder> GetParentAsync(IFolder folder);

    Task RenameAsync(IFolder folder, string desiredName);

    Task RenameAsync(IFolder folder, string desiredName, NameCollisionOption option);

    Task<IStorageItem> TryGetItemAsync(IFolder folder, string name);
}
