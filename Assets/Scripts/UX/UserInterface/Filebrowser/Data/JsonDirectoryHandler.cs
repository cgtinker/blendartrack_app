using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

namespace ArRetarget
{
    public class JsonDirectoryHandler : MonoBehaviour
    {
        #region get dirs
        private string[] suffixes = { "cam", "face" };
        public List<JsonDirectory> GetDirectories(string persistentPath)
        {
            //checking for dirs
            string[] dirs = FileManagement.GetDirectories(persistentPath);
            List<JsonDirectory> tmp_jsonDirectories = new List<JsonDirectory>();

            for (int t = 0; t < dirs.Length; t++)
            {
                //plugin "NatSuite" generates an empty "gallery-folder" on recording - release build may contain an empty "Unity / unity" folder
                if (!FileManagement.StringEndsWith(dirs[t], "Gallery") && !FileManagement.StringEndsWith(dirs[t], "nity") && !FileManagement.StringEndsWith(dirs[t], "il2cpp"))
                {
                    IgnoreEmptyCacheFolders(tmp_jsonDirectories, dirs, t);
                }
            }

            if (tmp_jsonDirectories.Count == 0)
                return tmp_jsonDirectories;

            for (int i = 0; i < tmp_jsonDirectories.Count; i++)
            {
                CheckForSuffix(tmp_jsonDirectories, i);
            }

            return tmp_jsonDirectories;
        }

        private void CheckForSuffix(List<JsonDirectory> tmp_jsonDirectories, int i)
        {
                foreach (string suffix in suffixes)
            {
                if (FileManagement.StringEndsWith(tmp_jsonDirectories[i].dirName, suffix))
                {
                    //create new dir obj pointing to json and overwrite previous jsonDir
                    var updatedDir = SetupDirectoryPointerToJson(tmp_jsonDirectories[i].dirPath, suffix, tmp_jsonDirectories[i]);
                    tmp_jsonDirectories[i] = updatedDir;
                }
            }
        }

        private void IgnoreEmptyCacheFolders(List<JsonDirectory> tmp_jsonDirectories, string[] dirs, int t)
        {
            JsonDirectory m_dir = new JsonDirectory();
            m_dir.dirName = FileManagement.GetDirectoryName(dirs[t]);
            m_dir.dirPath = dirs[t];
            tmp_jsonDirectories.Add(m_dir);
        }
        #endregion

        #region subdir pointer to file
        //subdir
        private JsonDirectory SetupDirectoryPointerToJson(string path, string suffix, JsonDirectory m_dir)
        {
            //JsonDirectory m_dir = new JsonDirectory();

            //looping through sub dir
            if (FileManagement.ValidateDirectory(path))
            {
                //assign for empty folders
                m_dir.value = 0;

                //jsons in sub dir
                FileInfo[] jsonFiles = FileManagement.GetJsonsAtPath(path);
                foreach (FileInfo json in jsonFiles)
                {
                    CheckForFileSuffix(json, suffix, m_dir);
                }

                return m_dir;
            }

            LogManager.Instance.Log($"folder doesn't contain valid json contents <br><br>{path}", LogManager.Message.Warning);
            Debug.LogWarning("Directory doesn't contain a valid json");
            return m_dir;
        }

        private void CheckForFileSuffix(FileInfo json, string suffix, JsonDirectory m_dir)
        {
            string jsonFilename = json.Name;

            //setup JsonDirectoryData
            if (FileManagement.StringEndsWith(jsonFilename, suffix + ".json"))
            {
                //setting up the serializeable JsonDir obj
                m_dir.dirName = FileManagement.RemoveSuffixFromEnd(json.Name, ".json");
                m_dir.value = FileManagement.StringToInt(m_dir.dirName);
                m_dir.active = false;
                m_dir.jsonFilePath = json.FullName;
            }
        }
        #endregion
    }
}
