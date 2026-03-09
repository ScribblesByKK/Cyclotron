using System.Diagnostics.CodeAnalysis;

namespace Cyclotron.FileSystemAdapter.WinUI.Pickers;

/// <summary>
/// WinUI implementation of the <see cref="IFileSavePicker"/> interface.
/// </summary>
/// <remarks>
/// This class provides a file save picker dialog for WinUI applications.
/// </remarks>
[ExcludeFromCodeCoverage(Justification = "Opens an interactive OS shell dialog; cannot be automated in tests.")]
internal class WinUIFileSavePicker : IFileSavePicker
{
    /// <inheritdoc/>
    public required string CommitButtonText { get; set; }

    /// <inheritdoc/>
    public IDictionary<string, IList<string>> FileTypeChoices { get; }

    /// <inheritdoc/>
    public required string SuggestedFileName { get; set; }

    /// <inheritdoc/>
    public required IFile SuggestedSaveFile { get; set; }

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
        var fileSavePicker = new Microsoft.Windows.Storage.Pickers.FileSavePicker(WindowUtil.GetActiveWindowId())
        {
            CommitButtonText = CommitButtonText,
            SuggestedFileName = SuggestedFileName,
            SuggestedStartLocation = (Microsoft.Windows.Storage.Pickers.PickerLocationId)SuggestedStartLocation
        };

        foreach (var fileTypeChoice in FileTypeChoices)
        {
            fileSavePicker.FileTypeChoices.Add(fileTypeChoice.Key, fileTypeChoice.Value);
        }

        var fileResult = await fileSavePicker.PickSaveFileAsync();
        if (fileResult == null)
        {
            return default!;
        }

        var file = await Windows.Storage.StorageFile.GetFileFromPathAsync(fileResult.Path);
        return file.AsIFile();
    }
}
