using System;
using System.Windows;
using System.Windows.Controls;
using VersionManagerUI.Tutorial.Pages;

namespace VersionManagerUI.Tutorial
{
    /// <summary>
    /// Interaction logic for TutorialWindow.xaml
    /// </summary>
    public partial class TutorialWindow : Window, IPagination
    {
        private Page[] _pages { get; set; }
        private int _pageIndex = 0;
        private bool _tutorialCompleted { get; set; }

        public TutorialWindow()
        {
            InitializeComponent();
            _tutorialCompleted = false;
            _pages = new Page[] { new Welcome(this), new HowItWorks(this), new Location(this), new Completed(this) };
            DisplayPage(_pages[_pageIndex]);
            Closing += Window_Closing;
        }

        private void DisplayPage(Page page)
        {
            frmContents.Navigate(page);
        }

        public void NextPage()
        {
            if (_pageIndex < _pages.Length - 1)
                DisplayPage(_pages[++_pageIndex]);
            else
            {
                _tutorialCompleted = true;
                Close();
            }
        }

        public void PreviousPage()
        {
            if (_pageIndex > 0)
                DisplayPage(_pages[--_pageIndex]);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!_tutorialCompleted)
            {
                e.Cancel = true;
                Environment.Exit(0);
            }
        }
    }
}
