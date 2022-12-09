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
using Wpf.Ui.Mvvm.Contracts;
using Wpf.Ui.Mvvm.Services;
//using TextEditor.Dialogs;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm;
using Wpf.Ui.Controls;

namespace TextEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Wpf.Ui.Controls.UiWindow
    {
        private readonly IThemeService _themeService;
        private readonly IDialogControl _dialogControl;
        private readonly ISnackbarService _snackbarService;

        //IDialogControl dialogControl, IDialogService dialogService, ISnackbarControl snackbarControl, ISnackbarService snackbarService

        public MainWindow()
        {
            DialogService dlgservice = new DialogService();
            SnackbarService snackbarservice = new SnackbarService();
            ThemeService ts = new ThemeService();

            InitializeComponent();

            _themeService = ts;
            //_dialogControl = dlgservice.GetDialogControl();
            //_snackbarService = (ISnackbarService?)snackbarservice.GetSnackbarControl();
            Wpf.Ui.Appearance.Accent.ApplySystemAccent();
            _themeService.SetTheme(Wpf.Ui.Appearance.ThemeType.Dark);
        }

        private void ThemeMainMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            _themeService.SetTheme(_themeService.GetTheme() == Wpf.Ui.Appearance.ThemeType.Dark ? Wpf.Ui.Appearance.ThemeType.Light : Wpf.Ui.Appearance.ThemeType.Dark);
        }

        private void NewMainMenuBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OpenMainMenuBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SaveMainMenuBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FileTemplatesMainMenuBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PreferencesMainMenuBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void InstanceManagerMainMenuBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExitMainMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void StatisticsMainMenuBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
