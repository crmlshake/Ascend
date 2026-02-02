using Ascend.Helpers;
using Ascend.MVVM.View.SettingsViews;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Ascend.MVVM.View
{
    public partial class SettingsView : UserControl
    {

        private readonly string _userName;

        public SettingsView(string userName)
        {
            InitializeComponent();
            _userName = userName;
        }

        //Vorerst nicht in Betrieb!
        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            ThemeManager.SwitchToWhiteMode(true);
        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            ThemeManager.SwitchToWhiteMode(false);
        }

        private void LoadProfileSettingsView()
        {
            RightContentControl.Content = new ProfileSettingsView(_userName);
        }

        private void ProfileSettingsButton_Checked(object sender, RoutedEventArgs e)
        {
            DeselectOtherNavButtons((ToggleButton)sender);
            LoadProfileSettingsView();
        }

        private void LoadPrivacySettingsView()
        {
            RightContentControl.Content = new PrivacySettingsView();
        }

        private void PrivacySettingsButton_Checked(object sender, RoutedEventArgs e)
        {
            DeselectOtherNavButtons((ToggleButton)sender);
            LoadPrivacySettingsView();
        }

        private void LoadLanguageSettingsView()
        {
            RightContentControl.Content = new LanguageSettingsView();
        }

        private void LanguageSettingsButton_Checked(object sender, RoutedEventArgs e)
        {
            DeselectOtherNavButtons((ToggleButton)sender);
            LoadLanguageSettingsView();
        }

        private void LoadExtendedSettingsView()
        {
            RightContentControl.Content = new ExtendedSettingsView();
        }

        private void ExtendedSettingsButton_Checked(object sender, RoutedEventArgs e)
        {
            DeselectOtherNavButtons((ToggleButton)sender);
            LoadExtendedSettingsView();
        }

        private void DeselectOtherNavButtons(ToggleButton clickedButton)
        {
            foreach (var button in new[] {
                     ProfileSettingsToggleButton, PrivacySettingsToggleButton, LanguageSettingsToggleButton,
                     ExtendedSettingsToggleButton })
            {
                if (button != clickedButton)
                    button.IsChecked = false;
            }
        }

    }
}