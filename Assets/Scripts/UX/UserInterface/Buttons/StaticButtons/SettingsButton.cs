using UnityEngine;
using ArRetarget;

public class SettingsButton : MonoBehaviour
{
	public GameObject interfaceObj;
	InputHandler inputHandler;
	TrackingDataManager dataManager;
	SettingsButtonManager settingsManager;

	public GameObject settingsMenu;
	public GameObject mainScreen;

	private void Awake()
	{
		GameObject obj = GameObject.FindGameObjectWithTag("manager");
		dataManager = obj.GetComponent<TrackingDataManager>();
		inputHandler = interfaceObj.GetComponent<InputHandler>();
		settingsManager = interfaceObj.GetComponent<SettingsButtonManager>();
	}

	public void OnOpenSettings()
	{
		if (dataManager._recording)
		{
			string tmp = FileManagement.GetParagraph();

			inputHandler.GeneratedFilePopup($"failed to open settings{tmp}", "please finish recording");
			return;
		}

		else
		{
			settingsManager.GenerateRecordingSettingsButtons();
			settingsMenu.SetActive(true);
			inputHandler.PurgeOrphanPopups();
			//inputHandler.DisableArSession();
			mainScreen.SetActive(false);
		}
	}
}
