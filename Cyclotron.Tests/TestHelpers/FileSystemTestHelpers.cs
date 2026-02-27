using AwesomeAssertions;

namespace Cyclotron.Tests.TestHelpers;

/// <summary>
/// Helper methods for file system test assertions and setup.
/// </summary>
public static class FileSystemTestHelpers
{
    /// <summary>
    /// Creates a test file with specified content.
    /// </summary>
    public static async Task CreateTestFileAsync(string path, string content)
    {
        await File.WriteAllTextAsync(path, content);
    }

    /// <summary>
    /// Creates a test directory.
    /// </summary>
    public static void CreateTestDirectory(string path)
    {
        Directory.CreateDirectory(path);
    }

    /// <summary>
    /// Asserts that a file exists at the specified path.
    /// </summary>
    public static void AssertFileExists(string path)
    {
        File.Exists(path).Should().BeTrue($"file should exist at {path}");
    }

    /// <summary>
    /// Asserts that a file has the expected content.
    /// </summary>
    public static async Task AssertFileContentAsync(string path, string expectedContent)
    {
        var actualContent = await File.ReadAllTextAsync(path);
        actualContent.Should().Be(expectedContent);
    }

    /// <summary>
    /// Creates a large file for stress testing.
    /// </summary>
    public static async Task CreateLargeFileAsync(string path, int sizeInMB)
    {
        var bytes = GenerateRandomBytes(sizeInMB * 1024 * 1024);
        await File.WriteAllBytesAsync(path, bytes);
    }

    /// <summary>
    /// Generates random bytes for testing.
    /// </summary>
    public static byte[] GenerateRandomBytes(int count)
    {
        var bytes = new byte[count];
        Random.Shared.NextBytes(bytes);
        return bytes;
    }
}
