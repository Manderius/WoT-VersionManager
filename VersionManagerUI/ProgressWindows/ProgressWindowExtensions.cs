using System.Threading.Tasks;
using System.Windows;

namespace VersionManagerUI.ProgressWindows
{
    public static class ProgressWindowExtensions
    {
        public static async Task<bool?> ShowDialogAsync(this Window @this)
        {
            await Task.Yield();
            if (@this.IsActive)
                return true;
            return @this.ShowDialog();
        }
    }
}
