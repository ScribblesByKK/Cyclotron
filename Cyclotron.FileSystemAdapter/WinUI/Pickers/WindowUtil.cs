using Microsoft.UI;
using Windows.Win32;

namespace Cyclotron.FileSystemAdapter.WinUI.Pickers;

internal static class WindowUtil
{
    public static WindowId GetActiveWindowId()
    {
        IntPtr hwndActive = PInvoke.GetActiveWindow();
        if (hwndActive == IntPtr.Zero)
        {
            throw new InvalidOperationException("No active window is available.");
        }

        return Win32Interop.GetWindowIdFromWindow(hwndActive);
    }

    public static WindowId GetForegroundWindowId()
    {
        IntPtr hwndForeground = PInvoke.GetForegroundWindow();
        if (hwndForeground == IntPtr.Zero)
        {
            throw new InvalidOperationException("No foreground window is available.");
        }
        return Win32Interop.GetWindowIdFromWindow(hwndForeground);
    }
}
