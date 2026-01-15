namespace Cyclotron.FileSystemAdapter;

public static class Constants
{
    public static readonly IList<string> AllTypeFilter = new List<string>(1)
    {
        "*"
    }.AsReadOnly();

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

    public static readonly IList<string> AudioTypeFilter = new List<string>(6)
    {
        ".aac",
        ".mp3",
        ".wav",
        ".wma",
        ".flac",
        ".m4a"
    }.AsReadOnly();

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

