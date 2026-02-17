namespace Cyclotron.Tests.Integration.Fixtures;

/// <summary>
/// Provides isolated temporary directory for file system tests.
/// Automatically cleans up after tests complete.
/// </summary>
public class TempFileSystemFixture : IAsyncDisposable
{
    public string RootPath { get; }

    public TempFileSystemFixture()
    {
        var tempBase = Path.Combine(Path.GetTempPath(), "Cyclotron.Tests");
        RootPath = Path.Combine(tempBase, Guid.NewGuid().ToString());
        Directory.CreateDirectory(RootPath);
    }

    /// <summary>
    /// Generates a unique file path in the temp directory.
    /// </summary>
    public string GetTempFilePath(string? fileName = null)
    {
        fileName ??= $"{Guid.NewGuid()}.tmp";
        return Path.Combine(RootPath, fileName);
    }

    /// <summary>
    /// Generates a unique directory path in the temp directory.
    /// </summary>
    public string GetTempDirectoryPath(string? dirName = null)
    {
        dirName ??= Guid.NewGuid().ToString();
        return Path.Combine(RootPath, dirName);
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            if (Directory.Exists(RootPath))
            {
                Directory.Delete(RootPath, recursive: true);
            }
        }
        catch
        {
            // Best effort cleanup
        }

        await Task.CompletedTask;
    }
}
