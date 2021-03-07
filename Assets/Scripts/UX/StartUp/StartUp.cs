using System.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

/*
public class StartUp : MonoBehaviour
{
	public GameObject Tutorial;
	public GameObject SupportChecker;
	public ARSession arSession;

	private IEnumerator Start()
	{
		ARCoreSession();
		//PortraitScreenOrientation();

		//time for animation
		yield return new WaitForSeconds(3.0f);

		//assinging references
		var obj = GameObject.FindGameObjectWithTag("manager");
		var sceneManager = obj.GetComponent<AdditiveSceneManager>();

		//turn of sleep mode
		//Screen.sleepTimeout = SleepTimeout.NeverSleep;

		yield return new WaitForSeconds(0.25f);

		//if first time
		if (PlayerPrefs.GetInt("firstTime", -1) == -1)
			PlayerPrefsFirstTime();

		BeginSessionORTutorial(sceneManager);
	}

	void ARCoreSession()
	{
		//check if ar core is installed
		if (DeviceManager.Instance.device == DeviceManager.Device.Android)
		{
			SupportChecker.SetActive(true);
		}

		else
		{
			SupportChecker.SetActive(false);
			arSession.enabled = true;
		}
	}

	void PortraitScreenOrientation()
	{
		//lock to portait screen in startup
		Screen.autorotateToLandscapeLeft = false;
		Screen.autorotateToLandscapeRight = false;
		Screen.orientation = ScreenOrientation.Portrait;
	}

	void AutoScreenOrientation()
	{
		Screen.autorotateToLandscapeLeft = true;
		Screen.autorotateToLandscapeRight = true;
		Screen.orientation = ScreenOrientation.AutoRotation;
	}

	void PlayerPrefsFirstTime()
	{
		//must be 1 in the end
		PlayerPrefs.SetInt("firstTime", 1);

		PlayerPrefs.SetInt("scene", 1);
		PlayerPrefs.SetInt("tutorial", 1);
		PlayerPrefs.SetInt("hints", 1);
		PlayerPrefs.SetInt("reference", 1);
		PlayerPrefs.SetInt("recordCam", 1);
		PlayerPrefs.SetInt("vidzip", 1);
	}

	void BeginSessionORTutorial(AdditiveSceneManager sceneManager)
	{
		if (PlayerPrefs.GetInt("tutorial", -1) == 1)
		{
			Tutorial.SetActive(true);
			Destroy(this.gameObject);
		}

		else
		{
			Tutorial.SetActive(false);

			//enable auto rotation
			AutoScreenOrientation();

			//loading scene
			int scene = PlayerPrefs.GetInt("scene", 1);
			sceneManager.SwitchScene(scene);

			Destroy(this.gameObject);
		}
	}
}
*/