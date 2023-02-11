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
using TextEditor.Pages;
using System.Reflection;
using System.Diagnostics.Contracts;
using Newtonsoft.Json;
using System.Windows.Media.Media3D;
using System.IO;
using TextEditor.Enums;
using System.Windows.Threading;
using TextEditor.Windows;
using TextEditor.Functions;

namespace TextEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class BetterMainWindow : Wpf.Ui.Controls.UiWindow
    {
        public struct SimpleDialogParams
        {
            public string Title;
            public string Content;

            public string ButtonPrimaryText;
            public string ButtonSecondaryText;

            public bool PrimaryButtonBlue;

        }

        public void ApplySettings()
        {
            Settings.GetSettings();
            if (Settings.SettingsValues.Theme == "DARK")
            {
                //default is dark, no need to change anything
            }
            if (Settings.SettingsValues.Theme == "LIGHT")
            {
                ThemeMainMenuBtn_Click(null, null);
            }

            if (Settings.SettingsValues.ToolbarVisibility == true)
            {
                QuickBarTray.Visibility = Visibility.Visible;
            }
            else if (Settings.SettingsValues.ToolbarVisibility == false)
            {
                QuickBarTray.Visibility = Visibility.Collapsed;
            }
        }

        public BetterMainWindow()
        {
            Globals.TS = new ThemeService();

            InitializeComponent();
            Globals.TS.SetSystemAccent();

            DefTextBox.Name = "TextBoxDefault";
            RegisterName("TextBoxDefault", DefTextBox);

            this.Title = "TextEditor";
            DefaultTab.Visibility = Visibility.Collapsed;
            DefTextBox.Visibility = Visibility.Collapsed;

            //start the timer
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_tick;
            timer.Start();

            if (!Version.versionType.Contains("Debug"))
            {
                LoggerSeperatorONLYONDEBUG.Visibility = Visibility.Collapsed;
                LoggerMenuBtn.Visibility = Visibility.Collapsed;
            }
            AddTabBtn_Click(null, null);
            ControlTabs.Items.Remove(DefaultTab);

            ApplySettings();
            UpdateStatus();
        }

        //i might start a new timer for this, so it updates faster
        void UpdateStatus()
        {
            if (Config.TabsCount != 0)
            {
                if (GetCurrentlySelectedTabTextBox() == null)
                {
                    LinesAndCharsStatusBarBlock.Text = string.Empty;
                    return;
                }

                TextRange MyText = new TextRange(
                GetCurrentlySelectedTabTextBox().Document.ContentStart,
                GetCurrentlySelectedTabTextBox().Document.ContentEnd
                );

                string[] splittedLines = MyText.Text.Split(new[] { Environment.NewLine }
                                              , StringSplitOptions.None); // or StringSplitOptions.RemoveEmptyEntries
                int Alllines = splittedLines.Length;

                TextPointer caretLineStart = GetCurrentlySelectedTabTextBox().CaretPosition.GetLineStartPosition(0);
                TextPointer p = GetCurrentlySelectedTabTextBox().Document.ContentStart.GetLineStartPosition(0);
                int caretLineNumber = 1;

                while (true)
                {
                    if (caretLineStart.CompareTo(p) < 0)
                    {
                        break;
                    }

                    int result;
                    p = p.GetLineStartPosition(1, out result);

                    if (result == 0)
                    {
                        break;
                    }

                    caretLineNumber++;
                }

                string CurrentCharacter = GetCurrentlySelectedTabTextBox().Document.ContentStart.GetOffsetToPosition(GetCurrentlySelectedTabTextBox().CaretPosition).ToString();

                string text = "Line: " + caretLineNumber.ToString() +" Character: "+ CurrentCharacter +" |"+ " Total Lines: " + Alllines.ToString() + " | All Characters: 0";

                LinesAndCharsStatusBarBlock.Text = text;
            }
            else if (Config.TabsCount == 0)
            {
                LinesAndCharsStatusBarBlock.Text = string.Empty;
            }
        }

        void timer_tick(object sender, EventArgs e)
        {
            TimeBtn.Content = DateTime.Now.ToString("HH:mm:ss");
            Globals.RunningTimeInSeconds++;
            Globals.CurrentDateTime = DateTime.Now;

            //PosTextBox.Text = GetCurrentlySelectedTabTextBox().Document.ContentStart.GetOffsetToPosition(GetCurrentlySelectedTabTextBox().CaretPosition).ToString(); not sure what i did here

            if (Config.bLog)
            {
                if (Globals.TimeSinceLastTabsLogDump >= 5)
                {
                    Logger.Log("DUMPING TABS STATE:");
                    Logger.Log("    Currently opened tabs: " + Config.TabsCount);
                    if (GetCurrentlySelectedTab() == null)
                    {
                        Logger.Log("    No opened Tabs! ");
                    }
                    else
                    {
                        Logger.Log("    Currently selected tab header: " + GetCurrentlySelectedTab().Header);
                        Logger.Log("    Currently selected tab Guid: " + GetCurrentlySelectedTabGuid());
                    }
                    Logger.Log("DUMPED TABS STATE!");
                    Globals.TimeSinceLastTabsLogDump = 0;
                }
                else { Globals.TimeSinceLastTabsLogDump++; }
            }
        }

        private void ThemeMainMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            Globals.TS.SetTheme(Globals.TS.GetTheme() == Wpf.Ui.Appearance.ThemeType.Dark ? Wpf.Ui.Appearance.ThemeType.Light : Wpf.Ui.Appearance.ThemeType.Dark);
            Settings.SwitchThemeSetting();

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
                if (Globals.TS.GetTheme() == Wpf.Ui.Appearance.ThemeType.Light)
                {
                    GetCurrentlySelectedTabTextBox().Foreground = Brushes.Black;
                }
                else if (Globals.TS.GetTheme() == Wpf.Ui.Appearance.ThemeType.Dark)
                {
                    GetCurrentlySelectedTabTextBox().Foreground = Brushes.White;
                }
            }

        }

        private void NewMainMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            if (GetCurrentlySelectedTabTextBox() == null)
            {
                AddNewTab(); //first add new tab if none of them exist
                return;
            }
            else
                GetCurrentlySelectedTabTextBox().Document.Blocks.Clear();
        }

        private void OpenMainMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.ShowDialog();
            string docToOpen = openFileDialog.FileName;
            string tabcontent = openFileDialog.SafeFileName;
            if (docToOpen == "")
            {
                //no doc to open, do nothing
            }
            else if (docToOpen != "")
            {
                if (GetCurrentlySelectedTabTextBox() == null)
                {
                    AddNewTab(); //first add new tab if none of them exist
                    return;
                }
                GetCurrentlySelectedTabTextBox().Document.Blocks.Clear();
                GetCurrentlySelectedTabTextBox().Document.Blocks.Add(new Paragraph(new System.Windows.Documents.Run(File.ReadAllText(docToOpen))));
                //AdjustAppTitleByDocumentName(docToOpen);
                GetCurrentlySelectedTab().Header = tabcontent;
            }
        }

        private void SaveMainMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "Text File (*.txt)|*.txt|Show All Files (*.*)|*.*";
            saveFileDialog.FileName = "Untitled";
            saveFileDialog.Title = "Save As";
            string range = new TextRange(GetCurrentlySelectedTabTextBox().Document.ContentStart, GetCurrentlySelectedTabTextBox().Document.ContentEnd).Text;
            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, range);
                GetCurrentlySelectedTab().Header = saveFileDialog.SafeFileName;
            }
        }

        private void PreferencesMainMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            SettingsPage page = new SettingsPage();
            AddNewTabWithPage(page, PageEnum.Settings);
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

        #region OLDTabs
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
            return CurrentTabName.Remove(0, 3);
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
            tab.Name = "TAB" + name;
            RichTextBox rtextbox = new RichTextBox();
            rtextbox.Name = "TextBox" + name;

            rtextbox.Foreground = Brushes.White;
            rtextbox.FontWeight = FontWeights.Regular;
            rtextbox.CaretBrush = Brushes.White;
            rtextbox.Background = Brushes.Transparent;

            rtextbox.TextChanged += RTextBox_TextChanged;
            //rtextbox.Background = Brushes.DimGray;
            //MessageBox.Show(guid);
            //tab.Background = Brushes.Transparent;

            RegisterName(rtextbox.Name, rtextbox);
            tab.Content = rtextbox;
            ControlTabs.Items.Insert(1, tab);
            //Dispatcher.BeginInvoke((Action)(() => ControlTabs.SelectedIndex = Config.TabsCount));
            ControlTabs.SelectedItem= tab;
        }
        public void AddNewTabWithPage(Wpf.Ui.Controls.UiPage Page, PageEnum SelectedPage)
        {
            Dispatcher.BeginInvoke((Action)(() => ControlTabs.SelectedIndex = Config.TabsCount));
            Config.TabsCount++;
            TabItem tab = new TabItem();
            //set the tab header based on the page enum
            switch (SelectedPage)
            {
                case PageEnum.About:
                    tab.Name = "AboutTab";
                    tab.Header = "About TextEditor";
                    break;
                case PageEnum.Settings:
                    tab.Name = "SettingsTab";
                    tab.Header = "Settings";
                    break;
            }

            Frame tabFrame = new Frame();
            tabFrame.Content = Page;

            tab.Content = tabFrame;

            ControlTabs.Items.Insert(1, tab);
            Dispatcher.BeginInvoke((Action)(() => ControlTabs.SelectedIndex = Config.TabsCount));
        }
        public void RemoveTab()
        {
            if (Config.TabsCount == 1)
            {
                //this is the last tab, we should not close it to prevent crash
                NewMainMenuBtn_Click(null, null);
            }
            else
            {
                //Check if this is the about tab i might do
                if (GetCurrentlySelectedTab().Name == "AboutTab")
                {
                    ControlTabs.Items.Remove(GetCurrentlySelectedTab());
                }
                else if (GetCurrentlySelectedTab().Name == "SettingsTab")
                {
                    ControlTabs.Items.Remove(GetCurrentlySelectedTab());
                }
                else
                {
                    Config.TabsCount--;
                    UnregisterName("TextBox" + GetCurrentlySelectedTabGuid());
                    ControlTabs.Items.Remove(GetCurrentlySelectedTab());
                }
            }
        }
        #endregion

        private void AboutMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            AboutPage page = new AboutPage();
            AddNewTabWithPage(page, PageEnum.About);
        }

        private void NewWindowMainMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Owner = null;
            window.Show();
        }

        private void UndoMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            if (GetCurrentlySelectedTabTextBox() == null)
            {
                return;
            }
            else
            {
                GetCurrentlySelectedTabTextBox().Undo();
            }
        }

        private void RedoMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            if (GetCurrentlySelectedTabTextBox() == null)
            {
                return;
            }
            else 
               GetCurrentlySelectedTabTextBox().Redo();
        }

        private void CutMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            if (GetCurrentlySelectedTabTextBox() == null)
            {
                return;
            }
            else
            {
                GetCurrentlySelectedTabTextBox().Cut();
            }
        }

        private void CopyMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            if (GetCurrentlySelectedTabTextBox() == null)
            {
                return;
            }
            else
            {
                GetCurrentlySelectedTabTextBox().Copy();
            }
        }

        private void PasteMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            if (GetCurrentlySelectedTabTextBox() == null)
            {
                return;
            }
            else
            {
                GetCurrentlySelectedTabTextBox().Paste();
            }
        }

        private void SelectAllMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            if (GetCurrentlySelectedTabTextBox() == null)
            {
                return;
            }
            else { GetCurrentlySelectedTabTextBox().SelectAll(); }
        }

        private void DateTimeMenuBTn_Click(object sender, RoutedEventArgs e)
        {
            string CurrentText = new TextRange(GetCurrentlySelectedTabTextBox().Document.ContentStart, GetCurrentlySelectedTabTextBox().Document.ContentEnd).Text;
            string NewText = CurrentText + DateTime.Now.ToString();

            GetCurrentlySelectedTabTextBox().Document.Blocks.Clear();
            GetCurrentlySelectedTabTextBox().Document.Blocks.Add(new Paragraph(new Run(NewText)));
        }

        private void FontMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            if (GetCurrentlySelectedTabTextBox() == null)
            {
                return;
            }
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
            if (GetCurrentlySelectedTabTextBox() == null)
            {
                return;
            }
            System.Windows.Forms.ColorDialog dlg = new System.Windows.Forms.ColorDialog();
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                GetCurrentlySelectedTabTextBox().Foreground = new SolidColorBrush(Color.FromArgb(dlg.Color.A, dlg.Color.R, dlg.Color.G, dlg.Color.B));
            }
        }

        private void WordWrapCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (GetCurrentlySelectedTabTextBox() == null)
            {
                return;
            }
            else
            {
                //todo
            }
        }

        private void WordWrapCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (GetCurrentlySelectedTabTextBox() == null)
            {
                return;
            }
            else
            {
                //todo
            }
        }

        private void BoldMenuButton_Click(object sender, RoutedEventArgs e)
        {
            if (GetCurrentlySelectedTabTextBox() == null)
            {
                return;
            }

            GetCurrentlySelectedTabTextBox().Selection.ApplyPropertyValue(FontWeightProperty, FontWeights.Bold);
        }

        private void ItalicMenuButton_Click(object sender, RoutedEventArgs e)
        {
            if (GetCurrentlySelectedTabTextBox() == null)
            {
                return;
            }

            GetCurrentlySelectedTabTextBox().Selection.ApplyPropertyValue(FontStyleProperty, FontStyles.Italic);
        }

        private void UnderlineMenuButton_Click(object sender, RoutedEventArgs e)
        {
            if (GetCurrentlySelectedTabTextBox() == null)
            {
                return;
            }

            GetCurrentlySelectedTabTextBox().Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
        }

        private void UpdaterMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            UpdateWindow window = new UpdateWindow();
            window.ShowDialog();
        }

        private void LoggerMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            LoggerWindow window = new LoggerWindow();
            window.Show();
        }

        private void RTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateStatus();
        }

        private void ControlTabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GetCurrentlySelectedTab() == null)
            {
                return;
            }
            UpdateStatus();
            if (GetCurrentlySelectedTab() != null)
            {
                if (!GetCurrentlySelectedTab().Header.ToString().Contains("Default"))
                {
                    string Title = GetCurrentlySelectedTab().Header + "-" + Globals.AppTitle;
                    this.Title = Title;
                    MWindowTitleBar.Title = Title;
                }
            }
            else
            {
                string Title = Globals.AppTitle;
                this.Title = Title;
                MWindowTitleBar.Title = Title;
            }
        }
    }
}
