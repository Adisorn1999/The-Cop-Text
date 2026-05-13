using System.Diagnostics;
using System.Runtime.InteropServices;
using Forms = System.Windows.Forms;

namespace WpfApp1.Services;

public class KeyboardHookService
{
    private static IntPtr _hookID =
        IntPtr.Zero;

    private static LowLevelKeyboardProc _proc =
        HookCallback;

    private static string _currentWord =
        "";

    private delegate IntPtr LowLevelKeyboardProc(
        int nCode,
        IntPtr wParam,
        IntPtr lParam);

    private const int WH_KEYBOARD_LL =
        13;

    private const int WM_KEYDOWN =
        0x0100;

    public void Start()
    {
        _hookID =
            SetHook(_proc);
    }

    private static IntPtr SetHook(
        LowLevelKeyboardProc proc)
    {
        using Process curProcess =
            Process.GetCurrentProcess();

        using ProcessModule curModule =
            curProcess.MainModule!;

        return SetWindowsHookEx(
            WH_KEYBOARD_LL,
            proc,
            GetModuleHandle(
                curModule.ModuleName),
            0);
    }

    private static IntPtr HookCallback(
        int nCode,
        IntPtr wParam,
        IntPtr lParam)
    {
        if (nCode >= 0)
        {
            int vkCode =
                Marshal.ReadInt32(lParam);

            // SPACE
            if (vkCode == 32)
            {
                System.Windows.MessageBox.Show(
        _currentWord);
                System.Windows.MessageBox.Show(
    vkCode.ToString());
                if (_currentWord == ".7")
                {
                    ReplaceText("asda");
                }

                _currentWord = "";
            }

            // BACKSPACE
            else if (vkCode == 8)
            {
                if (_currentWord.Length > 0)
                {
                    _currentWord =
                        _currentWord[..^1];
                }
            }

            else
            {
                char c =
                    GetChar(vkCode);

                if (c != '\0')
                {
                    _currentWord += c;
                }
            }
        }

        return CallNextHookEx(
            _hookID,
            nCode,
            wParam,
            lParam);
    }

    private static char GetChar(
        int vkCode)
    {
        // 0-9
        if (vkCode >= 48 &&
            vkCode <= 57)
        {
            return (char)vkCode;
        }

        // .
        if (vkCode == 190 || vkCode == 110)
        {
            return '.';
        }
        return '\0';
    }

    private static void ReplaceText(
        string replacement)
    {
        // ลบ .7
        for (int i = 0; i < 2; i++)
        {
            Forms.SendKeys.SendWait(
                "{BACKSPACE}");
        }

        Forms.SendKeys.SendWait(
            replacement);
    }

    [DllImport("user32.dll")]
    private static extern IntPtr SetWindowsHookEx(
        int idHook,
        LowLevelKeyboardProc lpfn,
        IntPtr hMod,
        uint dwThreadId);

    [DllImport("user32.dll")]
    private static extern bool UnhookWindowsHookEx(
        IntPtr hhk);

    [DllImport("user32.dll")]
    private static extern IntPtr CallNextHookEx(
        IntPtr hhk,
        int nCode,
        IntPtr wParam,
        IntPtr lParam);

    [DllImport("kernel32.dll")]
    private static extern IntPtr GetModuleHandle(
        string lpModuleName);
}