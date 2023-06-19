using generator_zavrsni_rad;
using generator_zavrsni_rad.Generator_PL;

namespace Generator_Extension
{
    [Command(PackageIds.MyCommand)]
    internal sealed class MyCommand : BaseCommand<MyCommand>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            FrmDatabase frmDatabase = new FrmDatabase();
            frmDatabase.ShowDialog();
        }
    }
}
