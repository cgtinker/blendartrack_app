using UnityEngine;
using System.Collections;
using System.IO;
using System.Globalization;
using System;

namespace ArRetarget
{
    public static class FileManagementHelper
    {
        //TODO: Combine FileManager and FileManagementHelper
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
                    Debug.Log(mediaPath);
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
    }
}
