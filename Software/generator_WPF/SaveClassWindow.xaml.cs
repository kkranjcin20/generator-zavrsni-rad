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
        string _projectPath;

        int i = 0;

        public SaveClassWindow(List<string> classNames, List<string> generatedClassCodes)
        {
            InitializeComponent();
            _classNames = classNames;
            _generatedClassCodes = generatedClassCodes;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(_projectPath == "")
            {
                _projectPath = classSaver.GetProjectPath("");
            }
            else
            {
                _projectPath = classSaver.GetProjectPath(_projectPath);
            }
            txtPath.Text = _projectPath;
            txtClassName.Text = _classNames.First();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (i == 0)
            {
                classSaver.SetupProject(txtPath.Text);
                btnChangeProject.IsEnabled = false;
            }
            if (txtPath.Text != "")
            {
                classSaver.SaveClass(_classNames[i], _generatedClassCodes[i]);
                i++;
                if(_classNames.Count != i)
                {
                    txtClassName.Text = _classNames[i];
                }
                else
                {
                    classSaver.SaveAndUnloadProject();
                    Close();
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Invalid path!\nPress the button 'Change Project' and choose a valid path.");
            }
        }
        private void btnSaveAllClasses_Click(object sender, RoutedEventArgs e)
        {
            if (i == 0)
            {
                classSaver.SetupProject(txtPath.Text);
            }
            for (; i < _classNames.Count; i++)
            {
                classSaver.SaveClass(_classNames[i], _generatedClassCodes[i]);
            }
            classSaver.SaveAndUnloadProject();
            Close();
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
