using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections;
using TMPro;
using System.Collections.Generic;

namespace ArRetarget
{
	public class InputHandler : MonoBehaviour
	{
		//[Header("Runtime Button")]
		//public GameObject SceneButtonPrefab;
		//public GameObject MainMenu;

		[Header("Pop Up Display")]
		[SerializeField]
		private GameObject PopupPrefab = null;
		[SerializeField]
		private Transform PopupParent = null;
		//public GameObject FileBrowserButton;

		private List<GameObject> popupList = new List<GameObject>();

		[Header("on finish rec Display")]
		[SerializeField]
		private GameObject OnFinishRecordingPrefab = null;
		private TrackingDataManager dataManager;

		public bool recording = false;

		private void Awake()
		{
			GameObject obj = GameObject.FindGameObjectWithTag("manager");
			dataManager = obj.GetComponent<TrackingDataManager>();
			OnFinishRecordingPrefab.SetActive(false);
		}

		#region tracking
		public void StartTracking()
		{
			recording = true;
			dataManager.ToggleRecording();
		}

		public void StopTrackingAndSerializeData()
		{
			recording = false;
			dataManager.ToggleRecording();
			OnFinishRecordingPrefab.SetActive(true);
		}
		#endregion

		#region UI Events
		public void GeneratedFilePopup(string message, string filename)
		{
			if (OnFinishRecordingPrefab.activeSelf)
				return;

			//generating popup element
			var m_popup = Instantiate(PopupPrefab) as GameObject;
			popupList.Add(m_popup);

			//script reference to set contents
			var popupDisplay = m_popup.GetComponent<PopUpDisplay>();

			//if (filename.Length != 0)
			//{
			//	popupDisplay.type = PopUpDisplay.PopupType.Notification;
			//	//travel timings
			//	popupDisplay.travelDuration = 5f;
			//	popupDisplay.staticDuration = 5f;

			//	popupDisplay.desitionation = FileBrowserButton;
			//	popupDisplay.text = $"{message}{filename}";
			//}

			//else
			//{
			popupDisplay.type = PopUpDisplay.PopupType.Notification;
			popupDisplay.staticDuration = 8f;
			popupDisplay.text = message;
			//}

			popupDisplay.DisplayPopup(PopupParent);
		}

		public void PurgeOrphanPopups()
		{
			OnFinishRecordingPrefab.SetActive(false);
			foreach (GameObject popup in popupList)
			{
				Destroy(popup);
			}

			popupList.Clear();
		}
		#endregion
	}
}
