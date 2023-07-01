using EnvDTE;
using generator_WPF;
using System.Windows.Forms;

namespace Generator_Extension
{
    [Command(PackageIds.MyCommand)]
    internal sealed class MyCommand : BaseCommand<MyCommand>
    {
        protected override void Execute(object sender, EventArgs e)
        {
            if (System.Windows.Forms.MessageBox.Show("The generated class will be displayed in the Notepad application, after which you will have the opportunity to copy it and save it to your desired location.", "", MessageBoxButtons.OK) == DialogResult.OK)
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
