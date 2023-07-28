using System;
using System.IO;
using System.Windows.Forms;
using System.Windows;
using System.Security.AccessControl;

namespace generator.Generator_BLL
{
    public class FileCreation
    {
        public void CreateFile(string filePath, string generatedCode)
        {
            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    using (var streamWriter = new StreamWriter(fileStream))
                    {
                        streamWriter.Write(generatedCode);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error creating file: " + ex.Message, "File Creation", (MessageBoxButton)MessageBoxButtons.OK, (MessageBoxImage)MessageBoxIcon.Error);
            }
        }
    }
}
