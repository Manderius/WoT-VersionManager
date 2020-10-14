using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Debugging.Common
{
    public class Utils
    {
        public static string SelectXML(string filename = "")
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Version Manager XML File|*.xml";
            dialog.FileName = filename;
            if (dialog.ShowDialog().GetValueOrDefault())
            {
                return dialog.FileName;
            }
            return null;
        }

        public static string SelectDirectory(string windowName = "")
        {
            using (CommonOpenFileDialog dialog = new CommonOpenFileDialog(windowName))
            {
                dialog.IsFolderPicker = true;
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    return dialog.FileName;
                }
            }
            return null;
        }
    }
}
