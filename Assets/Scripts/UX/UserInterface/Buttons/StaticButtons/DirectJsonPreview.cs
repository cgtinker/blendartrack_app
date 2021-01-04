using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace ArRetarget
{
    public class DirectJsonPreview : MonoBehaviour
    {
        public FileBrowserEventManager fileBrowserEventManager;
        public FileBrowserButton fileBrowserButton;

        public GameObject recorderPopup;

        public void OnOpenPreview()
        {
            StartCoroutine(Open());
        }

        IEnumerator Open()
        {
            yield return new WaitForSeconds(0.2f);

            fileBrowserButton.OnOpenFileBrowser();
            fileBrowserEventManager.OnToggleViewer(0, true, FileManagement.FileContents(fileBrowserEventManager.JsonDirectories[0].jsonFilePath));

            //recorderPopup.SetActive(false);
        }
    }
}
