using System.IO;
using System.Text;

namespace Utility.FileUtil
{
    public static class FileContentUtil
    {
        public static string GetFileContent(string filePath)
        {
            StringBuilder sbOut = new StringBuilder();

            string line;

            // Read the file and display it line by line.  
            StreamReader file = new StreamReader(filePath);
            while ((line = file.ReadLine()) != null)
            {
                sbOut.AppendLine(line);
                // fileContent += line;
            }
            file.Close();
            return sbOut.ToString();
        }
    }
}
