using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileHelper;

namespace SqlServerConnectionHelper
{
    class SSCFileManager : FileManager
    {
        public SSCFileManager(string filePath, string newFileName) : base(filePath, newFileName)
        {
            this.FileName = newFileName;
            this.FolderPath = filePath;
        }
    }
}
