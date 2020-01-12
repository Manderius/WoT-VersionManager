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
using VersionSwitcher_Server;

namespace VSUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ChangePage("Overview");
        }

        private void MenuSettings_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ChangePage("Settings");
        }

        private void MenuOverview_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ChangePage("Overview");
        }

        private void MenuDownload_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ChangePage("Download");
        }

        private void MenuImportGame_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ChangePage("Import");
        }

        private void ChangePage(string name)
        {
            frmMainContent.Source = new Uri("Pages/" + name + ".xaml", UriKind.Relative);
        }
    }
}
