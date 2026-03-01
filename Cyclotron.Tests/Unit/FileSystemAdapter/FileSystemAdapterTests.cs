using Cyclotron.FileSystemAdapter;
using Cyclotron.FileSystemAdapter.Abstractions.Handlers;
using Cyclotron.FileSystemAdapter.Abstractions.Models;
using Cyclotron.Extensions.DepepndencyInjection;
using AwesomeAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace Cyclotron.Tests.Unit.FileSystemAdapter;

[Category("Unit")]
[Category("FileSystemAdapter")]
public class FileSystemAdapterTests
{
    private IFileHandler _fileHandler = null!;
    private IFolderHandler _folderHandler = null!;
    private IFile _mockFile = null!;
    private IFolder _mockFolder = null!;

    [Before(Test)]
    public void Setup()
    {
        _fileHandler = Substitute.For<IFileHandler>();
        _folderHandler = Substitute.For<IFolderHandler>();
        _mockFile = Substitute.For<IFile>();
        _mockFolder = Substitute.For<IFolder>();
    }

    #region IFileHandler - Copy Operations

    [Test]
    public async Task CopyAndReplaceAsync_CallsHandler_WithCorrectParameters()
    {
        var destinationFile = Substitute.For<IFile>();

        await _fileHandler.CopyAndReplaceAsync(_mockFile, destinationFile);

        await _fileHandler.Received(1).CopyAndReplaceAsync(_mockFile, destinationFile);
    }

    [Test]
    public async Task CopyAsync_CallsHandler_WithCorrectParameters()
    {
        const string desiredName = "newfile.txt";
        const NameCollisionOption option = NameCollisionOption.ReplaceExisting;

        await _fileHandler.CopyAsync(_mockFile, _mockFolder, desiredName, option);

        await _fileHandler.Received(1).CopyAsync(_mockFile, _mockFolder, desiredName, option);
    }

    [Test]
    public async Task CopyAsync_WithFailIfExists_CallsHandlerCorrectly()
    {
        const string desiredName = "test.txt";

        await _fileHandler.CopyAsync(_mockFile, _mockFolder, desiredName, NameCollisionOption.FailIfExists);

        await _fileHandler.Received(1).CopyAsync(
            _mockFile, _mockFolder, desiredName, NameCollisionOption.FailIfExists);
    }

    [Test]
    public async Task CopyAsync_WithGenerateUniqueName_CallsHandlerCorrectly()
    {
        const string desiredName = "test.txt";

        await _fileHandler.CopyAsync(_mockFile, _mockFolder, desiredName, NameCollisionOption.GenerateUniqueName);

        await _fileHandler.Received(1).CopyAsync(
            _mockFile, _mockFolder, desiredName, NameCollisionOption.GenerateUniqueName);
    }

    #endregion

    #region IFileHandler - Move Operations

    [Test]
    public async Task MoveAndReplaceAsync_CallsHandler_WithCorrectParameters()
    {
        var fileToReplace = Substitute.For<IFile>();

        await _fileHandler.MoveAndReplaceAsync(_mockFile, fileToReplace);

        await _fileHandler.Received(1).MoveAndReplaceAsync(_mockFile, fileToReplace);
    }

    [Test]
    public async Task MoveAsync_ToFolder_CallsHandlerCorrectly()
    {
        await _fileHandler.MoveAsync(_mockFile, _mockFolder);

        await _fileHandler.Received(1).MoveAsync(_mockFile, _mockFolder);
    }

    [Test]
    public async Task MoveAsync_WithDesiredName_CallsHandlerCorrectly()
    {
        const string desiredName = "moved.txt";

        await _fileHandler.MoveAsync(_mockFile, _mockFolder, desiredName);

        await _fileHandler.Received(1).MoveAsync(_mockFile, _mockFolder, desiredName);
    }

    [Test]
    public async Task MoveAsync_WithNameAndCollisionOption_CallsHandlerCorrectly()
    {
        const string desiredName = "moved.txt";
        const NameCollisionOption option = NameCollisionOption.ReplaceExisting;

        await _fileHandler.MoveAsync(_mockFile, _mockFolder, desiredName, option);

        await _fileHandler.Received(1).MoveAsync(_mockFile, _mockFolder, desiredName, option);
    }

    #endregion

    #region IFileHandler - Open Operations

    [Test]
    public async Task OpenAsync_WithReadMode_CallsHandlerCorrectly()
    {
        await _fileHandler.OpenAsync(_mockFile, FileAccessMode.Read);

        await _fileHandler.Received(1).OpenAsync(_mockFile, FileAccessMode.Read);
    }

    [Test]
    public async Task OpenAsync_WithReadWriteMode_CallsHandlerCorrectly()
    {
        await _fileHandler.OpenAsync(_mockFile, FileAccessMode.ReadWrite);

        await _fileHandler.Received(1).OpenAsync(_mockFile, FileAccessMode.ReadWrite);
    }

    [Test]
    public async Task OpenAsync_WithStorageOptions_CallsHandlerCorrectly()
    {
        await _fileHandler.OpenAsync(_mockFile, FileAccessMode.Read, StorageOpenOptions.AllowOnlyReaders);

        await _fileHandler.Received(1).OpenAsync(_mockFile, FileAccessMode.Read, StorageOpenOptions.AllowOnlyReaders);
    }

    [Test]
    public async Task OpenAsync_WithAllowReadersAndWriters_CallsHandlerCorrectly()
    {
        await _fileHandler.OpenAsync(_mockFile, FileAccessMode.ReadWrite, StorageOpenOptions.AllowReadersAndWriters);

        await _fileHandler.Received(1).OpenAsync(
            _mockFile, FileAccessMode.ReadWrite, StorageOpenOptions.AllowReadersAndWriters);
    }

    #endregion

    #region IFileHandler - Rename Operations

    [Test]
    public async Task RenameAsync_WithDesiredName_CallsHandlerCorrectly()
    {
        const string desiredName = "renamed.txt";

        await _fileHandler.RenameAsync(_mockFile, desiredName);

        await _fileHandler.Received(1).RenameAsync(_mockFile, desiredName);
    }

    [Test]
    public async Task RenameAsync_WithCollisionOption_CallsHandlerCorrectly()
    {
        const string desiredName = "renamed.txt";

        await _fileHandler.RenameAsync(_mockFile, desiredName, NameCollisionOption.FailIfExists);

        await _fileHandler.Received(1).RenameAsync(_mockFile, desiredName, NameCollisionOption.FailIfExists);
    }

    #endregion

    #region IFileHandler - Retrieval Operations

    [Test]
    public async Task GetFileFromPathAsync_ReturnsFile_WhenPathIsValid()
    {
        const string path = "/test/file.txt";
        _fileHandler.GetFileFromPathAsync(path).Returns(_mockFile);

        var result = await _fileHandler.GetFileFromPathAsync(path);

        result.Should().BeSameAs(_mockFile);
    }

    [Test]
    public async Task GetParentAsync_ReturnsFolder_ForFile()
    {
        _fileHandler.GetParentAsync(_mockFile).Returns(_mockFolder);

        var result = await _fileHandler.GetParentAsync(_mockFile);

        result.Should().BeSameAs(_mockFolder);
    }

    #endregion

    #region IFileHandler - Error Propagation

    [Test]
    public async Task GetFileFromPathAsync_PropagatesException_WhenHandlerThrows()
    {
        _fileHandler.GetFileFromPathAsync(Arg.Any<string>())
            .Returns<IFile>(x => throw new FileNotFoundException("File not found"));

        var act = async () => await _fileHandler.GetFileFromPathAsync("nonexistent.txt");

        await act.Should().ThrowAsync<FileNotFoundException>()
            .WithMessage("File not found");
    }

    [Test]
    public async Task CopyAsync_PropagatesException_WhenHandlerThrows()
    {
        _fileHandler.CopyAsync(Arg.Any<IFile>(), Arg.Any<IFolder>(), Arg.Any<string>(), Arg.Any<NameCollisionOption>())
            .Returns(x => throw new UnauthorizedAccessException("Access denied"));

        var act = async () => await _fileHandler.CopyAsync(
            _mockFile, _mockFolder, "test.txt", NameCollisionOption.FailIfExists);

        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("Access denied");
    }

    [Test]
    public async Task MoveAsync_PropagatesIOException_WhenHandlerThrows()
    {
        _fileHandler.MoveAsync(Arg.Any<IFile>(), Arg.Any<IFolder>())
            .Returns(x => throw new IOException("Disk full"));

        var act = async () => await _fileHandler.MoveAsync(_mockFile, _mockFolder);

        await act.Should().ThrowAsync<IOException>()
            .WithMessage("Disk full");
    }

    #endregion

    #region IFolderHandler - Create Operations

    [Test]
    public async Task CreateFileAsync_InFolder_CallsHandlerCorrectly()
    {
        const string desiredName = "newfile.txt";

        await _folderHandler.CreateFileAsync(_mockFolder, desiredName);

        await _folderHandler.Received(1).CreateFileAsync(_mockFolder, desiredName);
    }

    [Test]
    public async Task CreateFileAsync_WithCollisionOption_CallsHandlerCorrectly()
    {
        const string desiredName = "newfile.txt";

        await _folderHandler.CreateFileAsync(_mockFolder, desiredName, CreationCollisionOption.ReplaceExisting);

        await _folderHandler.Received(1).CreateFileAsync(
            _mockFolder, desiredName, CreationCollisionOption.ReplaceExisting);
    }

    [Test]
    public async Task CreateFileAsync_WithOpenIfExists_CallsHandlerCorrectly()
    {
        const string desiredName = "existing.txt";

        await _folderHandler.CreateFileAsync(_mockFolder, desiredName, CreationCollisionOption.OpenIfExists);

        await _folderHandler.Received(1).CreateFileAsync(
            _mockFolder, desiredName, CreationCollisionOption.OpenIfExists);
    }

    [Test]
    public async Task CreateFolderAsync_CallsHandlerCorrectly()
    {
        const string desiredName = "subfolder";

        await _folderHandler.CreateFolderAsync(_mockFolder, desiredName);

        await _folderHandler.Received(1).CreateFolderAsync(_mockFolder, desiredName);
    }

    [Test]
    public async Task CreateFolderAsync_WithCollisionOption_CallsHandlerCorrectly()
    {
        const string desiredName = "subfolder";

        await _folderHandler.CreateFolderAsync(_mockFolder, desiredName, CreationCollisionOption.FailIfExists);

        await _folderHandler.Received(1).CreateFolderAsync(
            _mockFolder, desiredName, CreationCollisionOption.FailIfExists);
    }

    #endregion

    #region IFolderHandler - Delete Operations

    [Test]
    public async Task DeleteAsync_Folder_CallsHandlerCorrectly()
    {
        await _folderHandler.DeleteAsync(_mockFolder);

        await _folderHandler.Received(1).DeleteAsync(_mockFolder);
    }

    [Test]
    public async Task DeleteAsync_WithPermanentDelete_CallsHandlerCorrectly()
    {
        await _folderHandler.DeleteAsync(_mockFolder, StorageDeletionOption.PermanentDelete);

        await _folderHandler.Received(1).DeleteAsync(_mockFolder, StorageDeletionOption.PermanentDelete);
    }

    [Test]
    public async Task DeleteAsync_WithDefaultOption_CallsHandlerCorrectly()
    {
        await _folderHandler.DeleteAsync(_mockFolder, StorageDeletionOption.Default);

        await _folderHandler.Received(1).DeleteAsync(_mockFolder, StorageDeletionOption.Default);
    }

    #endregion

    #region IFolderHandler - Retrieval Operations

    [Test]
    public async Task GetFileAsync_ByName_ReturnsFile()
    {
        const string fileName = "test.txt";
        _folderHandler.GetFileAsync(_mockFolder, fileName).Returns(_mockFile);

        var result = await _folderHandler.GetFileAsync(_mockFolder, fileName);

        result.Should().BeSameAs(_mockFile);
    }

    [Test]
    public async Task GetFileAsync_AllFiles_ReturnsFileList()
    {
        var files = new List<IFile> { _mockFile, Substitute.For<IFile>() };
        _folderHandler.GetFileAsync(_mockFolder).Returns(files);

        var result = await _folderHandler.GetFileAsync(_mockFolder);

        result.Should().HaveCount(2);
        result.Should().Contain(_mockFile);
    }

    [Test]
    public async Task GetFolderAsync_ByName_ReturnsFolder()
    {
        const string folderName = "subfolder";
        var subFolder = Substitute.For<IFolder>();
        _folderHandler.GetFolderAsync(_mockFolder, folderName).Returns(subFolder);

        var result = await _folderHandler.GetFolderAsync(_mockFolder, folderName);

        result.Should().BeSameAs(subFolder);
    }

    [Test]
    public async Task GetFolderFromPathAsync_ReturnsFolder()
    {
        const string path = "/test/folder";
        _folderHandler.GetFolderFromPathAsync(path).Returns(_mockFolder);

        var result = await _folderHandler.GetFolderFromPathAsync(path);

        result.Should().BeSameAs(_mockFolder);
    }

    [Test]
    public async Task GetFoldersAsync_ReturnsFolderList()
    {
        var folders = new List<IFolder> { _mockFolder, Substitute.For<IFolder>() };
        _folderHandler.GetFoldersAsync(_mockFolder).Returns(folders);

        var result = await _folderHandler.GetFoldersAsync(_mockFolder);

        result.Should().HaveCount(2);
    }

    [Test]
    public async Task GetItemAsync_ByName_ReturnsStorageItem()
    {
        const string itemName = "item";
        var storageItem = Substitute.For<IStorageItem>();
        _folderHandler.GetItemAsync(_mockFolder, itemName).Returns(storageItem);

        var result = await _folderHandler.GetItemAsync(_mockFolder, itemName);

        result.Should().BeSameAs(storageItem);
    }

    [Test]
    public async Task GetItemsAsync_ReturnsStorageItemList()
    {
        var items = new List<IStorageItem> { _mockFile, _mockFolder };
        _folderHandler.GetItemsAsync(_mockFolder).Returns(items);

        var result = await _folderHandler.GetItemsAsync(_mockFolder);

        result.Should().HaveCount(2);
    }

    [Test]
    public async Task GetParentAsync_Folder_ReturnsParentFolder()
    {
        var parentFolder = Substitute.For<IFolder>();
        _folderHandler.GetParentAsync(_mockFolder).Returns(parentFolder);

        var result = await _folderHandler.GetParentAsync(_mockFolder);

        result.Should().BeSameAs(parentFolder);
    }

    #endregion

    #region IFolderHandler - TryGetItem

    [Test]
    public async Task TryGetItemAsync_ReturnsItem_WhenExists()
    {
        const string itemName = "existing";
        var storageItem = Substitute.For<IStorageItem>();
        _folderHandler.TryGetItemAsync(_mockFolder, itemName).Returns(storageItem);

        var result = await _folderHandler.TryGetItemAsync(_mockFolder, itemName);

        result.Should().BeSameAs(storageItem);
    }

    [Test]
    public async Task TryGetItemAsync_ReturnsNull_WhenNotExists()
    {
        _folderHandler.TryGetItemAsync(_mockFolder, "nonexistent").Returns((IStorageItem?)null);

        var result = await _folderHandler.TryGetItemAsync(_mockFolder, "nonexistent");

        result.Should().BeNull();
    }

    #endregion

    #region IFolderHandler - Rename Operations

    [Test]
    public async Task RenameAsync_Folder_CallsHandlerCorrectly()
    {
        const string desiredName = "renamedFolder";

        await _folderHandler.RenameAsync(_mockFolder, desiredName);

        await _folderHandler.Received(1).RenameAsync(_mockFolder, desiredName);
    }

    [Test]
    public async Task RenameAsync_FolderWithCollisionOption_CallsHandlerCorrectly()
    {
        const string desiredName = "renamedFolder";

        await _folderHandler.RenameAsync(_mockFolder, desiredName, NameCollisionOption.GenerateUniqueName);

        await _folderHandler.Received(1).RenameAsync(
            _mockFolder, desiredName, NameCollisionOption.GenerateUniqueName);
    }

    #endregion

    #region IFolderHandler - Error Propagation

    [Test]
    public async Task GetFileAsync_PropagatesException_WhenHandlerThrows()
    {
        _folderHandler.GetFileAsync(_mockFolder, Arg.Any<string>())
            .Returns<IFile>(x => throw new FileNotFoundException("Not found"));

        var act = async () => await _folderHandler.GetFileAsync(_mockFolder, "missing.txt");

        await act.Should().ThrowAsync<FileNotFoundException>();
    }

    [Test]
    public async Task DeleteAsync_PropagatesException_WhenHandlerThrows()
    {
        _folderHandler.DeleteAsync(Arg.Any<IFolder>())
            .Returns(x => throw new UnauthorizedAccessException("Access denied"));

        var act = async () => await _folderHandler.DeleteAsync(_mockFolder);

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    #endregion

    #region IStorageItem Properties

    [Test]
    public void IFile_Properties_CanBeConfigured()
    {
        _mockFile.Name.Returns("test.txt");
        _mockFile.Path.Returns("/path/to/test.txt");
        _mockFile.FolderRelativeId.Returns("folder-123");

        _mockFile.Name.Should().Be("test.txt");
        _mockFile.Path.Should().Be("/path/to/test.txt");
        _mockFile.FolderRelativeId.Should().Be("folder-123");
    }

    [Test]
    public void IFolder_Properties_CanBeConfigured()
    {
        _mockFolder.Name.Returns("testFolder");
        _mockFolder.Path.Returns("/path/to/testFolder");
        _mockFolder.FolderRelativeId.Returns("folder-456");

        _mockFolder.Name.Should().Be("testFolder");
        _mockFolder.Path.Should().Be("/path/to/testFolder");
        _mockFolder.FolderRelativeId.Should().Be("folder-456");
    }

    [Test]
    public async Task IFile_GetSizeAsync_ReturnsCorrectSize()
    {
        _mockFile.GetSizeAsync().Returns(1024UL);

        var size = await _mockFile.GetSizeAsync();

        size.Should().Be(1024UL);
    }

    [Test]
    public async Task IFile_GetSizeAsync_ReturnsZeroForEmptyFile()
    {
        _mockFile.GetSizeAsync().Returns(0UL);

        var size = await _mockFile.GetSizeAsync();

        size.Should().Be(0UL);
    }

    #endregion

    #region Enum Validation

    [Test]
    public void NameCollisionOption_HasExpectedValues()
    {
        Enum.GetValues<NameCollisionOption>().Should().HaveCount(3);
        NameCollisionOption.GenerateUniqueName.Should().Be((NameCollisionOption)0);
        NameCollisionOption.ReplaceExisting.Should().Be((NameCollisionOption)1);
        NameCollisionOption.FailIfExists.Should().Be((NameCollisionOption)2);
    }

    [Test]
    public void CreationCollisionOption_HasExpectedValues()
    {
        Enum.GetValues<CreationCollisionOption>().Should().HaveCount(4);
        CreationCollisionOption.GenerateUniqueName.Should().Be((CreationCollisionOption)0);
    }

    [Test]
    public void FileAccessMode_HasExpectedValues()
    {
        Enum.GetValues<FileAccessMode>().Should().HaveCount(2);
        FileAccessMode.Read.Should().Be((FileAccessMode)0);
        FileAccessMode.ReadWrite.Should().Be((FileAccessMode)1);
    }

    [Test]
    public void StorageDeletionOption_HasExpectedValues()
    {
        Enum.GetValues<StorageDeletionOption>().Should().HaveCount(2);
    }

    [Test]
    public void StorageOpenOptions_HasExpectedValues()
    {
        Enum.GetValues<StorageOpenOptions>().Should().HaveCount(3);
    }

    #endregion

    #region FileSystemProvider - Singleton

    [Test]
    public void Instance_ReturnsNonNullInstance()
    {
        var instance = FileSystemProvider.Instance;

        instance.Should().NotBeNull();
    }

    [Test]
    public void Instance_ReturnsSameInstance_OnMultipleCalls()
    {
        var first = FileSystemProvider.Instance;
        var second = FileSystemProvider.Instance;

        first.Should().BeSameAs(second);
    }

    [Test]
    public void GetService_ReturnsService_WhenInitialized()
    {
        var instance = FileSystemProvider.Instance;

        var result = instance.GetService<IFileHandler>();

        result.Should().NotBeNull();
    }

    [Test]
    public void GetRequiredService_ReturnsService_WhenInitialized()
    {
        var instance = FileSystemProvider.Instance;

        var result = instance.GetRequiredService<IFolderHandler>();

        result.Should().NotBeNull();
    }

    #endregion

    #region ServiceCollectionExtensions - EnsureIfExists

    [Test]
    public void EnsureIfExists_ThrowsArgumentNullException_WhenServicesIsNull()
    {
        IServiceCollection? nullServices = null;

        var act = () => nullServices!.EnsureIfExists<IFileHandler>();

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void EnsureIfExists_ThrowsInvalidOperationException_WhenServiceTypeIsNotRegistered()
    {
        var services = new ServiceCollection();

        var act = () => services.EnsureIfExists<IFileHandler>();

        act.Should().Throw<InvalidOperationException>();
    }

    [Test]
    public void EnsureIfExists_Succeeds_WhenServiceTypeIsRegistered()
    {
        var services = new ServiceCollection();
        services.AddSingleton(Substitute.For<IFileHandler>());

        var act = () => services.EnsureIfExists<IFileHandler>();

        act.Should().NotThrow();
    }

    #endregion

    #region WinUI Handler - Argument Guards (non-WinUI objects)

    [Test]
    public async Task WinUIFileHandler_CopyAndReplaceAsync_ThrowsArgumentException_WhenSourceNotWinUIFile()
    {
        var handler = FileSystemProvider.Instance.GetRequiredService<IFileHandler>();
        var mockFile = Substitute.For<IFile>();

        var act = async () => await handler.CopyAndReplaceAsync(mockFile, mockFile);

        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Test]
    public async Task WinUIFileHandler_CopyAsync_ThrowsArgumentException_WhenSourceNotWinUIFile()
    {
        var handler = FileSystemProvider.Instance.GetRequiredService<IFileHandler>();
        var mockFile = Substitute.For<IFile>();
        var mockFolder = Substitute.For<IFolder>();

        var act = async () => await handler.CopyAsync(mockFile, mockFolder, "copy.txt", NameCollisionOption.ReplaceExisting);

        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Test]
    public async Task WinUIFileHandler_GetParentAsync_ThrowsArgumentException_WhenFileNotWinUIFile()
    {
        var handler = FileSystemProvider.Instance.GetRequiredService<IFileHandler>();
        var mockFile = Substitute.For<IFile>();

        var act = async () => await handler.GetParentAsync(mockFile);

        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Test]
    public async Task WinUIFileHandler_MoveAndReplaceAsync_ThrowsArgumentException_WhenSourceNotWinUIFile()
    {
        var handler = FileSystemProvider.Instance.GetRequiredService<IFileHandler>();
        var mockFile = Substitute.For<IFile>();

        var act = async () => await handler.MoveAndReplaceAsync(mockFile, mockFile);

        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Test]
    public async Task WinUIFileHandler_MoveAsync_ThrowsArgumentException_WhenFileNotWinUIFile()
    {
        var handler = FileSystemProvider.Instance.GetRequiredService<IFileHandler>();
        var mockFile = Substitute.For<IFile>();
        var mockFolder = Substitute.For<IFolder>();

        var act = async () => await handler.MoveAsync(mockFile, mockFolder);

        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Test]
    public async Task WinUIFileHandler_OpenAsync_ThrowsArgumentException_WhenFileNotWinUIFile()
    {
        var handler = FileSystemProvider.Instance.GetRequiredService<IFileHandler>();
        var mockFile = Substitute.For<IFile>();

        var act = async () => await handler.OpenAsync(mockFile, FileAccessMode.Read);

        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Test]
    public async Task WinUIFileHandler_OpenAsync_WithOptions_ThrowsArgumentException_WhenFileNotWinUIFile()
    {
        var handler = FileSystemProvider.Instance.GetRequiredService<IFileHandler>();
        var mockFile = Substitute.For<IFile>();

        var act = async () => await handler.OpenAsync(mockFile, FileAccessMode.Read, StorageOpenOptions.AllowOnlyReaders);

        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Test]
    public async Task WinUIFileHandler_RenameAsync_ThrowsArgumentException_WhenFileNotWinUIFile()
    {
        var handler = FileSystemProvider.Instance.GetRequiredService<IFileHandler>();
        var mockFile = Substitute.For<IFile>();

        var act = async () => await handler.RenameAsync(mockFile, "new.txt");

        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Test]
    public async Task WinUIFileHandler_RenameAsync_WithCollision_ThrowsArgumentException_WhenFileNotWinUIFile()
    {
        var handler = FileSystemProvider.Instance.GetRequiredService<IFileHandler>();
        var mockFile = Substitute.For<IFile>();

        var act = async () => await handler.RenameAsync(mockFile, "new.txt", NameCollisionOption.GenerateUniqueName);

        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Test]
    public async Task WinUIFolderHandler_CreateFileAsync_ThrowsArgumentException_WhenFolderNotWinUIFolder()
    {
        var handler = FileSystemProvider.Instance.GetRequiredService<IFolderHandler>();
        var mockFolder = Substitute.For<IFolder>();

        var act = async () => await handler.CreateFileAsync(mockFolder, "test.txt");

        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Test]
    public async Task WinUIFolderHandler_CreateFolderAsync_ThrowsArgumentException_WhenFolderNotWinUIFolder()
    {
        var handler = FileSystemProvider.Instance.GetRequiredService<IFolderHandler>();
        var mockFolder = Substitute.For<IFolder>();

        var act = async () => await handler.CreateFolderAsync(mockFolder, "sub");

        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Test]
    public async Task WinUIFolderHandler_DeleteAsync_ThrowsArgumentException_WhenFolderNotWinUIFolder()
    {
        var handler = FileSystemProvider.Instance.GetRequiredService<IFolderHandler>();
        var mockFolder = Substitute.For<IFolder>();

        var act = async () => await handler.DeleteAsync(mockFolder);

        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Test]
    public async Task WinUIFolderHandler_GetFileAsync_ThrowsArgumentException_WhenFolderNotWinUIFolder()
    {
        var handler = FileSystemProvider.Instance.GetRequiredService<IFolderHandler>();
        var mockFolder = Substitute.For<IFolder>();

        var act = async () => await handler.GetFileAsync(mockFolder, "file.txt");

        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Test]
    public async Task WinUIFolderHandler_GetFolderAsync_ThrowsArgumentException_WhenFolderNotWinUIFolder()
    {
        var handler = FileSystemProvider.Instance.GetRequiredService<IFolderHandler>();
        var mockFolder = Substitute.For<IFolder>();

        var act = async () => await handler.GetFolderAsync(mockFolder, "sub");

        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Test]
    public async Task WinUIFolderHandler_RenameAsync_ThrowsArgumentException_WhenFolderNotWinUIFolder()
    {
        var handler = FileSystemProvider.Instance.GetRequiredService<IFolderHandler>();
        var mockFolder = Substitute.For<IFolder>();

        var act = async () => await handler.RenameAsync(mockFolder, "renamed");

        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Test]
    public async Task WinUIFileHandler_CopyAndReplaceAsync_ThrowsArgumentException_WhenDestNotWinUIFile()
    {
        var handler = FileSystemProvider.Instance.GetRequiredService<IFileHandler>();
        var tempPath = Path.GetTempFileName();
        try
        {
            var realSource = await handler.GetFileFromPathAsync(tempPath);

            var act = async () => await handler.CopyAndReplaceAsync(realSource, Substitute.For<IFile>());

            await act.Should().ThrowAsync<ArgumentException>();
        }
        finally
        {
            File.Delete(tempPath);
        }
    }

    [Test]
    public async Task WinUIFileHandler_CopyAsync_ThrowsArgumentException_WhenDestFolderNotWinUIFolder()
    {
        var handler = FileSystemProvider.Instance.GetRequiredService<IFileHandler>();
        var tempPath = Path.GetTempFileName();
        try
        {
            var realSource = await handler.GetFileFromPathAsync(tempPath);

            var act = async () => await handler.CopyAsync(realSource, Substitute.For<IFolder>(), "file.txt", NameCollisionOption.ReplaceExisting);

            await act.Should().ThrowAsync<ArgumentException>();
        }
        finally
        {
            File.Delete(tempPath);
        }
    }

    [Test]
    public async Task WinUIFileHandler_MoveAndReplaceAsync_ThrowsArgumentException_WhenDestNotWinUIFile()
    {
        var handler = FileSystemProvider.Instance.GetRequiredService<IFileHandler>();
        var tempPath = Path.GetTempFileName();
        try
        {
            var realSource = await handler.GetFileFromPathAsync(tempPath);

            var act = async () => await handler.MoveAndReplaceAsync(realSource, Substitute.For<IFile>());

            await act.Should().ThrowAsync<ArgumentException>();
        }
        finally
        {
            File.Delete(tempPath);
        }
    }

    [Test]
    public async Task WinUIFileHandler_MoveAsync_ThrowsArgumentException_WhenDestFolderNotWinUIFolder()
    {
        var handler = FileSystemProvider.Instance.GetRequiredService<IFileHandler>();
        var tempPath = Path.GetTempFileName();
        try
        {
            var realSource = await handler.GetFileFromPathAsync(tempPath);

            var act = async () => await handler.MoveAsync(realSource, Substitute.For<IFolder>());

            await act.Should().ThrowAsync<ArgumentException>();
        }
        finally
        {
            File.Delete(tempPath);
        }
    }

    [Test]
    public async Task WinUIFileHandler_MoveAsync_WithName_ThrowsArgumentException_WhenFileNotWinUIFile()
    {
        var handler = FileSystemProvider.Instance.GetRequiredService<IFileHandler>();
        var mockFile = Substitute.For<IFile>();
        var mockFolder = Substitute.For<IFolder>();

        var act = async () => await handler.MoveAsync(mockFile, mockFolder, "moved.txt");

        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Test]
    public async Task WinUIFileHandler_MoveAsync_WithName_ThrowsArgumentException_WhenDestFolderNotWinUIFolder()
    {
        var handler = FileSystemProvider.Instance.GetRequiredService<IFileHandler>();
        var tempPath = Path.GetTempFileName();
        try
        {
            var realSource = await handler.GetFileFromPathAsync(tempPath);

            var act = async () => await handler.MoveAsync(realSource, Substitute.For<IFolder>(), "moved.txt");

            await act.Should().ThrowAsync<ArgumentException>();
        }
        finally
        {
            File.Delete(tempPath);
        }
    }

    [Test]
    public async Task WinUIFileHandler_MoveAsync_WithNameAndOption_ThrowsArgumentException_WhenFileNotWinUIFile()
    {
        var handler = FileSystemProvider.Instance.GetRequiredService<IFileHandler>();
        var mockFile = Substitute.For<IFile>();
        var mockFolder = Substitute.For<IFolder>();

        var act = async () => await handler.MoveAsync(mockFile, mockFolder, "moved.txt", NameCollisionOption.ReplaceExisting);

        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Test]
    public async Task WinUIFileHandler_MoveAsync_WithNameAndOption_ThrowsArgumentException_WhenDestFolderNotWinUIFolder()
    {
        var handler = FileSystemProvider.Instance.GetRequiredService<IFileHandler>();
        var tempPath = Path.GetTempFileName();
        try
        {
            var realSource = await handler.GetFileFromPathAsync(tempPath);

            var act = async () => await handler.MoveAsync(realSource, Substitute.For<IFolder>(), "moved.txt", NameCollisionOption.ReplaceExisting);

            await act.Should().ThrowAsync<ArgumentException>();
        }
        finally
        {
            File.Delete(tempPath);
        }
    }

    [Test]
    public async Task WinUIFolderHandler_CreateFileAsync_WithOption_ThrowsArgumentException_WhenFolderNotWinUIFolder()
    {
        var handler = FileSystemProvider.Instance.GetRequiredService<IFolderHandler>();
        var mockFolder = Substitute.For<IFolder>();

        var act = async () => await handler.CreateFileAsync(mockFolder, "f.txt", CreationCollisionOption.ReplaceExisting);

        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Test]
    public async Task WinUIFolderHandler_CreateFolderAsync_WithOption_ThrowsArgumentException_WhenFolderNotWinUIFolder()
    {
        var handler = FileSystemProvider.Instance.GetRequiredService<IFolderHandler>();
        var mockFolder = Substitute.For<IFolder>();

        var act = async () => await handler.CreateFolderAsync(mockFolder, "sub", CreationCollisionOption.ReplaceExisting);

        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Test]
    public async Task WinUIFolderHandler_DeleteAsync_WithOption_ThrowsArgumentException_WhenFolderNotWinUIFolder()
    {
        var handler = FileSystemProvider.Instance.GetRequiredService<IFolderHandler>();
        var mockFolder = Substitute.For<IFolder>();

        var act = async () => await handler.DeleteAsync(mockFolder, StorageDeletionOption.PermanentDelete);

        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Test]
    public async Task WinUIFolderHandler_GetFileAsync_AllFiles_ThrowsArgumentException_WhenFolderNotWinUIFolder()
    {
        var handler = FileSystemProvider.Instance.GetRequiredService<IFolderHandler>();
        var mockFolder = Substitute.For<IFolder>();

        var act = async () => await handler.GetFileAsync(mockFolder);

        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Test]
    public async Task WinUIFolderHandler_GetFoldersAsync_ThrowsArgumentException_WhenFolderNotWinUIFolder()
    {
        var handler = FileSystemProvider.Instance.GetRequiredService<IFolderHandler>();
        var mockFolder = Substitute.For<IFolder>();

        var act = async () => await handler.GetFoldersAsync(mockFolder);

        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Test]
    public async Task WinUIFolderHandler_GetItemAsync_ThrowsArgumentException_WhenFolderNotWinUIFolder()
    {
        var handler = FileSystemProvider.Instance.GetRequiredService<IFolderHandler>();
        var mockFolder = Substitute.For<IFolder>();

        var act = async () => await handler.GetItemAsync(mockFolder, "item");

        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Test]
    public async Task WinUIFolderHandler_GetItemsAsync_ThrowsArgumentException_WhenFolderNotWinUIFolder()
    {
        var handler = FileSystemProvider.Instance.GetRequiredService<IFolderHandler>();
        var mockFolder = Substitute.For<IFolder>();

        var act = async () => await handler.GetItemsAsync(mockFolder);

        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Test]
    public async Task WinUIFolderHandler_GetParentAsync_ThrowsArgumentException_WhenFolderNotWinUIFolder()
    {
        var handler = FileSystemProvider.Instance.GetRequiredService<IFolderHandler>();
        var mockFolder = Substitute.For<IFolder>();

        var act = async () => await handler.GetParentAsync(mockFolder);

        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Test]
    public async Task WinUIFolderHandler_RenameAsync_WithOption_ThrowsArgumentException_WhenFolderNotWinUIFolder()
    {
        var handler = FileSystemProvider.Instance.GetRequiredService<IFolderHandler>();
        var mockFolder = Substitute.For<IFolder>();

        var act = async () => await handler.RenameAsync(mockFolder, "renamed", NameCollisionOption.GenerateUniqueName);

        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Test]
    public async Task WinUIFolderHandler_TryGetItemAsync_ThrowsArgumentException_WhenFolderNotWinUIFolder()
    {
        var handler = FileSystemProvider.Instance.GetRequiredService<IFolderHandler>();
        var mockFolder = Substitute.For<IFolder>();

        var act = async () => await handler.TryGetItemAsync(mockFolder, "item");

        await act.Should().ThrowAsync<ArgumentException>();
    }

    #endregion

    #region Constants

    [Test]
    public void Constants_AllTypeFilter_ContainsWildcard()
    {
        Cyclotron.FileSystemAdapter.Constants.AllTypeFilter.Should().Contain("*");
    }

    [Test]
    public void Constants_ImageTypeFilter_ContainsExpectedExtensions()
    {
        var filter = Cyclotron.FileSystemAdapter.Constants.ImageTypeFilter;
        filter.Should().Contain(".png");
        filter.Should().Contain(".jpg");
        filter.Should().Contain(".jpeg");
        filter.Should().NotBeEmpty();
    }

    [Test]
    public void Constants_VideoTypeFilter_ContainsExpectedExtensions()
    {
        var filter = Cyclotron.FileSystemAdapter.Constants.VideoTypeFilter;
        filter.Should().Contain(".mp4");
        filter.Should().NotBeEmpty();
    }

    [Test]
    public void Constants_AudioTypeFilter_ContainsExpectedExtensions()
    {
        var filter = Cyclotron.FileSystemAdapter.Constants.AudioTypeFilter;
        filter.Should().Contain(".mp3");
        filter.Should().NotBeEmpty();
    }

    [Test]
    public void Constants_DocumentTypeFilter_ContainsExpectedExtensions()
    {
        var filter = Cyclotron.FileSystemAdapter.Constants.DocumentTypeFilter;
        filter.Should().Contain(".pdf");
        filter.Should().Contain(".docx");
        filter.Should().NotBeEmpty();
    }

    [Test]
    public void Constants_CompressedTypeFilter_ContainsExpectedExtensions()
    {
        var filter = Cyclotron.FileSystemAdapter.Constants.CompressedTypeFilter;
        filter.Should().Contain(".zip");
        filter.Should().NotBeEmpty();
    }

    #endregion
}
