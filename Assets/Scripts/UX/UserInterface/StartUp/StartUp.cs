using System.Collections;
using UnityEngine;

public class StartUp : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1.5f);

        //loading last loaded scene
        int sceneIndex = UserPreferences.Instance.GetIntPref("scene");
        GameObject.FindGameObjectWithTag("manager").GetComponent<AdditiveSceneManager>().SwitchScene(sceneIndex);

        yield return new WaitForSeconds(0.25f);

        Destroy(this.gameObject);
    }
}
