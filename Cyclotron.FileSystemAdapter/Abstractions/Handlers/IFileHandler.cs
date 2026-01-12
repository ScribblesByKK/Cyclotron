namespace Cyclotron.FileSystemAdapter.Abstractions.Handlers;

public interface IFileHandler
{
    Task CopyAndReplaceAsync(IFile sourceFile, IFile file);

    Task CopyAsync(IFile sourceFile, IFolder destinationFolder, string desiredNewName, NameCollisionOption option);

    Task<IFile> GetFileFromPathAsync(string path);

    Task<IFolder> GetParentAsync(IFile file);

    Task MoveAndReplaceAsync(IFile sourceFile, IFile fileToReplace);

    Task MoveAsync(IFile sourceFile, IFolder destinationFolder);

    Task MoveAsync(IFile sourceFile, IFolder destinationFolder, string desiredNewName);

    Task MoveAsync(IFile sourceFile, IFolder destinationFolder, string desiredNewName, NameCollisionOption option);

    Task OpenAsync(IFile file, FileAcessMode accessMode);

    Task OpenAsync(IFile file, FileAcessMode accessMode, StorageOpenOptions options);

    Task RenameAsync(IFile file, string desiredName);

    Task RenameAsync(IFile file, string desiredName, NameCollisionOption option);
}
