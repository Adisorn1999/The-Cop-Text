using Gma.System.MouseKeyHook;
using System.Text;
using Forms = System.Windows.Forms;
using System.Linq;
using WpfApp1.Data;
using System.Windows;
namespace WpfApp1.Services;

public class GlobalHookService
{
    private IKeyboardMouseEvents? _hook;

    private readonly StringBuilder _buffer =
        new();

    private readonly SQLiteService _db =
        new();
    public bool IsEnabled =
    true;
    public event Action<bool>? OnToggleChanged;
    public void Start()
    {
        _hook =
            Hook.GlobalEvents();

        _hook.KeyDown += Hook_KeyDown;
    }

    private void Hook_KeyDown(
    object? sender,
    Forms.KeyEventArgs e)

    {
        // CTRL + F12
        if (e.Control &&
            e.KeyCode == Forms.Keys.F12)
        {
            IsEnabled = !IsEnabled;

            OnToggleChanged?.Invoke(
                IsEnabled);

            return;
        }
        if (!IsEnabled)
        {
            return;
        }
        // BACKSPACE
        if (e.KeyCode == Forms.Keys.Back)
        {
            if (_buffer.Length > 0)
            {
                _buffer.Remove(
                    _buffer.Length - 1,
                    1);
            }

            return;
        }

        // ENTER
        if (e.KeyCode == Forms.Keys.Enter)
        {
            _buffer.Clear();
            return;
        }
        if (e.KeyCode == Forms.Keys.Space)
        {
            _buffer.Clear();
            return;
        }

        char c =
            GetCharFromKey(e.KeyCode);

        if (c == '\0')
        {
            return;
        }

        _buffer.Append(c);
        Thread.Sleep(20);

        string keyword =
            _buffer.ToString();

        string converted =
            KeyboardLayoutHelper
                .ConvertLayout(keyword);

        var snippets =
            _db.GetAllSnippets();

        var snippet =
            snippets.FirstOrDefault(
                x =>
                    x.Keyword == keyword
                    ||
                    x.Keyword == converted);

        if (snippet == null)
        {
            return;
        }

        // BLOCK KEY ตัวล่าสุด
        e.Handled = true;
        Thread.Sleep(80);

        ReplaceText(
            keyword,
            snippet.Content);

        _buffer.Clear();
    }
    private char GetCharFromKey(
    Forms.Keys key)
    {
        // A-Z
        if (key >= Forms.Keys.A &&
            key <= Forms.Keys.Z)
        {
            return key.ToString()
                .ToLower()[0];
        }
        // NUMPAD 0-9
        if (key >= Forms.Keys.NumPad0 &&
            key <= Forms.Keys.NumPad9)
        {
            return (char)(
                '0' + (key - Forms.Keys.NumPad0));
        }

        // 0-9
        if (key >= Forms.Keys.D0 &&
            key <= Forms.Keys.D9)
        {
            return (char)(
                '0' + (key - Forms.Keys.D0));
        }

        // .
        if (key == Forms.Keys.OemPeriod ||
    key == Forms.Keys.Decimal)
        {
            return '.';
        }

        return '\0';
    }

    private void ProcessKeyword(
    string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
        {
            return;
        }

        string converted =
            KeyboardLayoutHelper
                .ConvertLayout(keyword);

        var snippets =
            _db.GetAllSnippets();

        var snippet =
            snippets.FirstOrDefault(
                x =>
                    x.Keyword == keyword
                    ||
                    x.Keyword == converted);

        if (snippet == null)
        {
            return;
        }

        ReplaceText(
    keyword,
    snippet.Content);

        _buffer.Clear();
    }

    private void ReplaceText(
    string keyword,
    string replacement)
    {
        // รอให้ key ล่าสุดเข้า textbox ก่อน
        Thread.Sleep(AppSettings.ExpandDelay);

        // ลบ keyword ที่พิมพ์
        for (int i = 0;
     i < keyword.Length+1;
     i++)
        {
            Forms.SendKeys.SendWait(
                "{BACKSPACE}");
        
    }

        Thread.Sleep(AppSettings.BeforePasteDelay);

        // backup clipboard เดิม
        string oldClipboard =
            System.Windows.Clipboard.GetText();

        // set ข้อความใหม่
        System.Windows.Clipboard.SetText(
            replacement);

        // paste
        Forms.SendKeys.SendWait(
            "^v");

        Thread.Sleep(AppSettings.AfterPasteDelay);

        // คืน clipboard เดิม
        System.Windows.Clipboard.SetText(
            oldClipboard);
    }
}