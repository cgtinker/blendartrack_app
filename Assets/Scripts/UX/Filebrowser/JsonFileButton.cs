using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

namespace ArRetarget
{
	public class JsonFileButton : MonoBehaviour
	{
		[SerializeField]
		private GameObject ViewDataButton;
		[SerializeField]
		private Image viewedDataImage;
		[SerializeField]
		private Sprite viewedDataIcon;
		[SerializeField]
		private Sprite unviewedDataIcon;

		[SerializeField]
		private TextMeshProUGUI filenameText;

		[SerializeField]
		private bool btnIsOn = false;
		[SerializeField]
		private GameObject selected;
		[SerializeField]
		private GameObject deselected;

		private FilebrowserEventListener eventListener;
		private JsonDirectory m_jsonDirData;

		private IEnumerator Start()
		{
			yield return new WaitForEndOfFrame();
			var obj = GameObject.FindGameObjectWithTag(TagManager.InterfaceManager);
			if (obj)
				eventListener = obj.GetComponent<FilebrowserEventListener>();
		}

		public void InitFileButton(JsonDirectory jsonFileData)
		{
			m_jsonDirData = jsonFileData;
			filenameText.text = jsonFileData.dirName;
			bool viewable = ViewableJsonContents(jsonFileData);

			if (ViewDataButton)
				ViewDataButton.SetActive(viewable);

			btnIsOn = m_jsonDirData.active;
			ChangeViewedDisplayStatus(jsonFileData.viewed);
			ChangeSelectionToggleStatus(btnIsOn);
		}

		public void OnToggleButton()
		{
			btnIsOn = !btnIsOn;
			if (eventListener)
				eventListener.OnSelectFile();
			ChangeSelectionToggleStatus(btnIsOn);
		}

		public void OnTouchViewData()
		{
			if (!ViewableJsonContents(m_jsonDirData))
				return;

			m_jsonDirData.viewed = true;
			ChangeViewedDisplayStatus(true);

			FileManager.ChangeDirectoryProperties(m_jsonDirData);
			FileManager.JsonPreview = m_jsonDirData;
			StateMachine.Instance.SetState(StateMachine.State.JsonViewer);
		}

		private void ChangeViewedDisplayStatus(bool viewed)
		{
			if (viewed)
				viewedDataImage.sprite = viewedDataIcon;

			else
				viewedDataImage.sprite = unviewedDataIcon;
		}

		private bool ViewableJsonContents(JsonDirectory jsonFileData)
		{
			if (jsonFileData.jsonSize >= 65 || string.IsNullOrEmpty(m_jsonDirData.jsonFilePath))
			{
				return false;
			}

			return true;
		}

		private void ChangeSelectionToggleStatus(bool status)
		{
			selected.SetActive(status);
			deselected.SetActive(!status);
			m_jsonDirData.active = status;
			FileManager.ChangeDirectoryProperties(m_jsonDirData);
		}

		public void ChangeButtonStatus(bool status)
		{
			btnIsOn = status;
			ChangeSelectionToggleStatus(status);
		}
	}
}
