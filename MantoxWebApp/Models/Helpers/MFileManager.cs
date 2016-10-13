using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileHelper;

namespace CefSharp.MinimalExample.WinForms
{
    class SHFileManager : FileManager
    {
        public SHFileManager(string filePath, string newFileName) : base(filePath, newFileName)
        {
            this.FileName = newFileName;
            this.FolderPath = filePath;
        }
    }
}
