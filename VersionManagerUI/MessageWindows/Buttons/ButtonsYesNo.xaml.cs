using System.Windows;
using System.Windows.Controls;

namespace VersionManagerUI.MessageWindows.Buttons
{
    /// <summary>
    /// Interaction logic for ButtonsYesNo.xaml
    /// </summary>
    public partial class ButtonsYesNo : Page
    {
        private MessageWindow _parent { get; set; }
        public ButtonsYesNo(MessageWindow parent)
        {
            _parent = parent;
            InitializeComponent();
        }

        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            _parent.ButtonClicked(MessageWindowResult.No);
        }

        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            _parent.ButtonClicked(MessageWindowResult.Yes);
        }
    }
}
