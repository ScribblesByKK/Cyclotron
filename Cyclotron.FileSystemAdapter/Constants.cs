namespace Cyclotron.FileSystemAdapter;

/// <summary>
/// Provides predefined file type filter constants for file dialogs and pickers.
/// </summary>
public static class Constants
{
    /// <summary>
    /// Represents a filter that includes all file types.
    /// </summary>
    public static readonly IList<string> AllTypeFilter = new List<string>(1)
    {
        "*"
    }.AsReadOnly();

    /// <summary>
    /// Represents a filter that includes common image file types: BMP, GIF, JPEG, PNG, TIFF, ICO, and HEIC.
    /// </summary>
    public static readonly IList<string> ImageTypeFilter = new List<string>(9)
    {
        ".bmp",
        ".gif",
        ".jpeg",
        ".jpg",
        ".png",
        ".tif",
        ".tiff",
        ".ico",
        ".heic"
    }.AsReadOnly();

    /// <summary>
    /// Represents a filter that includes common video file types: 3GP, AVI, MP4, M4V, MOV, WMV, and FLV.
    /// </summary>
    public static readonly IList<string> VideoTypeFilter = new List<string>(7)
    {
        ".3gp",
        ".avi",
        ".mp4",
        ".m4v",
        ".mov",
        ".wmv",
        ".flv"
    }.AsReadOnly();

    /// <summary>
    /// Represents a filter that includes common audio file types: AAC, MP3, WAV, WMA, FLAC, and M4A.
    /// </summary>
    public static readonly IList<string> AudioTypeFilter = new List<string>(6)
    {
        ".aac",
        ".mp3",
        ".wav",
        ".wma",
        ".flac",
        ".m4a"
    }.AsReadOnly();

    /// <summary>
    /// Represents a filter that includes common document file types: DOC, DOCX, PDF, PPT, PPTX, XLS, XLSX, and TXT.
    /// </summary>
    public static readonly IList<string> DocumentTypeFilter = new List<string>(8)
    {
        ".doc",
        ".docx",
        ".pdf",
        ".ppt",
        ".pptx",
        ".xls",
        ".xlsx",
        ".txt"
    }.AsReadOnly();

    /// <summary>
    /// Represents a filter that includes common compressed/archive file types: ZIP, RAR, 7Z, TAR, GZ, BZ2, XZ, and ISO.
    /// </summary>
    public static readonly IList<string> CompressedTypeFilter = new List<string>(8)
    {
        ".zip",
        ".rar",
        ".7z",
        ".tar",
        ".gz",
        ".bz2",
        ".xz",
        ".iso"
    }.AsReadOnly();
}

