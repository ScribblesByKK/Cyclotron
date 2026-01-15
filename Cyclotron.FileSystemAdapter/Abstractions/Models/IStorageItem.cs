namespace Cyclotron.FileSystemAdapter.Abstractions.Models;

public interface IStorageItem
{
    string Name { get; }

    string Path { get; }

    string FolderRelativeId { get; }
}
