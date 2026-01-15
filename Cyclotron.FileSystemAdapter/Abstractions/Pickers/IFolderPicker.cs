namespace Cyclotron.FileSystemAdapter.Abstractions.Pickers;

public interface IFolderPicker
{
    string CommitButtonText { get; set; }

    IList<string> FileTypeFilter { get; }

    PickerLocationId SuggestedStartLocation { get; set; }

    PickerViewMode ViewMode { get; set; }

    Task<IFolder> PickFolderAsync();
}

