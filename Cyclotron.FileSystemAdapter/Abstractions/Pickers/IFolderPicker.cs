namespace Cyclotron.FileSystemAdapter.Abstractions.Pickers;

/// <summary>
/// Defines the contract for a folder picker that allows users to select a folder.
/// </summary>
public interface IFolderPicker
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
    /// This filter may be used to show only folders containing files of specific types.
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
    /// Gets or sets the view mode for displaying folders in the picker.
    /// </summary>
    /// <value>The view mode (list or thumbnail).</value>
    PickerViewMode ViewMode { get; set; }

    /// <summary>
    /// Asynchronously displays a dialog to pick a single folder.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The result contains the selected folder, or null if no folder was selected.</returns>
    Task<IFolder> PickFolderAsync();
}

