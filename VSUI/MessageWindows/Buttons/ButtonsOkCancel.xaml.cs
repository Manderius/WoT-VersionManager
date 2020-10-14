using System.Windows;
using System.Windows.Controls;

namespace VersionManagerUI.MessageWindows.Buttons
{
    /// <summary>
    /// Interaction logic for ButtonsOkCancel.xaml
    /// </summary>
    public partial class ButtonsOkCancel : Page
    {
        private MessageWindow _parent { get; set; }

        public ButtonsOkCancel(MessageWindow parent)
        {
            _parent = parent;
            InitializeComponent();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            _parent.ButtonClicked(MessageWindowResult.OK);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            _parent.ButtonClicked(MessageWindowResult.Cancel);
        }
    }
}
