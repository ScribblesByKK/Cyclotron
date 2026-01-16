using Windows.Storage;

namespace Cyclotron.FileSystemAdapter.WinUI.Models;

/// <summary>
/// WinUI implementation of the <see cref="IFolder"/> interface.
/// </summary>
/// <remarks>
/// This class wraps a <see cref="StorageFolder"/> object and provides access to folder properties.
/// </remarks>
internal class WinUIFolder : IFolder
{
    /// <inheritdoc/>
    public string Name { get { return _Folder.Name; } }

    /// <inheritdoc/>
    public string Path { get { return _Folder.Path; } }

    /// <inheritdoc/>
    public string FolderRelativeId { get { return _Folder.FolderRelativeId; } }

    /// <summary>
    /// The underlying <see cref="StorageFolder"/> object.
    /// </summary>
    private readonly StorageFolder _Folder;

    /// <summary>
    /// Initializes a new instance of the <see cref="WinUIFolder"/> class.
    /// </summary>
    /// <param name="folder">The <see cref="StorageFolder"/> to wrap.</param>
    public WinUIFolder(StorageFolder folder)
    {
        _Folder = folder;
    }
}
