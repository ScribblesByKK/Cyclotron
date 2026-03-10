using Cyclotron.FileSystemAdapter;
using Cyclotron.FileSystemAdapter.Abstractions.Handlers;
using Cyclotron.FileSystemAdapter.Abstractions.Models;
using Cyclotron.Extensions.DepepndencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Cyclotron.Tests.Unit.FileSystemAdapter;

[Category("Unit")]
[Category("FileSystemAdapter")]
public class FileSystemAdapterTests
{
    private Mock<IFileHandler> _fileHandler = null!;
    private Mock<IFolderHandler> _folderHandler = null!;
    private Mock<IFile> _mockFile = null!;
    private Mock<IFolder> _mockFolder = null!;

    [Before(Test)]
    public void Setup()
    {
        _fileHandler = Mock.Of<IFileHandler>();
        _folderHandler = Mock.Of<IFolderHandler>();
        _mockFile = Mock.Of<IFile>();
        _mockFolder = Mock.Of<IFolder>();
    }

    #region IFileHandler - Copy Operations

    [Test]
    public async Task CopyAndReplaceAsync_CallsHandler_WithCorrectParameters()
    {
        var destinationFile = Mock.Of<IFile>();

        await _fileHandler.Object.CopyAndReplaceAsync(_mockFile.Object, destinationFile.Object);

        _fileHandler.CopyAndReplaceAsync(Any<IFile>(), Any<IFile>()).WasCalled(Times.Once);
    }

    [Test]
    public async Task CopyAsync_CallsHandler_WithCorrectParameters()
    {
        const string desiredName = "newfile.txt";
        const NameCollisionOption option = NameCollisionOption.ReplaceExisting;

        await _fileHandler.Object.CopyAsync(_mockFile.Object, _mockFolder.Object, desiredName, option);

        _fileHandler.CopyAsync(Any<IFile>(), Any<IFolder>(), desiredName, option).WasCalled(Times.Once);
    }

    [Test]
    public async Task CopyAsync_WithFailIfExists_CallsHandlerCorrectly()
    {
        const string desiredName = "test.txt";

        await _fileHandler.Object.CopyAsync(_mockFile.Object, _mockFolder.Object, desiredName, NameCollisionOption.FailIfExists);

        _fileHandler.CopyAsync(Any<IFile>(), Any<IFolder>(), desiredName, NameCollisionOption.FailIfExists).WasCalled(Times.Once);
    }

    [Test]
    public async Task CopyAsync_WithGenerateUniqueName_CallsHandlerCorrectly()
    {
        const string desiredName = "test.txt";

        await _fileHandler.Object.CopyAsync(_mockFile.Object, _mockFolder.Object, desiredName, NameCollisionOption.GenerateUniqueName);

        _fileHandler.CopyAsync(Any<IFile>(), Any<IFolder>(), desiredName, NameCollisionOption.GenerateUniqueName).WasCalled(Times.Once);
    }

    #endregion

    #region IFileHandler - Move Operations

    [Test]
    public async Task MoveAndReplaceAsync_CallsHandler_WithCorrectParameters()
    {
        var fileToReplace = Mock.Of<IFile>();

        await _fileHandler.Object.MoveAndReplaceAsync(_mockFile.Object, fileToReplace.Object);

        _fileHandler.MoveAndReplaceAsync(Any<IFile>(), Any<IFile>()).WasCalled(Times.Once);
    }

    [Test]
    public async Task MoveAsync_ToFolder_CallsHandlerCorrectly()
    {
        await _fileHandler.Object.MoveAsync(_mockFile.Object, _mockFolder.Object);

        _fileHandler.MoveAsync(Any<IFile>(), Any<IFolder>()).WasCalled(Times.Once);
    }

    [Test]
    public async Task MoveAsync_WithDesiredName_CallsHandlerCorrectly()
    {
        const string desiredName = "moved.txt";

        await _fileHandler.Object.MoveAsync(_mockFile.Object, _mockFolder.Object, desiredName);

        _fileHandler.MoveAsync(Any<IFile>(), Any<IFolder>(), desiredName).WasCalled(Times.Once);
    }

    [Test]
    public async Task MoveAsync_WithNameAndCollisionOption_CallsHandlerCorrectly()
    {
        const string desiredName = "moved.txt";
        const NameCollisionOption option = NameCollisionOption.ReplaceExisting;

        await _fileHandler.Object.MoveAsync(_mockFile.Object, _mockFolder.Object, desiredName, option);

        _fileHandler.MoveAsync(Any<IFile>(), Any<IFolder>(), desiredName, option).WasCalled(Times.Once);
    }

    #endregion

    #region IFileHandler - Open Operations

    [Test]
    public async Task OpenAsync_WithReadMode_CallsHandlerCorrectly()
    {
        await _fileHandler.Object.OpenAsync(_mockFile.Object, FileAccessMode.Read);

        _fileHandler.OpenAsync(Any<IFile>(), FileAccessMode.Read).WasCalled(Times.Once);
    }

    [Test]
    public async Task OpenAsync_WithReadWriteMode_CallsHandlerCorrectly()
    {
        await _fileHandler.Object.OpenAsync(_mockFile.Object, FileAccessMode.ReadWrite);

        _fileHandler.OpenAsync(Any<IFile>(), FileAccessMode.ReadWrite).WasCalled(Times.Once);
    }

    [Test]
    public async Task OpenAsync_WithStorageOptions_CallsHandlerCorrectly()
    {
        await _fileHandler.Object.OpenAsync(_mockFile.Object, FileAccessMode.Read, StorageOpenOptions.AllowOnlyReaders);

        _fileHandler.OpenAsync(Any<IFile>(), FileAccessMode.Read, StorageOpenOptions.AllowOnlyReaders).WasCalled(Times.Once);
    }

    [Test]
    public async Task OpenAsync_WithAllowReadersAndWriters_CallsHandlerCorrectly()
    {
        await _fileHandler.Object.OpenAsync(_mockFile.Object, FileAccessMode.ReadWrite, StorageOpenOptions.AllowReadersAndWriters);

        _fileHandler.OpenAsync(Any<IFile>(), FileAccessMode.ReadWrite, StorageOpenOptions.AllowReadersAndWriters).WasCalled(Times.Once);
    }

    #endregion

    #region IFileHandler - Rename Operations

    [Test]
    public async Task RenameAsync_WithDesiredName_CallsHandlerCorrectly()
    {
        const string desiredName = "renamed.txt";

        await _fileHandler.Object.RenameAsync(_mockFile.Object, desiredName);

        _fileHandler.RenameAsync(Any<IFile>(), desiredName).WasCalled(Times.Once);
    }

    [Test]
    public async Task RenameAsync_WithCollisionOption_CallsHandlerCorrectly()
    {
        const string desiredName = "renamed.txt";

        await _fileHandler.Object.RenameAsync(_mockFile.Object, desiredName, NameCollisionOption.FailIfExists);

        _fileHandler.RenameAsync(Any<IFile>(), desiredName, NameCollisionOption.FailIfExists).WasCalled(Times.Once);
    }

    #endregion

    #region IFileHandler - Retrieval Operations

    [Test]
    public async Task GetFileFromPathAsync_ReturnsFile_WhenPathIsValid()
    {
        const string path = "/test/file.txt";
        _fileHandler.GetFileFromPathAsync(path).Returns(_mockFile.Object);

        var result = await _fileHandler.Object.GetFileFromPathAsync(path);

        await Assert.That(result).IsSameReferenceAs(_mockFile.Object);
    }

    [Test]
    public async Task GetParentAsync_ReturnsFolder_ForFile()
    {
        _fileHandler.GetParentAsync(Any<IFile>()).Returns(_mockFolder.Object);

        var result = await _fileHandler.Object.GetParentAsync(_mockFile.Object);

        await Assert.That(result).IsSameReferenceAs(_mockFolder.Object);
    }

    #endregion

    #region IFileHandler - Error Propagation

    [Test]
    public async Task GetFileFromPathAsync_PropagatesException_WhenHandlerThrows()
    {
        _fileHandler.GetFileFromPathAsync(Any<string>())
            .Throws(new FileNotFoundException("File not found"));

        var act = async () => await _fileHandler.Object.GetFileFromPathAsync("nonexistent.txt");

        await Assert.That(act).ThrowsExactly<FileNotFoundException>()
            .WithMessage("File not found");
    }

    [Test]
    public async Task CopyAsync_PropagatesException_WhenHandlerThrows()
    {
        _fileHandler.CopyAsync(Any<IFile>(), Any<IFolder>(), Any<string>(), Any<NameCollisionOption>())
            .Throws(new UnauthorizedAccessException("Access denied"));

        var act = async () => await _fileHandler.Object.CopyAsync(
            _mockFile.Object, _mockFolder.Object, "test.txt", NameCollisionOption.FailIfExists);

        await Assert.That(act).ThrowsExactly<UnauthorizedAccessException>()
            .WithMessage("Access denied");
    }

    [Test]
    public async Task MoveAsync_PropagatesIOException_WhenHandlerThrows()
    {
        _fileHandler.MoveAsync(Any<IFile>(), Any<IFolder>())
            .Throws(new IOException("Disk full"));

        var act = async () => await _fileHandler.Object.MoveAsync(_mockFile.Object, _mockFolder.Object);

        await Assert.That(act).ThrowsExactly<IOException>()
            .WithMessage("Disk full");
    }

    #endregion

    #region IFolderHandler - Create Operations

    [Test]
    public async Task CreateFileAsync_InFolder_CallsHandlerCorrectly()
    {
        const string desiredName = "newfile.txt";

        await _folderHandler.Object.CreateFileAsync(_mockFolder.Object, desiredName);

        _folderHandler.CreateFileAsync(Any<IFolder>(), desiredName).WasCalled(Times.Once);
    }

    [Test]
    public async Task CreateFileAsync_WithCollisionOption_CallsHandlerCorrectly()
    {
        const string desiredName = "newfile.txt";

        await _folderHandler.Object.CreateFileAsync(_mockFolder.Object, desiredName, CreationCollisionOption.ReplaceExisting);

        _folderHandler.CreateFileAsync(Any<IFolder>(), desiredName, CreationCollisionOption.ReplaceExisting).WasCalled(Times.Once);
    }

    [Test]
    public async Task CreateFileAsync_WithOpenIfExists_CallsHandlerCorrectly()
    {
        const string desiredName = "existing.txt";

        await _folderHandler.Object.CreateFileAsync(_mockFolder.Object, desiredName, CreationCollisionOption.OpenIfExists);

        _folderHandler.CreateFileAsync(Any<IFolder>(), desiredName, CreationCollisionOption.OpenIfExists).WasCalled(Times.Once);
    }

    [Test]
    public async Task CreateFolderAsync_CallsHandlerCorrectly()
    {
        const string desiredName = "subfolder";

        await _folderHandler.Object.CreateFolderAsync(_mockFolder.Object, desiredName);

        _folderHandler.CreateFolderAsync(Any<IFolder>(), desiredName).WasCalled(Times.Once);
    }

    [Test]
    public async Task CreateFolderAsync_WithCollisionOption_CallsHandlerCorrectly()
    {
        const string desiredName = "subfolder";

        await _folderHandler.Object.CreateFolderAsync(_mockFolder.Object, desiredName, CreationCollisionOption.FailIfExists);

        _folderHandler.CreateFolderAsync(Any<IFolder>(), desiredName, CreationCollisionOption.FailIfExists).WasCalled(Times.Once);
    }

    #endregion

    #region IFolderHandler - Delete Operations

    [Test]
    public async Task DeleteAsync_Folder_CallsHandlerCorrectly()
    {
        await _folderHandler.Object.DeleteAsync(_mockFolder.Object);

        _folderHandler.DeleteAsync(Any<IFolder>()).WasCalled(Times.Once);
    }

    [Test]
    public async Task DeleteAsync_WithPermanentDelete_CallsHandlerCorrectly()
    {
        await _folderHandler.Object.DeleteAsync(_mockFolder.Object, StorageDeletionOption.PermanentDelete);

        _folderHandler.DeleteAsync(Any<IFolder>(), StorageDeletionOption.PermanentDelete).WasCalled(Times.Once);
    }

    [Test]
    public async Task DeleteAsync_WithDefaultOption_CallsHandlerCorrectly()
    {
        await _folderHandler.Object.DeleteAsync(_mockFolder.Object, StorageDeletionOption.Default);

        _folderHandler.DeleteAsync(Any<IFolder>(), StorageDeletionOption.Default).WasCalled(Times.Once);
    }

    #endregion

    #region IFolderHandler - Retrieval Operations

    [Test]
    public async Task GetFileAsync_ByName_ReturnsFile()
    {
        const string fileName = "test.txt";
        _folderHandler.GetFileAsync(Any<IFolder>(), fileName).Returns(_mockFile.Object);

        var result = await _folderHandler.Object.GetFileAsync(_mockFolder.Object, fileName);

        await Assert.That(result).IsSameReferenceAs(_mockFile.Object);
    }

    [Test]
    public async Task GetFileAsync_AllFiles_ReturnsFileList()
    {
        var secondFile = Mock.Of<IFile>();
        var files = new List<IFile> { _mockFile.Object, secondFile.Object };
        _folderHandler.GetFileAsync(Any<IFolder>()).Returns(files);

        var result = await _folderHandler.Object.GetFileAsync(_mockFolder.Object);

        await Assert.That(result).Count().IsEqualTo(2);
        await Assert.That(result).Contains(_mockFile.Object);
    }

    [Test]
    public async Task GetFolderAsync_ByName_ReturnsFolder()
    {
        const string folderName = "subfolder";
        var subFolder = Mock.Of<IFolder>();
        _folderHandler.GetFolderAsync(Any<IFolder>(), folderName).Returns(subFolder.Object);

        var result = await _folderHandler.Object.GetFolderAsync(_mockFolder.Object, folderName);

        await Assert.That(result).IsSameReferenceAs(subFolder.Object);
    }

    [Test]
    public async Task GetFolderFromPathAsync_ReturnsFolder()
    {
        const string path = "/test/folder";
        _folderHandler.GetFolderFromPathAsync(path).Returns(_mockFolder.Object);

        var result = await _folderHandler.Object.GetFolderFromPathAsync(path);

        await Assert.That(result).IsSameReferenceAs(_mockFolder.Object);
    }

    [Test]
    public async Task GetFoldersAsync_ReturnsFolderList()
    {
        var secondFolder = Mock.Of<IFolder>();
        var folders = new List<IFolder> { _mockFolder.Object, secondFolder.Object };
        _folderHandler.GetFoldersAsync(Any<IFolder>()).Returns(folders);

        var result = await _folderHandler.Object.GetFoldersAsync(_mockFolder.Object);

        await Assert.That(result).Count().IsEqualTo(2);
    }

    [Test]
    public async Task GetItemAsync_ByName_ReturnsStorageItem()
    {
        const string itemName = "item";
        var storageItem = Mock.Of<IStorageItem>();
        _folderHandler.GetItemAsync(Any<IFolder>(), itemName).Returns(storageItem.Object);

        var result = await _folderHandler.Object.GetItemAsync(_mockFolder.Object, itemName);

        await Assert.That(result).IsSameReferenceAs(storageItem.Object);
    }

    [Test]
    public async Task GetItemsAsync_ReturnsStorageItemList()
    {
        var items = new List<IStorageItem> { _mockFile.Object, _mockFolder.Object };
        _folderHandler.GetItemsAsync(Any<IFolder>()).Returns(items);

        var result = await _folderHandler.Object.GetItemsAsync(_mockFolder.Object);

        await Assert.That(result).Count().IsEqualTo(2);
    }

    [Test]
    public async Task GetParentAsync_Folder_ReturnsParentFolder()
    {
        var parentFolder = Mock.Of<IFolder>();
        _folderHandler.GetParentAsync(Any<IFolder>()).Returns(parentFolder.Object);

        var result = await _folderHandler.Object.GetParentAsync(_mockFolder.Object);

        await Assert.That(result).IsSameReferenceAs(parentFolder.Object);
    }

    #endregion

    #region IFolderHandler - TryGetItem

    [Test]
    public async Task TryGetItemAsync_ReturnsItem_WhenExists()
    {
        const string itemName = "existing";
        var storageItem = Mock.Of<IStorageItem>();
        _folderHandler.TryGetItemAsync(Any<IFolder>(), itemName).Returns(storageItem.Object);

        var result = await _folderHandler.Object.TryGetItemAsync(_mockFolder.Object, itemName);

        await Assert.That(result).IsSameReferenceAs(storageItem.Object);
    }

    [Test]
    public async Task TryGetItemAsync_ReturnsNull_WhenNotExists()
    {
        _folderHandler.TryGetItemAsync(Any<IFolder>(), "nonexistent").Returns((IStorageItem?)null);

        var result = await _folderHandler.Object.TryGetItemAsync(_mockFolder.Object, "nonexistent");

        await Assert.That(result).IsNull();
    }

    #endregion

    #region IFolderHandler - Rename Operations

    [Test]
    public async Task RenameAsync_Folder_CallsHandlerCorrectly()
    {
        const string desiredName = "renamedFolder";

        await _folderHandler.Object.RenameAsync(_mockFolder.Object, desiredName);

        _folderHandler.RenameAsync(Any<IFolder>(), desiredName).WasCalled(Times.Once);
    }

    [Test]
    public async Task RenameAsync_FolderWithCollisionOption_CallsHandlerCorrectly()
    {
        const string desiredName = "renamedFolder";

        await _folderHandler.Object.RenameAsync(_mockFolder.Object, desiredName, NameCollisionOption.GenerateUniqueName);

        _folderHandler.RenameAsync(Any<IFolder>(), desiredName, NameCollisionOption.GenerateUniqueName).WasCalled(Times.Once);
    }

    #endregion

    #region IFolderHandler - Error Propagation

    [Test]
    public async Task GetFileAsync_PropagatesException_WhenHandlerThrows()
    {
        _folderHandler.GetFileAsync(Any<IFolder>(), Any<string>())
            .Throws(new FileNotFoundException("Not found"));

        var act = async () => await _folderHandler.Object.GetFileAsync(_mockFolder.Object, "missing.txt");

        await Assert.That(act).ThrowsExactly<FileNotFoundException>();
    }

    [Test]
    public async Task DeleteAsync_PropagatesException_WhenHandlerThrows()
    {
        _folderHandler.DeleteAsync(Any<IFolder>())
            .Throws(new UnauthorizedAccessException("Access denied"));

        var act = async () => await _folderHandler.Object.DeleteAsync(_mockFolder.Object);

        await Assert.That(act).ThrowsExactly<UnauthorizedAccessException>();
    }

    #endregion

    #region IStorageItem Properties

    [Test]
    public async Task IFile_Properties_CanBeConfigured()
    {
        _mockFile.Name.Returns("test.txt");
        _mockFile.Path.Returns("/path/to/test.txt");
        _mockFile.FolderRelativeId.Returns("folder-123");

        await Assert.That(_mockFile.Object.Name).IsEqualTo("test.txt");
        await Assert.That(_mockFile.Object.Path).IsEqualTo("/path/to/test.txt");
        await Assert.That(_mockFile.Object.FolderRelativeId).IsEqualTo("folder-123");
    }

    [Test]
    public async Task IFolder_Properties_CanBeConfigured()
    {
        _mockFolder.Name.Returns("testFolder");
        _mockFolder.Path.Returns("/path/to/testFolder");
        _mockFolder.FolderRelativeId.Returns("folder-456");

        await Assert.That(_mockFolder.Object.Name).IsEqualTo("testFolder");
        await Assert.That(_mockFolder.Object.Path).IsEqualTo("/path/to/testFolder");
        await Assert.That(_mockFolder.Object.FolderRelativeId).IsEqualTo("folder-456");
    }

    [Test]
    public async Task IFile_GetSizeAsync_ReturnsCorrectSize()
    {
        _mockFile.GetSizeAsync().Returns(1024UL);

        var size = await _mockFile.Object.GetSizeAsync();

        await Assert.That(size).IsEqualTo(1024UL);
    }

    [Test]
    public async Task IFile_GetSizeAsync_ReturnsZeroForEmptyFile()
    {
        _mockFile.GetSizeAsync().Returns(0UL);

        var size = await _mockFile.Object.GetSizeAsync();

        await Assert.That(size).IsEqualTo(0UL);
    }

    #endregion

    #region Enum Validation

    [Test]
    public async Task NameCollisionOption_HasExpectedValues()
    {
        await Assert.That(Enum.GetValues<NameCollisionOption>()).Count().IsEqualTo(3);
        await Assert.That(NameCollisionOption.GenerateUniqueName).IsEqualTo((NameCollisionOption)0);
        await Assert.That(NameCollisionOption.ReplaceExisting).IsEqualTo((NameCollisionOption)1);
        await Assert.That(NameCollisionOption.FailIfExists).IsEqualTo((NameCollisionOption)2);
    }

    [Test]
    public async Task CreationCollisionOption_HasExpectedValues()
    {
        await Assert.That(Enum.GetValues<CreationCollisionOption>()).Count().IsEqualTo(4);
        await Assert.That(CreationCollisionOption.GenerateUniqueName).IsEqualTo((CreationCollisionOption)0);
    }

    [Test]
    public async Task FileAccessMode_HasExpectedValues()
    {
        await Assert.That(Enum.GetValues<FileAccessMode>()).Count().IsEqualTo(2);
        await Assert.That(FileAccessMode.Read).IsEqualTo((FileAccessMode)0);
        await Assert.That(FileAccessMode.ReadWrite).IsEqualTo((FileAccessMode)1);
    }

    [Test]
    public async Task StorageDeletionOption_HasExpectedValues()
    {
        await Assert.That(Enum.GetValues<StorageDeletionOption>()).Count().IsEqualTo(2);
    }

    [Test]
    public async Task StorageOpenOptions_HasExpectedValues()
    {
        await Assert.That(Enum.GetValues<StorageOpenOptions>()).Count().IsEqualTo(3);
    }

    #endregion

    #region FileSystemProvider - Singleton

    [Test]
    public async Task Instance_ReturnsNonNullInstance()
    {
        var instance = FileSystemProvider.Instance;

        await Assert.That(instance).IsNotNull();
    }

    [Test]
    public async Task Instance_ReturnsSameInstance_OnMultipleCalls()
    {
        var first = FileSystemProvider.Instance;
        var second = FileSystemProvider.Instance;

        await Assert.That(first).IsSameReferenceAs(second);
    }

    [Test]
    public async Task GetService_ThrowsInvalidOperationException_WhenServiceProviderIsNull()
    {
        var instance = FileSystemProvider.Instance;

        var act = () => instance.GetService<IFileHandler>();

        await Assert.That(act).Throws<InvalidOperationException>();
    }

    [Test]
    public async Task GetRequiredService_ThrowsInvalidOperationException_WhenServiceProviderIsNull()
    {
        var instance = FileSystemProvider.Instance;

        var act = () => instance.GetRequiredService<IFileHandler>();

        await Assert.That(act).Throws<InvalidOperationException>();
    }

    #endregion

    #region ServiceCollectionExtensions - EnsureIfExists

    [Test]
    public async Task EnsureIfExists_ThrowsArgumentNullException_WhenServicesIsNull()
    {
        IServiceCollection? nullServices = null;

        var act = () => nullServices!.EnsureIfExists<IFileHandler>();

        await Assert.That(act).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task EnsureIfExists_ThrowsInvalidOperationException_WhenServiceTypeIsNotRegistered()
    {
        var services = new ServiceCollection();

        var act = () => services.EnsureIfExists<IFileHandler>();

        await Assert.That(act).Throws<InvalidOperationException>();
    }

    [Test]
    public async Task EnsureIfExists_Succeeds_WhenServiceTypeIsRegistered()
    {
        var services = new ServiceCollection();
        var mockFileHandler = Mock.Of<IFileHandler>();
        services.AddSingleton(mockFileHandler.Object);

        var act = () => services.EnsureIfExists<IFileHandler>();

        await Assert.That(act).ThrowsNothing();
    }

    #endregion
}
