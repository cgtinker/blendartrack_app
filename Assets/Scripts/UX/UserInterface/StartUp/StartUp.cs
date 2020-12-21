using System.Collections;
using UnityEngine;

public class StartUp : MonoBehaviour
{
    private IEnumerator Start()
    {
        //time for animation
        yield return new WaitForSeconds(3.0f);

        //assinging references
        var obj = GameObject.FindGameObjectWithTag("manager");
        var sceneManager = obj.GetComponent<AdditiveSceneManager>();

        //fixing player prefs if first time
        int test = PlayerPrefs.GetInt("firstTime", -1);
        if (test == -1)
        {
            PlayerPrefs.SetInt("firstTime", 1);

            PlayerPrefs.SetInt("hints", 1);
            PlayerPrefs.SetInt("reference", 1);
            PlayerPrefs.SetInt("recordCam", 1);
        }

        //turn of sleep mode
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        //loading scene
        int scene = PlayerPrefs.GetInt("scene", 1);
        sceneManager.SwitchScene(scene);
        yield return new WaitForSeconds(0.5f);

        Destroy(this.gameObject);
    }
}
