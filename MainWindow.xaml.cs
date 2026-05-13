using System.IO;
using System.Windows;
using System.Windows.Controls;

using WpfApp1.Data;
using WpfApp1.Services;
using WpfApp1.Views;
using System.Text;
using System.Windows.Input;
using Forms = System.Windows.Forms;
namespace WpfApp1;

public partial class MainWindow : Window
{
    private bool isExpansionEnabled =
        true;

    public bool IsDarkModeEnabled =
        false;

    private readonly SQLiteService _db =
        new();

    private readonly GlobalHookService _hook =
    new();

    private readonly StringBuilder _buffer =
    new();
    private readonly GlobalHookService _hookService =
    new();
    public MainWindow()
    {
        InitializeComponent();

        _hookService.Start();
        _hookService.OnToggleChanged +=
    HookService_OnToggleChanged;

        ReloadSnippets();

        LoadProfileName();
    }

    // =========================
    // SAVE
    // =========================

    private void SaveButton_Click(
        object sender,
        RoutedEventArgs e)
    {
        var keyword =
            KeywordTextBox.Text;

        var content =
            ContentTextBox.Text;

        if (string.IsNullOrWhiteSpace(keyword))
        {
            System.Windows.MessageBox.Show(
                "กรุณากรอก Keyword");

            return;
        }

        if (string.IsNullOrWhiteSpace(content))
        {
            System.Windows.MessageBox.Show(
                "กรุณากรอกข้อความ");

            return;
        }

        if (_db.KeywordExists(keyword))
        {
            _db.UpdateSnippet(
                keyword,
                content);

            System.Windows.MessageBox.Show(
                "อัปเดตสำเร็จ");
        }
        else
        {
            _db.AddSnippet(
                keyword,
                content);

            System.Windows.MessageBox.Show(
                "บันทึกสำเร็จ");
        }

        ReloadSnippets();
    }

    // =========================
    // RELOAD
    // =========================

    public void ReloadSnippets()
    {
        var snippets =
            _db.GetAllSnippets();

        SnippetListBox.ItemsSource =
            null;

        SnippetListBox.ItemsSource =
            snippets;
    }

    // =========================
    // SELECT
    // =========================

    private void SnippetListBox_SelectionChanged(
        object sender,
        SelectionChangedEventArgs e)
    {
        if (SnippetListBox.SelectedItem
            is not Models.Snippet snippet)
        {
            return;
        }

        KeywordTextBox.Text =
            snippet.Keyword;

        ContentTextBox.Text =
            snippet.Content;
    }

    // =========================
    // DELETE
    // =========================

    private void DeleteButton_Click(
        object sender,
        RoutedEventArgs e)
    {
        var keyword =
            KeywordTextBox.Text;

        if (string.IsNullOrWhiteSpace(keyword))
        {
            System.Windows.MessageBox.Show(
                "กรุณาเลือก Keyword");

            return;
        }

        var result =
            System.Windows.MessageBox.Show(
                $"ลบ '{keyword}' ?",
                "ยืนยัน",
                System.Windows.MessageBoxButton.YesNo);

        if (result != System.Windows.MessageBoxResult.Yes)
        {
            return;
        }

        _db.DeleteSnippet(
            keyword);

        KeywordTextBox.Clear();

        ContentTextBox.Clear();

        ReloadSnippets();

        System.Windows.MessageBox.Show(
            "ลบสำเร็จ");
    }

    // =========================
    // SEARCH
    // =========================

    private void SearchTextBox_TextChanged(
        object sender,
        TextChangedEventArgs e)
    {
        if (SnippetListBox == null)
        {
            return;
        }

        var keyword =
            SearchTextBox.Text;

        var snippets =
            _db.SearchSnippets(keyword);

        SnippetListBox.ItemsSource =
            snippets;
    }

    // =========================
    // SETTINGS
    // =========================

    private void SettingsButton_Click(
        object sender,
        RoutedEventArgs e)
    {
        var window =
            new SettingsWindow(this);

        window.Owner =
            this;

        window.ShowDialog();
    }

    // =========================
    // PROFILE
    // =========================

    private void LoadProfileName()
    {
        var dbName =
            Path.GetFileName(
                "snippets.db");

        ProfileNameText.Text =
            dbName;
    }

    // =========================
    // TOGGLE
    // =========================

    private void SystemToggle_Checked(
        object sender,
        RoutedEventArgs e)
    {
        _hookService.IsEnabled =
         true;
    }

    private void SystemToggle_Unchecked(
        object sender,
        RoutedEventArgs e)
    {
        _hookService.IsEnabled =
      false;
    }
    private void Window_PreviewKeyDown(
     object sender,
     System.Windows.Input.KeyEventArgs e)
    {
        // .
        if (e.Key == Key.OemPeriod ||
            e.Key == Key.Decimal)
        {
            _buffer.Append(".");
        }

        // 0-9
        else if (e.Key >= Key.D0 &&
                 e.Key <= Key.D9)
        {
            string number =
                (e.Key - Key.D0).ToString();

            _buffer.Append(number);
        }

        // BACKSPACE
        else if (e.Key == Key.Back)
        {
            if (_buffer.Length > 0)
            {
                _buffer.Remove(
                    _buffer.Length - 1,
                    1);
            }
        }

        // SPACE
        else if (e.Key == Key.Space)
        {
            string text =
                _buffer.ToString();

            if (text == ".7")
            {
                // ลบ .7 + space
                for (int i = 0; i < 3; i++)
                {
                    Forms.SendKeys.SendWait(
                        "{BACKSPACE}");
                }

                Forms.SendKeys.SendWait(
                    "asda");
            }

            _buffer.Clear();
        }
    }
    private void HookService_OnToggleChanged(
    bool enabled)
    {
        Dispatcher.Invoke(() =>
        {
            SystemToggle.IsChecked =
                enabled;
        });
    }
}