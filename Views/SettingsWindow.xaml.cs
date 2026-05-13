using Microsoft.Win32;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using WpfApp1;
namespace WpfApp1.Views;

public partial class SettingsWindow : Window
{
    private readonly MainWindow _mainWindow;

    public SettingsWindow(
     MainWindow mainWindow)
    {
        InitializeComponent();

        _mainWindow = mainWindow;

        DarkModeToggle.IsChecked =
            _mainWindow.IsDarkModeEnabled;
    }
    private void ExportButton_Click(
        object sender,
        RoutedEventArgs e)
    {
        var dialog =
            new Microsoft.Win32.SaveFileDialog();

        dialog.FileName =
            "snippets.db";

        dialog.Filter =
            "Database File|*.db";

        if (dialog.ShowDialog() != true)
        {
            return;
        }

        File.Copy(
            "snippets.db",
            dialog.FileName,
            true);

        System.Windows.MessageBox.Show(
            "Export สำเร็จ");
    }
    private void ImportButton_Click(
     object sender,
     RoutedEventArgs e)
    {
        var dialog =
            new Microsoft.Win32.OpenFileDialog();

        dialog.Filter =
            "Database File|*.db";

        if (dialog.ShowDialog() != true)
        {
            return;
        }

        try
        {
            File.Copy(
                dialog.FileName,
                "snippets.db",
                true);

            _mainWindow.ReloadSnippets();

            System.Windows.MessageBox.Show(
                "Import สำเร็จ");
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show(
                ex.Message);
        }
    }

    private void HideAllPanels()
    {
        GeneralPanel.Visibility =
            Visibility.Collapsed;

        BackupPanel.Visibility =
            Visibility.Collapsed;

        SystemPanel.Visibility =
            Visibility.Collapsed;

        AboutPanel.Visibility =
            Visibility.Collapsed;
    }

    private void GeneralMenu_Click(
        object sender,
        MouseButtonEventArgs e)
    {
        HideAllPanels();

        GeneralPanel.Visibility =
            Visibility.Visible;
    }

    private void BackupMenu_Click(
        object sender,
        MouseButtonEventArgs e)
    {
        HideAllPanels();

        BackupPanel.Visibility =
            Visibility.Visible;
    }

    private void SystemMenu_Click(
        object sender,
        MouseButtonEventArgs e)
    {
        HideAllPanels();

        SystemPanel.Visibility =
            Visibility.Visible;
    }

    private void AboutMenu_Click(
        object sender,
        MouseButtonEventArgs e)
    {
        HideAllPanels();

        AboutPanel.Visibility =
            Visibility.Visible;
    }
    private void DelayTextBox_PreviewTextInput(
    object sender,
    TextCompositionEventArgs e)
    {
        Regex regex =
            new("[^0-9]+");

        e.Handled =
            regex.IsMatch(e.Text);
    }
    private void SaveDelayButton_Click(
    object sender,
    RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(
            DelayTextBox.Text))
        {
            System.Windows.MessageBox.Show(
                "กรุณากรอกค่า delay");

            return;
        }

        System.Windows.MessageBox.Show(
            $"บันทึก Delay = {DelayTextBox.Text} ms");
    }
    private void DarkModeToggle_Checked(
    object sender,
    RoutedEventArgs e)
    {
        _mainWindow.IsDarkModeEnabled = true;
        System.Windows.Application.Current.Resources["WindowBackground"] =
            new System.Windows.Media.SolidColorBrush(
                (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#1E1F22"));

        System.Windows.Application.Current.Resources["SidebarBackground"] =
            new SolidColorBrush(
                (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#18191C"));

        System.Windows.Application.Current.Resources["CardBackground"] =
            new SolidColorBrush(
                (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#2B2D31"));

        System.Windows.Application.Current.Resources["PrimaryText"] =
             System.Windows.Media.Brushes.White;

        System.Windows.Application.Current.Resources["SecondaryText"] =
            new SolidColorBrush(
                (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#B5BAC1"));

        System.Windows.Application.Current.Resources["BorderColor"] =
            new SolidColorBrush(
                (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#3A3D44"));

        System.Windows.Application.Current.Resources["MenuActiveBackground"] =
            new SolidColorBrush(
                (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#2B2D31"));
    }


    private void DarkModeToggle_Unchecked(
        object sender,
        RoutedEventArgs e)
    {
        _mainWindow.IsDarkModeEnabled = false;
        System.Windows.Application.Current.Resources["WindowBackground"] =
            new System.Windows.Media.SolidColorBrush(
                (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#F7F7F7"));

        System.Windows.Application.Current.Resources["SidebarBackground"] =
            System.Windows.Media.Brushes.White;

        System.Windows.Application.Current.Resources["CardBackground"] =
            System.Windows.Media.Brushes.White;

        System.Windows.Application.Current.Resources["PrimaryText"] =
            new SolidColorBrush(
                (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#111111"));

        System.Windows.Application.Current.Resources["SecondaryText"] =
            new SolidColorBrush(
                (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#777777"));

        System.Windows.Application.Current.Resources["BorderColor"] =
            new SolidColorBrush(
                (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#EAEAEA"));

        System.Windows.Application.Current.Resources["MenuActiveBackground"] =
            new SolidColorBrush(
                (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#EEF5FF"));
    }

    private void SystemToggle_Checked(
    object sender,
    RoutedEventArgs e)
    {
       
    }

    private void SystemToggle_Unchecked(
        object sender,
        RoutedEventArgs e)
    {
       
    }

}