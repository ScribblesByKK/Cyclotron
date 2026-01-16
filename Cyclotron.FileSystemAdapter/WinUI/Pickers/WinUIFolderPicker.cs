using Cyclotron.FileSystemAdapter.WinUI;

namespace Cyclotron.FileSystemAdapter.WinUI.Pickers;

/// <summary>
/// WinUI implementation of the <see cref="IFolderPicker"/> interface.
/// </summary>
/// <remarks>
/// This class provides a folder picker dialog for WinUI applications.
/// </remarks>
internal class WinUIFolderPicker : IFolderPicker
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
    /// Initializes a new instance of the <see cref="WinUIFolderPicker"/> class.
    /// </summary>
    public WinUIFolderPicker()
    {
        FileTypeFilter = new List<string>();
    }

    /// <inheritdoc/>
    public async Task<IFolder> PickFolderAsync()
    {
        var folderPicker = new Microsoft.Windows.Storage.Pickers.FolderPicker(default)
        {
            CommitButtonText = this.CommitButtonText,
            SuggestedStartLocation = (Microsoft.Windows.Storage.Pickers.PickerLocationId)this.SuggestedStartLocation,
            ViewMode = (Microsoft.Windows.Storage.Pickers.PickerViewMode)this.ViewMode
        };
        var folderResult = await folderPicker.PickSingleFolderAsync();
        if (string.IsNullOrEmpty(folderResult.Path))
        {
            return default;
        }
        var folder = await Windows.Storage.StorageFolder.GetFolderFromPathAsync(folderResult.Path);
        return folder.AsIFolder();
    }
}
