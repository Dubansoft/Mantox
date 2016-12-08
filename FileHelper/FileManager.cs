using System;
using System.IO;
using System.Windows.Forms;


namespace FileHelper
{
    public class FileManager
    {
        #region "Members"

        private static string textToAppend;
        public static string TextToAppend
        {
            get { return FileManager.textToAppend; }
            set { FileManager.textToAppend = value; }
        }

        public string Text
        {
            set { TextToAppend = value; }
        }

        public static Action WriteFileDelegate;

        private string folderPath;
        public string FolderPath
        {
            get { return folderPath; }
            set { folderPath = value; }
        }

        public string FullFilePath
        {
            get { return folderPath + "\\" + fileName; }
        }

        private string fileName;
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        #endregion

        /// <summary>
        /// FileManager initialiser with path and filename parameters
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="newFileName"></param>
        public FileManager(string filePath, string newFileName)
        {
            this.FileName = newFileName;
            this.FolderPath = filePath;
        }

        public bool CreateFolder()
        {
            try
            {
                if (Directory.Exists(FolderPath))
                {
                    Console.WriteLine("That path exists already.");
                    return true;
                }
                else
                {
                    // Try to create the directory.
                    DirectoryInfo di = Directory.CreateDirectory(FolderPath);
                    return true;
                }

            }
            catch (Exception e)
            {
                TextToAppend = e.Message.ToString();
                this.WriteToFile();
                return false;
            }
        }

        public bool CreateFile()
        {
            CreateFolder();

            try
            {
                if (!File.Exists(FullFilePath))
                {
                    File.Create(FullFilePath).Dispose();
                }
                return true;
            }
            catch (Exception e)
            {
                //EventLogger.LogEvent(this, e.Message.ToString(), e);
                return false;
            }
        }

        public void WriteToFile()
        {
            CreateFile();

            try
            {
                if (File.Exists(FullFilePath))
                {
                    using (StreamWriter sw = File.AppendText(FullFilePath))
                    {
                        sw.WriteLine(TextToAppend);
                        sw.Close();
                    }
                }
            }
            catch (Exception e)
            {
                //EventLogger.LogEvent(this, e.Message.ToString(), e);
            }
        }

        public string ReadFile()
        {
            try
            {
                if (File.Exists(FullFilePath))
                {
                    string fileText = string.Empty;

                    using (TextReader tr = new StreamReader(FullFilePath))
                    {
                        fileText = tr.ReadToEnd();
                        tr.Close();
                        return fileText;
                    }
                }
                return string.Empty;
            }
            catch (Exception e)
            {
                //EventLogger.LogEvent(this, e.Message.ToString(), e);
                return string.Empty;
            }
        }

        public void RemoveFile()
        {
            try
            {
                File.Delete(FullFilePath);
            }
            catch (Exception e)
            {
                //EventLogger.LogEvent(this, e.Message.ToString(), e);
            }
        }
    }

    public static class EventLogger
    {

        /// <summary>
        /// Writes a new line to the InkalertLog.txt log file
        /// </summary>
        /// <param name="sender">The form or object that sends the event</param>
        /// <param name="ErrorMessage">The message to be added to the log file</param>
        /// <param name="e">Exception reference e</param>
        public static string LogEvent(object sender, string ErrorMessage, Exception e, string methodName = "<?>")
        {
            try
            {
                string activeControl = methodName;
                string senderFormText = "<no form set>";

                try
                {
                    senderFormText = (sender.GetType().Name);
                }
                catch (Exception)
                {
                }

                if (sender is string[])
                {
                    string[] senderArray = (string[])sender;
                    senderFormText = "Método o clase estático:" + (string)senderArray[0];
                    activeControl = (string)senderArray[0];
                }

                FileManager myFileManager = new FileManager(Application.StartupPath.ToString(), "ApplicationLog.txt");
                FileManager.TextToAppend = FHDateEngine.CurrentDateTimeShort + " :: " + senderFormText + " :: " + methodName + " :: " + ErrorMessage;

                try
                {
                    FileManager.TextToAppend += "\n\t\tException type is " + e.GetType().Name;

                    FileManager.TextToAppend += "\n\t\tSource: " + e.Source.ToString();

                    if (e.InnerException != null)
                    {
                        FileManager.TextToAppend += "\n\t\tInnerException: " + e.InnerException.ToString();
                    }

                    FileManager.TextToAppend += "\n\t\tStackTrace: " + e.StackTrace.ToString();
                    FileManager.TextToAppend += "\n\t\tType: " + e.GetType().ToString();

                }
                catch (Exception ex)
                {
                    Console.Write("An error has ocurred: " + ex.Message.ToString());
                    return "An error has ocurred: " + ex.Message.ToString();
                }

                FileManager.TextToAppend += "\n\n";

                FileManager.WriteFileDelegate = new Action(myFileManager.WriteToFile);

                FileManager.WriteFileDelegate?.Invoke();

                e = null;

                return FileManager.TextToAppend;
            }
            catch (Exception ex)
            {
                Console.Write("An error has ocurred: " + ex.Message.ToString());
                return "An error has ocurred: " + ex.Message.ToString();
                //Sorry, but we cant log an error that happened in the event logger itself
            }
        }
    }
}
