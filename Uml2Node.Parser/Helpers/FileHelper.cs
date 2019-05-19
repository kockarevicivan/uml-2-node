using System.IO;

namespace Uml2Node.Parser.Helpers
{
    internal static class FileHelper
    {
        internal static void ClearTempDirectory()
        {
            DirectoryInfo di = new DirectoryInfo("temp");

            foreach (FileInfo file in di.GetFiles())
                file.Delete();

            foreach (DirectoryInfo dir in di.GetDirectories())
                dir.Delete(true);
        }
    }
}