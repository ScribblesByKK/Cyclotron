using Cyclotron.FileSystemAdapter.WinUI.Handlers;
using Cyclotron.FileSystemAdapter.WinUI.Models;
using Windows.Storage;

namespace Cyclotron.FileSystemAdapter.WinUI;

public static class FileExtensions
{
    private static readonly WinUIFileHandler _fileHandler = new();

    public static Task CopyAndReplaceAsync(this IFile sourceFile, IFile file)
    {
        return _fileHandler.CopyAndReplaceAsync(sourceFile, file);
    }

    public static Task CopyAsync(this IFile sourceFile, IFolder destinationFolder, string desiredNewName, NameCollisionOption option)
    {
        return _fileHandler.CopyAsync(sourceFile, destinationFolder, desiredNewName, option);
    }

    public static Task<IFolder> GetParentAsync(this IFile file)
    {
        return _fileHandler.GetParentAsync(file);
    }

    public static Task MoveAndReplaceAsync(this IFile sourceFile, IFile fileToReplace)
    {
        return _fileHandler.MoveAndReplaceAsync(sourceFile, fileToReplace);
    }

    public static Task MoveAsync(this IFile sourceFile, IFolder destinationFolder)
    {
        return _fileHandler.MoveAsync(sourceFile, destinationFolder);
    }

    public static Task MoveAsync(this IFile sourceFile, IFolder destinationFolder, string desiredNewName)
    {
        return _fileHandler.MoveAsync(sourceFile, destinationFolder, desiredNewName);
    }

    public static Task MoveAsync(this IFile sourceFile, IFolder destinationFolder, string desiredNewName, NameCollisionOption option)
    {
        return _fileHandler.MoveAsync(sourceFile, destinationFolder, desiredNewName, option);
    }

    public static Task OpenAsync(this IFile file, FileAcessMode accessMode)
    {
        return _fileHandler.OpenAsync(file, accessMode);
    }

    public static Task OpenAsync(this IFile file, FileAcessMode accessMode, StorageOpenOptions options)
    {
        return _fileHandler.OpenAsync(file, accessMode, options);
    }

    public static Task RenameAsync(this IFile file, string desiredName)
    {
        return _fileHandler.RenameAsync(file, desiredName);
    }

    public static Task RenameAsync(this IFile file, string desiredName, NameCollisionOption option)
    {
        return _fileHandler.RenameAsync(file, desiredName, option);
    }

    public static IFile AsIFile(this StorageFile file)
    {
        return new WinUIFile(file);
    }
}
