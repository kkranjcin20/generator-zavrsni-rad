using generator_WPF;
using System.Windows;
using System.Windows.Forms;

namespace Generator_Extension
{
    [Command(PackageIds.MyCommand)]
    internal sealed class MyCommand : BaseCommand<MyCommand>
    {
        protected override void Execute(object sender, EventArgs e)
        {
            if (System.Windows.Forms.MessageBox.Show("Solution must be reloaded to save generated classes. Do you still want to proceed?", "Reloading the solution", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
            }
            else
            {
                return;
            }
        }
    }
}
