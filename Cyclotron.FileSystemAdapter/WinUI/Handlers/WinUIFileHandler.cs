using Cyclotron.FileSystemAdapter.WinUI.Models;
using Windows.Storage;

namespace Cyclotron.FileSystemAdapter.WinUI.Handlers;

internal class WinUIFileHandler : IFileHandler
{
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

    public async Task<IFile> GetFileFromPathAsync(string path)
    {
        var file = await StorageFile.GetFileFromPathAsync(path);
        return new WinUIFile(file);
    }

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

    public async Task RenameAsync(IFile file, string desiredName)
    {
        if (file is not WinUIFile winUIFile)
        {
            throw new ArgumentException("File must be a WinUIFile instance", nameof(file));
        }

        var storageFile = await StorageFile.GetFileFromPathAsync(winUIFile.Path);
        await storageFile.RenameAsync(desiredName);
    }

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

    private static Windows.Storage.FileAccessMode ConvertFileAccessMode(FileAcessMode accessMode)
    {
        return accessMode switch
        {
            FileAcessMode.Read => Windows.Storage.FileAccessMode.Read,
            FileAcessMode.ReadWrite => Windows.Storage.FileAccessMode.ReadWrite,
            _ => throw new ArgumentOutOfRangeException(nameof(accessMode))
        };
    }

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
