using UnityEngine;
using System.Collections;

namespace ArRetarget
{
    public class FilebrowserEventListener : MonoBehaviour
    {
        public static string persistentPath;

        private void Start()
        {
            persistentPath = Application.persistentDataPath;
        }

        #region Share and Delete
        public void OnShareFiles()
        {

        }

        public void OnDeleteFiles()
        {

        }
        #endregion

        #region Select and Preview
        public void OnSelectFile()
        {

        }

        public void OnPreviewFile()
        {

        }
        #endregion

        #region Selection Helper
        public void OnSelectAllFiles()
        {

        }

        public void OnSelectNoneFiles()
        {

        }

        public void OnSelectTodayFiles()
        {

        }
        #endregion

        #region Navigation
        public void OnReturnToFilebrowser()
        {

        }

        public void OnReturnToTracking()
        {

        }

        public void OnOpenViewer()
        {

        }
        #endregion
    }
}
