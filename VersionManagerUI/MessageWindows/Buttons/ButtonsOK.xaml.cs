using System.Windows;
using System.Windows.Controls;

namespace VersionManagerUI.MessageWindows.Buttons
{
    /// <summary>
    /// Interaction logic for OK.xaml
    /// </summary>
    public partial class ButtonsOK : Page
    {
        private MessageWindow _parent { get; set; }
        public ButtonsOK(MessageWindow parent)
        {
            _parent = parent;
            InitializeComponent();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            _parent.ButtonClicked(MessageWindowResult.OK);
        }
    }
}
