namespace Cyclotron.FileSystemAdapter.Abstractions.Pickers;

/// <summary>
/// Defines the contract for a file open picker that allows users to select files.
/// </summary>
public interface IFileOpenPicker
{
    /// <summary>
    /// Gets or sets the label for the commit button.
    /// </summary>
    /// <value>The text displayed on the commit button.</value>
    string CommitButtonText { get; set; }

    /// <summary>
    /// Gets the list of file type extensions to filter by in the picker.
    /// </summary>
    /// <remarks>
    /// File extensions should include the leading period (e.g., ".txt", ".pdf").
    /// </remarks>
    /// <value>A list of file extensions.</value>
    IList<string> FileTypeFilter { get; }

    /// <summary>
    /// Gets or sets the initial location for the picker to start browsing.
    /// </summary>
    /// <value>The suggested start location.</value>
    PickerLocationId SuggestedStartLocation { get; set; }

    /// <summary>
    /// Gets or sets the view mode for displaying files in the picker.
    /// </summary>
    /// <value>The view mode (list or thumbnail).</value>
    PickerViewMode ViewMode { get; set; }

    /// <summary>
    /// Asynchronously displays a dialog to pick a single file.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The result contains the selected file, or null if no file was selected.</returns>
    Task<IFile?> PickSingleFileAsync();

    /// <summary>
    /// Asynchronously displays a dialog to pick multiple files.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The result contains a list of selected files.</returns>
    Task<IList<IFile>> PickMultipleFilesAsync();
}