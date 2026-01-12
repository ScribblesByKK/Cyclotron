using Microsoft.Windows.Storage.Pickers;
using Windows.Storage;

namespace Cyclotron.FileSystemAdapter.WinUI.Pickers;

internal class WinUIFileSavePicker : IFileSavePicker
{
    public string CommitButtonText { get; set; }

    public IDictionary<string, IList<string>> FileTypeChoices { get; }

    public string SuggestedFileName { get; set; }

    public IFile SuggestedSaveFile { get; set; }

    public PickerLocationId SuggestedStartLocation { get; set; }

    public WinUIFileSavePicker()
    {
        FileTypeChoices = new Dictionary<string, IList<string>>();
    }

    public async Task<IFile> PickSaveFileAsync()
    {
        var fileSavePicker = new FileSavePicker(default)
        {
            CommitButtonText = this.CommitButtonText,
            SuggestedFileName = this.SuggestedFileName,
            SuggestedStartLocation = (Microsoft.Windows.Storage.Pickers.PickerLocationId)this.SuggestedStartLocation
        };

        foreach (var fileTypeChoice in this.FileTypeChoices)
        {
            fileSavePicker.FileTypeChoices.Add(fileTypeChoice.Key, fileTypeChoice.Value);
        }

        var fileResult = await fileSavePicker.PickSaveFileAsync();
        if (string.IsNullOrEmpty(fileResult.Path))
        {
            return default;
        }

        var file = await StorageFile.GetFileFromPathAsync(fileResult.Path);
        return file.AsIFile();
    }
}
