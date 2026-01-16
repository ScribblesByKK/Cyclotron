namespace Cyclotron.FileSystemAdapter;

/// <summary>
/// Specifies the behavior to apply when a file or folder with the same name already exists.
/// </summary>
public enum NameCollisionOption
{
    /// <summary>
    /// Generate a unique name by appending a number to the original name.
    /// </summary>
    GenerateUniqueName = 0,

    /// <summary>
    /// Replace the existing file or folder.
    /// </summary>
    ReplaceExisting,

    /// <summary>
    /// Fail the operation if a file or folder with the same name exists.
    /// </summary>
    FailIfExists
}

/// <summary>
/// Specifies the options for deleting a storage item.
/// </summary>
public enum StorageDeletionOption
{
    /// <summary>
    /// Move the item to the Recycle Bin.
    /// </summary>
    Default,

    /// <summary>
    /// Permanently delete the item without moving it to the Recycle Bin.
    /// </summary>
    PermanentDelete
}

/// <summary>
/// Specifies the access mode for opening a file.
/// </summary>
public enum FileAcessMode
{
    /// <summary>
    /// Open the file for reading only.
    /// </summary>
    Read,

    /// <summary>
    /// Open the file for both reading and writing.
    /// </summary>
    ReadWrite
}

/// <summary>
/// Specifies options for opening a storage item.
/// </summary>
public enum StorageOpenOptions
{
    /// <summary>
    /// No specific options are applied.
    /// </summary>
    None,

    /// <summary>
    /// Allow only readers to access the item.
    /// </summary>
    AllowOnlyReaders,

    /// <summary>
    /// Allow both readers and writers to access the item.
    /// </summary>
    AllowReadersAndWriters
}

/// <summary>
/// Specifies the behavior when creating a storage item that might already exist.
/// </summary>
public enum CreationCollisionOption
{
    /// <summary>
    /// Generate a unique name if the item already exists.
    /// </summary>
    GenerateUniqueName = 0,

    /// <summary>
    /// Replace the existing item if it exists.
    /// </summary>
    ReplaceExisting,

    /// <summary>
    /// Fail if the item already exists.
    /// </summary>
    FailIfExists,

    /// <summary>
    /// Open the existing item if it exists, otherwise create a new one.
    /// </summary>
    OpenIfExists
}

/// <summary>
/// Specifies the view mode for a file or folder picker.
/// </summary>
public enum PickerViewMode
{
    /// <summary>
    /// Display items in a list view.
    /// </summary>
    List = 0,

    /// <summary>
    /// Display items as thumbnails.
    /// </summary>
    Thumbnail
}

/// <summary>
/// Specifies the location for the picker's suggested start directory.
/// </summary>
public enum PickerLocationId
{
    /// <summary>
    /// The Documents library.
    /// </summary>
    DocumentsLibrary = 0,

    /// <summary>
    /// The Computer folder (shows all drives).
    /// </summary>
    ComputerFolder,

    /// <summary>
    /// The Desktop folder.
    /// </summary>
    Desktop,

    /// <summary>
    /// The Downloads folder.
    /// </summary>
    Downloads,

    /// <summary>
    /// The HomeGroup folder.
    /// </summary>
    HomeGroup,

    /// <summary>
    /// The Music library.
    /// </summary>
    MusicLibrary,

    /// <summary>
    /// The Pictures library.
    /// </summary>
    PicturesLibrary,

    /// <summary>
    /// The Videos library.
    /// </summary>
    VideosLibrary,

    /// <summary>
    /// The 3D Objects folder.
    /// </summary>
    Objects3D,

    /// <summary>
    /// An unspecified location.
    /// </summary>
    Unspecified
}

/// <summary>
/// Specifies the type of a file based on its content or purpose.
/// </summary>
public enum FileType
{
    /// <summary>
    /// Unknown file type.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// Image file type.
    /// </summary>
    Image,

    /// <summary>
    /// Video file type.
    /// </summary>
    Video,

    /// <summary>
    /// Audio file type.
    /// </summary>
    Audio,

    /// <summary>
    /// Compressed/Archive file type.
    /// </summary>
    Zip,

    /// <summary>
    /// Document file type.
    /// </summary>
    Document
}
