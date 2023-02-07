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
using TextEditor.Windows;
using System.Reflection;
using System.Diagnostics.Contracts;
using Newtonsoft.Json;
using System.Windows.Media.Media3D;

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
            //_themeService.SetTheme(Wpf.Ui.Appearance.ThemeType.Dark);

            DefTextBox.Name = "TextBoxDefault";
            RegisterName("TextBoxDefault", DefTextBox);

            this.Title = "TextEditor";
            //ControlTabs.Items.Remove(DefaultTab);
            DefaultTab.Visibility = Visibility.Collapsed;
            DefTextBox.Visibility= Visibility.Collapsed;
        }

        private void ThemeMainMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            _themeService.SetTheme(_themeService.GetTheme() == Wpf.Ui.Appearance.ThemeType.Dark ? Wpf.Ui.Appearance.ThemeType.Light : Wpf.Ui.Appearance.ThemeType.Dark);

            if (GetCurrentlySelectedTabTextBox() == null)
            {
                return;
            }
            else
            {
                /*
                TextRange rangeOfText1 = new TextRange(GetCurrentlySelectedTabTextBox().Document.ContentEnd, GetCurrentlySelectedTabTextBox().Document.ContentEnd);
                rangeOfText1.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Black);
                */
                if (_themeService.GetTheme() == Wpf.Ui.Appearance.ThemeType.Light)
                {
                    GetCurrentlySelectedTabTextBox().Foreground = Brushes.Black;
                }
                else if (_themeService.GetTheme() == Wpf.Ui.Appearance.ThemeType.Dark)
                {
                    GetCurrentlySelectedTabTextBox().Foreground = Brushes.White;
                }
            }

        }

        private void NewMainMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            GetCurrentlySelectedTabTextBox().Document.Blocks.Clear();
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
        public TabItem GetCurrentlySelectedTabOLD()
        {
            return (TabItem)ControlTabs.SelectedItem;
        }
        public int GetSelectedTabIndexOLD()
        {
            return ControlTabs.SelectedIndex;
        }
        public RichTextBox GetCurrentlySelectedTabTextBoxOLD()
        {
            //MessageBox.Show(GetSelectedTabIndex().ToString());
            if (GetSelectedTabIndexOLD() == 0)
            {
                return ((RichTextBox)DPanel.FindName("TextBoxDefault")); //this is the default notepad that is created in normal xaml
            }
            return ((RichTextBox)DPanel.FindName("TextBox" + GetSelectedTabIndexOLD().ToString()));
        }
        private void CloseTabBtn_Click(object sender, RoutedEventArgs e)
        {
            RemoveTab();
        }

        public void RemoveTabOld()
        {
            //unregister the name before it the tab gets removed and count is decremented (hope this works)
            if (GetSelectedTabIndexOLD() == 0)
            {
                //we must not close the only open tab, we can just reset the text in it
                //MenuNewBtn_Click(sender, e);
            }
            else
            {
                UnregisterName("TextBox" + GetSelectedTabIndexOLD().ToString());
                ControlTabs.Items.Remove(GetCurrentlySelectedTab());
                Config.TabsCount--;
            }
        }
        public void AddNewTabOLD()
        {
            Dispatcher.BeginInvoke((Action)(() => ControlTabs.SelectedIndex = Config.TabsCount));
            Config.TabsCount++;
            TabItem tab = new TabItem();
            tab.Header = "New Tab";

            ///TODO
            ///Implement a way to check if the tab its trying to add already exists
            ///
            ///something like tghis
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

        private void AddTabBtn_Click(object sender, RoutedEventArgs e)
        {
            AddNewTab();
        }
        #endregion

        #region NewTabs
        //Some code here is recycled from the old Tabs
        public TabItem GetCurrentlySelectedTab()
        {
            return (TabItem)ControlTabs.SelectedItem;
        }
        public int GetSelectedTabIndex()
        {
            return ControlTabs.SelectedIndex;
        }
        public string GetCurrentlySelectedTabName()
        {
            return GetCurrentlySelectedTab().Name;
        }
        public string GetCurrentlySelectedTabGuid()
        {
            //in short, this just removes the "TAB" at the start of the name, and returns the guid
            string CurrentTabName = GetCurrentlySelectedTabName();
            return CurrentTabName.Remove(0,3);
        }
        public RichTextBox GetCurrentlySelectedTabTextBox()
        {
            //MessageBox.Show(GetCurrentlySelectedTextBoxName());
            return (RichTextBox)DPanel.FindName("TextBox" + GetCurrentlySelectedTabGuid());
        }
        public void AddNewTab()
        {
            Dispatcher.BeginInvoke((Action)(() => ControlTabs.SelectedIndex = Config.TabsCount));
            Config.TabsCount++;
            TabItem tab = new TabItem();
            tab.Header = "New Tab";

            var guid = Guid.NewGuid().ToString();
            string name = guid.Replace("-", "_");
            tab.Name ="TAB"+ name;
            RichTextBox rtextbox = new RichTextBox();
            rtextbox.Name = "TextBox" + name;

            rtextbox.Foreground = Brushes.White;
            rtextbox.FontWeight = FontWeights.Regular;
            rtextbox.CaretBrush= Brushes.White;
             
            //rtextbox.Background = Brushes.DimGray;
            //MessageBox.Show(guid);

            RegisterName(rtextbox.Name, rtextbox);
            tab.Content = rtextbox;
            ControlTabs.Items.Insert(1, tab);
            Dispatcher.BeginInvoke((Action)(() => ControlTabs.SelectedIndex = Config.TabsCount));
        }
        public void RemoveTab()
        {
            Config.TabsCount--;
            UnregisterName("TextBox" + GetCurrentlySelectedTabGuid());
            ControlTabs.Items.Remove(GetCurrentlySelectedTab());
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

        private void UndoMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            GetCurrentlySelectedTabTextBox().Undo();
        }

        private void RedoMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            GetCurrentlySelectedTabTextBox().Redo();
        }

        private void CutMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            GetCurrentlySelectedTabTextBox().Cut();
        }

        private void CopyMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            GetCurrentlySelectedTabTextBox().Copy();
        }

        private void PasteMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            GetCurrentlySelectedTabTextBox().Paste();
        }

        private void SelectAllMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            GetCurrentlySelectedTabTextBox().SelectAll();
        }

        private void DateTimeMenuBTn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FontMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FontDialog dlg = new System.Windows.Forms.FontDialog();
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                GetCurrentlySelectedTabTextBox().FontFamily = new FontFamily(dlg.Font.Name);
                GetCurrentlySelectedTabTextBox().FontSize = dlg.Font.Size * 98.0 / 72.0;
                GetCurrentlySelectedTabTextBox().FontWeight = dlg.Font.Bold ? FontWeights.Bold : FontWeights.Regular;
                GetCurrentlySelectedTabTextBox().FontStyle = dlg.Font.Italic ? FontStyles.Italic : FontStyles.Normal;
            }
        }

        private void ColorMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.ColorDialog dlg = new System.Windows.Forms.ColorDialog();
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                GetCurrentlySelectedTabTextBox().Foreground = new SolidColorBrush(Color.FromArgb(dlg.Color.A, dlg.Color.R, dlg.Color.G, dlg.Color.B));
            }
        }
    }
}
