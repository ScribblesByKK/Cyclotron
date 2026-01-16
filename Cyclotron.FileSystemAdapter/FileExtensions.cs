namespace Cyclotron.FileSystemAdapter;

/// <summary>
/// Provides extension methods for <see cref="IFile"/> operations in WinUI.
/// </summary>
public static class FileExtensions
{
    /// <summary>
    /// A static instance of the <see cref="IFileHandler"/> used for all file operations.
    /// </summary>
    private static readonly IFileHandler _fileHandler = new();

    /// <summary>
    /// Asynchronously copies a source file and replaces the destination file.
    /// </summary>
    /// <param name="sourceFile">The source file to copy.</param>
    /// <param name="file">The destination file to replace.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static Task CopyAndReplaceAsync(this IFile sourceFile, IFile file)
    {
        return _fileHandler.CopyAndReplaceAsync(sourceFile, file);
    }

    /// <summary>
    /// Asynchronously copies a source file to a destination folder.
    /// </summary>
    /// <param name="sourceFile">The source file to copy.</param>
    /// <param name="destinationFolder">The destination folder.</param>
    /// <param name="desiredNewName">The new name for the copied file.</param>
    /// <param name="option">The collision option if a file with the same name already exists.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static Task CopyAsync(this IFile sourceFile, IFolder destinationFolder, string desiredNewName, NameCollisionOption option)
    {
        return _fileHandler.CopyAsync(sourceFile, destinationFolder, desiredNewName, option);
    }

    /// <summary>
    /// Asynchronously retrieves the parent folder of a file.
    /// </summary>
    /// <param name="file">The file whose parent folder is to be retrieved.</param>
    /// <returns>A task that represents the asynchronous operation. The result contains the parent folder.</returns>
    public static Task<IFolder> GetParentAsync(this IFile file)
    {
        return _fileHandler.GetParentAsync(file);
    }

    /// <summary>
    /// Asynchronously moves a source file and replaces the destination file.
    /// </summary>
    /// <param name="sourceFile">The source file to move.</param>
    /// <param name="fileToReplace">The destination file to replace.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static Task MoveAndReplaceAsync(this IFile sourceFile, IFile fileToReplace)
    {
        return _fileHandler.MoveAndReplaceAsync(sourceFile, fileToReplace);
    }

    /// <summary>
    /// Asynchronously moves a file to a destination folder.
    /// </summary>
    /// <param name="sourceFile">The source file to move.</param>
    /// <param name="destinationFolder">The destination folder.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static Task MoveAsync(this IFile sourceFile, IFolder destinationFolder)
    {
        return _fileHandler.MoveAsync(sourceFile, destinationFolder);
    }

    /// <summary>
    /// Asynchronously moves a file to a destination folder with a new name.
    /// </summary>
    /// <param name="sourceFile">The source file to move.</param>
    /// <param name="destinationFolder">The destination folder.</param>
    /// <param name="desiredNewName">The new name for the moved file.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static Task MoveAsync(this IFile sourceFile, IFolder destinationFolder, string desiredNewName)
    {
        return _fileHandler.MoveAsync(sourceFile, destinationFolder, desiredNewName);
    }

    /// <summary>
    /// Asynchronously moves a file to a destination folder with a new name and collision option.
    /// </summary>
    /// <param name="sourceFile">The source file to move.</param>
    /// <param name="destinationFolder">The destination folder.</param>
    /// <param name="desiredNewName">The new name for the moved file.</param>
    /// <param name="option">The collision option if a file with the same name already exists.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static Task MoveAsync(this IFile sourceFile, IFolder destinationFolder, string desiredNewName, NameCollisionOption option)
    {
        return _fileHandler.MoveAsync(sourceFile, destinationFolder, desiredNewName, option);
    }

    /// <summary>
    /// Asynchronously opens a file with the specified access mode.
    /// </summary>
    /// <param name="file">The file to open.</param>
    /// <param name="accessMode">The access mode (read or read-write).</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static Task OpenAsync(this IFile file, FileAcessMode accessMode)
    {
        return _fileHandler.OpenAsync(file, accessMode);
    }

    /// <summary>
    /// Asynchronously opens a file with the specified access mode and options.
    /// </summary>
    /// <param name="file">The file to open.</param>
    /// <param name="accessMode">The access mode (read or read-write).</param>
    /// <param name="options">Additional storage options for opening the file.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static Task OpenAsync(this IFile file, FileAcessMode accessMode, StorageOpenOptions options)
    {
        return _fileHandler.OpenAsync(file, accessMode, options);
    }

    /// <summary>
    /// Asynchronously renames a file.
    /// </summary>
    /// <param name="file">The file to rename.</param>
    /// <param name="desiredName">The new name for the file.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static Task RenameAsync(this IFile file, string desiredName)
    {
        return _fileHandler.RenameAsync(file, desiredName);
    }

    /// <summary>
    /// Asynchronously renames a file with a collision option.
    /// </summary>
    /// <param name="file">The file to rename.</param>
    /// <param name="desiredName">The new name for the file.</param>
    /// <param name="option">The collision option if a file with the same name already exists.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static Task RenameAsync(this IFile file, string desiredName, NameCollisionOption option)
    {
        return _fileHandler.RenameAsync(file, desiredName, option);
    }

    /// <summary>
    /// Converts a <see cref="StorageFile"/> to an <see cref="IFile"/> instance.
    /// </summary>
    /// <param name="file">The <see cref="StorageFile"/> to convert.</param>
    /// <returns>An <see cref="IFile"/> instance wrapping the storage file.</returns>
    // public static IFile AsIFile(this StorageFile file)
    // {
    //     return new WinUIFile(file);
    // }
}
