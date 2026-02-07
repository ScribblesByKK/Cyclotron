using Microsoft.UI;
using Windows.Win32;

namespace Cyclotron.FileSystemAdapter.WinUI.Pickers;

internal static class WindowUtil
{
    public static WindowId GetActiveWindowId()
    {
        IntPtr hwndActive = PInvoke.GetActiveWindow();
        return Win32Interop.GetWindowIdFromWindow(hwndActive);
    }

    public static WindowId GetForegroundWindowId()
    {
        IntPtr hwndForeground = PInvoke.GetForegroundWindow();
        return Win32Interop.GetWindowIdFromWindow(hwndForeground);
    }
}
