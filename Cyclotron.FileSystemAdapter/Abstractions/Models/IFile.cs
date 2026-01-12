namespace Cyclotron.FileSystemAdapter.Abstractions.Models;

public interface IFile : IStorageItem
{
    Task<ulong> GetSizeAsync();
}
