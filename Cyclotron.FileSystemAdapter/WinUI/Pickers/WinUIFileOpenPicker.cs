using Cyclotron.FileSystemAdapter.WinUI;

namespace Cyclotron.FileSystemAdapter.WinUI.Pickers;

/// <summary>
/// WinUI implementation of the <see cref="IFileOpenPicker"/> interface.
/// </summary>
/// <remarks>
/// This class provides a file open picker dialog for WinUI applications.
/// </remarks>
internal class WinUIFileOpenPicker : IFileOpenPicker
{
    /// <inheritdoc/>
    public string CommitButtonText { get; set; }

    /// <inheritdoc/>
    public IList<string> FileTypeFilter { get; }

    /// <inheritdoc/>
    public PickerLocationId SuggestedStartLocation { get; set; }

    /// <inheritdoc/>
    public PickerViewMode ViewMode { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="WinUIFileOpenPicker"/> class.
    /// </summary>
    public WinUIFileOpenPicker()
    {
        FileTypeFilter = new List<string>();
    }

    /// <inheritdoc/>
    public async Task<IFile> PickSingleFileAsync()
    {
        var fileOpenPicker = new Microsoft.Windows.Storage.Pickers.FileOpenPicker(default)
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

        var file = await Windows.Storage.StorageFile.GetFileFromPathAsync(fileResult.Path);
        return file.AsIFile();
    }

    /// <inheritdoc/>
    public async Task<IList<IFile>> PickMultipleFilesAsync()
    {
        var fileOpenPicker = new Microsoft.Windows.Storage.Pickers.FileOpenPicker(default)
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
                var file = await Windows.Storage.StorageFile.GetFileFromPathAsync(fileResult.Path);
                files.Add(file.AsIFile());
            }
        }

        return files;
    }
}
