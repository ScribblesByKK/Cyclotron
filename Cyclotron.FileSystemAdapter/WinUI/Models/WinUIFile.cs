using Windows.Storage;

namespace Cyclotron.FileSystemAdapter.WinUI.Models;

/// <summary>
/// WinUI implementation of the <see cref="IFile"/> interface.
/// </summary>
/// <remarks>
/// This class wraps a <see cref="StorageFile"/> object and provides access to file properties and operations.
/// </remarks>
public class WinUIFile : IFile
{
    /// <inheritdoc/>
    public string Name { get { return _File.Name; } }

    /// <inheritdoc/>
    public string Path { get { return _File.Path; } }

    /// <inheritdoc/>
    public string FolderRelativeId { get { return _File.FolderRelativeId; } }

    /// <summary>
    /// The underlying <see cref="StorageFile"/> object.
    /// </summary>
    private readonly StorageFile _File;

    /// <summary>
    /// Initializes a new instance of the <see cref="WinUIFile"/> class.
    /// </summary>
    /// <param name="file">The <see cref="StorageFile"/> to wrap.</param>
    public WinUIFile(StorageFile file)
    {
        _File = file;
    }

    /// <inheritdoc/>
    public async Task<ulong> GetSizeAsync()
    {
        var basicProperties = await _File.GetBasicPropertiesAsync();
        return basicProperties.Size;
    }
}
