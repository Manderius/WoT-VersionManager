using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace VersionManagerUI.ProgressWindows
{
    /// <summary>
    /// Interaction logic for ProgressWindow.xaml
    /// </summary>
    public partial class ProgressWindow : Window
    {
        public bool CloseOnComplete { get; set; }
        public string WindowTitle { get; set; }
        public string Text { get; set; }
        public PBar ProgressBarData = new PBar();

        public ProgressWindow(string title, string text)
        {
            WindowTitle = title;
            Text = text;
            DataContext = this;
            Owner = App.Current.MainWindow;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            InitializeComponent();
            ProgressBar.DataContext = ProgressBarData;
            Closing += new CancelEventHandler(OnClosing);
        }

        void OnClosing(object sender, CancelEventArgs e)
        {
            if (ProgressBarData.Progress < 100)
                e.Cancel = true;
        }

        public void SetProgress(int percent)
        {
            ProgressBarData.Progress = percent;
            if (percent == 100 && CloseOnComplete)
                Close();
        }
    }

    public class PBar : INotifyPropertyChanged
    {
        private int _progress;
        public int Progress
        {
            get { return _progress; }
            set
            {
                if (value != _progress)
                {
                    _progress = value;
                    OnPropertyChanged("progress");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
    }
}
