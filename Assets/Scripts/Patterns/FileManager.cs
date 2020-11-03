using UnityEngine;
using System.IO;

public class FileManager : MonoBehaviour
{
    #region delete data
    public static void DeleteFilesInDir(string dir)
    {
        foreach (FileInfo info in JsonsInDir(dir))
        {
            DeleteFileAtMediaPath(info.FullName);
        }
    }

    public static void DeleteFileAtMediaPath(string mediaPath)
    {
        if (ValidatePath(mediaPath))
        {
            File.Delete(mediaPath);
            Debug.Log("Deleted temp File at " + mediaPath);
        }
    }
    #endregion

    #region Share Data
    public static void ShareJsonFiles(string dir, string subject, string text)
    {
        FileInfo[] files = JsonsInDir(dir);

        if (files.Length > 0)
        {
            var nativeShare = new NativeShare();

            foreach (FileInfo file in files)
            {
                if (ValidatePath(mediaPath: file.FullName))
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
        if (ValidatePath(mediaPath))
        {
            var nativeShare = new NativeShare();
            nativeShare.SetSubject(subject).SetText(text);
            nativeShare.Share();
        }
    }
    #endregion

    #region helper
    public static FileInfo[] JsonsInDir(string dir)
    {
        var dirInfo = new DirectoryInfo(dir);
        FileInfo[] info = dirInfo.GetFiles("*.json");
        return info;
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
    #endregion
}
