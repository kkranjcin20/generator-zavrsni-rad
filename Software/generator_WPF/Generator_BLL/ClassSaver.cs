using Microsoft.Build.Evaluation;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.IO;
using System.Windows.Forms;

namespace generator.Generator_BLL
{
    public class ClassSaver
    {
        Project project;
        FileCreation fileCreation;
        string projectDirectory;

        public string GetProjectPath(string filePath)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Project Files (*.csproj)|*.csproj";
                openFileDialog.Title = "Select Project File";

                if(filePath == "")
                {
                    filePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                    string source = "source";
                    string sourceDir = Path.Combine(filePath, source);
                    if (Directory.Exists(filePath))
                    {
                        filePath = sourceDir;
                    }
                    string repos = "repos";
                    string reposDir = Path.Combine(sourceDir, repos);
                    if (Directory.Exists(filePath))
                    {
                        filePath = reposDir;
                    }
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

        public void SetupProject(string projectPath)
        {
            project = new Project(projectPath);
            fileCreation = new FileCreation();
            projectDirectory = Path.GetDirectoryName(projectPath);
        }

        public void SaveAndUnloadProject()
        {
            project.Save();
            ProjectCollection.GlobalProjectCollection.UnloadProject(project);
        }

        public void SaveClass(string className, string generatedCode)
        {
            className += ".cs";
            string filePath = Path.Combine(projectDirectory, className);

            fileCreation.CreateFile(filePath, generatedCode);

            project.AddItem("Compile", className);
        }
    }
}
