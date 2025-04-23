using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game_archive_manager.Helper
{
    internal static class FileHelper
    {
        public static string GetFileType(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentException("File name cannot be null or empty.", nameof(fileName));
            }

            var extension = Path.GetExtension(fileName);
            return string.IsNullOrEmpty(extension) ? "unknown" : extension.ToLower();
        }
    }
}
