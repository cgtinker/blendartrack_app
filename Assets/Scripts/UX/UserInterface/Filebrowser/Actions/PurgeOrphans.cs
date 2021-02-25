using UnityEngine;
using ArRetarget;
using System.IO;
using System.Collections.Generic;

namespace ArRetarget
{
    public static class PurgeOrphans
    {
        public static void PurgeErrorMessages()
        {
            //LogManager.Instance.Log("", LogManager.Message.Disable);
        }

        public static void PurgeOrphanZips()
        {
            try
            {
                FileInfo[] zipFiles = FileManagement.GetZipsAtPath(FilebrowserManager.persistentPath);

                if (zipFiles.Length > 0)
                {
                    Debug.Log("Purging " + zipFiles.Length + " orphan zips");
                    foreach (FileInfo zip in zipFiles)
                    {
                        FileManagement.DeleteFile(zip.FullName);
                    }
                }
            }

            catch
            {
                Debug.LogWarning("Cannot purge zips in certain circumstance");
            }
        }

        public static void DestroyOrphanViewer()
        {
            GameObject[] orphanViewer = GameObject.FindGameObjectsWithTag("viewer");
            if (orphanViewer.Length != 0)
            {
                foreach (GameObject viewer in orphanViewer)
                {
                    GameObject.Destroy(viewer);
                }
            }
        }

        public static void DestroyOrphanButtons(List<JsonDirectory> JsonDirectories)
        {
            Debug.Log("Purging buttons");
            foreach (JsonDirectory obj in JsonDirectories)
            {
                GameObject.Destroy(obj.obj);
            }
        }
    }

}
