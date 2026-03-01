using Cyclotron.FileSystemAdapter;
using Cyclotron.FileSystemAdapter.Abstractions.Handlers;
using Cyclotron.FileSystemAdapter.Abstractions.Models;
using Cyclotron.FileSystemAdapter.WinUI;
using Cyclotron.Tests.Integration.Fixtures;
using AwesomeAssertions;
using WinStorage = Windows.Storage;

namespace Cyclotron.Tests.Integration.FileSystemAdapter;

[Category("Integration")]
[Category("FileSystemAdapter")]
public class FileSystemAdapterIntegrationTests
{
    private TempFileSystemFixture _fixture = null!;
    private IFolderHandler _folderHandler = null!;
    private IFileHandler _fileHandler = null!;
    private IFolder _rootFolder = null!;

    [Before(Test)]
    public async Task Setup()
    {
        _fixture = new TempFileSystemFixture();
        _folderHandler = FileSystemProvider.Instance.GetRequiredService<IFolderHandler>();
        _fileHandler = FileSystemProvider.Instance.GetRequiredService<IFileHandler>();
        _rootFolder = await _folderHandler.GetFolderFromPathAsync(_fixture.RootPath);
    }

    [After(Test)]
    public async Task Cleanup()
    {
        await _fixture.DisposeAsync();
    }

    #region WinUIFolder - Properties

    [Test]
    public void WinUIFolder_Name_ReturnsCorrectName()
    {
        _rootFolder.Name.Should().NotBeNullOrEmpty();
    }

    [Test]
    public void WinUIFolder_Path_ReturnsRootPath()
    {
        _rootFolder.Path.Should().Be(_fixture.RootPath);
    }

    [Test]
    public void WinUIFolder_FolderRelativeId_IsNotEmpty()
    {
        _rootFolder.FolderRelativeId.Should().NotBeNullOrEmpty();
    }

    #endregion

    #region WinUIFolderHandler - Create

    [Test]
    public async Task FolderHandler_CreateFileAsync_CreatesFile()
    {
        await _folderHandler.CreateFileAsync(_rootFolder, "created.txt");

        File.Exists(Path.Combine(_fixture.RootPath, "created.txt")).Should().BeTrue();
    }

    [Test]
    public async Task FolderHandler_CreateFileAsync_WithCollisionOption_CreatesFile()
    {
        await _folderHandler.CreateFileAsync(_rootFolder, "created2.txt", CreationCollisionOption.ReplaceExisting);

        File.Exists(Path.Combine(_fixture.RootPath, "created2.txt")).Should().BeTrue();
    }

    [Test]
    public async Task FolderHandler_CreateFolderAsync_CreatesSubfolder()
    {
        await _folderHandler.CreateFolderAsync(_rootFolder, "subfolder");

        Directory.Exists(Path.Combine(_fixture.RootPath, "subfolder")).Should().BeTrue();
    }

    [Test]
    public async Task FolderHandler_CreateFolderAsync_WithCollisionOption_CreatesSubfolder()
    {
        await _folderHandler.CreateFolderAsync(_rootFolder, "subfolder2", CreationCollisionOption.OpenIfExists);

        Directory.Exists(Path.Combine(_fixture.RootPath, "subfolder2")).Should().BeTrue();
    }

    #endregion

    #region WinUIFolderHandler - Retrieve

    [Test]
    public async Task FolderHandler_GetFileAsync_ByName_ReturnsWinUIFile()
    {
        await _folderHandler.CreateFileAsync(_rootFolder, "get.txt");

        var file = await _folderHandler.GetFileAsync(_rootFolder, "get.txt");

        file.Should().NotBeNull();
        file.Name.Should().Be("get.txt");
    }

    [Test]
    public async Task FolderHandler_GetFileAsync_AllFiles_ReturnsList()
    {
        await _folderHandler.CreateFileAsync(_rootFolder, "a.txt");
        await _folderHandler.CreateFileAsync(_rootFolder, "b.txt");

        var files = await _folderHandler.GetFileAsync(_rootFolder);

        files.Should().HaveCountGreaterThanOrEqualTo(2);
    }

    [Test]
    public async Task FolderHandler_GetFolderAsync_ByName_ReturnsSubfolder()
    {
        await _folderHandler.CreateFolderAsync(_rootFolder, "sub");

        var sub = await _folderHandler.GetFolderAsync(_rootFolder, "sub");

        sub.Should().NotBeNull();
        sub.Name.Should().Be("sub");
    }

    [Test]
    public async Task FolderHandler_GetFoldersAsync_ReturnsList()
    {
        await _folderHandler.CreateFolderAsync(_rootFolder, "sub1");
        await _folderHandler.CreateFolderAsync(_rootFolder, "sub2");

        var folders = await _folderHandler.GetFoldersAsync(_rootFolder);

        folders.Should().HaveCountGreaterThanOrEqualTo(2);
    }

    [Test]
    public async Task FolderHandler_GetItemAsync_ReturnsItem()
    {
        await _folderHandler.CreateFileAsync(_rootFolder, "item.txt");

        var item = await _folderHandler.GetItemAsync(_rootFolder, "item.txt");

        item.Should().NotBeNull();
        item.Name.Should().Be("item.txt");
    }

    [Test]
    public async Task FolderHandler_GetItemsAsync_ReturnsAllItems()
    {
        await _folderHandler.CreateFileAsync(_rootFolder, "f.txt");
        await _folderHandler.CreateFolderAsync(_rootFolder, "d");

        var items = await _folderHandler.GetItemsAsync(_rootFolder);

        items.Should().HaveCountGreaterThanOrEqualTo(2);
    }

    [Test]
    public async Task FolderHandler_TryGetItemAsync_ReturnsItem_WhenExists()
    {
        await _folderHandler.CreateFileAsync(_rootFolder, "tryget.txt");

        var item = await _folderHandler.TryGetItemAsync(_rootFolder, "tryget.txt");

        item.Should().NotBeNull();
    }

    [Test]
    public async Task FolderHandler_TryGetItemAsync_ReturnsNull_WhenNotExists()
    {
        var item = await _folderHandler.TryGetItemAsync(_rootFolder, "nonexistent.txt");

        item.Should().BeNull();
    }

    [Test]
    public async Task FolderHandler_GetParentAsync_ReturnsParent()
    {
        await _folderHandler.CreateFolderAsync(_rootFolder, "child");
        var child = await _folderHandler.GetFolderAsync(_rootFolder, "child");

        var parent = await _folderHandler.GetParentAsync(child);

        parent.Should().NotBeNull();
        parent.Path.Should().Be(_fixture.RootPath);
    }

    #endregion

    #region WinUIFolderHandler - Rename & Delete

    [Test]
    public async Task FolderHandler_RenameAsync_RenamesFolder()
    {
        await _folderHandler.CreateFolderAsync(_rootFolder, "original");
        var original = await _folderHandler.GetFolderAsync(_rootFolder, "original");

        await _folderHandler.RenameAsync(original, "renamed");

        Directory.Exists(Path.Combine(_fixture.RootPath, "renamed")).Should().BeTrue();
    }

    [Test]
    public async Task FolderHandler_RenameAsync_WithCollisionOption_RenamesFolder()
    {
        await _folderHandler.CreateFolderAsync(_rootFolder, "orig2");
        var folder = await _folderHandler.GetFolderAsync(_rootFolder, "orig2");

        await _folderHandler.RenameAsync(folder, "ren2", NameCollisionOption.GenerateUniqueName);

        Directory.Exists(Path.Combine(_fixture.RootPath, "orig2")).Should().BeFalse();
    }

    [Test]
    public async Task FolderHandler_DeleteAsync_DeletesFolder()
    {
        await _folderHandler.CreateFolderAsync(_rootFolder, "todelete");
        var toDelete = await _folderHandler.GetFolderAsync(_rootFolder, "todelete");

        await _folderHandler.DeleteAsync(toDelete);

        Directory.Exists(Path.Combine(_fixture.RootPath, "todelete")).Should().BeFalse();
    }

    [Test]
    public async Task FolderHandler_DeleteAsync_WithOption_DeletesFolder()
    {
        await _folderHandler.CreateFolderAsync(_rootFolder, "perm");
        var perm = await _folderHandler.GetFolderAsync(_rootFolder, "perm");

        await _folderHandler.DeleteAsync(perm, StorageDeletionOption.PermanentDelete);

        Directory.Exists(Path.Combine(_fixture.RootPath, "perm")).Should().BeFalse();
    }

    #endregion

    #region WinUIFileHandler - Get & Properties

    [Test]
    public async Task FileHandler_GetFileFromPathAsync_ReturnsWinUIFile()
    {
        var path = _fixture.GetTempFilePath("read.txt");
        await File.WriteAllTextAsync(path, "hello");

        var file = await _fileHandler.GetFileFromPathAsync(path);

        file.Should().NotBeNull();
        file.Name.Should().Be("read.txt");
        file.Path.Should().Be(path);
        file.FolderRelativeId.Should().NotBeNullOrEmpty();
    }

    [Test]
    public async Task WinUIFile_GetSizeAsync_ReturnsFileSize()
    {
        var path = _fixture.GetTempFilePath("sized.txt");
        await File.WriteAllTextAsync(path, "hello");

        var file = await _fileHandler.GetFileFromPathAsync(path);
        var size = await file.GetSizeAsync();

        size.Should().BeGreaterThan(0);
    }

    [Test]
    public async Task FileHandler_GetParentAsync_ReturnsParentFolder()
    {
        var path = _fixture.GetTempFilePath("parented.txt");
        await File.WriteAllTextAsync(path, "content");
        var file = await _fileHandler.GetFileFromPathAsync(path);

        var parent = await _fileHandler.GetParentAsync(file);

        parent.Should().NotBeNull();
        parent.Path.Should().Be(_fixture.RootPath);
    }

    #endregion

    #region WinUIFileHandler - Rename

    [Test]
    public async Task FileHandler_RenameAsync_RenamesFile()
    {
        await _folderHandler.CreateFileAsync(_rootFolder, "orig.txt");
        var file = await _folderHandler.GetFileAsync(_rootFolder, "orig.txt");

        await _fileHandler.RenameAsync(file, "renamed.txt");

        File.Exists(Path.Combine(_fixture.RootPath, "renamed.txt")).Should().BeTrue();
    }

    [Test]
    public async Task FileHandler_RenameAsync_WithCollisionOption_RenamesFile()
    {
        await _folderHandler.CreateFileAsync(_rootFolder, "orig2.txt");
        var file = await _folderHandler.GetFileAsync(_rootFolder, "orig2.txt");

        await _fileHandler.RenameAsync(file, "ren2.txt", NameCollisionOption.GenerateUniqueName);

        File.Exists(Path.Combine(_fixture.RootPath, "orig2.txt")).Should().BeFalse();
    }

    #endregion

    #region WinUIFileHandler - Copy

    [Test]
    public async Task FileHandler_CopyAsync_CopiesFile()
    {
        await _folderHandler.CreateFileAsync(_rootFolder, "src.txt");
        var src = await _folderHandler.GetFileAsync(_rootFolder, "src.txt");
        await _folderHandler.CreateFolderAsync(_rootFolder, "dest");
        var destFolder = await _folderHandler.GetFolderAsync(_rootFolder, "dest");

        await _fileHandler.CopyAsync(src, destFolder, "copied.txt", NameCollisionOption.ReplaceExisting);

        File.Exists(Path.Combine(_fixture.RootPath, "dest", "copied.txt")).Should().BeTrue();
    }

    [Test]
    public async Task FileHandler_CopyAndReplaceAsync_ReplacesDestFile()
    {
        await _folderHandler.CreateFileAsync(_rootFolder, "csrc.txt");
        await _folderHandler.CreateFileAsync(_rootFolder, "cdest.txt");
        var src = await _folderHandler.GetFileAsync(_rootFolder, "csrc.txt");
        var dest = await _folderHandler.GetFileAsync(_rootFolder, "cdest.txt");

        await _fileHandler.CopyAndReplaceAsync(src, dest);

        File.Exists(Path.Combine(_fixture.RootPath, "cdest.txt")).Should().BeTrue();
    }

    #endregion

    #region WinUIFileHandler - Move

    [Test]
    public async Task FileHandler_MoveAsync_ToFolder_MovesFile()
    {
        await _folderHandler.CreateFileAsync(_rootFolder, "msrc.txt");
        var src = await _folderHandler.GetFileAsync(_rootFolder, "msrc.txt");
        await _folderHandler.CreateFolderAsync(_rootFolder, "mdest");
        var destFolder = await _folderHandler.GetFolderAsync(_rootFolder, "mdest");

        await _fileHandler.MoveAsync(src, destFolder);

        File.Exists(Path.Combine(_fixture.RootPath, "mdest", "msrc.txt")).Should().BeTrue();
    }

    [Test]
    public async Task FileHandler_MoveAsync_WithName_MovesFile()
    {
        await _folderHandler.CreateFileAsync(_rootFolder, "msrc2.txt");
        var src = await _folderHandler.GetFileAsync(_rootFolder, "msrc2.txt");
        await _folderHandler.CreateFolderAsync(_rootFolder, "mdest2");
        var destFolder = await _folderHandler.GetFolderAsync(_rootFolder, "mdest2");

        await _fileHandler.MoveAsync(src, destFolder, "moved2.txt");

        File.Exists(Path.Combine(_fixture.RootPath, "mdest2", "moved2.txt")).Should().BeTrue();
    }

    [Test]
    public async Task FileHandler_MoveAsync_WithCollisionOption_MovesFile()
    {
        await _folderHandler.CreateFileAsync(_rootFolder, "msrc3.txt");
        var src = await _folderHandler.GetFileAsync(_rootFolder, "msrc3.txt");
        await _folderHandler.CreateFolderAsync(_rootFolder, "mdest3");
        var destFolder = await _folderHandler.GetFolderAsync(_rootFolder, "mdest3");

        await _fileHandler.MoveAsync(src, destFolder, "moved3.txt", NameCollisionOption.ReplaceExisting);

        File.Exists(Path.Combine(_fixture.RootPath, "mdest3", "moved3.txt")).Should().BeTrue();
    }

    [Test]
    public async Task FileHandler_MoveAndReplaceAsync_ReplacesDestFile()
    {
        await _folderHandler.CreateFileAsync(_rootFolder, "mrsrc.txt");
        await _folderHandler.CreateFileAsync(_rootFolder, "mrdest.txt");
        var src = await _folderHandler.GetFileAsync(_rootFolder, "mrsrc.txt");
        var dest = await _folderHandler.GetFileAsync(_rootFolder, "mrdest.txt");

        await _fileHandler.MoveAndReplaceAsync(src, dest);

        File.Exists(Path.Combine(_fixture.RootPath, "mrdest.txt")).Should().BeTrue();
        File.Exists(Path.Combine(_fixture.RootPath, "mrsrc.txt")).Should().BeFalse();
    }

    #endregion

    #region WinUIFileHandler - Open

    [Test]
    public async Task FileHandler_OpenAsync_Read_DoesNotThrow()
    {
        var path = _fixture.GetTempFilePath("open.txt");
        await File.WriteAllTextAsync(path, "content");
        var file = await _fileHandler.GetFileFromPathAsync(path);

        var act = async () => await _fileHandler.OpenAsync(file, FileAccessMode.Read);

        await act.Should().NotThrowAsync();
    }

    [Test]
    public async Task FileHandler_OpenAsync_WithStorageOptions_DoesNotThrow()
    {
        var path = _fixture.GetTempFilePath("open2.txt");
        await File.WriteAllTextAsync(path, "content");
        var file = await _fileHandler.GetFileFromPathAsync(path);

        var act = async () => await _fileHandler.OpenAsync(file, FileAccessMode.Read, StorageOpenOptions.AllowOnlyReaders);

        await act.Should().NotThrowAsync();
    }

    #endregion

    #region FileExtensions & FolderExtensions (via WinUI objects)

    [Test]
    public async Task FileExtensions_RenameAsync_RenamesFile()
    {
        await _folderHandler.CreateFileAsync(_rootFolder, "ext_orig.txt");
        var file = await _folderHandler.GetFileAsync(_rootFolder, "ext_orig.txt");

        await file.RenameAsync("ext_renamed.txt");

        File.Exists(Path.Combine(_fixture.RootPath, "ext_renamed.txt")).Should().BeTrue();
    }

    [Test]
    public async Task FileExtensions_RenameAsync_WithCollision_RenamesFile()
    {
        await _folderHandler.CreateFileAsync(_rootFolder, "ext_orig2.txt");
        var file = await _folderHandler.GetFileAsync(_rootFolder, "ext_orig2.txt");

        await file.RenameAsync("ext_ren2.txt", NameCollisionOption.GenerateUniqueName);

        File.Exists(Path.Combine(_fixture.RootPath, "ext_orig2.txt")).Should().BeFalse();
    }

    [Test]
    public async Task FileExtensions_GetParentAsync_ReturnsParent()
    {
        await _folderHandler.CreateFileAsync(_rootFolder, "ext_par.txt");
        var file = await _folderHandler.GetFileAsync(_rootFolder, "ext_par.txt");

        var parent = await file.GetParentAsync();

        parent.Path.Should().Be(_fixture.RootPath);
    }

    [Test]
    public async Task FileExtensions_MoveAsync_MovesFile()
    {
        await _folderHandler.CreateFileAsync(_rootFolder, "ext_mv.txt");
        var file = await _folderHandler.GetFileAsync(_rootFolder, "ext_mv.txt");
        await _folderHandler.CreateFolderAsync(_rootFolder, "ext_mvdest");
        var dest = await _folderHandler.GetFolderAsync(_rootFolder, "ext_mvdest");

        await file.MoveAsync(dest);

        File.Exists(Path.Combine(_fixture.RootPath, "ext_mvdest", "ext_mv.txt")).Should().BeTrue();
    }

    [Test]
    public async Task FileExtensions_MoveAsync_WithName_MovesFile()
    {
        await _folderHandler.CreateFileAsync(_rootFolder, "ext_mv2.txt");
        var file = await _folderHandler.GetFileAsync(_rootFolder, "ext_mv2.txt");
        await _folderHandler.CreateFolderAsync(_rootFolder, "ext_mvdest2");
        var dest = await _folderHandler.GetFolderAsync(_rootFolder, "ext_mvdest2");

        await file.MoveAsync(dest, "ext_moved2.txt");

        File.Exists(Path.Combine(_fixture.RootPath, "ext_mvdest2", "ext_moved2.txt")).Should().BeTrue();
    }

    [Test]
    public async Task FileExtensions_MoveAsync_WithCollision_MovesFile()
    {
        await _folderHandler.CreateFileAsync(_rootFolder, "ext_mv3.txt");
        var file = await _folderHandler.GetFileAsync(_rootFolder, "ext_mv3.txt");
        await _folderHandler.CreateFolderAsync(_rootFolder, "ext_mvdest3");
        var dest = await _folderHandler.GetFolderAsync(_rootFolder, "ext_mvdest3");

        await file.MoveAsync(dest, "ext_moved3.txt", NameCollisionOption.ReplaceExisting);

        File.Exists(Path.Combine(_fixture.RootPath, "ext_mvdest3", "ext_moved3.txt")).Should().BeTrue();
    }

    [Test]
    public async Task FileExtensions_CopyAsync_CopiesFile()
    {
        await _folderHandler.CreateFileAsync(_rootFolder, "ext_cp.txt");
        var file = await _folderHandler.GetFileAsync(_rootFolder, "ext_cp.txt");
        await _folderHandler.CreateFolderAsync(_rootFolder, "ext_cpdest");
        var dest = await _folderHandler.GetFolderAsync(_rootFolder, "ext_cpdest");

        await file.CopyAsync(dest, "ext_copied.txt", NameCollisionOption.ReplaceExisting);

        File.Exists(Path.Combine(_fixture.RootPath, "ext_cpdest", "ext_copied.txt")).Should().BeTrue();
    }

    [Test]
    public async Task FileExtensions_CopyAndReplaceAsync_ReplacesFile()
    {
        await _folderHandler.CreateFileAsync(_rootFolder, "ext_cpsrc.txt");
        await _folderHandler.CreateFileAsync(_rootFolder, "ext_cpdst.txt");
        var src = await _folderHandler.GetFileAsync(_rootFolder, "ext_cpsrc.txt");
        var dst = await _folderHandler.GetFileAsync(_rootFolder, "ext_cpdst.txt");

        await src.CopyAndReplaceAsync(dst);

        File.Exists(Path.Combine(_fixture.RootPath, "ext_cpdst.txt")).Should().BeTrue();
    }

    [Test]
    public async Task FileExtensions_MoveAndReplaceAsync_ReplacesFile()
    {
        await _folderHandler.CreateFileAsync(_rootFolder, "ext_mrsrc.txt");
        await _folderHandler.CreateFileAsync(_rootFolder, "ext_mrdst.txt");
        var src = await _folderHandler.GetFileAsync(_rootFolder, "ext_mrsrc.txt");
        var dst = await _folderHandler.GetFileAsync(_rootFolder, "ext_mrdst.txt");

        await src.MoveAndReplaceAsync(dst);

        File.Exists(Path.Combine(_fixture.RootPath, "ext_mrsrc.txt")).Should().BeFalse();
    }

    [Test]
    public async Task FileExtensions_OpenAsync_DoesNotThrow()
    {
        var path = _fixture.GetTempFilePath("ext_open.txt");
        await File.WriteAllTextAsync(path, "data");
        var file = await _fileHandler.GetFileFromPathAsync(path);

        var act = async () => await file.OpenAsync(FileAccessMode.Read);

        await act.Should().NotThrowAsync();
    }

    [Test]
    public async Task FileExtensions_OpenAsync_WithOptions_DoesNotThrow()
    {
        var path = _fixture.GetTempFilePath("ext_open2.txt");
        await File.WriteAllTextAsync(path, "data");
        var file = await _fileHandler.GetFileFromPathAsync(path);

        var act = async () => await file.OpenAsync(FileAccessMode.Read, StorageOpenOptions.AllowOnlyReaders);

        await act.Should().NotThrowAsync();
    }

    [Test]
    public async Task FolderExtensions_CreateFileAsync_CreatesFile()
    {
        await _rootFolder.CreateFileAsync("fext_create.txt");

        File.Exists(Path.Combine(_fixture.RootPath, "fext_create.txt")).Should().BeTrue();
    }

    [Test]
    public async Task FolderExtensions_CreateFileAsync_WithCollision_CreatesFile()
    {
        await _rootFolder.CreateFileAsync("fext_create2.txt", CreationCollisionOption.ReplaceExisting);

        File.Exists(Path.Combine(_fixture.RootPath, "fext_create2.txt")).Should().BeTrue();
    }

    [Test]
    public async Task FolderExtensions_CreateFolderAsync_CreatesSubfolder()
    {
        await _rootFolder.CreateFolderAsync("fext_sub");

        Directory.Exists(Path.Combine(_fixture.RootPath, "fext_sub")).Should().BeTrue();
    }

    [Test]
    public async Task FolderExtensions_CreateFolderAsync_WithCollision_CreatesSubfolder()
    {
        await _rootFolder.CreateFolderAsync("fext_sub2", CreationCollisionOption.OpenIfExists);

        Directory.Exists(Path.Combine(_fixture.RootPath, "fext_sub2")).Should().BeTrue();
    }

    [Test]
    public async Task FolderExtensions_GetFileAsync_ByName_ReturnsFile()
    {
        await _folderHandler.CreateFileAsync(_rootFolder, "fext_get.txt");

        var file = await _rootFolder.GetFileAsync("fext_get.txt");

        file.Name.Should().Be("fext_get.txt");
    }

    [Test]
    public async Task FolderExtensions_GetFilesAsync_ReturnsList()
    {
        await _folderHandler.CreateFileAsync(_rootFolder, "fext_gfa.txt");

        var files = await _rootFolder.GetFilesAsync();

        files.Should().NotBeEmpty();
    }

    [Test]
    public async Task FolderExtensions_GetFolderAsync_ReturnsSubfolder()
    {
        await _folderHandler.CreateFolderAsync(_rootFolder, "fext_gfo");

        var folder = await _rootFolder.GetFolderAsync("fext_gfo");

        folder.Name.Should().Be("fext_gfo");
    }

    [Test]
    public async Task FolderExtensions_GetFoldersAsync_ReturnsList()
    {
        await _folderHandler.CreateFolderAsync(_rootFolder, "fext_gfoa");

        var folders = await _rootFolder.GetFoldersAsync();

        folders.Should().NotBeEmpty();
    }

    [Test]
    public async Task FolderExtensions_GetItemAsync_ReturnsItem()
    {
        await _folderHandler.CreateFileAsync(_rootFolder, "fext_item.txt");

        var item = await _rootFolder.GetItemAsync("fext_item.txt");

        item.Name.Should().Be("fext_item.txt");
    }

    [Test]
    public async Task FolderExtensions_GetItemsAsync_ReturnsList()
    {
        await _folderHandler.CreateFileAsync(_rootFolder, "fext_items.txt");

        var items = await _rootFolder.GetItemsAsync();

        items.Should().NotBeEmpty();
    }

    [Test]
    public async Task FolderExtensions_GetParentAsync_ReturnsParent()
    {
        await _folderHandler.CreateFolderAsync(_rootFolder, "fext_child");
        var child = await _folderHandler.GetFolderAsync(_rootFolder, "fext_child");

        var parent = await child.GetParentAsync();

        parent.Path.Should().Be(_fixture.RootPath);
    }

    [Test]
    public async Task FolderExtensions_TryGetItemAsync_ReturnsItem_WhenExists()
    {
        await _folderHandler.CreateFileAsync(_rootFolder, "fext_try.txt");

        var item = await _rootFolder.TryGetItemAsync("fext_try.txt");

        item.Should().NotBeNull();
    }

    [Test]
    public async Task FolderExtensions_TryGetItemAsync_ReturnsNull_WhenNotExists()
    {
        var item = await _rootFolder.TryGetItemAsync("fext_noexist.txt");

        item.Should().BeNull();
    }

    [Test]
    public async Task FolderExtensions_RenameAsync_RenamesFolder()
    {
        await _folderHandler.CreateFolderAsync(_rootFolder, "fext_ren");
        var folder = await _folderHandler.GetFolderAsync(_rootFolder, "fext_ren");

        await folder.RenameAsync("fext_renamed");

        Directory.Exists(Path.Combine(_fixture.RootPath, "fext_renamed")).Should().BeTrue();
    }

    [Test]
    public async Task FolderExtensions_RenameAsync_WithCollision_RenamesFolder()
    {
        await _folderHandler.CreateFolderAsync(_rootFolder, "fext_ren2");
        var folder = await _folderHandler.GetFolderAsync(_rootFolder, "fext_ren2");

        await folder.RenameAsync("fext_ren2b", NameCollisionOption.GenerateUniqueName);

        Directory.Exists(Path.Combine(_fixture.RootPath, "fext_ren2")).Should().BeFalse();
    }

    [Test]
    public async Task FolderExtensions_DeleteAsync_DeletesFolder()
    {
        await _folderHandler.CreateFolderAsync(_rootFolder, "fext_del");
        var folder = await _folderHandler.GetFolderAsync(_rootFolder, "fext_del");

        await folder.DeleteAsync();

        Directory.Exists(Path.Combine(_fixture.RootPath, "fext_del")).Should().BeFalse();
    }

    [Test]
    public async Task FolderExtensions_DeleteAsync_WithOption_DeletesFolder()
    {
        await _folderHandler.CreateFolderAsync(_rootFolder, "fext_delp");
        var folder = await _folderHandler.GetFolderAsync(_rootFolder, "fext_delp");

        await folder.DeleteAsync(StorageDeletionOption.PermanentDelete);

        Directory.Exists(Path.Combine(_fixture.RootPath, "fext_delp")).Should().BeFalse();
    }

    #endregion

    #region WinUI Storage Extensions (AsIFile / AsIFolder)

    [Test]
    public async Task StorageFile_AsIFile_ReturnsWinUIFile()
    {
        var path = _fixture.GetTempFilePath("asfile.txt");
        await File.WriteAllTextAsync(path, "content");
        var storageFile = await WinStorage.StorageFile.GetFileFromPathAsync(path);

        var iFile = storageFile.AsIFile();

        iFile.Should().NotBeNull();
        iFile.Name.Should().Be("asfile.txt");
    }

    [Test]
    public async Task StorageFolder_AsIFolder_ReturnsWinUIFolder()
    {
        var storageFolder = await WinStorage.StorageFolder.GetFolderFromPathAsync(_fixture.RootPath);

        var iFolder = storageFolder.AsIFolder();

        iFolder.Should().NotBeNull();
        iFolder.Path.Should().Be(_fixture.RootPath);
    }

    #endregion
}

