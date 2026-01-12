namespace Cyclotron.FileSystemAdapter.Abstractions.Pickers;

public interface IFileSavePicker
{
    string CommitButtonText { get; set; }

    IDictionary<string, IList<string>> FileTypeChoices { get; }

    string SuggestedFileName { get; set; }

    IFile SuggestedSaveFile { get; set; }

    PickerLocationId SuggestedStartLocation { get; set; }

    Task<IFile> PickSaveFileAsync();
}

