using Microsoft.Windows.Storage.Pickers;
using Windows.Storage;

namespace Cyclotron.FileSystemAdapter.WinUI.Pickers;

internal class WinUIFileOpenPicker : IFileOpenPicker
{
    public string CommitButtonText { get; set; }

    public IList<string> FileTypeFilter { get; }

    public PickerLocationId SuggestedStartLocation { get; set; }

    public PickerViewMode ViewMode { get; set; }

    public WinUIFileOpenPicker()
    {
        FileTypeFilter = new List<string>();
    }

    public async Task<IFile> PickSingleFileAsync()
    {
        var fileOpenPicker = new FileOpenPicker(default)
        {
            CommitButtonText = this.CommitButtonText,
            SuggestedStartLocation = (Microsoft.Windows.Storage.Pickers.PickerLocationId)this.SuggestedStartLocation,
            ViewMode = (Microsoft.Windows.Storage.Pickers.PickerViewMode)this.ViewMode
        };

        foreach (var fileType in this.FileTypeFilter)
        {
            fileOpenPicker.FileTypeFilter.Add(fileType);
        }

        var fileResult = await fileOpenPicker.PickSingleFileAsync();
        if (string.IsNullOrEmpty(fileResult.Path))
        {
            return default;
        }

        var file = await StorageFile.GetFileFromPathAsync(fileResult.Path);
        return file.AsIFile();
    }

    public async Task<IList<IFile>> PickMultipleFilesAsync()
    {
        var fileOpenPicker = new FileOpenPicker(default)
        {
            CommitButtonText = this.CommitButtonText,
            SuggestedStartLocation = (Microsoft.Windows.Storage.Pickers.PickerLocationId)this.SuggestedStartLocation,
            ViewMode = (Microsoft.Windows.Storage.Pickers.PickerViewMode)this.ViewMode
        };

        foreach (var fileType in this.FileTypeFilter)
        {
            fileOpenPicker.FileTypeFilter.Add(fileType);
        }

        var filesResult = await fileOpenPicker.PickMultipleFilesAsync();
        if (filesResult == null || filesResult.Count == 0)
        {
            return new List<IFile>();
        }

        var files = new List<IFile>();
        foreach (var fileResult in filesResult)
        {
            if (!string.IsNullOrEmpty(fileResult.Path))
            {
                var file = await StorageFile.GetFileFromPathAsync(fileResult.Path);
                files.Add(file.AsIFile());
            }
        }

        return files;
    }
}
