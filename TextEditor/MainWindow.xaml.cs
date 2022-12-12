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
using TextEditor.Dialogs;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm;
using Wpf.Ui.Controls;
using TextEditor.Windows;

namespace TextEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Wpf.Ui.Controls.UiWindow
    {
        public struct SimpleDialogParams
        {
            public string Title;
            public string Content;

            public string ButtonPrimaryText;
            public string ButtonSecondaryText;

            public bool PrimaryButtonBlue;

        }
        private readonly IThemeService _themeService;

        //IDialogControl dialogControl, IDialogService dialogService, ISnackbarControl snackbarControl, ISnackbarService snackbarService

        public MainWindow()
        {
            ThemeService ts = new ThemeService();

            InitializeComponent();

            _themeService = ts;
            Wpf.Ui.Appearance.Accent.ApplySystemAccent();
            _themeService.SetTheme(Wpf.Ui.Appearance.ThemeType.Dark);

            TextBox.Name = "TextBoxDefault";
            RegisterName("TextBoxDefault", TextBox);

            this.Title = "TextEditor";
        }

        private void ThemeMainMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            _themeService.SetTheme(_themeService.GetTheme() == Wpf.Ui.Appearance.ThemeType.Dark ? Wpf.Ui.Appearance.ThemeType.Light : Wpf.Ui.Appearance.ThemeType.Dark);

            TextRange rangeOfText1 = new TextRange(GetCurrentlySelectedTabTextBox().Document.ContentEnd, GetCurrentlySelectedTabTextBox().Document.ContentEnd);
            rangeOfText1.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Black);

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
            PreferencesWindow window = new PreferencesWindow();
            window.ShowDialog();
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

        #region Tabs
        public TabItem GetCurrentlySelectedTab()
        {
            return (TabItem)ControlTabs.SelectedItem;
        }
        public int GetSelectedTabIndex()
        {
            return ControlTabs.SelectedIndex;
        }
        public RichTextBox GetCurrentlySelectedTabTextBox()
        {
            //MessageBox.Show(GetSelectedTabIndex().ToString());
            if (GetSelectedTabIndex() == 0)
            {
                return ((RichTextBox)DPanel.FindName("TextBoxDefault")); //this is the default notepad that is created in normal xaml
            }
            return ((RichTextBox)DPanel.FindName("TextBox" + GetSelectedTabIndex().ToString()));
        }
        private void CloseTabBtn_Click(object sender, RoutedEventArgs e)
        {
            //unregister the name before it the tab gets removed and count is decremented (hope this works)
            if (GetSelectedTabIndex() == 0)
            {
                //we must not close the only open tab, we can just reset the text in it
                //MenuNewBtn_Click(sender, e);
            }
            else
            {
                UnregisterName("TextBox" + GetSelectedTabIndex().ToString());
                ControlTabs.Items.Remove(GetCurrentlySelectedTab());
                Config.TabsCount--;
            }
        }

        private void AddTabBtn_Click(object sender, RoutedEventArgs e)
        {
            Config.TabsCount++;
            TabItem tab = new TabItem();
            tab.Header = "New Tab";

            ///TODO
            ///Implement a way to check if the tab its trying to add already exists
            ///
            ///something like tghis
            ///  will fix this for 0.4
            /*
            int newindex = 0;

            bool bFoundNewIndex = false;
            while (!bFoundNewIndex)
            { 
                //explanation

                //we create a new variable and increment it, then search if it exists
                int currentindex = 0;
                currentindex++;
                string name = "TextBox" + currentindex.ToString();
                var bExists = FindName(name);
                if ((bool)bExists)
                {
                    //if it does we just continiue
                    continue;
                }
                else if (!(bool)bExists)
                {
                    //if it doesnt, we are done and stop the loop
                    newindex = currentindex;
                    bFoundNewIndex = true;
                }
            }
            */

            int index = 0;
            while (index < Config.TabsCount)
            {
                index++;
            }

            RichTextBox rtextbox = new RichTextBox();
            //int math = GetSelectedTabIndex() + 1;
            rtextbox.Name = "TextBox" + index.ToString();
            tab.Content = rtextbox;
            //MessageBox.Show(rtextbox.Name);
            //try to register the name so it can be found using FindName
            RegisterName(rtextbox.Name, rtextbox);

            ControlTabs.Items.Insert(1, tab);
            //we should also select the newly added tab
            //ControlTabs.SelectedItem = tab;
            Dispatcher.BeginInvoke((Action)(() => ControlTabs.SelectedIndex = Config.TabsCount));
            //bFoundNewIndex = false; //for later
        }
        #endregion

        private void AboutMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow window = new AboutWindow();
            window.Show();
        }

        private void NewWindowMainMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Owner = this;
            window.Show();
        }
    }
}
