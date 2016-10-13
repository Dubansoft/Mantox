﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DateHelper;
using System.Data.SqlClient;
using System.Data;


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
        public static string LogEvent(object sender, string ErrorMessage, object e, string methodName = "<?>")
        {
            try
            {
                Exception renderedException = null;
                MySqlException renderedMySqlException = null;
                SqlException renderedSqlException = null;

                try { renderedException = (Exception)e; }
                catch (Exception) { }

                try { renderedMySqlException = (MySqlException)e; }
                catch (Exception) { }

                try { renderedSqlException = (SqlException)e; }
                catch (Exception) { }

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

                if (renderedException is Exception)
                {
                    Exception myException = (Exception)e;

                    FileManager.TextToAppend += "\n\t\tException type is System.Exception";

                    FileManager.TextToAppend += "\n\t\tSource: " + myException.Source.ToString();

                    if (renderedException.InnerException != null)
                    {
                        FileManager.TextToAppend += "\n\t\tInnerException: " + myException.InnerException.ToString();
                    }

                    FileManager.TextToAppend += "\n\t\tStackTrace: " + myException.StackTrace.ToString();
                    FileManager.TextToAppend += "\n\t\tType: " + e.GetType().ToString();
                }

                if (renderedMySqlException is MySqlException)
                {
                    Exception mySqlException = (Exception)e;

                    FileManager.TextToAppend += "\n\t\tException type is MySql.Data.MySqlCliente.MySqlException";

                    FileManager.TextToAppend += "\n\t\tSource: " + mySqlException.Source.ToString();

                    if (renderedMySqlException.InnerException != null)
                    {
                        FileManager.TextToAppend += "\n\t\tInnerException: " + mySqlException.InnerException.ToString();
                    }

                    FileManager.TextToAppend += "\n\t\tStackTrace: " + mySqlException.StackTrace.ToString();
                    FileManager.TextToAppend += "\n\t\tType: " + e.GetType().ToString();
                }

                if (renderedSqlException is SqlException)
                {
                    Exception sqlException = (Exception)e;

                    FileManager.TextToAppend += "\n\t\tException type is MySql.Data.MySqlCliente.MySqlException";

                    FileManager.TextToAppend += "\n\t\tSource: " + sqlException.Source.ToString();

                    if (renderedMySqlException.InnerException != null)
                    {
                        FileManager.TextToAppend += "\n\t\tInnerException: " + sqlException.InnerException.ToString();
                    }

                    FileManager.TextToAppend += "\n\t\tStackTrace: " + sqlException.StackTrace.ToString();
                    FileManager.TextToAppend += "\n\t\tType: " + e.GetType().ToString();
                }

                FileManager.TextToAppend += "\n\n";

                FileManager.WriteFileDelegate = new Action(myFileManager.WriteToFile);

                FileManager.WriteFileDelegate?.Invoke();

                e = null;

                return FileManager.TextToAppend;
            }
            catch (Exception)
            {
                return "Sorry, but we cant log an error that happened in the event logger itself";
                //Sorry, but we cant log an error that happened in the event logger itself
            }
        }
    }
}
