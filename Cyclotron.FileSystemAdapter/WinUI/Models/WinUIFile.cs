using Windows.Storage;

namespace Cyclotron.FileSystemAdapter.WinUI.Models;

public class WinUIFile : IFile
{
    public string Name { get { return _File.Name; } }

    public string Path { get { return _File.Path; } }

    public string FolderRelativeId { get { return _File.FolderRelativeId; } }

    private readonly StorageFile _File;

    public WinUIFile(StorageFile file)
    {
        _File = file;
    }

    public async Task<ulong> GetSizeAsync()
    {
        var basicProperties = await _File.GetBasicPropertiesAsync();
        return basicProperties.Size;
    }
}
