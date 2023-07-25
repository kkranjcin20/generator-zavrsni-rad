using EnvDTE;
using generator_WPF;
using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell.Settings;
using System.Windows.Forms;

namespace Generator_Extension
{
    [Command(PackageIds.MyCommand)]
    internal sealed class MyCommand : BaseCommand<MyCommand>
    {
        protected override Task InitializeCompletedAsync()
        {
            FirstTimeMessageHandler.ShowFirstTimeMessage();
            return Task.CompletedTask;
        }

        protected override void Execute(object sender, EventArgs e)
        {
            if (FirstTimeMessageHandler.isFirstTime)
            {
                System.Windows.Forms.MessageBox.Show("The generated class will be displayed in the Notepad application, " +
                    "after which you will have the opportunity to copy it and save it to your desired location.", "", MessageBoxButtons.OK);

                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
            }
            else
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
            }
        }

        public static class FirstTimeMessageHandler
        {
            private const string FirstTimeRunSettingKey = "IsFirstTimeRun";

            public static bool isFirstTime { get; private set; }

            public static void ShowFirstTimeMessage()
            {
                ThreadHelper.ThrowIfNotOnUIThread();
                var settingsManager = new ShellSettingsManager(ServiceProvider.GlobalProvider);
                var writableSettingsStore = settingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);

                if (!writableSettingsStore.CollectionExists("GeneratorExtension"))
                {
                    writableSettingsStore.CreateCollection("GeneratorExtension");
                }

                if (!writableSettingsStore.GetBoolean("GeneratorExtension", FirstTimeRunSettingKey, defaultValue: false))
                {
                    writableSettingsStore.SetBoolean("GeneratorExtension", FirstTimeRunSettingKey, true);
                    isFirstTime = true;
                }
                else
                {
                    isFirstTime = false;
                }
            }
        }
    }
}
