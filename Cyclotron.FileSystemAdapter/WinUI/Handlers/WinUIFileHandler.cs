using Windows.Storage;

namespace Cyclotron.FileSystemAdapter.WinUI.Handlers;

/// <summary>
/// WinUI implementation of the <see cref="IFileHandler"/> interface.
/// </summary>
/// <remarks>
/// This class provides file operations for WinUI applications by wrapping Windows.Storage APIs.
/// </remarks>
internal class WinUIFileHandler : IFileHandler
{
    /// <inheritdoc/>
    public async Task CopyAndReplaceAsync(IFile sourceFile, IFile file)
    {
        if (sourceFile is not WinUIFile winUISourceFile)
        {
            throw new ArgumentException("Source file must be a WinUIFile instance", nameof(sourceFile));
        }

        if (file is not WinUIFile winUIFile)
        {
            throw new ArgumentException("File must be a WinUIFile instance", nameof(file));
        }

        var sourceStorageFile = await StorageFile.GetFileFromPathAsync(winUISourceFile.Path);
        var targetFile = await StorageFile.GetFileFromPathAsync(winUIFile.Path);
        await sourceStorageFile.CopyAndReplaceAsync(targetFile);
    }

    /// <inheritdoc/>
    public async Task CopyAsync(IFile sourceFile, IFolder destinationFolder, string desiredNewName, NameCollisionOption option)
    {
        if (sourceFile is not WinUIFile winUISourceFile)
        {
            throw new ArgumentException("Source file must be a WinUIFile instance", nameof(sourceFile));
        }

        if (destinationFolder is not WinUIFolder winUIFolder)
        {
            throw new ArgumentException("Destination folder must be a WinUIFolder instance", nameof(destinationFolder));
        }

        var sourceStorageFile = await StorageFile.GetFileFromPathAsync(winUISourceFile.Path);
        var targetFolder = await StorageFolder.GetFolderFromPathAsync(winUIFolder.Path);
        var winUIOption = ConvertNameCollisionOption(option);
        await sourceStorageFile.CopyAsync(targetFolder, desiredNewName, winUIOption);
    }

    /// <inheritdoc/>
    public async Task<IFile> GetFileFromPathAsync(string path)
    {
        var file = await StorageFile.GetFileFromPathAsync(path);
        return new WinUIFile(file);
    }

    /// <inheritdoc/>
    public async Task<IFolder> GetParentAsync(IFile file)
    {
        if (file is not WinUIFile winUIFile)
        {
            throw new ArgumentException("File must be a WinUIFile instance", nameof(file));
        }

        var storageFile = await StorageFile.GetFileFromPathAsync(winUIFile.Path);
        var folder = await storageFile.GetParentAsync();
        return new WinUIFolder(folder);
    }

    /// <inheritdoc/>
    public async Task MoveAndReplaceAsync(IFile sourceFile, IFile fileToReplace)
    {
        if (sourceFile is not WinUIFile winUISourceFile)
        {
            throw new ArgumentException("Source file must be a WinUIFile instance", nameof(sourceFile));
        }

        if (fileToReplace is not WinUIFile winUIFile)
        {
            throw new ArgumentException("File must be a WinUIFile instance", nameof(fileToReplace));
        }

        var sourceStorageFile = await StorageFile.GetFileFromPathAsync(winUISourceFile.Path);
        var targetFile = await StorageFile.GetFileFromPathAsync(winUIFile.Path);
        await sourceStorageFile.MoveAndReplaceAsync(targetFile);
    }

    /// <inheritdoc/>
    public async Task MoveAsync(IFile sourceFile, IFolder destinationFolder)
    {
        if (sourceFile is not WinUIFile winUISourceFile)
        {
            throw new ArgumentException("Source file must be a WinUIFile instance", nameof(sourceFile));
        }

        if (destinationFolder is not WinUIFolder winUIFolder)
        {
            throw new ArgumentException("Destination folder must be a WinUIFolder instance", nameof(destinationFolder));
        }

        var sourceStorageFile = await StorageFile.GetFileFromPathAsync(winUISourceFile.Path);
        var targetFolder = await StorageFolder.GetFolderFromPathAsync(winUIFolder.Path);
        await sourceStorageFile.MoveAsync(targetFolder);
    }

    /// <inheritdoc/>
    public async Task MoveAsync(IFile sourceFile, IFolder destinationFolder, string desiredNewName)
    {
        if (sourceFile is not WinUIFile winUISourceFile)
        {
            throw new ArgumentException("Source file must be a WinUIFile instance", nameof(sourceFile));
        }

        if (destinationFolder is not WinUIFolder winUIFolder)
        {
            throw new ArgumentException("Destination folder must be a WinUIFolder instance", nameof(destinationFolder));
        }

        var sourceStorageFile = await StorageFile.GetFileFromPathAsync(winUISourceFile.Path);
        var targetFolder = await StorageFolder.GetFolderFromPathAsync(winUIFolder.Path);
        await sourceStorageFile.MoveAsync(targetFolder, desiredNewName);
    }

    /// <inheritdoc/>
    public async Task MoveAsync(IFile sourceFile, IFolder destinationFolder, string desiredNewName, NameCollisionOption option)
    {
        if (sourceFile is not WinUIFile winUISourceFile)
        {
            throw new ArgumentException("Source file must be a WinUIFile instance", nameof(sourceFile));
        }

        if (destinationFolder is not WinUIFolder winUIFolder)
        {
            throw new ArgumentException("Destination folder must be a WinUIFolder instance", nameof(destinationFolder));
        }

        var sourceStorageFile = await StorageFile.GetFileFromPathAsync(winUISourceFile.Path);
        var targetFolder = await StorageFolder.GetFolderFromPathAsync(winUIFolder.Path);
        var winUIOption = ConvertNameCollisionOption(option);
        await sourceStorageFile.MoveAsync(targetFolder, desiredNewName, winUIOption);
    }

    /// <inheritdoc/>
    public async Task OpenAsync(IFile file, FileAcessMode accessMode)
    {
        if (file is not WinUIFile winUIFile)
        {
            throw new ArgumentException("File must be a WinUIFile instance", nameof(file));
        }

        var storageFile = await StorageFile.GetFileFromPathAsync(winUIFile.Path);
        var winUIAccessMode = ConvertFileAccessMode(accessMode);
        await storageFile.OpenAsync(winUIAccessMode);
    }

    /// <inheritdoc/>
    public async Task OpenAsync(IFile file, FileAcessMode accessMode, StorageOpenOptions options)
    {
        if (file is not WinUIFile winUIFile)
        {
            throw new ArgumentException("File must be a WinUIFile instance", nameof(file));
        }

        var storageFile = await StorageFile.GetFileFromPathAsync(winUIFile.Path);
        var winUIAccessMode = ConvertFileAccessMode(accessMode);
        var winUIOptions = ConvertStorageOpenOptions(options);
        await storageFile.OpenAsync(winUIAccessMode, winUIOptions);
    }

    /// <inheritdoc/>
    public async Task RenameAsync(IFile file, string desiredName)
    {
        if (file is not WinUIFile winUIFile)
        {
            throw new ArgumentException("File must be a WinUIFile instance", nameof(file));
        }

        var storageFile = await StorageFile.GetFileFromPathAsync(winUIFile.Path);
        await storageFile.RenameAsync(desiredName);
    }

    /// <inheritdoc/>
    public async Task RenameAsync(IFile file, string desiredName, NameCollisionOption option)
    {
        if (file is not WinUIFile winUIFile)
        {
            throw new ArgumentException("File must be a WinUIFile instance", nameof(file));
        }

        var storageFile = await StorageFile.GetFileFromPathAsync(winUIFile.Path);
        var winUIOption = ConvertNameCollisionOption(option);
        await storageFile.RenameAsync(desiredName, winUIOption);
    }

    /// <summary>
    /// Converts a <see cref="NameCollisionOption"/> to a <see cref="Windows.Storage.NameCollisionOption"/>.
    /// </summary>
    /// <param name="option">The collision option to convert.</param>
    /// <returns>The corresponding Windows.Storage collision option.</returns>
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

    /// <summary>
    /// Converts a <see cref="FileAcessMode"/> to a <see cref="Windows.Storage.FileAccessMode"/>.
    /// </summary>
    /// <param name="accessMode">The access mode to convert.</param>
    /// <returns>The corresponding Windows.Storage access mode.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the access mode is not recognized.</exception>
    private static Windows.Storage.FileAccessMode ConvertFileAccessMode(FileAcessMode accessMode)
    {
        return accessMode switch
        {
            FileAcessMode.Read => Windows.Storage.FileAccessMode.Read,
            FileAcessMode.ReadWrite => Windows.Storage.FileAccessMode.ReadWrite,
            _ => throw new ArgumentOutOfRangeException(nameof(accessMode))
        };
    }

    /// <summary>
    /// Converts a <see cref="StorageOpenOptions"/> to a <see cref="Windows.Storage.StorageOpenOptions"/>.
    /// </summary>
    /// <param name="options">The storage open options to convert.</param>
    /// <returns>The corresponding Windows.Storage open options.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the options are not recognized.</exception>
    private static Windows.Storage.StorageOpenOptions ConvertStorageOpenOptions(StorageOpenOptions options)
    {
        return options switch
        {
            StorageOpenOptions.None => Windows.Storage.StorageOpenOptions.None,
            StorageOpenOptions.AllowOnlyReaders => Windows.Storage.StorageOpenOptions.AllowOnlyReaders,
            StorageOpenOptions.AllowReadersAndWriters => Windows.Storage.StorageOpenOptions.AllowReadersAndWriters,
            _ => throw new ArgumentOutOfRangeException(nameof(options))
        };
    }
}