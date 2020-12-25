using System.Collections;
using UnityEngine;

public class StartUp : MonoBehaviour
{
    private IEnumerator Start()
    {
        //lock to portait screen in startup
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;
        Screen.orientation = ScreenOrientation.Portrait;

        //time for animation
        yield return new WaitForSeconds(3.0f);

        //assinging references
        var obj = GameObject.FindGameObjectWithTag("manager");
        var sceneManager = obj.GetComponent<AdditiveSceneManager>();

        //player prefs if first time
        int firstTime = PlayerPrefs.GetInt("firstTime", -1);
        if (firstTime == -1)
        {
            PlayerPrefs.SetInt("firstTime", 1);

            PlayerPrefs.SetInt("tutorial", 1);
            PlayerPrefs.SetInt("hints", 1);
            PlayerPrefs.SetInt("reference", 1);
            PlayerPrefs.SetInt("recordCam", 1);
            PlayerPrefs.SetInt("vidzip", 1);
        }

        //turn of sleep mode
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        yield return new WaitForSeconds(0.25f);

        //lock to portait screen in startup
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.orientation = ScreenOrientation.AutoRotation;

        //loading scene
        int scene = PlayerPrefs.GetInt("scene", 1);
        sceneManager.SwitchScene(scene);

        Destroy(this.gameObject);
    }
}
