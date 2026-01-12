using Windows.Storage;

namespace Cyclotron.FileSystemAdapter.WinUI.Models;

internal class WinUIFolder : IFolder
{
    public string Name { get { return _Folder.Name; } }

    public string Path { get { return _Folder.Path; } }

    public string FolderRelativeId { get { return _Folder.FolderRelativeId; } }

    private readonly StorageFolder _Folder;

    public WinUIFolder(StorageFolder folder)
    {
        _Folder = folder;
    }
}
