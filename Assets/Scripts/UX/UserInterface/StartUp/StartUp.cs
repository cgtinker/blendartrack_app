using System.Collections;
using UnityEngine;

public class StartUp : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(3.0f);
        var obj = GameObject.FindGameObjectWithTag("manager");
        var sceneManager = obj.GetComponent<AdditiveSceneManager>();

        //loading scene one to assign recording settings
        //sceneManager.SwitchScene(1);
        yield return new WaitForSeconds(0.15f);

        //loading last loaded scene
        int scene = UserPreferences.Instance.GetIntPref("scene");
        sceneManager.SwitchScene(scene);

        //GameObject.FindGameObjectWithTag("manager").GetComponent<AdditiveSceneManager>().SwitchScene(scene);
        yield return new WaitForSeconds(0.1f);
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        Destroy(this.gameObject);
    }
}
