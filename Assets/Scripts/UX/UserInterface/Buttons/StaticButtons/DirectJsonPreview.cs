using System;
using UnityEngine;

namespace ArRetarget
{
    public class DirectJsonPreview : MonoBehaviour
    {
        public FileBrowserEventManager fileBrowserEventManager;
        public FileBrowserButton fileBrowserButton;

        public void OnOpenPreview()
        {
            fileBrowserButton.OnOpenFileBrowser();
            fileBrowserEventManager.OnToggleViewer(0, true, FileManagement.FileContents(fileBrowserEventManager.JsonDirectories[0].jsonFilePath));
        }
    }
}
