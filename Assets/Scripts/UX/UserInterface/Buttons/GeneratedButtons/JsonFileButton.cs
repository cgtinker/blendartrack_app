using UnityEngine;
using TMPro;
using UnityEngine.UI;

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

		//file name of the json
		[SerializeField]
		private TextMeshProUGUI filenameText;

		//visual selection state
		//public Toggle selectToggleBtn;
		//public Button selectToggleBtn;
		[SerializeField]
		private bool btnIsOn = false;

		[SerializeField]
		private GameObject selected;
		[SerializeField]
		private GameObject deselected;

		//info about the safed json file
		public JsonDirectory m_jsonDirData;

		public void InitFileButton(JsonDirectory jsonFileData)
		{
			m_jsonDirData = new JsonDirectory();
			m_jsonDirData = jsonFileData;
			filenameText.text = jsonFileData.dirName;

			if (jsonFileData.jsonSize >= 65)
			{
				ViewDataButton.SetActive(false);
			}

			btnIsOn = m_jsonDirData.active;
			ChangeViewedDisplayStatus(jsonFileData.viewed);
			ChangeSelectionToggleStatus(btnIsOn);
		}

		public void OnTouchViewData()
		{
			m_jsonDirData.viewed = true;
			ChangeViewedDisplayStatus(true);

			if (string.IsNullOrEmpty(m_jsonDirData.jsonFilePath))
			{
				viewedDataImage.sprite = viewedDataIcon;
				LogManager.Instance.Log("File reference couldn't be found. May the included files are corrupted or empty. Consider to check the data manually on your desktop.", LogManager.Message.Error);
				return;
			}

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

		public void OnToggleButton()
		{
			btnIsOn = !btnIsOn;
			ChangeSelectionToggleStatus(btnIsOn);
		}

		public void ChangeSelectionToggleStatus(bool status)
		{
			selected.SetActive(status);
			deselected.SetActive(!status);

			if (m_jsonDirData.active == status)
			{
				return;
			}

			m_jsonDirData.active = status;
			FileManager.ChangeDirectoryProperties(m_jsonDirData);
		}
	}
}
