//using System.Runtime.InteropServices;
//using System.Windows.Threading;
//using Forms = System.Windows.Forms;

//namespace WpfApp1.Services;

//public class TextExpansionService
//{
//    [DllImport("user32.dll")]
//    private static extern short GetAsyncKeyState(
//        int vKey);

//    private readonly DispatcherTimer _timer =
//        new();

//    private readonly HashSet<int> _pressedKeys =
//        new();

//    private string _buffer =
//        "";

//    public void Start()
//    {
//        _timer.Interval =
//            TimeSpan.FromMilliseconds(20);

//        _timer.Tick += Timer_Tick;

//        _timer.Start();
//    }

//    private void Timer_Tick(
//        object? sender,
//        EventArgs e)
//    {
//        // .
//        HandleKey(190, '.');
//        HandleKey(110, '.');

//        // 0-9
//        for (int i = 48; i <= 57; i++)
//        {
//            HandleKey(
//                i,
//                (char)i);
//        }

//        // BACKSPACE
//        if (IsKeyPressedOnce(8))
//        {
//            if (_buffer.Length > 0)
//            {
//                _buffer =
//                    _buffer[..^1];
//            }
//        }

//        // SPACE
//        if (IsKeyPressedOnce(32))
//        {
//            System.Windows.MessageBox.Show(
//                "[" + _buffer + "]");

//            if (_buffer.Trim() == ".7")
//            {
//                ReplaceText(
//                    "asda");
//            }

//            _buffer = "";
//        }
//    }

//    private void HandleKey(
//        int key,
//        char c)
//    {
//        if (IsKeyPressedOnce(key))
//        {
//            _buffer += c;
//        }
//    }

//    private bool IsKeyPressedOnce(
//        int key)
//    {
//        bool isPressed =
//            (GetAsyncKeyState(key) & 0x8000) != 0;

//        if (isPressed &&
//            !_pressedKeys.Contains(key))
//        {
//            _pressedKeys.Add(key);

//            return true;
//        }

//        if (!isPressed &&
//            _pressedKeys.Contains(key))
//        {
//            _pressedKeys.Remove(key);
//        }

//        return false;
//    }

//    private void ReplaceText(
//        string replacement)
//    {
//        for (int i = 0; i < 3; i++)
//        {
//            Forms.SendKeys.SendWait(
//                "{BACKSPACE}");

//        }
//        Thread.Sleep(50);
//        Forms.SendKeys.SendWait(
//        replacement);
//    }
//}