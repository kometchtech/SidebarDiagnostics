﻿using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using SidebarDiagnostics.Windows;
using SidebarDiagnostics.Helpers;

namespace SidebarDiagnostics
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();

            DockEdgeComboBox.Items.Add(DockEdge.Left);
            DockEdgeComboBox.Items.Add(DockEdge.Right);
            DockEdgeComboBox.SelectedValue = Properties.Settings.Default.DockEdge;

            int _screenCount = Monitors.GetNoOfMonitors();

            for (int i = 0; i < _screenCount; i++)
            {
                ScreenComboBox.Items.Add(new { Text = string.Format("#{0}", i + 1), Value = i });
            }

            ScreenComboBox.DisplayMemberPath = "Text";
            ScreenComboBox.SelectedValuePath = "Value";

            if (Properties.Settings.Default.ScreenIndex < _screenCount)
            {
                ScreenComboBox.SelectedValue = Properties.Settings.Default.ScreenIndex;
            }
            else
            {
                ScreenComboBox.SelectedValue = 0;
            }

            SidebarWidthSlider.Value = Properties.Settings.Default.SidebarWidth;

            BGColorTextBox.Text = Properties.Settings.Default.BGColor;

            BGOpacitySlider.Value = Properties.Settings.Default.BGOpacity;
            
            FontSizeComboBox.Items.Add(FontSetting.x10);
            FontSizeComboBox.Items.Add(FontSetting.x12);
            FontSizeComboBox.Items.Add(FontSetting.x14);
            FontSizeComboBox.Items.Add(FontSetting.x16);
            FontSizeComboBox.Items.Add(FontSetting.x18);

            FontSizeComboBox.DisplayMemberPath = FontSizeComboBox.SelectedValuePath = "FontSize";
            FontSizeComboBox.SelectedValue = Properties.Settings.Default.FontSize;

            FontColorTextBox.Text = Properties.Settings.Default.FontColor;

            PollingIntervalSlider.Value = Properties.Settings.Default.PollingInterval;

            Clock24HRCheckBox.IsChecked = Properties.Settings.Default.Clock24HR;

            UseAppBarCheckBox.IsChecked = Properties.Settings.Default.UseAppBar;

            ClickThroughCheckBox.IsChecked = Properties.Settings.Default.ClickThrough;

            AlwaysTopCheckBox.IsChecked = Properties.Settings.Default.AlwaysTop;

            UpdatesCheckBox.IsChecked = Properties.Settings.Default.CheckForUpdates;

            StartupCheckBox.IsChecked = Utilities.StartupTaskExists();
        }

        private void Save()
        {
            Properties.Settings.Default.DockEdge = (DockEdge)DockEdgeComboBox.SelectedValue;
            Properties.Settings.Default.ScreenIndex = (int)ScreenComboBox.SelectedValue;
            Properties.Settings.Default.SidebarWidth = (int)SidebarWidthSlider.Value;
            Properties.Settings.Default.BGColor = BGColorTextBox.Text;
            Properties.Settings.Default.BGOpacity = BGOpacitySlider.Value;

            FontSetting _fontSetting = (FontSetting)FontSizeComboBox.SelectedItem;
            Properties.Settings.Default.FontSize = _fontSetting.FontSize;
            Properties.Settings.Default.TitleFontSize = _fontSetting.TitleFontSize;
            Properties.Settings.Default.IconSize = _fontSetting.IconSize;

            Properties.Settings.Default.FontColor = FontColorTextBox.Text;
            Properties.Settings.Default.PollingInterval = (int)PollingIntervalSlider.Value;
            Properties.Settings.Default.Clock24HR = Clock24HRCheckBox.IsChecked == true;
            Properties.Settings.Default.UseAppBar = UseAppBarCheckBox.IsChecked == true;
            Properties.Settings.Default.ClickThrough = ClickThroughCheckBox.IsChecked == true;
            Properties.Settings.Default.AlwaysTop = AlwaysTopCheckBox.IsChecked == true;
            Properties.Settings.Default.CheckForUpdates = UpdatesCheckBox.IsChecked == true;
            Properties.Settings.Default.Save();

            if (StartupCheckBox.IsChecked == true)
            {
                Utilities.EnableStartupTask();
            }
            else
            {
                Utilities.DisableStartupTask();
            }

            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, (Action)(() =>
            {
                (Owner as AppBar).Reload();
            }));
        }

        private void ColorTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox _textbox = (TextBox)sender;

            if (!new Regex("^#[a-fA-F0-9]{6}$").IsMatch(_textbox.Text))
            {
                _textbox.Text = "#000000";
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Save();
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
