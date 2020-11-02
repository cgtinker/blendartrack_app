using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUp : MonoBehaviour
{
    public AdditiveSceneManager sceneManager;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);

        //loading last loaded scene
        int sceneIndex = UserPreferences.Instance.GetIntPref("scene");
        sceneManager.SwitchScene(sceneIndex);

        Destroy(this.gameObject);
    }
}
