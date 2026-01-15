using Microsoft.Windows.Storage.Pickers;
using Windows.Storage;

namespace Cyclotron.FileSystemAdapter.WinUI.Pickers;

internal class WinUIFolderPicker : IFolderPicker
{
    public string CommitButtonText { get; set; }

    public IList<string> FileTypeFilter { get; }

    public PickerLocationId SuggestedStartLocation { get; set; }

    public PickerViewMode ViewMode { get; set; }

    public async Task<IFolder> PickFolderAsync()
    {
        var folderPicker = new FolderPicker(default)
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
        var folder = await StorageFolder.GetFolderFromPathAsync(folderResult.Path);
        return folder.AsIFolder();
    }
}
