namespace Cyclotron.FileSystemAdapter.Abstractions.Handlers;

/// <summary>
/// Defines the contract for file operations such as copying, moving, renaming, and opening files.
/// </summary>
public interface IFileHandler
{
    /// <summary>
    /// Asynchronously copies a source file and replaces the destination file.
    /// </summary>
    /// <param name="sourceFile">The source file to copy.</param>
    /// <param name="file">The destination file to replace.</param>
    Task CopyAndReplaceAsync(IFile sourceFile, IFile file);

    /// <summary>
    /// Asynchronously copies a source file to a destination folder with a new name.
    /// </summary>
    /// <param name="sourceFile">The source file to copy.</param>
    /// <param name="destinationFolder">The destination folder where the file will be copied.</param>
    /// <param name="desiredNewName">The new name for the copied file.</param>
    /// <param name="option">The collision option to apply if a file with the same name already exists.</param>
    Task CopyAsync(IFile sourceFile, IFolder destinationFolder, string desiredNewName, NameCollisionOption option);

    /// <summary>
    /// Asynchronously retrieves a file from the specified path.
    /// </summary>
    /// <param name="path">The file path.</param>
    /// <returns>A task that represents the asynchronous operation. The result contains the file.</returns>
    Task<IFile> GetFileFromPathAsync(string path);

    /// <summary>
    /// Asynchronously retrieves the parent folder of a file.
    /// </summary>
    /// <param name="file">The file whose parent folder is to be retrieved.</param>
    /// <returns>A task that represents the asynchronous operation. The result contains the parent folder.</returns>
    Task<IFolder> GetParentAsync(IFile file);

    /// <summary>
    /// Asynchronously moves a source file and replaces the destination file.
    /// </summary>
    /// <param name="sourceFile">The source file to move.</param>
    /// <param name="fileToReplace">The destination file to replace.</param>
    Task MoveAndReplaceAsync(IFile sourceFile, IFile fileToReplace);

    /// <summary>
    /// Asynchronously moves a file to a destination folder.
    /// </summary>
    /// <param name="sourceFile">The source file to move.</param>
    /// <param name="destinationFolder">The destination folder.</param>
    Task MoveAsync(IFile sourceFile, IFolder destinationFolder);

    /// <summary>
    /// Asynchronously moves a file to a destination folder with a new name.
    /// </summary>
    /// <param name="sourceFile">The source file to move.</param>
    /// <param name="destinationFolder">The destination folder.</param>
    /// <param name="desiredNewName">The new name for the moved file.</param>
    Task MoveAsync(IFile sourceFile, IFolder destinationFolder, string desiredNewName);

    /// <summary>
    /// Asynchronously moves a file to a destination folder with a new name and a collision option.
    /// </summary>
    /// <param name="sourceFile">The source file to move.</param>
    /// <param name="destinationFolder">The destination folder.</param>
    /// <param name="desiredNewName">The new name for the moved file.</param>
    /// <param name="option">The collision option to apply if a file with the same name already exists.</param>
    Task MoveAsync(IFile sourceFile, IFolder destinationFolder, string desiredNewName, NameCollisionOption option);

    /// <summary>
    /// Asynchronously opens a file with the specified access mode.
    /// </summary>
    /// <param name="file">The file to open.</param>
    /// <param name="accessMode">The access mode (read or read-write).</param>
    Task OpenAsync(IFile file, FileAccessMode accessMode);

    /// <summary>
    /// Asynchronously opens a file with the specified access mode and storage options.
    /// </summary>
    /// <param name="file">The file to open.</param>
    /// <param name="accessMode">The access mode (read or read-write).</param>
    /// <param name="options">Additional storage options for opening the file.</param>
    Task OpenAsync(IFile file, FileAccessMode accessMode, StorageOpenOptions options);

    /// <summary>
    /// Asynchronously renames a file with a new name.
    /// </summary>
    /// <param name="file">The file to rename.</param>
    /// <param name="desiredName">The new name for the file.</param>
    Task RenameAsync(IFile file, string desiredName);

    /// <summary>
    /// Asynchronously renames a file with a new name and a collision option.
    /// </summary>
    /// <param name="file">The file to rename.</param>
    /// <param name="desiredName">The new name for the file.</param>
    /// <param name="option">The collision option to apply if a file with the same name already exists.</param>
    Task RenameAsync(IFile file, string desiredName, NameCollisionOption option);
}
