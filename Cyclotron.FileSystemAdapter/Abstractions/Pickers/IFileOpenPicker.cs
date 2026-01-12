namespace Cyclotron.FileSystemAdapter.Abstractions.Pickers;

public interface IFileOpenPicker
{
    string CommitButtonText { get; set; }

    IList<string> FileTypeFilter { get; }

    PickerLocationId SuggestedStartLocation { get; set; }

    PickerViewMode ViewMode { get; set; }

    Task<IFile> PickSingleFileAsync();

    Task<IList<IFile>> PickMultipleFilesAsync();
}
