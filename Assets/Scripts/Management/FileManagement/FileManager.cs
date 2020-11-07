using UnityEngine;
using System.IO;

namespace ArRetarget
{
    public class FileManager : MonoBehaviour
    {
        #region delete data
        public static void DeleteFilesInDir(string dir)
        {
            foreach (FileInfo info in FileManagementHelper.JsonsInDir(dir))
            {
                DeleteFileAtMediaPath(info.FullName);
            }
        }

        public static void DeleteFileAtMediaPath(string mediaPath)
        {
            if (FileManagementHelper.ValidatePath(mediaPath))
            {
                File.Delete(mediaPath);
                Debug.Log("Deleted temp File at " + mediaPath);
            }
        }
        #endregion

        #region Share Data
        public static void ShareJsonFiles(string dir, string subject, string text)
        {
            FileInfo[] files = FileManagementHelper.JsonsInDir(dir);

            if (files.Length > 0)
            {
                var nativeShare = new NativeShare();

                foreach (FileInfo file in files)
                {
                    if (FileManagementHelper.ValidatePath(mediaPath: file.FullName))
                    {
                        nativeShare.AddFile(file.FullName);
                    }
                }

                nativeShare.SetSubject(subject).SetText(text);
                nativeShare.Share();
            }
        }

        public static void ShareJson(string mediaPath, string subject, string text)
        {
            if (FileManagementHelper.ValidatePath(mediaPath))
            {
                var nativeShare = new NativeShare();
                nativeShare.SetSubject(subject).SetText(text);
                nativeShare.Share();
            }
        }
        #endregion
    }
}
