using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using VersionManagerUI.Pages;
using VersionManagerUI.Utils;

namespace VersionManagerUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private InstanceCache _cache = new InstanceCache();
        private List<StackPanel> _menuItems;
        private Brush _itemSelected = new SolidColorBrush(Color.FromRgb(0, 85, 155));
        private Brush _itemDeselected = new SolidColorBrush(Color.FromRgb(0, 55, 100));

        public MainWindow()
        {
            InitializeComponent();
            _menuItems = new List<StackPanel> { MenuOverview, MenuDownload, MenuImportGame, MenuReplays, MenuHelp};
            _cache = VMSettings.LoadSettings();
            ChangePage<Overview>();
            SelectMenuItem(MenuOverview);
        }

        private void MenuHelp_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ChangePage<Help>();
            SelectMenuItem(sender as StackPanel);
        }

        private void MenuOverview_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ChangePage<Overview>();
            SelectMenuItem(sender as StackPanel);
        }

        private void MenuDownload_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ChangePage<Download>();
            SelectMenuItem(sender as StackPanel);
        }

        private void MenuImportGame_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ChangePage<Import>();
            SelectMenuItem(sender as StackPanel);
        }

        private void MenuReplays_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ChangePage<Replays>();
            SelectMenuItem(sender as StackPanel);
        }

        private void ChangePage<T>()
        {
            frmMainContent.Navigate(_cache.Get<T>());
        }

        private void SelectMenuItem(StackPanel selected)
        {
            _menuItems.ForEach(panel => panel.Background = _itemDeselected);
            selected.Background = _itemSelected;
        }
    }
}
