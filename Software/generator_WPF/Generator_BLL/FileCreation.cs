using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;

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

            System.Diagnostics.Process.Start("notepad.exe", filePath);
        }
    }
}
