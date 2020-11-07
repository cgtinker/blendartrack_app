using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUp : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1.5f);

        //loading last loaded scene
        int sceneIndex = UserPreferences.Instance.GetIntPref("scene");
        GameObject.FindGameObjectWithTag("manager").GetComponent<AdditiveSceneManager>().SwitchScene(sceneIndex);

        Destroy(this.gameObject);
    }
}
