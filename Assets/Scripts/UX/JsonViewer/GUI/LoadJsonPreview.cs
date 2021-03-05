using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

namespace ArRetarget
{
	/*
	public class LoadJsonPreview : MonoBehaviour
	{
		[SerializeField]
		private GameObject LoadingScreen = null;
		[SerializeField]
		private TextMeshProUGUI LoadingMessage = null;
		[SerializeField]
		private GameObject jsonViewerReference = null;
		[SerializeField]
		private GameObject JsonViewerPrefab = null;

		#region json viewer
		//setting all other buttons inactive
		public void OnToggleViewer(int btnIndex, bool activateViewer, List<JsonDirectory> JsonDirectories)
		{
			//TODO: Manage in Load Method
			PurgeOrphans.DestroyOrphanViewer();

			if (activateViewer)
			{
				StartCoroutine(OpeningFile(btnIndex, JsonDirectories));
			}

			else
			{
				DeactivateViewer(JsonDirectories);
				LoadingScreen.SetActive(false);
			}
		}

		private IEnumerator OpeningFile(int btnIndex, List<JsonDirectory> JsonDirectories)
		{
			double mib = FileManagement.GetFileSize(JsonDirectories[btnIndex].jsonFilePath);
			Debug.Log(mib);

			if (mib > 20.0 && mib < 65.0)
			{
				LoadingScreen.SetActive(true);
				LoadingMessage.text = "loading .json preview...";
				yield return new WaitForSeconds(0.2f);
			}

			else if (mib > 65.0)
			{
				LogManager.Instance.Log(
						"The recording size is to large for the in app viewer. " + "But you can still import the data in blender!", LogManager.Message.Warning);

				var jsonFile = JsonDirectories[btnIndex].obj.GetComponent<JsonFileButton>();
				jsonFile.ViewDataButton.SetActive(false);
				jsonFile.viewerActive = false;
				yield break;
			}

			string contents = FileManagement.FileContents(JsonDirectories[btnIndex].jsonFilePath);
			Debug.Log("attempt to preview data");
			ActivateViewerButtons(true);

			//instantiating the viewer
			jsonViewerReference = Instantiate(JsonViewerPrefab, Vector3.zero, Quaternion.identity);
			var jsonDataImporter = jsonViewerReference.GetComponent<JsonDataImporter>();

			//open the json file and import the data to preview it
			bool fileOpen = jsonDataImporter.OpenFile(contents);

			yield return new WaitForEndOfFrame();

			if (fileOpen)
			{
				//deactivating the other buttons
				foreach (JsonDirectory data in JsonDirectories)
				{
					if (data.index != btnIndex)
					{
						data.obj.SetActive(false);
					}

					else
					{
						//change selection and visual
						var jsonFileButton = data.obj.GetComponent<JsonFileButton>();
						jsonFileButton.ChangeSelectionToggleStatus(true);
						jsonFileButton.btnIsOn = true;
						jsonFileButton.ViewDataButton.SetActive(false);
						jsonFileButton.viewedDataImage.sprite = jsonFileButton.viewedDataIcon;
						LoadingScreen.SetActive(false);
					}
				}
			}
		}
	
		private void ActivateViewerButtons(bool status)
		{
			//TODO: Create Viewer Scene

			
			FileBrowserBackground.enabled = !status;

			//changing the back buttons
			ViewerAcitveBackButton.SetActive(status);
			ViewerInactiveBackButton.SetActive(!status);

			//changing title
			ViewerActiveTitle.SetActive(status);
			ViewerInactiveTitle.SetActive(!status);

			//changing footer
			MenuFooter.SetActive(!status);
			SupportFooter.SetActive(status);

			//deactivate selection helper
			SelectionHelperParent.SetActive(!status);
			
		}
	
		//single for back button
		public void DeactivateViewer(List<JsonDirectory> JsonDirectories)
		{
			//TODO: Manage in Load Method
			PurgeOrphans.DestroyOrphanViewer();

			Debug.Log("stop viewing data");
			ActivateViewerButtons(false);

			foreach (JsonDirectory data in JsonDirectories)
			{
				data.obj.GetComponent<JsonFileButton>().ViewDataButton.SetActive(true);

				if (!data.obj.activeSelf)
				{
					data.obj.SetActive(true);
				}
			}
		}
		#endregion
	}
	*/
}