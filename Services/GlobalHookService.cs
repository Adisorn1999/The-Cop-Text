using Gma.System.MouseKeyHook;
using System.Text;
using Forms = System.Windows.Forms;
using WpfApp1.Data;
using WpfApp1.Models;

namespace WpfApp1.Services;

public class GlobalHookService : IDisposable
{
    private IKeyboardMouseEvents? _hook;

    private readonly StringBuilder _buffer =
        new();

    private readonly object _syncRoot =
        new();

    private readonly SQLiteService _db =
        new();

    private List<Snippet> _cachedSnippets =
        new();

    public bool IsEnabled =
        true;

    public event Action<bool>? OnToggleChanged;

    public void Start()
    {
        _hook =
            Hook.GlobalEvents();

        _hook.KeyDown += Hook_KeyDown;

        RefreshSnippets();
    }

    public void RefreshSnippets()
    {
        _cachedSnippets =
            _db.GetAllSnippets();
    }

    public void Stop()
    {
        if (_hook == null)
        {
            return;
        }

        _hook.KeyDown -= Hook_KeyDown;

        _hook.Dispose();

        _hook = null;
    }

    public void Dispose()
    {
        Stop();
    }

    private void Hook_KeyDown(
        object? sender,
        Forms.KeyEventArgs e)
    {
        if (e.Control && e.KeyCode == Forms.Keys.F12)
        {
            IsEnabled = !IsEnabled;
            OnToggleChanged?.Invoke(IsEnabled);
            return;
        }

        if (!IsEnabled)
        {
            return;
        }

        if (e.Control || e.Alt || e.KeyCode == Forms.Keys.LWin || e.KeyCode == Forms.Keys.RWin)
        {
            lock (_syncRoot)
            {
                _buffer.Clear();
            }
            return;
        }

        if (e.KeyCode == Forms.Keys.Delete ||
            e.KeyCode == Forms.Keys.Escape ||
            e.KeyCode == Forms.Keys.Tab ||
            e.KeyCode == Forms.Keys.Left ||
            e.KeyCode == Forms.Keys.Right ||
            e.KeyCode == Forms.Keys.Up ||
            e.KeyCode == Forms.Keys.Down ||
            e.KeyCode == Forms.Keys.Home ||
            e.KeyCode == Forms.Keys.End ||
            e.KeyCode == Forms.Keys.PageUp ||
            e.KeyCode == Forms.Keys.PageDown)
        {
            lock (_syncRoot)
            {
                _buffer.Clear();
            }
            return;
        }

        if (e.KeyCode == Forms.Keys.Back)
        {
            lock (_syncRoot)
            {
                if (_buffer.Length > 0)
                {
                    _buffer.Remove(_buffer.Length - 1, 1);
                }
            }
            return;
        }

        if (e.KeyCode == Forms.Keys.Enter || e.KeyCode == Forms.Keys.Space)
        {
            lock (_syncRoot)
            {
                _buffer.Clear();
            }
            return;
        }

        char c = GetCharFromKey(e.KeyCode);
        if (c == '\0')
        {
            return;
        }

        string keyword;
        lock (_syncRoot)
        {
            _buffer.Append(c);
            keyword = _buffer.ToString();
        }

        string converted =
            KeyboardLayoutHelper.ConvertLayout(keyword);

        var snippet =
            _cachedSnippets.FirstOrDefault(x => x.Keyword == keyword || x.Keyword == converted);

        if (snippet == null)
        {
            return;
        }

        e.Handled = true;
        ReplaceText(keyword, snippet.Content);

        lock (_syncRoot)
        {
            _buffer.Clear();
        }
    }

    private static char GetCharFromKey(Forms.Keys key)
    {
        if (key >= Forms.Keys.A && key <= Forms.Keys.Z)
        {
            return key.ToString().ToLower()[0];
        }

        if (key >= Forms.Keys.NumPad0 && key <= Forms.Keys.NumPad9)
        {
            return (char)('0' + (key - Forms.Keys.NumPad0));
        }

        if (key >= Forms.Keys.D0 && key <= Forms.Keys.D9)
        {
            return (char)('0' + (key - Forms.Keys.D0));
        }

        if (key == Forms.Keys.OemPeriod || key == Forms.Keys.Decimal)
        {
            return '.';
        }

        return '\0';
    }

    private static void ReplaceText(string keyword, string replacement)
    {
        Thread.Sleep(AppSettings.ExpandDelay);

        for (int i = 0; i < keyword.Length + 1; i++)
        {
            Forms.SendKeys.SendWait("{BACKSPACE}");
        }

        Thread.Sleep(AppSettings.BeforePasteDelay);

        var hadClipboardText =
            System.Windows.Clipboard.ContainsText();

        var oldClipboard =
            hadClipboardText ? System.Windows.Clipboard.GetText() : string.Empty;

        System.Windows.Clipboard.SetText(replacement);

        Forms.SendKeys.SendWait("^v");

        Thread.Sleep(AppSettings.AfterPasteDelay);

        if (hadClipboardText)
        {
            System.Windows.Clipboard.SetText(oldClipboard);
        }
        else
        {
            System.Windows.Clipboard.Clear();
        }
    }
}