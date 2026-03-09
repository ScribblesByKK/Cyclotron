using Cyclotron.FileSystemAdapter;

namespace Cyclotron.Tests.TestHelpers;

/// <summary>
/// Session-level setup that initializes FileSystemProvider with real WinUI services before any test runs.
/// </summary>
public class FileSystemAdapterSetup
{
    [Before(TestSession)]
    public static void InitializeFileSystemProvider()
    {
        FileSystemProvider.Initialize();
    }
}
