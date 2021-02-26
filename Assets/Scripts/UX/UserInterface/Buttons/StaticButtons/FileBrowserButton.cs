using UnityEngine;
using ArRetarget;

public class FileBrowserButton : MonoBehaviour
{
	public GameObject interfaceObj;
	InputHandler inputHandler;
	TrackingDataManager dataManager;
	//FileBrowserEventManager fileBrowserEventManager;

	public GameObject fileBrowser;
	public GameObject mainScreen;

	private void Awake()
	{
		GameObject obj = GameObject.FindGameObjectWithTag("manager");
		dataManager = obj.GetComponent<TrackingDataManager>();
		inputHandler = interfaceObj.GetComponent<InputHandler>();
		//fileBrowserEventManager = interfaceObj.GetComponent<FileBrowserEventManager>();
	}

	public void OnOpenFileBrowser()
	{
		if (dataManager._recording)
		{
			string tmp = FileManagement.GetParagraph();

			inputHandler.GeneratedFilePopup($"failed to open file browser{tmp}", "please finish recording");
			return;
		}

		else
		{
			Debug.Log("Attempt to open File Browser");

			OpenFileBrowser();
			Cleanup();
		}
	}

	//capsuled for instant preview
	public void OpenFileBrowser()
	{
		//fileBrowserEventManager.GenerateButtons();
		//inputHandler.DisableArSession();
		//fileBrowser.SetActive(true);
	}

	public void Cleanup()
	{
		//inputHandler.PurgeOrphanPopups();
		//mainScreen.SetActive(false);
	}
}
