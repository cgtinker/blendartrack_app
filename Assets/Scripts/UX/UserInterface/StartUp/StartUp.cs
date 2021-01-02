using System.Collections;
using UnityEngine;

public class StartUp : MonoBehaviour
{
    public GameObject Tutorial;

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

        //turn of sleep mode
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        yield return new WaitForSeconds(0.25f);

        //if first time
        if (PlayerPrefs.GetInt("firstTime", -1) == -1)
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

        if (PlayerPrefs.GetInt("tutorial", -1) == 1)
        {
            Tutorial.SetActive(true);
            Destroy(this.gameObject);
        }

        else
        {
            Tutorial.SetActive(false);

            //enable auto rotation
            Screen.autorotateToLandscapeLeft = true;
            Screen.autorotateToLandscapeRight = true;
            Screen.orientation = ScreenOrientation.AutoRotation;

            //loading scene
            int scene = PlayerPrefs.GetInt("scene", 1);
            sceneManager.SwitchScene(scene);

            Destroy(this.gameObject);
        }

    }
}
