using generator_zavrsni_rad;
using generator_zavrsni_rad.Generator_PL;
using System.Windows.Forms;

namespace Generator_Extension
{
    [Command(PackageIds.MyCommand)]
    internal sealed class MyCommand : BaseCommand<MyCommand>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await Task.Run(() =>
            {
                if (System.Windows.Forms.MessageBox.Show("Solution must be reloaded to save generated classes. Do you still want to proceed?", "Reloading the solution", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    FrmDatabase frmDatabase = new();
                    frmDatabase.ShowDialog();
                }
                else
                {
                    return;
                }
            });
        }
    }
}
