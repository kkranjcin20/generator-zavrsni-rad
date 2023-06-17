using generator_zavrsni_rad;

namespace Generator_Extension
{
    [Command(PackageIds.MyCommand)]
    internal sealed class MyCommand : BaseCommand<MyCommand>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            FrmGenerator frmGenerator = new FrmGenerator();
            frmGenerator.ShowDialog();
        }
    }
}
