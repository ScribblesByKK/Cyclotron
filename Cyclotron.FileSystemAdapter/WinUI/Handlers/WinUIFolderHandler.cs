using Cyclotron.FileSystemAdapter.WinUI.Models;

namespace Cyclotron.FileSystemAdapter.WinUI.Handlers;

/// <summary>
/// WinUI implementation of the <see cref="IFolderHandler"/> interface.
/// </summary>
/// <remarks>
/// This class provides folder operations for WinUI applications by wrapping Windows.Storage APIs.
/// </remarks>
internal class WinUIFolderHandler : IFolderHandler
{
    /// <inheritdoc/>
    public async Task CreateFileAsync(IFolder folder, string desiredName)
    {
        if (folder is not WinUIFolder winUIFolder)
        {
            throw new ArgumentException("Folder must be a WinUIFolder instance", nameof(folder));
        }

        var storageFolder = await Windows.Storage.StorageFolder.GetFolderFromPathAsync(winUIFolder.Path);
        await storageFolder.CreateFileAsync(desiredName);
    }

    /// <inheritdoc/>
    public async Task CreateFileAsync(IFolder folder, string desiredName, CreationCollisionOption options)
    {
        if (folder is not WinUIFolder winUIFolder)
        {
            throw new ArgumentException("Folder must be a WinUIFolder instance", nameof(folder));
        }

        var storageFolder = await Windows.Storage.StorageFolder.GetFolderFromPathAsync(winUIFolder.Path);
        var winUIOption = ConvertCreationCollisionOption(options);
        await storageFolder.CreateFileAsync(desiredName, winUIOption);
    }

    /// <inheritdoc/>
    public async Task CreateFolderAsync(IFolder folder, string desiredName)
    {
        if (folder is not WinUIFolder winUIFolder)
        {
            throw new ArgumentException("Folder must be a WinUIFolder instance", nameof(folder));
        }

        var storageFolder = await Windows.Storage.StorageFolder.GetFolderFromPathAsync(winUIFolder.Path);
        await storageFolder.CreateFolderAsync(desiredName);
    }

    /// <inheritdoc/>
    public async Task CreateFolderAsync(IFolder folder, string desiredName, CreationCollisionOption options)
    {
        if (folder is not WinUIFolder winUIFolder)
        {
            throw new ArgumentException("Folder must be a WinUIFolder instance", nameof(folder));
        }

        var storageFolder = await Windows.Storage.StorageFolder.GetFolderFromPathAsync(winUIFolder.Path);
        var winUIOption = ConvertCreationCollisionOption(options);
        await storageFolder.CreateFolderAsync(desiredName, winUIOption);
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(IFolder folder)
    {
        if (folder is not WinUIFolder winUIFolder)
        {
            throw new ArgumentException("Folder must be a WinUIFolder instance", nameof(folder));
        }

        var storageFolder = await Windows.Storage.StorageFolder.GetFolderFromPathAsync(winUIFolder.Path);
        await storageFolder.DeleteAsync();
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(IFolder folder, StorageDeletionOption option)
    {
        if (folder is not WinUIFolder winUIFolder)
        {
            throw new ArgumentException("Folder must be a WinUIFolder instance", nameof(folder));
        }

        var storageFolder = await Windows.Storage.StorageFolder.GetFolderFromPathAsync(winUIFolder.Path);
        var winUIOption = ConvertStorageDeletionOption(option);
        await storageFolder.DeleteAsync(winUIOption);
    }

    /// <inheritdoc/>
    public async Task<IFile> GetFileAsync(IFolder folder, string name)
    {
        if (folder is not WinUIFolder winUIFolder)
        {
            throw new ArgumentException("Folder must be a WinUIFolder instance", nameof(folder));
        }

        var storageFolder = await Windows.Storage.StorageFolder.GetFolderFromPathAsync(winUIFolder.Path);
        var file = await storageFolder.GetFileAsync(name);
        return new WinUIFile(file);
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<IFile>> GetFileAsync(IFolder folder)
    {
        if (folder is not WinUIFolder winUIFolder)
        {
            throw new ArgumentException("Folder must be a WinUIFolder instance", nameof(folder));
        }

        var storageFolder = await Windows.Storage.StorageFolder.GetFolderFromPathAsync(winUIFolder.Path);
        var files = await storageFolder.GetFilesAsync();
        return files.Select(f => (IFile)new WinUIFile(f)).ToList();
    }

    /// <inheritdoc/>
    public async Task<IFolder> GetFolderAsync(IFolder folder, string name)
    {
        if (folder is not WinUIFolder winUIFolder)
        {
            throw new ArgumentException("Folder must be a WinUIFolder instance", nameof(folder));
        }

        var storageFolder = await Windows.Storage.StorageFolder.GetFolderFromPathAsync(winUIFolder.Path);
        var subFolder = await storageFolder.GetFolderAsync(name);
        return new WinUIFolder(subFolder);
    }

    /// <inheritdoc/>
    public async Task<IFolder> GetFolderFromPathAsync(string path)
    {
        var folder = await Windows.Storage.StorageFolder.GetFolderFromPathAsync(path);
        return new WinUIFolder(folder);
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<IFolder>> GetFoldersAsync(IFolder folder)
    {
        if (folder is not WinUIFolder winUIFolder)
        {
            throw new ArgumentException("Folder must be a WinUIFolder instance", nameof(folder));
        }

        var storageFolder = await Windows.Storage.StorageFolder.GetFolderFromPathAsync(winUIFolder.Path);
        var folders = await storageFolder.GetFoldersAsync();
        return folders.Select(f => (IFolder)new WinUIFolder(f)).ToList();
    }

    /// <inheritdoc/>
    public async Task<Abstractions.Models.IStorageItem> GetItemAsync(IFolder folder, string name)
    {
        if (folder is not WinUIFolder winUIFolder)
        {
            throw new ArgumentException("Folder must be a WinUIFolder instance", nameof(folder));
        }

        var storageFolder = await Windows.Storage.StorageFolder.GetFolderFromPathAsync(winUIFolder.Path);
        var item = await storageFolder.GetItemAsync(name);
        return ConvertStorageItem(item);
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<Abstractions.Models.IStorageItem>> GetItemsAsync(IFolder folder)
    {
        if (folder is not WinUIFolder winUIFolder)
        {
            throw new ArgumentException("Folder must be a WinUIFolder instance", nameof(folder));
        }

        var storageFolder = await Windows.Storage.StorageFolder.GetFolderFromPathAsync(winUIFolder.Path);
        var items = await storageFolder.GetItemsAsync();
        return items.Select(ConvertStorageItem).ToList();
    }

    /// <inheritdoc/>
    public async Task<IFolder> GetParentAsync(IFolder folder)
    {
        if (folder is not WinUIFolder winUIFolder)
        {
            throw new ArgumentException("Folder must be a WinUIFolder instance", nameof(folder));
        }

        var storageFolder = await Windows.Storage.StorageFolder.GetFolderFromPathAsync(winUIFolder.Path);
        var parentFolder = await storageFolder.GetParentAsync();
        return new WinUIFolder(parentFolder);
    }

    /// <inheritdoc/>
    public async Task RenameAsync(IFolder folder, string desiredName)
    {
        if (folder is not WinUIFolder winUIFolder)
        {
            throw new ArgumentException("Folder must be a WinUIFolder instance", nameof(folder));
        }

        var storageFolder = await Windows.Storage.StorageFolder.GetFolderFromPathAsync(winUIFolder.Path);
        await storageFolder.RenameAsync(desiredName);
    }

    /// <inheritdoc/>
    public async Task RenameAsync(IFolder folder, string desiredName, NameCollisionOption option)
    {
        if (folder is not WinUIFolder winUIFolder)
        {
            throw new ArgumentException("Folder must be a WinUIFolder instance", nameof(folder));
        }

        var storageFolder = await Windows.Storage.StorageFolder.GetFolderFromPathAsync(winUIFolder.Path);
        var winUIOption = ConvertNameCollisionOption(option);
        await storageFolder.RenameAsync(desiredName, winUIOption);
    }

    /// <inheritdoc/>
    public async Task<Abstractions.Models.IStorageItem> TryGetItemAsync(IFolder folder, string name)
    {
        if (folder is not WinUIFolder winUIFolder)
        {
            throw new ArgumentException("Folder must be a WinUIFolder instance", nameof(folder));
        }

        var storageFolder = await Windows.Storage.StorageFolder.GetFolderFromPathAsync(winUIFolder.Path);
        var item = await storageFolder.TryGetItemAsync(name);
        return item != null ? ConvertStorageItem(item) : null;
    }

    /// <summary>
    /// Converts a <see cref="Windows.Storage.IStorageItem"/> to an <see cref="IStorageItem"/>.
    /// </summary>
    /// <param name="item">The storage item to convert.</param>
    /// <returns>The converted storage item.</returns>
    /// <exception cref="NotSupportedException">Thrown if the storage item type is not supported.</exception>
    private static Abstractions.Models.IStorageItem ConvertStorageItem(Windows.Storage.IStorageItem item)
    {
        return item switch
        {
            Windows.Storage.StorageFile file => new WinUIFile(file),
            Windows.Storage.StorageFolder folder => new WinUIFolder(folder),
            _ => throw new NotSupportedException($"Storage item type '{item.GetType().Name}' is not supported")
        };
    }

    /// <summary>
    /// Converts a <see cref="CreationCollisionOption"/> to a <see cref="Windows.Storage.CreationCollisionOption"/>.
    /// </summary>
    /// <param name="option">The creation collision option to convert.</param>
    /// <returns>The corresponding Windows.Storage creation collision option.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the option is not recognized.</exception>
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

    /// <summary>
    /// Converts a <see cref="StorageDeletionOption"/> to a <see cref="Windows.Storage.StorageDeleteOption"/>.
    /// </summary>
    /// <param name="option">The storage deletion option to convert.</param>
    /// <returns>The corresponding Windows.Storage delete option.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the option is not recognized.</exception>
    private static Windows.Storage.StorageDeleteOption ConvertStorageDeletionOption(StorageDeletionOption option)
    {
        return option switch
        {
            StorageDeletionOption.Default => Windows.Storage.StorageDeleteOption.Default,
            StorageDeletionOption.PermanentDelete => Windows.Storage.StorageDeleteOption.PermanentDelete,
            _ => throw new ArgumentOutOfRangeException(nameof(option))
        };
    }

    /// <summary>
    /// Converts a <see cref="NameCollisionOption"/> to a <see cref="Windows.Storage.NameCollisionOption"/>.
    /// </summary>
    /// <param name="option">The name collision option to convert.</param>
    /// <returns>The corresponding Windows.Storage name collision option.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the option is not recognized.</exception>
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