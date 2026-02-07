using Windows.Storage;

namespace Cyclotron.FileSystemAdapter.WinUI.Models;

/// <summary>
/// WinUI implementation of the <see cref="IFile"/> interface.
/// </summary>
/// <remarks>
/// This class wraps a <see cref="StorageFile"/> object and provides access to file properties and operations.
/// </remarks>
internal class WinUIFile : IFile
{
    /// <inheritdoc/>
    public string Name => _file.Name;

    /// <inheritdoc/>
    public string Path => _file.Path;

    /// <inheritdoc/>
    public string FolderRelativeId => _file.FolderRelativeId;

    /// <summary>
    /// The underlying <see cref="StorageFile"/> object.
    /// </summary>
    private readonly StorageFile _file;

    /// <summary>
    /// Initializes a new instance of the <see cref="WinUIFile"/> class.
    /// </summary>
    /// <param name="file">The <see cref="StorageFile"/> to wrap.</param>
    public WinUIFile(StorageFile file)
    {
        _file = file;
    }

    /// <inheritdoc/>
    public async Task<ulong> GetSizeAsync()
    {
        var basicProperties = await _file.GetBasicPropertiesAsync();
        return basicProperties.Size;
    }
}
