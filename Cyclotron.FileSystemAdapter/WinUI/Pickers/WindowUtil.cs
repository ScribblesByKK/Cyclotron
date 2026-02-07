using Microsoft.UI;
using Windows.Win32;

namespace Cyclotron.FileSystemAdapter.WinUI.Pickers;

/// <summary>
/// Utility class for retrieving WinUI window handles and IDs for file picker operations.
/// </summary>
/// <remarks>
/// This internal utility provides helper methods to get the active or foreground window ID,
/// which is required when creating WinUI file/folder picker dialogs that need to be parented to a specific window.
/// </remarks>
internal static class WindowUtil
{
    /// <summary>
    /// Gets the window ID of the currently active window.
    /// </summary>
    /// <returns>The <see cref="WindowId"/> of the active window.</returns>
    /// <exception cref="InvalidOperationException">Thrown if no active window is available.</exception>
    public static WindowId GetActiveWindowId()
    {
        IntPtr hwndActive = PInvoke.GetActiveWindow();
        if (hwndActive == IntPtr.Zero)
        {
            throw new InvalidOperationException("No active window is available.");
        }

        return Win32Interop.GetWindowIdFromWindow(hwndActive);
    }

    /// <summary>
    /// Gets the window ID of the currently foreground (topmost) window.
    /// </summary>
    /// <returns>The <see cref="WindowId"/> of the foreground window.</returns>
    /// <exception cref="InvalidOperationException">Thrown if no foreground window is available.</exception>
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
