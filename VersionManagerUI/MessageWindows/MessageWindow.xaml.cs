using System.Windows;
using VersionManagerUI.MessageWindows.Buttons;

namespace VersionManagerUI.MessageWindows
{
    /// <summary>
    /// Interaction logic for MessageWindow.xaml
    /// </summary>
    public partial class MessageWindow : Window
    {
        public string WindowTitle { get; set; }
        public string Text { get; set; }
        public MessageWindowResult Result { get; set; }
        public MessageWindow(string title, string text, MessageWindowButtons buttons)
        {
            WindowTitle = title;
            Text = text;
            DataContext = this;
            Owner = App.Current.MainWindow;
            Result = MessageWindowResult.Unknown;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            InitializeComponent();
            ShowButtons(buttons);
        }

        private void ShowButtons(MessageWindowButtons type)
        {
            switch (type)
            {
                case MessageWindowButtons.OK:
                    frmOptions.Navigate(new ButtonsOK(this));
                    break;
                case MessageWindowButtons.OKCancel:
                    frmOptions.Navigate(new ButtonsOkCancel(this));
                    break;
                case MessageWindowButtons.YesNo:
                    frmOptions.Navigate(new ButtonsYesNo(this));
                    break;
            }
        }

        public void ButtonClicked(MessageWindowResult result) {
            Result = result;
            Close();
        }
    }
}
