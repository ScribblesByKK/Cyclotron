using Cyclotron.FileSystemAdapter.Abstractions.Pickers;

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
    public required string CommitButtonText { get; set; }

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
        FileTypeFilter = [];
    }

    /// <inheritdoc/>
    public async Task<IFile?> PickSingleFileAsync()
    {
        var fileOpenPicker = new Microsoft.Windows.Storage.Pickers.FileOpenPicker(WindowUtil.GetActiveWindowId())
        {
            CommitButtonText = CommitButtonText,
            SuggestedStartLocation = (Microsoft.Windows.Storage.Pickers.PickerLocationId)SuggestedStartLocation,
            ViewMode = (Microsoft.Windows.Storage.Pickers.PickerViewMode)ViewMode
        };

        foreach (var fileType in FileTypeFilter)
        {
            fileOpenPicker.FileTypeFilter.Add(fileType);
        }

        var fileResult = await fileOpenPicker.PickSingleFileAsync();
        if (fileResult == null)
        {
            return default;
        }

        var file = await Windows.Storage.StorageFile.GetFileFromPathAsync(fileResult.Path);
        return file.AsIFile();
    }

    /// <inheritdoc/>
    public async Task<IList<IFile>> PickMultipleFilesAsync()
    {
        var fileOpenPicker = new Microsoft.Windows.Storage.Pickers.FileOpenPicker(WindowUtil.GetActiveWindowId())
        {
            CommitButtonText = CommitButtonText,
            SuggestedStartLocation = (Microsoft.Windows.Storage.Pickers.PickerLocationId)SuggestedStartLocation,
            ViewMode = (Microsoft.Windows.Storage.Pickers.PickerViewMode)ViewMode
        };

        foreach (var fileType in FileTypeFilter)
        {
            fileOpenPicker.FileTypeFilter.Add(fileType);
        }

        var filesResult = await fileOpenPicker.PickMultipleFilesAsync();
        if (filesResult == default || filesResult.Count == 0)
        {
            return [];
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
