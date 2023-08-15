using Microsoft.Build.Evaluation;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace generator.Generator_BLL
{
    public class ClassSaver
    {
        Project project;
        FileManager fileManager;
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
            fileManager = new FileManager();
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

            fileManager.CreateFile(filePath, generatedCode);

            ProjectItem existingItem = project.GetItems("Compile")
                                      .FirstOrDefault(item => item.EvaluatedInclude.Equals(className, StringComparison.OrdinalIgnoreCase));

            if (existingItem == null)
            {
                project.AddItem("Compile", className);
            }
        }
    }
}
