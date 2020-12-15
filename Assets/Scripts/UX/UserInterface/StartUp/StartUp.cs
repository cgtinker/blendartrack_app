using System.Collections;
using UnityEngine;

public class StartUp : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(3.0f);
        var obj = GameObject.FindGameObjectWithTag("manager");
        var sceneManager = obj.GetComponent<AdditiveSceneManager>();

        //loading last loaded scene
        int test = PlayerPrefs.GetInt("scene", -1);
        {
            if (test == -1)
            {
                PlayerPrefs.SetInt("hints", 1);
                PlayerPrefs.SetInt("reference", 1);
                PlayerPrefs.SetInt("recordCam", 1);
            }
        }

        int scene = PlayerPrefs.GetInt("scene", 1);
        sceneManager.SwitchScene(scene);
        yield return new WaitForSeconds(0.2f);

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        yield return new WaitForSeconds(0.1f);

        Destroy(this.gameObject);
    }
}
