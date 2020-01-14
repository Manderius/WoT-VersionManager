using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VSUI.Pages;
using VSUI.Services;
using VSUI.Utils;

namespace VSUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Hashtable _services = new Hashtable();
        private Hashtable _pages = new Hashtable();

        public MainWindow()
        {
            InitializeComponent();
            ChangePage(_pages.GetOrInsert(typeof(Overview), new Overview(_services.GetOrInsert("LocalVersionService", new LocalVersionsService()))));
        }

        private void MenuSettings_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ChangePage("Settings");
        }

        private void MenuOverview_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ChangePage(_pages.GetOrInsert(typeof(Overview), new Overview(_services.GetOrInsert("LocalVersionService", new LocalVersionsService()))));
        }

        private void MenuDownload_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ChangePage("Download");
        }

        private void MenuImportGame_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ChangePage(_pages.GetOrInsert(typeof(Import), new Import(_services.GetOrInsert("LocalVersionService", new LocalVersionsService()))));
        }

        private void MenuReplays_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ChangePage(_pages.GetOrInsert(typeof(Replays), new Replays()));
        }

        private void ChangePage(string name)
        {
            frmMainContent.Source = new Uri("Pages/" + name + ".xaml", UriKind.Relative);
        }

        private void ChangePage(Page page)
        {
            frmMainContent.Navigate(page);
        }


    }
}
