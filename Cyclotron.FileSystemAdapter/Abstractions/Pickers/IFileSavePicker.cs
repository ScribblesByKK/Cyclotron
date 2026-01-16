namespace Cyclotron.FileSystemAdapter.Abstractions.Pickers;

/// <summary>
/// Defines the contract for a file save picker that allows users to select a location and name for saving a file.
/// </summary>
public interface IFileSavePicker
{
    /// <summary>
    /// Gets or sets the label for the commit button.
    /// </summary>
    /// <value>The text displayed on the commit button.</value>
    string CommitButtonText { get; set; }

    /// <summary>
    /// Gets a dictionary of file type choices where the key is the file type name and the value is a list of extensions.
    /// </summary>
    /// <remarks>
    /// File extensions should include the leading period (e.g., ".txt", ".pdf").
    /// </remarks>
    /// <value>A dictionary mapping file type names to lists of extensions.</value>
    IDictionary<string, IList<string>> FileTypeChoices { get; }

    /// <summary>
    /// Gets or sets the suggested file name for the save operation.
    /// </summary>
    /// <value>The suggested file name without the extension.</value>
    string SuggestedFileName { get; set; }

    /// <summary>
    /// Gets or sets the suggested file to save (for save as operations).
    /// </summary>
    /// <value>The suggested file.</value>
    IFile SuggestedSaveFile { get; set; }

    /// <summary>
    /// Gets or sets the initial location for the picker to start browsing.
    /// </summary>
    /// <value>The suggested start location.</value>
    PickerLocationId SuggestedStartLocation { get; set; }

    /// <summary>
    /// Asynchronously displays a dialog to pick a save file location and name.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The result contains the file location for saving, or null if the operation was canceled.</returns>
    Task<IFile> PickSaveFileAsync();
}

