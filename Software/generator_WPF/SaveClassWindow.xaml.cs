using generator.Generator_BLL;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace generator
{
    /// <summary>
    /// Interaction logic for SaveClassWindow.xaml
    /// </summary>
    public partial class SaveClassWindow : Window
    {
        ClassSaver classSaver = new ClassSaver();
        private List<string> _classNames;
        private List<string> _generatedClassCodes;
        int i = 0;

        public SaveClassWindow(List<string> classNames, List<string> generatedClassCodes)
        {
            InitializeComponent();
            _classNames = classNames;
            _generatedClassCodes = generatedClassCodes;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string projectPath = classSaver.GetProjectPath("");
            txtPath.Text = projectPath;
            txtClassName.Text = _classNames.First();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtPath.Text != "")
            {
                classSaver.SaveClass(_classNames[i], txtPath.Text, _generatedClassCodes[i]);
                i++;
                if(_classNames.Count != i)
                {
                    txtClassName.Text = _classNames[i];
                }
                else
                {
                    Close();
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Invalid path!\nPress the button 'Change Project' and choose a valid path.");
            }
        }

        private void btnChangeProject_Click(object sender, RoutedEventArgs e)
        {
            string filePath = Path.GetDirectoryName(txtPath.Text);
            string projectPath = classSaver.GetProjectPath(filePath);
            if (projectPath != "")
            {
                txtPath.Text = projectPath;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
