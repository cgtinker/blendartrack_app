using System.Collections;
using UnityEngine;

public class StartUp : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1.5f);

        //loading last loaded scene
        int scene = UserPreferences.Instance.GetIntPref("scene");
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        GameObject.FindGameObjectWithTag("manager").GetComponent<AdditiveSceneManager>().SwitchScene(scene);

        yield return new WaitForSeconds(0.25f);

        Destroy(this.gameObject);
    }
}
