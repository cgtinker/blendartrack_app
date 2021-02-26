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
        public GameObject BlackOverlay;

        public void OnOpenPreview()
        {
            StartCoroutine(OpenPreview());
            BlackOverlay.SetActive(true);
        }

        IEnumerator OpenPreview()
        {
            // some weird wait timings for iOS

            // guess the .net system io file handling
            // cause some longer wait times on iOS
            // resulting in weird timing handling

            yield return new WaitForEndOfFrame();

            //reusing btn event (wait for purge)
            fileBrowserButton.OpenFileBrowser();
            yield return new WaitForEndOfFrame();

            //create preview
            try
            {
                fileBrowserEventManager.OnToggleViewer(0, true);
            }

            catch
            {
                Debug.Log("Cannot launch instant preview");
            }

            yield return new WaitForSeconds(0.2f);
            BlackOverlay.SetActive(false);

            fileBrowserButton.Cleanup();
        }
    }
}
