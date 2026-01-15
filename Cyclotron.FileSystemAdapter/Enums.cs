namespace Cyclotron.FileSystemAdapter;

public enum NameCollisionOption
{
    GenerateUniqueName = 0,
    ReplaceExisting,
    FailIfExists
}

public enum StorageDeletionOption
{
    Default,
    PermanentDelete
}

public enum FileAcessMode
{
    Read,
    ReadWrite
}

public enum StorageOpenOptions
{
    None,
    AllowOnlyReaders,
    AllowReadersAndWriters
}

public enum CreationCollisionOption
{
    GenerateUniqueName = 0,
    ReplaceExisting,
    FailIfExists,
    OpenIfExists
}

public enum PickerViewMode
{
    List = 0,
    Thumbnail
}

public enum PickerLocationId
{
    DocumentsLibrary = 0,
    ComputerFolder,
    Desktop,
    Downloads,
    HomeGroup,
    MusicLibrary,
    PicturesLibrary,
    VideosLibrary,
    Objects3D,
    Unspecified
}

public enum FileType
{
    Unknown = 0,
    Image,
    Video,
    Audio,
    Zip,
    Document
}
