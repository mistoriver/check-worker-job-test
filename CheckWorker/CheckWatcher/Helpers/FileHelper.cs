using System;
using System.IO;
using System.Text;

namespace CheckWatcher.Helpers
{
    public static class FileHelper
    {
        public static string GetTextFromFile(string path)
        {
            var result = "";
            try
            {
                using (FileStream fstream = File.OpenRead(path))
                {
                    byte[] array = new byte[fstream.Length];
                    fstream.Read(array, 0, array.Length);
                    result = Encoding.Default.GetString(array);
                }
            }
            catch (Exception e)
            {
                ServiceLogger.Log.Error($"Произошла ошибка при получении файла.{Environment.NewLine + e}");
            }
            return result;
        }

        public static void DeleteFile(string path)
        {
            new FileInfo(path).Delete();
            ServiceLogger.Log.Info($"Файл по пути {path} удалён");
        }
        public static bool IsFileExists(string path)
        {
            return File.Exists(path);
        }
    }

}
