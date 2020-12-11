using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using System.IO.Compression;

namespace ArRetarget
{
    public static class FileManagement
    {
        #region date and text
        public static string GetDateTime()
        {
            DateTime localDate = DateTime.Now;
            string time = localDate.ToString($"yyyy-MM-dd_HH-mm-ss", CultureInfo.InvariantCulture);
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

        #region share / save files
        public static void WriteDataToDisk(string data, string persistentPath, string filename)
        {
            Debug.Log("Serializing json data");

            var path = $"{persistentPath}/{filename}";
            File.WriteAllText(path: path, contents: data, encoding: System.Text.Encoding.UTF8);
        }

        public static void ShareZip(string path, string subject, string text)
        {
            var nativeShare = new NativeShare();
            if (ValidatePath(path))
                nativeShare.AddFile(path);

            else
                Debug.LogError("path to zip not valid");

            nativeShare.SetSubject(subject).SetText(text);
            nativeShare.Share();
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

        #region delete files
        public static void DeleteFilesAtPath(List<string> pathList)
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
        #endregion

        #region json file info
        public static FileInfo[] GetJsonsAtPath(string dir)
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
        #region create
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
        #endregion

        #region delete
        public static void DeleteDirectories(List<string> tar_dirs)
        {
            Debug.Log("Deleting selected directories");
            foreach (string dir in tar_dirs)
            {
                DeleteDirectory(dir);
            }
        }

        public static void DeleteDirectory(string tar_dir)
        {
            string[] files = Directory.GetFiles(tar_dir);
            string[] dirs = GetDirectories(tar_dir);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(tar_dir, false);
        }
        #endregion

        #region get
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

        public static string GetDirectoryName(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            return di.Name;
        }
        #endregion

        #region validate
        public static bool ValidateDirectory(string path)
        {
            if (Directory.Exists(path))
                return true;

            else
                return false;
        }
        #endregion
        #endregion

        #region compress data
        public static string CompressDirectories(List<string> selectedDirPaths)
        {
            if (selectedDirPaths.Count == 1)
            {
                CompressDir(selectedDirPaths[0], $"{selectedDirPaths[0]}.zip");
                return $"{selectedDirPaths[0]}.zip";
            }

            else
            {
                //creating a tmp folder for ref
                string tmp = RemoveSuffixFromEnd(selectedDirPaths[0], GetDirectoryName(selectedDirPaths[0]));
                string curTime = GetDateTime();
                string m_dir = $"{tmp}{curTime}";
                CreateDirectory(m_dir);
                Debug.Log(m_dir);

                //compressing all dirs in the tmp folder
                foreach (string dir in selectedDirPaths)
                {
                    string dirname = GetDirectoryName(dir);
                    string tar = RemoveSuffixFromEnd(dir, dirname);
                    CompressDir(dir, $"{tar}/{curTime}/{dirname}.zip");
                }

                //compress the tmp folder
                CompressDir(m_dir, m_dir + ".zip");
                DeleteDirectory(m_dir);

                return m_dir + ".zip";
                //delete the tmp folder
            }
        }

        public static void CompressDir(string dirPath, string tarPath)
        {
            ZipFile.CreateFromDirectory(dirPath, tarPath);
        }
        #endregion

        #region String methods
        public static Int64 StringToInt(string input)
        {
            string b = string.Empty;
            Int64 val = 0;

            b = String.Join("", input.Where(char.IsDigit));

            if (b.Length > 0)
            {
                bool t = Int64.TryParse(b, out val);
            }

            return val;
        }

        public static bool StringEndsWith(string path, string suffix)
        {
            if (path.EndsWith(suffix))
                return true;

            else
                return false;
        }

        public static string RemoveSuffixFromEnd(this string s, string suffix)
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

        public static string RemoveLengthFromEnd(string s, int length)
        {
            try
            {
                return s.Substring(0, s.Length - length);
            }
            catch
            {
                return s;
            }
        }
        #endregion
    }
}
