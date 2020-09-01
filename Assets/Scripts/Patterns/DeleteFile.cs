using UnityEngine;
using System.IO;

namespace ArRetarget
{
    public class DeleteFile
    {
        public static void FileAtMediaPath(string mediaPath)
        {
            try
            {
                // Check if file exists with its full path    
                if (File.Exists(mediaPath))
                {
                    // If file found, delete it    
                    File.Delete(mediaPath);
                    Debug.Log("Deleted temp File at " + mediaPath);
                }

                else
                    Debug.Log("File not found");
            }

            catch (IOException ioExp)
            {
                Debug.Log(ioExp.Message);
            }
        }
    }
}
