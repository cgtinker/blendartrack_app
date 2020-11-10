using UnityEngine;
using System;
using System.IO;
using System.Globalization;
using System.Collections.Generic;

namespace ArRetarget
{
    public static class FileManagementHelper
    {
        #region date and text
        public static string GetDateTime()
        {
            DateTime localDate = DateTime.Now;
            string time = localDate.ToString("yyyyMMdd_HHmmss", CultureInfo.InvariantCulture);
            return time;
        }

        public static string GetDay()
        {
            DateTime localDate = DateTime.Now;
            string day = localDate.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
            return day;
        }

        public static string GetDateTimeText()
        {
            DateTime dateTime = DateTime.Now;
            return dateTime.ToString();
        }

        public static string GetParagraph()
        {
            string s = Environment.NewLine;
            return s;
        }
        #endregion

        #region share and delete files
        public static void DeleteFilesInDir(List<string> pathList)
        {
            foreach (string path in pathList)
            {
                if (ValidatePath(path))
                    File.Delete(path);
            }
        }

        public static void ShareJsonFiles(List<string> pathList, string subject, string text)
        {
            var nativeShare = new NativeShare();
            foreach (string path in pathList)
            {
                if (ValidatePath(path))
                {
                    nativeShare.AddFile(path);
                }
            }

            nativeShare.SetSubject(subject).SetText(text);
            nativeShare.Share();
        }
        #endregion

        #region file info
        public static FileInfo[] JsonsInDir(string dir)
        {
            var dirInfo = new DirectoryInfo(dir);
            FileInfo[] info = dirInfo.GetFiles("*.json");
            return info;
        }

        public static string FileContents(string path)
        {
            string fileContents = File.ReadAllText(path, System.Text.Encoding.UTF8);
            return fileContents;
        }

        public static bool ValidatePath(string mediaPath)
        {
            try
            {
                if (File.Exists(mediaPath))
                {
                    return true;
                }

                else
                {
                    Debug.Log("File not found");
                    return false;
                }
            }

            catch (IOException ioExp)
            {
                Debug.Log(ioExp.Message);
                return false;
            }
        }
        #endregion
    }
}
