using System.Collections;
using UnityEngine;

public class StartUp : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(3.0f);

        //loading last loaded scene
        int scene = UserPreferences.Instance.GetIntPref("scene");
        GameObject.FindGameObjectWithTag("manager").GetComponent<AdditiveSceneManager>().SwitchScene(scene);

        yield return new WaitForSeconds(0.25f);
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        Destroy(this.gameObject);
    }
}
