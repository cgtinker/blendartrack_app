using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ArRetarget
{
    public class FilebrowserManager : MonoBehaviour
    {
        public static string persistentPath;
        public static List<JsonDirectory> JsonDirectories = new List<JsonDirectory>();

        [SerializeField]
        private JsonDirectoryHandler jsonDirectoryHandler = null;
        [SerializeField]
        private GenerateJsonDirectoryButtons generateJsonDirectoryButtons = null;
        [SerializeField]
        private FilebrowserNoFilesAvailablePopup filesAvailablePopup = null;

        // referencing all stored directories containing (valid) .jsons
        private void Awake()
        {
            persistentPath = Application.persistentDataPath;
            JsonDirectories = jsonDirectoryHandler.GetDirectories(persistentPath);

            //sorting dirs by time
            if (JsonDirectories.Count > 1)
                JsonDirectories.Sort((JsonDirectory x, JsonDirectory y) => y.value.CompareTo(x.value));

            else
                filesAvailablePopup.InstantiatePopup();
        }
        
        // instantiate the buttons
        private void Start()
        {
            generateJsonDirectoryButtons.GenerateButtons(JsonDirectories);
        }

        /// clean up after leaving the file browser
        private void OnDisable()
        {
            PurgeOrphans.PurgeOrphanZips();
            PurgeOrphans.PurgeErrorMessages();
            PurgeOrphans.DestroyOrphanButtons(JsonDirectories);
            JsonDirectories.Clear();
        }
    }
}
