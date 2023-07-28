using Microsoft.Build.Evaluation;
using System;
using System.IO;
using System.Windows.Forms;

namespace generator.Generator_BLL
{
    public class ClassSaver
    {
        public string GetProjectPath(string filePath)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Project Files (*.csproj)|*.csproj";
                openFileDialog.Title = "Select Project File";

                if(filePath == "")
                {
                    filePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                }
                openFileDialog.InitialDirectory = filePath;

                DialogResult result = openFileDialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    return openFileDialog.FileName;
                }
                else
                {
                    return "";
                }
            }
        }

        public void SaveClass(string className, string projectPath, string generatedCode)
        {
            Project project = new Project(projectPath);
            string projectDirectory = Path.GetDirectoryName(projectPath);
            className += ".cs";
            string filePath = Path.Combine(projectDirectory, className);

            FileCreation fileCreation = new FileCreation();
            fileCreation.CreateFile(filePath, generatedCode);

            project.AddItem("Compile", className);
            project.Save();

            ProjectCollection.GlobalProjectCollection.UnloadProject(project);
        }
    }
}
