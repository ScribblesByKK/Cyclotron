using Cyclotron.Tests.Integration.Fixtures;
using Cyclotron.Tests.TestHelpers;
using FluentAssertions;

namespace Cyclotron.Tests.Integration.FileSystemAdapter;

[Category("Integration")]
[Category("FileSystemAdapter")]
public class FileSystemAdapterIntegrationTests
{
    private TempFileSystemFixture _fixture = null!;

    [Before(Test)]
    public void Setup()
    {
        _fixture = new TempFileSystemFixture();
    }

    [After(Test)]
    public async Task Cleanup()
    {
        await _fixture.DisposeAsync();
    }

    #region Write and Read Operations

    [Test]
    public async Task WriteAllTextAsync_ThenReadAllTextAsync_ReturnsSameContent()
    {
        var filePath = _fixture.GetTempFilePath("test.txt");
        const string content = "Hello, Cyclotron!";

        await File.WriteAllTextAsync(filePath, content);
        var readContent = await File.ReadAllTextAsync(filePath);

        readContent.Should().Be(content);
    }

    [Test]
    public async Task WriteAllTextAsync_WithUtf8Encoding_PreservesContent()
    {
        var filePath = _fixture.GetTempFilePath("utf8.txt");
        const string content = "こんにちは世界 🌍 Ñoño";

        await File.WriteAllTextAsync(filePath, content, System.Text.Encoding.UTF8);
        var readContent = await File.ReadAllTextAsync(filePath, System.Text.Encoding.UTF8);

        readContent.Should().Be(content);
    }

    [Test]
    public async Task WriteAllBytesAsync_ThenReadAllBytesAsync_PreservesBinaryData()
    {
        var filePath = _fixture.GetTempFilePath("binary.dat");
        var bytes = FileSystemTestHelpers.GenerateRandomBytes(1024);

        await File.WriteAllBytesAsync(filePath, bytes);
        var readBytes = await File.ReadAllBytesAsync(filePath);

        readBytes.Should().BeEquivalentTo(bytes);
    }

    [Test]
    public async Task WriteAllBytesAsync_EmptyArray_CreatesEmptyFile()
    {
        var filePath = _fixture.GetTempFilePath("empty.dat");
        var bytes = Array.Empty<byte>();

        await File.WriteAllBytesAsync(filePath, bytes);
        var readBytes = await File.ReadAllBytesAsync(filePath);

        readBytes.Should().BeEmpty();
    }

    #endregion

    #region File Existence

    [Test]
    public async Task FileExists_ReturnsTrue_WhenFileExists()
    {
        var filePath = _fixture.GetTempFilePath("exists.txt");
        await File.WriteAllTextAsync(filePath, "content");

        File.Exists(filePath).Should().BeTrue();
    }

    [Test]
    public void FileExists_ReturnsFalse_WhenFileDoesNotExist()
    {
        var filePath = _fixture.GetTempFilePath("nonexistent.txt");

        File.Exists(filePath).Should().BeFalse();
    }

    #endregion

    #region Directory Operations

    [Test]
    public void CreateDirectory_CreatesNestedDirectories()
    {
        var dirPath = Path.Combine(_fixture.RootPath, "level1", "level2", "level3");

        Directory.CreateDirectory(dirPath);

        Directory.Exists(dirPath).Should().BeTrue();
    }

    [Test]
    public void CreateDirectory_IsIdempotent()
    {
        var dirPath = _fixture.GetTempDirectoryPath("idempotent");

        Directory.CreateDirectory(dirPath);
        Directory.CreateDirectory(dirPath);

        Directory.Exists(dirPath).Should().BeTrue();
    }

    [Test]
    public void DeleteDirectory_RemovesEmptyDirectory()
    {
        var dirPath = _fixture.GetTempDirectoryPath("todelete");
        Directory.CreateDirectory(dirPath);

        Directory.Delete(dirPath);

        Directory.Exists(dirPath).Should().BeFalse();
    }

    [Test]
    public async Task DeleteDirectory_Recursive_RemovesNestedContent()
    {
        var dirPath = _fixture.GetTempDirectoryPath("recursive");
        var subDir = Path.Combine(dirPath, "sub");
        Directory.CreateDirectory(subDir);
        await File.WriteAllTextAsync(Path.Combine(subDir, "file.txt"), "content");

        Directory.Delete(dirPath, recursive: true);

        Directory.Exists(dirPath).Should().BeFalse();
    }

    [Test]
    public async Task EnumerateFiles_ReturnsCorrectFiles()
    {
        var dirPath = _fixture.GetTempDirectoryPath("enumerate");
        Directory.CreateDirectory(dirPath);
        await File.WriteAllTextAsync(Path.Combine(dirPath, "file1.txt"), "a");
        await File.WriteAllTextAsync(Path.Combine(dirPath, "file2.txt"), "b");
        await File.WriteAllTextAsync(Path.Combine(dirPath, "file3.log"), "c");

        var txtFiles = Directory.EnumerateFiles(dirPath, "*.txt").ToList();

        txtFiles.Should().HaveCount(2);
    }

    [Test]
    public void EnumerateFiles_EmptyDirectory_ReturnsEmpty()
    {
        var dirPath = _fixture.GetTempDirectoryPath("empty");
        Directory.CreateDirectory(dirPath);

        var files = Directory.EnumerateFiles(dirPath).ToList();

        files.Should().BeEmpty();
    }

    [Test]
    public async Task EnumerateFiles_Recursive_TraversesSubdirectories()
    {
        var dirPath = _fixture.GetTempDirectoryPath("deepenum");
        var subDir = Path.Combine(dirPath, "sub");
        Directory.CreateDirectory(subDir);
        await File.WriteAllTextAsync(Path.Combine(dirPath, "root.txt"), "r");
        await File.WriteAllTextAsync(Path.Combine(subDir, "child.txt"), "c");

        var files = Directory.EnumerateFiles(dirPath, "*.*", SearchOption.AllDirectories).ToList();

        files.Should().HaveCount(2);
    }

    #endregion

    #region File Manipulation

    [Test]
    public async Task FileCopy_CreatesIdenticalCopy()
    {
        var sourcePath = _fixture.GetTempFilePath("source.txt");
        var destPath = _fixture.GetTempFilePath("dest.txt");
        const string content = "Copy this content";
        await File.WriteAllTextAsync(sourcePath, content);

        File.Copy(sourcePath, destPath);

        File.Exists(destPath).Should().BeTrue();
        (await File.ReadAllTextAsync(destPath)).Should().Be(content);
    }

    [Test]
    public async Task FileMove_MovesFileToNewLocation()
    {
        var sourcePath = _fixture.GetTempFilePath("tomove.txt");
        var destPath = _fixture.GetTempFilePath("moved.txt");
        await File.WriteAllTextAsync(sourcePath, "move me");

        File.Move(sourcePath, destPath);

        File.Exists(sourcePath).Should().BeFalse();
        File.Exists(destPath).Should().BeTrue();
    }

    [Test]
    public async Task FileDelete_RemovesFileFromDisk()
    {
        var filePath = _fixture.GetTempFilePath("todelete.txt");
        await File.WriteAllTextAsync(filePath, "delete me");

        File.Delete(filePath);

        File.Exists(filePath).Should().BeFalse();
    }

    [Test]
    public async Task FileMove_AcrossDirectories_Works()
    {
        var dir1 = _fixture.GetTempDirectoryPath("dir1");
        var dir2 = _fixture.GetTempDirectoryPath("dir2");
        Directory.CreateDirectory(dir1);
        Directory.CreateDirectory(dir2);
        var sourcePath = Path.Combine(dir1, "file.txt");
        var destPath = Path.Combine(dir2, "file.txt");
        await File.WriteAllTextAsync(sourcePath, "cross-dir move");

        File.Move(sourcePath, destPath);

        File.Exists(sourcePath).Should().BeFalse();
        File.Exists(destPath).Should().BeTrue();
        (await File.ReadAllTextAsync(destPath)).Should().Be("cross-dir move");
    }

    #endregion

    #region Special Characters

    [Test]
    public async Task FileOperations_WithSpacesInName_Work()
    {
        var filePath = _fixture.GetTempFilePath("file with spaces.txt");
        await File.WriteAllTextAsync(filePath, "content");

        File.Exists(filePath).Should().BeTrue();
        (await File.ReadAllTextAsync(filePath)).Should().Be("content");
    }

    [Test]
    public async Task FileOperations_WithUnicodeInName_Work()
    {
        var filePath = _fixture.GetTempFilePath("файл_données_数据.txt");
        await File.WriteAllTextAsync(filePath, "unicode content");

        File.Exists(filePath).Should().BeTrue();
        (await File.ReadAllTextAsync(filePath)).Should().Be("unicode content");
    }

    #endregion

    #region Error Scenarios

    [Test]
    public void ReadNonExistentFile_ThrowsFileNotFoundException()
    {
        var filePath = _fixture.GetTempFilePath("nonexistent.txt");

        var act = () => File.ReadAllText(filePath);

        act.Should().Throw<FileNotFoundException>();
    }

    [Test]
    public void DeleteNonExistentFile_DoesNotThrow()
    {
        var filePath = _fixture.GetTempFilePath("nonexistent.txt");

        var act = () => File.Delete(filePath);

        act.Should().NotThrow();
    }

    #endregion

    #region Concurrency

    [Test]
    public async Task ParallelWrites_ToDifferentFiles_AllSucceed()
    {
        var tasks = Enumerable.Range(0, 50).Select(async i =>
        {
            var filePath = _fixture.GetTempFilePath($"parallel_{i}.txt");
            await File.WriteAllTextAsync(filePath, $"Content {i}");
        });

        await Task.WhenAll(tasks);

        var files = Directory.EnumerateFiles(_fixture.RootPath, "parallel_*.txt").ToList();
        files.Should().HaveCount(50);
    }

    [Test]
    public async Task ParallelReads_FromSameFile_AllSucceed()
    {
        var filePath = _fixture.GetTempFilePath("shared_read.txt");
        const string content = "Shared content for reading";
        await File.WriteAllTextAsync(filePath, content);

        var tasks = Enumerable.Range(0, 20).Select(async _ =>
        {
            var readContent = await File.ReadAllTextAsync(filePath);
            return readContent;
        });

        var results = await Task.WhenAll(tasks);
        results.Should().AllBe(content);
    }

    #endregion

    #region TempFileSystemFixture Tests

    [Test]
    public void Fixture_CreatesRootDirectory()
    {
        Directory.Exists(_fixture.RootPath).Should().BeTrue();
    }

    [Test]
    public void Fixture_GetTempFilePath_ReturnsPathInRootDirectory()
    {
        var path = _fixture.GetTempFilePath();

        Path.GetDirectoryName(path).Should().Be(_fixture.RootPath);
    }

    [Test]
    public void Fixture_GetTempFilePath_WithCustomName_UsesProvidedName()
    {
        var path = _fixture.GetTempFilePath("custom.txt");

        Path.GetFileName(path).Should().Be("custom.txt");
    }

    [Test]
    public void Fixture_GetTempDirectoryPath_ReturnsPathInRootDirectory()
    {
        var path = _fixture.GetTempDirectoryPath();

        Path.GetDirectoryName(path).Should().Be(_fixture.RootPath);
    }

    #endregion
}
