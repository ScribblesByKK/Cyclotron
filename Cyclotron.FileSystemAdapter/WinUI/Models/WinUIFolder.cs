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
    public string Name => _folder.Name;

    /// <inheritdoc/>
    public string Path => _folder.Path;

    /// <inheritdoc/>
    public string FolderRelativeId => _folder.FolderRelativeId;

    /// <summary>
    /// The underlying <see cref="StorageFolder"/> object.
    /// </summary>
    private readonly StorageFolder _folder;

    /// <summary>
    /// Initializes a new instance of the <see cref="WinUIFolder"/> class.
    /// </summary>
    /// <param name="folder">The <see cref="StorageFolder"/> to wrap.</param>
    public WinUIFolder(StorageFolder folder)
    {
        _folder = folder;
    }
}
