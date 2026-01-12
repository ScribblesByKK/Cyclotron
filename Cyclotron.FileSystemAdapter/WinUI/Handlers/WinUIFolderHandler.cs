using Cyclotron.FileSystemAdapter.WinUI.Models;
using Windows.Storage;

namespace Cyclotron.FileSystemAdapter.WinUI.Handlers;

internal class WinUIFolderHandler : IFolderHandler
{
    public async Task CreateFileAsync(IFolder folder, string desiredName)
    {
        if (folder is not WinUIFolder winUIFolder)
        {
            throw new ArgumentException("Folder must be a WinUIFolder instance", nameof(folder));
        }

        var storageFolder = await StorageFolder.GetFolderFromPathAsync(winUIFolder.Path);
        await storageFolder.CreateFileAsync(desiredName);
    }

    public async Task CreateFileAsync(IFolder folder, string desiredName, CreationCollisionOption options)
    {
        if (folder is not WinUIFolder winUIFolder)
        {
            throw new ArgumentException("Folder must be a WinUIFolder instance", nameof(folder));
        }

        var storageFolder = await StorageFolder.GetFolderFromPathAsync(winUIFolder.Path);
        var winUIOption = ConvertCreationCollisionOption(options);
        await storageFolder.CreateFileAsync(desiredName, winUIOption);
    }

    public async Task CreateFolderAsync(IFolder folder, string desiredName)
    {
        if (folder is not WinUIFolder winUIFolder)
        {
            throw new ArgumentException("Folder must be a WinUIFolder instance", nameof(folder));
        }

        var storageFolder = await StorageFolder.GetFolderFromPathAsync(winUIFolder.Path);
        await storageFolder.CreateFolderAsync(desiredName);
    }

    public async Task CreateFolderAsync(IFolder folder, string desiredName, CreationCollisionOption options)
    {
        if (folder is not WinUIFolder winUIFolder)
        {
            throw new ArgumentException("Folder must be a WinUIFolder instance", nameof(folder));
        }

        var storageFolder = await StorageFolder.GetFolderFromPathAsync(winUIFolder.Path);
        var winUIOption = ConvertCreationCollisionOption(options);
        await storageFolder.CreateFolderAsync(desiredName, winUIOption);
    }

    public async Task DeleteAsync(IFolder folder)
    {
        if (folder is not WinUIFolder winUIFolder)
        {
            throw new ArgumentException("Folder must be a WinUIFolder instance", nameof(folder));
        }

        var storageFolder = await StorageFolder.GetFolderFromPathAsync(winUIFolder.Path);
        await storageFolder.DeleteAsync();
    }

    public async Task DeleteAsync(IFolder folder, StorageDeletionOption option)
    {
        if (folder is not WinUIFolder winUIFolder)
        {
            throw new ArgumentException("Folder must be a WinUIFolder instance", nameof(folder));
        }

        var storageFolder = await StorageFolder.GetFolderFromPathAsync(winUIFolder.Path);
        var winUIOption = ConvertStorageDeletionOption(option);
        await storageFolder.DeleteAsync(winUIOption);
    }

    public async Task<IFile> GetFileAsync(IFolder folder, string name)
    {
        if (folder is not WinUIFolder winUIFolder)
        {
            throw new ArgumentException("Folder must be a WinUIFolder instance", nameof(folder));
        }

        var storageFolder = await StorageFolder.GetFolderFromPathAsync(winUIFolder.Path);
        var file = await storageFolder.GetFileAsync(name);
        return new WinUIFile(file);
    }

    public async Task<IReadOnlyList<IFile>> GetFileAsync(IFolder folder)
    {
        if (folder is not WinUIFolder winUIFolder)
        {
            throw new ArgumentException("Folder must be a WinUIFolder instance", nameof(folder));
        }

        var storageFolder = await StorageFolder.GetFolderFromPathAsync(winUIFolder.Path);
        var files = await storageFolder.GetFilesAsync();
        return files.Select(f => (IFile)new WinUIFile(f)).ToList();
    }

    public async Task<IFolder> GetFolderAsync(IFolder folder, string name)
    {
        if (folder is not WinUIFolder winUIFolder)
        {
            throw new ArgumentException("Folder must be a WinUIFolder instance", nameof(folder));
        }

        var storageFolder = await StorageFolder.GetFolderFromPathAsync(winUIFolder.Path);
        var subFolder = await storageFolder.GetFolderAsync(name);
        return new WinUIFolder(subFolder);
    }

    public async Task<IFolder> GetFolderFromPathAsync(string path)
    {
        var folder = await StorageFolder.GetFolderFromPathAsync(path);
        return new WinUIFolder(folder);
    }

    public async Task<IReadOnlyList<IFolder>> GetFoldersAsync(IFolder folder)
    {
        if (folder is not WinUIFolder winUIFolder)
        {
            throw new ArgumentException("Folder must be a WinUIFolder instance", nameof(folder));
        }

        var storageFolder = await StorageFolder.GetFolderFromPathAsync(winUIFolder.Path);
        var folders = await storageFolder.GetFoldersAsync();
        return folders.Select(f => (IFolder)new WinUIFolder(f)).ToList();
    }

    public async Task<Abstractions.Models.IStorageItem> GetItemAsync(IFolder folder, string name)
    {
        if (folder is not WinUIFolder winUIFolder)
        {
            throw new ArgumentException("Folder must be a WinUIFolder instance", nameof(folder));
        }

        var storageFolder = await StorageFolder.GetFolderFromPathAsync(winUIFolder.Path);
        var item = await storageFolder.GetItemAsync(name);
        return ConvertStorageItem(item);
    }

    public async Task<IReadOnlyList<Abstractions.Models.IStorageItem>> GetItemsAsync(IFolder folder)
    {
        if (folder is not WinUIFolder winUIFolder)
        {
            throw new ArgumentException("Folder must be a WinUIFolder instance", nameof(folder));
        }

        var storageFolder = await StorageFolder.GetFolderFromPathAsync(winUIFolder.Path);
        var items = await storageFolder.GetItemsAsync();
        return items.Select(ConvertStorageItem).ToList();
    }

    public async Task<IFolder> GetParentAsync(IFolder folder)
    {
        if (folder is not WinUIFolder winUIFolder)
        {
            throw new ArgumentException("Folder must be a WinUIFolder instance", nameof(folder));
        }

        var storageFolder = await StorageFolder.GetFolderFromPathAsync(winUIFolder.Path);
        var parentFolder = await storageFolder.GetParentAsync();
        return new WinUIFolder(parentFolder);
    }

    public async Task RenameAsync(IFolder folder, string desiredName)
    {
        if (folder is not WinUIFolder winUIFolder)
        {
            throw new ArgumentException("Folder must be a WinUIFolder instance", nameof(folder));
        }

        var storageFolder = await StorageFolder.GetFolderFromPathAsync(winUIFolder.Path);
        await storageFolder.RenameAsync(desiredName);
    }

    public async Task RenameAsync(IFolder folder, string desiredName, NameCollisionOption option)
    {
        if (folder is not WinUIFolder winUIFolder)
        {
            throw new ArgumentException("Folder must be a WinUIFolder instance", nameof(folder));
        }

        var storageFolder = await StorageFolder.GetFolderFromPathAsync(winUIFolder.Path);
        var winUIOption = ConvertNameCollisionOption(option);
        await storageFolder.RenameAsync(desiredName, winUIOption);
    }

    public async Task<Abstractions.Models.IStorageItem> TryGetItemAsync(IFolder folder, string name)
    {
        if (folder is not WinUIFolder winUIFolder)
        {
            throw new ArgumentException("Folder must be a WinUIFolder instance", nameof(folder));
        }

        var storageFolder = await StorageFolder.GetFolderFromPathAsync(winUIFolder.Path);
        var item = await storageFolder.TryGetItemAsync(name);
        return item != null ? ConvertStorageItem(item) : null;
    }

    private static Abstractions.Models.IStorageItem ConvertStorageItem(Windows.Storage.IStorageItem item)
    {
        return item switch
        {
            StorageFile file => new WinUIFile(file),
            StorageFolder folder => new WinUIFolder(folder),
            _ => throw new NotSupportedException($"Storage item type '{item.GetType().Name}' is not supported")
        };
    }

    private static Windows.Storage.CreationCollisionOption ConvertCreationCollisionOption(CreationCollisionOption option)
    {
        return option switch
        {
            CreationCollisionOption.GenerateUniqueName => Windows.Storage.CreationCollisionOption.GenerateUniqueName,
            CreationCollisionOption.ReplaceExisting => Windows.Storage.CreationCollisionOption.ReplaceExisting,
            CreationCollisionOption.FailIfExists => Windows.Storage.CreationCollisionOption.FailIfExists,
            CreationCollisionOption.OpenIfExists => Windows.Storage.CreationCollisionOption.OpenIfExists,
            _ => throw new ArgumentOutOfRangeException(nameof(option))
        };
    }

    private static Windows.Storage.StorageDeleteOption ConvertStorageDeletionOption(StorageDeletionOption option)
    {
        return option switch
        {
            StorageDeletionOption.Default => Windows.Storage.StorageDeleteOption.Default,
            StorageDeletionOption.PermanentDelete => Windows.Storage.StorageDeleteOption.PermanentDelete,
            _ => throw new ArgumentOutOfRangeException(nameof(option))
        };
    }

    private static Windows.Storage.NameCollisionOption ConvertNameCollisionOption(NameCollisionOption option)
    {
        return option switch
        {
            NameCollisionOption.GenerateUniqueName => Windows.Storage.NameCollisionOption.GenerateUniqueName,
            NameCollisionOption.ReplaceExisting => Windows.Storage.NameCollisionOption.ReplaceExisting,
            NameCollisionOption.FailIfExists => Windows.Storage.NameCollisionOption.FailIfExists,
            _ => throw new ArgumentOutOfRangeException(nameof(option))
        };
    }
}

