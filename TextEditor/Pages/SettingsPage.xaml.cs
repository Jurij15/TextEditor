using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using TextEditor.Functions;

namespace TextEditor.Pages
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Wpf.Ui.Controls.UiPage
    {
        bool bIsInitialized= false;
        void InitSettingsValues()
        {
            if (Globals.TS.GetTheme() == Wpf.Ui.Appearance.ThemeType.Light)
            {
                ThemeCombo.SelectedItem = LightThemeItem;
            }
            else
            {
                ThemeCombo.SelectedItem = DarkThemeItem;
            }
            if (Settings.SettingsValues.CornerPreference == Wpf.Ui.Appearance.WindowCornerPreference.Round)
            {
                CornerPreferenceCombo.SelectedItem = RoundCornerPreference;
            }
            else if (Settings.SettingsValues.CornerPreference == Wpf.Ui.Appearance.WindowCornerPreference.RoundSmall)
            {
                CornerPreferenceCombo.SelectedItem = RoundSmallCornerPreference;
            }
            else if (Settings.SettingsValues.CornerPreference == Wpf.Ui.Appearance.WindowCornerPreference.DoNotRound)
            {
                CornerPreferenceCombo.SelectedItem = DoNotRoundCornerPreference;
            }

            StatusBarVisibility.IsChecked = Settings.SettingsValues.StatusBarVisibility;
            QuickbarVisiblity.IsChecked = Settings.SettingsValues.ToolbarVisibility;

            bIsInitialized= true; //this prevents changing app theme while its loading
        }
        public SettingsPage()
        {
            InitializeComponent();
            InitSettingsValues();
        }

        private void LightThemeItem_Selected(object sender, RoutedEventArgs e)
        {
            if (!bIsInitialized)
            {
                return;
            }
            Globals.ChangeTheme();
        }

        private void DarkThemeItem_Selected(object sender, RoutedEventArgs e)
        {
            if (!bIsInitialized)
            {
                return;
            }
            Globals.ChangeTheme();
        }

        private void RoundCornerPreference_Selected(object sender, RoutedEventArgs e)
        {
            if (!bIsInitialized)
            {
                return;
            }
            Settings.ChangeWindowCornerPreferenceSetting(Wpf.Ui.Appearance.WindowCornerPreference.Round);
            Globals.MainWindow.WindowCornerPreference = Wpf.Ui.Appearance.WindowCornerPreference.Round;
        }

        private void RoundSmallCornerPreference_Selected(object sender, RoutedEventArgs e)
        {
            if (!bIsInitialized)
            {
                return;
            }
            Settings.ChangeWindowCornerPreferenceSetting(Wpf.Ui.Appearance.WindowCornerPreference.RoundSmall);
            Globals.MainWindow.WindowCornerPreference = Wpf.Ui.Appearance.WindowCornerPreference.RoundSmall;
        }

        private void DoNotRoundCornerPreference_Selected(object sender, RoutedEventArgs e)
        {
            if (!bIsInitialized)
            {
                return;
            }
            Settings.ChangeWindowCornerPreferenceSetting(Wpf.Ui.Appearance.WindowCornerPreference.DoNotRound);
            Globals.MainWindow.WindowCornerPreference = Wpf.Ui.Appearance.WindowCornerPreference.DoNotRound;
        }

        private void SettingsRefrestBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!bIsInitialized)
            {
                return;
            }
            bIsInitialized = false;
            InitSettingsValues();
        }

        private void StatusBarVisibility_Checked(object sender, RoutedEventArgs e)
        {
            if (!bIsInitialized)
            {
                return;
            }
            Settings.ChangeStatusBarVisuiblitySetting(true);
            //AppRestartBar.ShowAsync();
        }

        private void StatusBarVisibility_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!bIsInitialized)
            {
                return;
            }
            Settings.ChangeStatusBarVisuiblitySetting(false);
            //AppRestartBar.ShowAsync();
        }

        private void QuickbarVisiblity_Checked(object sender, RoutedEventArgs e)
        {
            if (!bIsInitialized)
            {
                return;
            }
            Settings.ChangeToolBarVisuiblitySetting(true);
            //AppRestartBar.ShowAsync();
        }

        private void QuickbarVisiblity_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!bIsInitialized)
            {
                return;
            }
            Settings.ChangeToolBarVisuiblitySetting(false);
            //AppRestartBar.ShowAsync();
        }

        private void ResetSettingsBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
