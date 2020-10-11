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
