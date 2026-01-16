using Microsoft.Windows.Storage.Pickers;
using Windows.Storage;

namespace Cyclotron.FileSystemAdapter.WinUI.Pickers;

/// <summary>
/// WinUI implementation of the <see cref="IFileSavePicker"/> interface.
/// </summary>
/// <remarks>
/// This class provides a file save picker dialog for WinUI applications.
/// </remarks>
internal class WinUIFileSavePicker : IFileSavePicker
{
    /// <inheritdoc/>
    public string CommitButtonText { get; set; }

    /// <inheritdoc/>
    public IDictionary<string, IList<string>> FileTypeChoices { get; }

    /// <inheritdoc/>
    public string SuggestedFileName { get; set; }

    /// <inheritdoc/>
    public IFile SuggestedSaveFile { get; set; }

    /// <inheritdoc/>
    public PickerLocationId SuggestedStartLocation { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="WinUIFileSavePicker"/> class.
    /// </summary>
    public WinUIFileSavePicker()
    {
        FileTypeChoices = new Dictionary<string, IList<string>>();
    }

    /// <inheritdoc/>
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
