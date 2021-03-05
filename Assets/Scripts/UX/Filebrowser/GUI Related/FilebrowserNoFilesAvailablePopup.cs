using UnityEngine;
using System.Collections;

namespace ArRetarget
{
    public class FilebrowserNoFilesAvailablePopup : MonoBehaviour
    {
        [SerializeField]
        private GameObject popupPrefab = null;
        [SerializeField]
        private Transform parent = null;

        public GameObject InstantiatePopup()
        {
            //new popup
            var popup = Instantiate(popupPrefab, Vector3.zero, Quaternion.identity);
            var m_pop = popup.GetComponent<PopUpDisplay>();

            //message
            var para = FileManagement.GetParagraph();
            m_pop.text = $"Wow, such empty!{para}Please record something to {para}preview and share files";
            m_pop.type = PopUpDisplay.PopupType.Static;

            //transforms
            popup.transform.localScale = Vector3.one;
            m_pop.DisplayPopup(parent);

            //reference for cleanup
            return popup;
        }
    }
}

