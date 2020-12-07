using UnityEngine;
using System;
using System.IO;
using System.Globalization;
using System.Collections.Generic;

namespace ArRetarget
{
    public static class FileManagement
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

        #region save, share and delete files
        public static void WriteDataToDisk(string data, string persistentPath, string filename)
        {
            Debug.Log("Serializing json data");

            var path = $"{persistentPath}/{filename}";
            File.WriteAllText(path: path, contents: data, encoding: System.Text.Encoding.UTF8);
        }

        public static void DeleteFilesInDir(List<string> pathList)
        {
            foreach (string path in pathList)
            {
                if (ValidatePath(path))
                    File.Delete(path);
            }
        }

        public static void DeleteFile(string path)
        {
            if (ValidatePath(path))
                File.Delete(path);
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

        #region directories
        public static void CreateDirectory(string path)
        {
            try
            {
                if (ValidateDirectory(path) == true)
                {
                    return;
                }

                DirectoryInfo di = Directory.CreateDirectory(path);
            }

            catch (Exception e)
            {
                Debug.LogError("Directory does already exist: " + e.ToString());
            }
        }

        public static void DeleteDirectory(string path)
        {
            try
            {
                if (ValidateDirectory(path) == false)
                {
                    return;
                }

                FileInfo[] info = JsonsInDir(path);

                foreach (FileInfo m_path in info)
                {
                    if (ValidatePath(m_path.FullName))
                        File.Delete(m_path.FullName);
                }

                Directory.Delete(path);
            }

            catch (Exception e)
            {
                Debug.LogError("Directory doesn't exist: " + e.ToString());
            }
        }

        public static string[] GetDirectories(string path)
        {
            try
            {
                string[] directories = Directory.GetDirectories(path);

                return directories;
            }

            catch (Exception e)
            {
                Debug.LogError("Directory doesn't exist: " + e.ToString());
                return new string[0];
            }
        }

        public static bool ValidateDirectory(string path)
        {
            if (Directory.Exists(path))
                return true;

            else
                return false;
        }
        #endregion

        #region String methods
        public static bool StringEndsWith(string path, string suffix)
        {
            if (path.EndsWith(suffix))
                return true;

            else
                return false;
        }

        public static string RemoveFromEnd(this string s, string suffix)
        {
            if (s.EndsWith(suffix))
            {
                return s.Substring(0, s.Length - suffix.Length);
            }
            else
            {
                return s;
            }
        }
        #endregion
    }
}
